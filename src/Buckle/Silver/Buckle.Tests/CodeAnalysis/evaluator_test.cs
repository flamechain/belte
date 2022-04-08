using System;
using System.Collections.Generic;
using Buckle.CodeAnalysis;
using Buckle.CodeAnalysis.Symbols;
using Buckle.CodeAnalysis.Syntax;
using Xunit;

namespace Buckle.Tests.CodeAnalysis {
    public class EvaluatorTests {
        [Theory]
        [InlineData(";", null)]

        [InlineData("1;", 1)]
        [InlineData("+1;", 1)]
        [InlineData("-1;", -1)]
        [InlineData("14 + 12;", 26)]
        [InlineData("12 - 3;", 9)]
        [InlineData("4 * 2;", 8)]
        [InlineData("4 ** 2;", 16)]
        [InlineData("9 / 3;", 3)]
        [InlineData("(10);", 10)]

        [InlineData("1 | 2;", 3)]
        [InlineData("1 | 0;", 1)]
        [InlineData("1 & 3;", 1)]
        [InlineData("1 & 0;", 0)]
        [InlineData("1 ^ 0;", 1)]
        [InlineData("0 ^ 1;", 1)]
        [InlineData("1 ^ 1;", 0)]
        [InlineData("1 ^ 3;", 2)]
        [InlineData("~1;", -2)]
        [InlineData("~4;", -5)]
        [InlineData("1 << 1;", 2)]
        [InlineData("3 << 2;", 12)]
        [InlineData("2 >> 1;", 1)]
        [InlineData("3 >> 1;", 1)]
        [InlineData("12 >> 2;", 3)]
        [InlineData("false | false;", false)]
        [InlineData("false | true;", true)]
        [InlineData("true | false;", true)]
        [InlineData("true | true;", true)]
        [InlineData("false & false;", false)]
        [InlineData("false & true;", false)]
        [InlineData("true & false;", false)]
        [InlineData("true & true;", true)]
        [InlineData("false ^ false;", false)]
        [InlineData("false ^ true;", true)]
        [InlineData("true ^ false;", true)]
        [InlineData("true ^ true;", false)]

        [InlineData("false == false;", true)]
        [InlineData("true == false;", false)]
        [InlineData("false != false;", false)]
        [InlineData("true != false;", true)]
        [InlineData("true && true;", true)]
        [InlineData("true && false;", false)]
        [InlineData("true;", true)]
        [InlineData("false;", false)]
        [InlineData("!true;", false)]
        [InlineData("!false;", true)]

        [InlineData("12 == 3;", false)]
        [InlineData("3 == 3;", true)]
        [InlineData("12 != 3;", true)]
        [InlineData("3 != 3;", false)]
        [InlineData("3 < 4;", true)]
        [InlineData("5 < 3;", false)]
        [InlineData("4 <= 4;", true)]
        [InlineData("4 <= 5;", true)]
        [InlineData("5 <= 4;", false)]
        [InlineData("3 > 4;", false)]
        [InlineData("5 > 3;", true)]
        [InlineData("4 >= 4;", true)]
        [InlineData("4 >= 5;", false)]
        [InlineData("5 >= 4;", true)]

        [InlineData("auto a = 10;", 10)]
        [InlineData("auto a = 10; a * a;", 100)]
        [InlineData("auto a = 1; a = 10 * a;", 10)]

        [InlineData("auto a = 0; if (a == 0) { a = 10; } a;", 10)]
        [InlineData("auto a = 0; if (a == 4) { a = 10; } a;", 0)]
        [InlineData("auto a = 0; if (a == 0) { a = 10; } else { a = 5; } a;", 10)]
        [InlineData("auto a = 0; if (a == 4) { a = 10; } else { a = 5; } a;", 5)]

        [InlineData("auto i = 10; auto result = 0; while (i > 0) { result = result + i; i = i - 1; } result;", 55)]
        [InlineData("auto result = 0; for (auto i=0; i<=10; i=i+1) { result = result + i; } result;", 55)]
        [InlineData("auto result = 0; do { result = result + 1; } while (result < 10); result;", 10)]
        public void Evaluator_Computes_CorrectValues(string text, object expectedValue) {
            AssertValue(text, expectedValue);
        }

        [Fact]
        public void Evaluator_InvokeFunctionArguments_NoInifiniteLoop() {
            var text = @"print(""Hi""[[=]][)];";

            var diagnostics = @"
                unexpected token '=', expected ')'
                unexpected token '=', expected identifier
                unexpected token ')', expected identifier
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_FunctionParameters_NoInfiniteLoop() {
            var text = @"
                void hi(string name[[[=]]][)] {
                    print(""Hi "" + name + ""!"");
                }[]
            ";

            var diagnostics = @"
                unexpected token '=', expected ')'
                unexpected token '=', expected '{'
                unexpected token '=', expected identifier
                unexpected token ')', expected identifier
                expected '}' at end of input
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_Block_NoInfiniteLoop() {
            var text = @"
                {
                [)][]
            ";

            var diagnostics = @"
                unexpected token ')', expected identifier
                expected '}' at end of input
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_IfStatement_Reports_CannotConvert() {
            var text = @"
                auto x = 0;
                if ([10]) x = 1;
            ";

            var diagnostics = @"
                cannot convert from type 'int' to 'bool'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_VariableDelcaration_Reports_Redeclaration() {
            var text = @"
                auto x = 10;
                auto y = 100;
                {
                    auto x = 10;
                }
                auto [x] = 5;
            ";

            var diagnostics = @"
                redefinition of 'x'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_NameExpression_Reports_Undefined() {
            var text = @"[x] * 10;";

            var diagnostics = @"
                undefined symbol 'x'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_NameExpression_Reports_NoErrorForInsertedToken() {
            var text = @"";

            var diagnostics = @"";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_AssignmentExpression_Reports_Undefined() {
            var text = @"[x] = 10;";

            var diagnostics = @"
                undefined symbol 'x'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_AssignmentExpression_Reports_Readonly() {
            var text = @"
                let x = 10;
                x [=] 0;
            ";

            var diagnostics = @"
                assignment of read-only variable 'x'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_AssignmentExpression_Reports_CannotConvert() {
            var text = @"
                auto x = 10;
                x = [false];
            ";

            var diagnostics = @"
                cannot convert from type 'bool' to 'int'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_UnaryOperator_Reports_Undefined() {
            var text = @"[+]true;";

            var diagnostics = @"
                operator '+' is not defined for type 'bool'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        [Fact]
        public void Evaluator_BinaryOperator_Reports_Undefined() {
            var text = @"10[+]true;";

            var diagnostics = @"
                operator '+' is not defined for types 'int' and 'bool'
            ";

            AssertDiagnostics(text, diagnostics);
        }

        private void AssertValue(string text, object expectedValue) {
            var tree = SyntaxTree.Parse(text);
            var compilation = new Compilation(tree);
            var variables = new Dictionary<VariableSymbol, object>();
            var result = compilation.Evaluate(variables);

            Assert.Empty(result.diagnostics.ToArray());
            Assert.Equal(expectedValue, result.value);
        }

        private void AssertDiagnostics(string text, string diagnosticText) {
            var annotatedText = AnnotatedText.Parse(text);
            var syntaxTree = SyntaxTree.Parse(annotatedText.text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

            var expectedDiagnostics = AnnotatedText.UnindentLines(diagnosticText);

            if (annotatedText.spans.Length != expectedDiagnostics.Length)
                throw new Exception("must mark as many spans as there are diagnostics");

            Assert.Equal(expectedDiagnostics.Length, result.diagnostics.count);

            for (int i = 0; i < expectedDiagnostics.Length; i++) {
                var diagnostic = result.diagnostics.Pop();

                var expectedMessage = expectedDiagnostics[i];
                var actualMessage = diagnostic.msg;
                Assert.Equal(expectedMessage, actualMessage);

                var expectedSpan = annotatedText.spans[i];
                var actualSpan = diagnostic.span;
                Assert.Equal(expectedSpan.start, actualSpan.start);
                Assert.Equal(expectedSpan.end, actualSpan.end);
                Assert.Equal(expectedSpan.length, actualSpan.length);
            }
        }
    }
}
