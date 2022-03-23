using System;
using System.Collections.Generic;
using Buckle.CodeAnalysis.Syntax;

namespace Buckle.CodeAnalysis.Binding {

    internal class Binder {
        public List<Diagnostic> diagnostics;

        public Binder() {
            diagnostics = new List<Diagnostic>();
        }

        public BoundExpression BindExpression(Expression expr) {
            switch (expr.type) {
                case SyntaxType.LITERAL_EXPR: return BindLiteralExpression((LiteralExpression)expr);
                case SyntaxType.UNARY_EXPR: return BindUnaryExpression((UnaryExpression)expr);
                case SyntaxType.BINARY_EXPR: return BindBinaryExpression((BinaryExpression)expr);
                default:
                    diagnostics.Add(new Diagnostic(DiagnosticType.fatal, $"unexpected syntax '{expr.type}'"));
                    return null;
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpression expr) {
            var value = expr.token.value as int? ?? 0; // as nullable int, if null instead 0
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpression expr) {
            var boundoperand = BindExpression(expr.operand);
            var boundop = BindUnaryOperatorType(expr.op.type, boundoperand.ltype);

            if (boundop == null) {
                diagnostics.Add(new Diagnostic(DiagnosticType.error, $"unary operator '{boundoperand.ltype}'"));
                return boundoperand;
            }

            return new BoundUnaryExpression(boundop.Value, boundoperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression expr) {
            var boundleft = BindExpression(expr.left);
            var boundright = BindExpression(expr.right);
            var boundop = BindBinaryOperatorType(expr.op.type, boundleft.ltype, boundright.ltype);

            if (boundop == null) {
                diagnostics.Add(new Diagnostic(DiagnosticType.error,
                    $"binary operator '{expr.op.text}' is not defined for types '{boundleft.ltype}' and '{boundright.ltype}'")
                );
                return boundleft;
            }

            return new BoundBinaryExpression(boundleft, boundop.Value, boundright);
        }

        private BoundUnaryOperatorType? BindUnaryOperatorType(SyntaxType type, Type ltype) {
            if (ltype != typeof(int)) return null;

            switch (type) {
                case SyntaxType.PLUS: return BoundUnaryOperatorType.Identity;
                case SyntaxType.MINUS: return BoundUnaryOperatorType.Negation;
                default:
                    diagnostics.Add(new Diagnostic(DiagnosticType.fatal, $"unexpected unary operator '{type}'"));
                    return BoundUnaryOperatorType.Invalid;
            }
        }

        private BoundBinaryOperatorType? BindBinaryOperatorType(SyntaxType type, Type lltype, Type rltype) {
            if (lltype != typeof(int) || rltype != typeof(int)) return null;

            switch (type) {
                case SyntaxType.PLUS: return BoundBinaryOperatorType.Add;
                case SyntaxType.MINUS: return BoundBinaryOperatorType.Subtract;
                case SyntaxType.ASTERISK: return BoundBinaryOperatorType.Multiply;
                case SyntaxType.SOLIDUS: return BoundBinaryOperatorType.Divide;
                default:
                    diagnostics.Add(new Diagnostic(DiagnosticType.fatal, $"unexpected unary operator '{type}'"));
                    return BoundBinaryOperatorType.Invalid;
            }
        }
    }
}