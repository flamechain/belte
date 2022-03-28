using System.Collections.Generic;
using Buckle.CodeAnalysis.Syntax;

namespace Buckle.CodeAnalysis.Binding {

    internal sealed class Binder {
        public DiagnosticQueue diagnostics;
        private BoundScope scope_;

        public Binder(BoundScope parent) {
            diagnostics = new DiagnosticQueue();
            scope_ = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope prev, CompilationUnit expr) {
            var parentScope = CreateParentScopes(prev);
            var binder = new Binder(parentScope);
            var expression = binder.BindExpression(expr.expr);
            var variables = binder.scope_.GetDeclaredVariables();

            if (prev != null)
                binder.diagnostics.diagnostics_.InsertRange(0, prev.diagnostics.diagnostics_);

            return new BoundGlobalScope(prev, binder.diagnostics, variables, expression);
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope prev) {
            var stack = new Stack<BoundGlobalScope>();
            // make all scopes cascade in order of repl statements

            while (prev != null) {
                stack.Push(prev);
                prev = prev.previous;
            }

            BoundScope parent = null;

            while (stack.Count > 0) {
                prev = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var variable in prev.variables)
                    scope.TryDeclare(variable);

                parent = scope;
            }

            return parent;
        }

        public BoundExpression BindExpression(Expression expr) {
            switch (expr.type) {
                case SyntaxType.LITERAL_EXPR: return BindLiteralExpression((LiteralExpression)expr);
                case SyntaxType.UNARY_EXPR: return BindUnaryExpression((UnaryExpression)expr);
                case SyntaxType.BINARY_EXPR: return BindBinaryExpression((BinaryExpression)expr);
                case SyntaxType.PAREN_EXPR: return BindParenExpression((ParenExpression)expr);
                case SyntaxType.NAME_EXPR: return BindNameExpression((NameExpression)expr);
                case SyntaxType.ASSIGN_EXPR: return BindAssignmentExpression((AssignmentExpression)expr);
                default:
                    diagnostics.Push(DiagnosticType.fatal, $"unexpected syntax {expr.type}");
                    return null;
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpression expr) {
            var value = expr.value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpression expr) {
            var boundoperand = BindExpression(expr.operand);
            var boundop = BoundUnaryOperator.Bind(expr.op.type, boundoperand.ltype);

            if (boundop == null) {
                diagnostics.Push(Error.InvalidUnaryOperatorUse(expr.op.span, expr.op.text, boundoperand.ltype));
                return boundoperand;
            }

            return new BoundUnaryExpression(boundop, boundoperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression expr) {
            var boundleft = BindExpression(expr.left);
            var boundright = BindExpression(expr.right);
            if (boundleft == null || boundright == null) return boundleft;
            var boundop = BoundBinaryOperator.Bind(expr.op.type, boundleft.ltype, boundright.ltype);

            if (boundop == null) {
                diagnostics.Push(
                    Error.InvalidBinaryOperatorUse(expr.op.span, expr.op.text, boundleft.ltype, boundright.ltype));
                return boundleft;
            }

            return new BoundBinaryExpression(boundleft, boundop, boundright);
        }

        private BoundExpression BindParenExpression(ParenExpression expr) {
            return BindExpression(expr.expr);
        }

        private BoundExpression BindNameExpression(NameExpression expr) {
            string name = expr.id.text;

            if (scope_.TryLookup(name, out var variable)) {
                return new BoundVariableExpression(variable);
            }

            diagnostics.Push(Error.UndefinedName(expr.id.span, name));
            return new BoundLiteralExpression(0);
        }

        private BoundExpression BindAssignmentExpression(AssignmentExpression expr) {
            var name = expr.id.text;
            var boundexpr = BindExpression(expr.expr);
            var variable = new VariableSymbol(name, boundexpr.ltype);

            if (!scope_.TryDeclare(variable)) {
                diagnostics.Push(Error.AlreadyDeclared(expr.id.span, name));
            }

            return new BoundAssignmentExpression(variable, boundexpr);
        }
    }
}
