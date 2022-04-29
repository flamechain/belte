
namespace Buckle.CodeAnalysis.Syntax;

internal abstract class Expression : Node {
    protected Expression(SyntaxTree syntaxTree) : base(syntaxTree) { }
}

internal sealed partial class LiteralExpression : Expression {
    public Token token { get; }
    public object value { get; }
    public override SyntaxType type => SyntaxType.LITERAL_EXPRESSION;

    public LiteralExpression(SyntaxTree syntaxTree, Token token_, object value_) : base(syntaxTree) {
        token = token_;
        value = value_;
    }

    public LiteralExpression(SyntaxTree syntaxTree, Token token_) : this(syntaxTree, token_, token_.value) { }
}

internal sealed partial class BinaryExpression : Expression {
    public Expression left { get; }
    public Token op { get; }
    public Expression right { get; }
    public override SyntaxType type => SyntaxType.BINARY_EXPRESSION;

    public BinaryExpression(SyntaxTree syntaxTree, Expression left_, Token op_, Expression right_)
        : base(syntaxTree) {
        left = left_;
        op = op_;
        right = right_;
    }
}

internal sealed partial class ParenthesisExpression : Expression {
    public Token openParenthesis { get; }
    public Expression expression { get; }
    public Token closeParenthesis { get; }
    public override SyntaxType type => SyntaxType.PARENTHESIZED_EXPRESSION;

    public ParenthesisExpression(
        SyntaxTree syntaxTree, Token openParenthesis_, Expression expression_, Token closeParenthesis_)
        : base(syntaxTree) {
        openParenthesis = openParenthesis_;
        expression = expression_;
        closeParenthesis = closeParenthesis_;
    }
}

internal sealed partial class UnaryExpression : Expression {
    public Token op { get; }
    public Expression operand { get; }
    public override SyntaxType type => SyntaxType.UNARY_EXPRESSION;

    public UnaryExpression(SyntaxTree syntaxTree, Token op_, Expression operand_) : base(syntaxTree) {
        op = op_;
        operand = operand_;
    }
}

internal sealed partial class NameExpression : Expression {
    public Token identifier { get; }
    public override SyntaxType type => SyntaxType.NAME_EXPRESSION;

    public NameExpression(SyntaxTree syntaxTree, Token identifier_) : base(syntaxTree) {
        identifier = identifier_;
    }
}

internal sealed partial class AssignmentExpression : Expression {
    public Token identifier { get; }
    public Token assignmentToken { get; }
    public Expression expression { get; }
    public override SyntaxType type => SyntaxType.ASSIGN_EXPRESSION;

    public AssignmentExpression(SyntaxTree syntaxTree, Token identifier_, Token assignmentToken_, Expression expression_)
        : base(syntaxTree) {
        identifier = identifier_;
        assignmentToken = assignmentToken_;
        expression = expression_;
    }
}

internal sealed partial class EmptyExpression : Expression {
    public override SyntaxType type => SyntaxType.EMPTY_EXPRESSION;

    public EmptyExpression(SyntaxTree syntaxTree) : base(syntaxTree) { }
}

internal sealed partial class CallExpression : Expression {
    public Token identifier { get; }
    public Token openParenthesis { get; }
    public SeparatedSyntaxList<Expression> arguments { get; }
    public Token closeParenthesis { get; }
    public override SyntaxType type => SyntaxType.CALL_EXPRESSION;

    public CallExpression(
        SyntaxTree syntaxTree, Token identifier_, Token openParenthesis_,
        SeparatedSyntaxList<Expression> arguments_, Token closeParenthesis_)
        : base(syntaxTree) {
        identifier = identifier_;
        openParenthesis = openParenthesis_;
        arguments = arguments_;
        closeParenthesis = closeParenthesis_;
    }
}