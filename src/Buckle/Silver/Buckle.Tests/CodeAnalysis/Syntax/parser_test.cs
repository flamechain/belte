using System.Collections.Generic;
using Buckle.CodeAnalysis.Syntax;
using Xunit;

namespace Buckle.Tests.CodeAnalysis.Syntax {
    public class ParserTests {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        internal void Parser_BinaryExpression_HonorsPrecedences(SyntaxType op1, SyntaxType op2) {
            var op1Precedence = SyntaxFacts.GetBinaryPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryPrecedence(op2);
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            var text = $"a {op1Text} b {op2Text} c";
            var expression = SyntaxTree.Parse(text).root;

            if (op1Precedence >= op2Precedence) {
                using (var e = new AssertingEnumerator(expression))
                {
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "a");
                    e.AssertToken(op1, op1Text);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "b");
                    e.AssertToken(op2, op2Text);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "c");
                }
            } else {
                using (var e = new AssertingEnumerator(expression)) {
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "a");
                    e.AssertToken(op1, op1Text);
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "b");
                    e.AssertToken(op2, op2Text);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "c");
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetUnaryOperatorPairsData))]
        internal void Parser_UnaryExpression_HonorsPrecedences(SyntaxType unaryKind, SyntaxType binaryKind) {
            var unaryPrecedence = SyntaxFacts.GetUnaryPrecedence(unaryKind);
            var binaryPrecedence = SyntaxFacts.GetBinaryPrecedence(binaryKind);
            var unaryText = SyntaxFacts.GetText(unaryKind);
            var binaryText = SyntaxFacts.GetText(binaryKind);
            var text = $"{unaryText} a {binaryText} b";
            var expression = SyntaxTree.Parse(text).root;

            if (unaryPrecedence >= binaryPrecedence) {
                using (var e = new AssertingEnumerator(expression)) {
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.UNARY_EXPR);
                    e.AssertToken(unaryKind, unaryText);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "a");
                    e.AssertToken(binaryKind, binaryText);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "b");
                }
            } else {
                using (var e = new AssertingEnumerator(expression)) {
                    e.AssertNode(SyntaxType.UNARY_EXPR);
                    e.AssertToken(unaryKind, unaryText);
                    e.AssertNode(SyntaxType.BINARY_EXPR);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "a");
                    e.AssertToken(binaryKind, binaryText);
                    e.AssertNode(SyntaxType.NAME_EXPR);
                    e.AssertToken(SyntaxType.IDENTIFIER, "b");
                }
            }
        }

        public static IEnumerable<object[]> GetBinaryOperatorPairsData() {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorKinds()) {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds()) {
                    yield return new object[] { op1, op2 };
                }
            }
        }

        public static IEnumerable<object[]> GetUnaryOperatorPairsData() {
            foreach (var unary in SyntaxFacts.GetUnaryOperatorKinds()) {
                foreach (var binary in SyntaxFacts.GetBinaryOperatorKinds()) {
                    yield return new object[] { unary, binary };
                }
            }
        }
    }
}