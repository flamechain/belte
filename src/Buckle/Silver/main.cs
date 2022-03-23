using System;
using System.Diagnostics;
using System.Collections.Generic;
using Buckle;

namespace CommandLine {

    public static class CmdLine {

        const int SUCCESS_EXIT_CODE = 0;
        const int ERROR_EXIT_CODE = 1;
        const int FATAL_EXIT_CODE = 2;

        private static CompilerState DecodeOptions(string[] args, out List<Diagnostic> diagnostics) {
            diagnostics = new List<Diagnostic>();
            CompilerState state = new CompilerState();
            state.tasks = new List<FileState>();

            bool specify_stage = false;
            bool specify_out = false;
            state.finish_stage = CompilerStage.linked;
            state.link_output_filename = "a.exe";

            for (int i=0; i<args.Length; i++) {
                string arg = args[i];

                if (arg.StartsWith('-')) {
                    switch (arg) {
                        case "-E":
                            specify_stage = true;
                            state.finish_stage = CompilerStage.preprocessed;
                            break;
                        case "-S":
                            specify_stage = true;
                            state.finish_stage = CompilerStage.compiled;
                            break;
                        case "-c":
                            specify_stage = true;
                            state.finish_stage = CompilerStage.assembled;
                            break;
                        case "-r":
                            return state;
                        case "-o":
                            specify_out = true;
                            if (i >= args.Length-1)
                                diagnostics.Add(new Diagnostic(DiagnosticType.fatal, "missing output filename (with '-o')"));
                            state.link_output_filename = args[i++];
                            break;
                        default:
                            diagnostics.Add(new Diagnostic(DiagnosticType.fatal, $"unknown argument '{arg}'"));
                            break;
                    }
                } else {
                    string filename = arg;
                    string[] parts = filename.Split('.');
                    string type = parts[parts.Length-1];
                    FileState task;
                    task.in_filename = filename;

                    switch (type) {
                        case "ble":
                            task.stage = CompilerStage.raw;
                            break;
                        case "pble":
                            task.stage = CompilerStage.preprocessed;
                            break;
                        case "s":
                        case "asm":
                            task.stage = CompilerStage.compiled;
                            break;
                        case "o":
                        case "obj":
                            task.stage = CompilerStage.assembled;
                            break;
                        default:
                            diagnostics.Add(new Diagnostic(DiagnosticType.fatal, $"unknown file type of input file '{filename}'; ignoring"));
                            break;
                    }
                }
            }

            if (specify_out && specify_stage)
                diagnostics.Add(new Diagnostic(DiagnosticType.fatal, "cannot specify output file with '-E', '-S', or '-c'"));
            if (state.tasks.Count == 0)
                diagnostics.Add(new Diagnostic(DiagnosticType.fatal, "no input files"));

            return state;
        }

        private static int ResolveDiagnostics(Compiler compiler) {
            if (compiler.diagnostics.Count == 0) return SUCCESS_EXIT_CODE;
            DiagnosticType worst = DiagnosticType.unknown;

            foreach (Diagnostic diagnostic in compiler.diagnostics) {
                if (diagnostic.type == DiagnosticType.unknown) continue;

                ConsoleColor prev = Console.ForegroundColor;
                Console.Write($"{compiler.me}: ");
                Console.ForegroundColor = ConsoleColor.Red;

                if (diagnostic.type == DiagnosticType.warning) {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("warning: ");

                    if (worst == DiagnosticType.unknown)
                        worst = DiagnosticType.warning;
                } else if (diagnostic.type == DiagnosticType.error) {
                    Console.Write("error: ");

                    if (worst != DiagnosticType.fatal)
                        worst = DiagnosticType.error;
                } else if (diagnostic.type == DiagnosticType.fatal) {
                    Console.Write("fatal error: ");
                    worst = DiagnosticType.fatal;
                }

                Console.ForegroundColor = prev;
                Console.WriteLine($"{diagnostic.msg}");
            }

            switch (worst) {
                case DiagnosticType.error: return ERROR_EXIT_CODE;
                case DiagnosticType.fatal: return FATAL_EXIT_CODE;
                case DiagnosticType.unknown:
                case DiagnosticType.warning:
                default: return SUCCESS_EXIT_CODE;
            }
        }

        public static int Main(string[] args) {
            Compiler compiler = new Compiler();
            compiler.me = Process.GetCurrentProcess().ProcessName;

            compiler.state = DecodeOptions(args, out List<Diagnostic> diagnostics);
            if (diagnostics.Count > 0) {
                compiler.diagnostics.AddRange(diagnostics); // CmdLine has no independent diagnostic handler
                return ResolveDiagnostics(compiler);
            }

            return ResolveDiagnostics(compiler);
        }
    }
}
