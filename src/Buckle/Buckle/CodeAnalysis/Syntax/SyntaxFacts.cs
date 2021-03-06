using System;
using System.Collections.Generic;

namespace Buckle.CodeAnalysis.Syntax;

internal static class SyntaxFacts {

    internal static int GetBinaryPrecedence(this SyntaxType type) {
        switch (type) {
            case SyntaxType.ASTERISK_ASTERISK_TOKEN:
                return 11;
            case SyntaxType.ASTERISK_TOKEN:
            case SyntaxType.SLASH_TOKEN:
                return 10;
            case SyntaxType.PLUS_TOKEN:
            case SyntaxType.MINUS_TOKEN:
                return 9;
            case SyntaxType.LESS_THAN_LESS_THAN_TOKEN:
            case SyntaxType.GREATER_THAN_GREATER_THAN_TOKEN:
                return 8;
            case SyntaxType.IS_KEYWORD:
            case SyntaxType.ISNT_KEYWORD:
            case SyntaxType.LESS_THAN_TOKEN:
            case SyntaxType.GREATER_THAN_TOKEN:
            case SyntaxType.LESS_THAN_EQUALS_TOKEN:
            case SyntaxType.GREATER_THAN_EQUALS_TOKEN:
                return 7;
            case SyntaxType.EQUALS_EQUALS_TOKEN:
            case SyntaxType.EXCLAMATION_EQUALS_TOKEN:
                return 6;
            case SyntaxType.AMPERSAND_TOKEN:
                return 5;
            case SyntaxType.CARET_TOKEN:
                return 4;
            case SyntaxType.PIPE_TOKEN:
                return 3;
            case SyntaxType.AMPERSAND_AMPERSAND_TOKEN:
                return 2;
            case SyntaxType.PIPE_PIPE_TOKEN:
                return 1;
            default:
                return 0;
        }
    }

    internal static int GetPrimaryPrecedence(this SyntaxType type) {
        switch (type) {
            case SyntaxType.OPEN_BRACKET_TOKEN:
            case SyntaxType.OPEN_PAREN_TOKEN:
                return 13;
            default:
                return 0;
        }
    }

    internal static int GetUnaryPrecedence(this SyntaxType type) {
        switch (type) {
            case SyntaxType.PLUS_PLUS_TOKEN:
            case SyntaxType.MINUS_MINUS_TOKEN:
            case SyntaxType.PLUS_TOKEN:
            case SyntaxType.MINUS_TOKEN:
            case SyntaxType.EXCLAMATION_TOKEN:
            case SyntaxType.TILDE_TOKEN:
                return 12;
            default:
                return 0;
        }
    }

    internal static SyntaxType GetKeywordType(string text) {
        switch (text) {
            case "true":
                return SyntaxType.TRUE_KEYWORD;
            case "false":
                return SyntaxType.FALSE_KEYWORD;
            case "null":
                return SyntaxType.NULL_KEYWORD;
            case "var":
                return SyntaxType.VAR_KEYWORD;
            case "const":
                return SyntaxType.CONST_KEYWORD;
            case "ref":
                return SyntaxType.REF_KEYWORD;
            case "if":
                return SyntaxType.IF_KEYWORD;
            case "else":
                return SyntaxType.ELSE_KEYWORD;
            case "while":
                return SyntaxType.WHILE_KEYWORD;
            case "for":
                return SyntaxType.FOR_KEYWORD;
            case "do":
                return SyntaxType.DO_KEYWORD;
            case "break":
                return SyntaxType.BREAK_KEYWORD;
            case "continue":
                return SyntaxType.CONTINUE_KEYWORD;
            case "try":
                return SyntaxType.TRY_KEYWORD;
            case "catch":
                return SyntaxType.CATCH_KEYWORD;
            case "finally":
                return SyntaxType.FINALLY_KEYWORD;
            case "return":
                return SyntaxType.RETURN_KEYWORD;
            case "is":
                return SyntaxType.IS_KEYWORD;
            case "isnt":
                return SyntaxType.ISNT_KEYWORD;
            default:
                return SyntaxType.IDENTIFIER_TOKEN;
        }
    }

    internal static string GetText(SyntaxType type) {
        switch (type) {
            case SyntaxType.COMMA_TOKEN:
                return ",";
            case SyntaxType.PLUS_TOKEN:
                return "+";
            case SyntaxType.MINUS_TOKEN:
                return "-";
            case SyntaxType.ASTERISK_TOKEN:
                return "*";
            case SyntaxType.SLASH_TOKEN:
                return "/";
            case SyntaxType.OPEN_PAREN_TOKEN:
                return "(";
            case SyntaxType.CLOSE_PAREN_TOKEN:
                return ")";
            case SyntaxType.EQUALS_TOKEN:
                return "=";
            case SyntaxType.TILDE_TOKEN:
                return "~";
            case SyntaxType.CARET_TOKEN:
                return "^";
            case SyntaxType.AMPERSAND_TOKEN:
                return "&";
            case SyntaxType.PIPE_TOKEN:
                return "|";
            case SyntaxType.LESS_THAN_LESS_THAN_TOKEN:
                return "<<";
            case SyntaxType.GREATER_THAN_GREATER_THAN_TOKEN:
                return ">>";
            case SyntaxType.EXCLAMATION_TOKEN:
                return "!";
            case SyntaxType.AMPERSAND_AMPERSAND_TOKEN:
                return "&&";
            case SyntaxType.PIPE_PIPE_TOKEN:
                return "||";
            case SyntaxType.ASTERISK_ASTERISK_TOKEN:
                return "**";
            case SyntaxType.PLUS_PLUS_TOKEN:
                return "++";
            case SyntaxType.MINUS_MINUS_TOKEN:
                return "--";
            case SyntaxType.OPEN_BRACE_TOKEN:
                return "{";
            case SyntaxType.CLOSE_BRACE_TOKEN:
                return "}";
            case SyntaxType.OPEN_BRACKET_TOKEN:
                return "[";
            case SyntaxType.CLOSE_BRACKET_TOKEN:
                return "]";
            case SyntaxType.SEMICOLON_TOKEN:
                return ";";
            case SyntaxType.EQUALS_EQUALS_TOKEN:
                return "==";
            case SyntaxType.EXCLAMATION_EQUALS_TOKEN:
                return "!=";
            case SyntaxType.LESS_THAN_TOKEN:
                return "<";
            case SyntaxType.GREATER_THAN_TOKEN:
                return ">";
            case SyntaxType.LESS_THAN_EQUALS_TOKEN:
                return "<=";
            case SyntaxType.GREATER_THAN_EQUALS_TOKEN:
                return ">=";
            case SyntaxType.AMPERSAND_EQUALS_TOKEN:
                return "&=";
            case SyntaxType.PIPE_EQUALS_TOKEN:
                return "|=";
            case SyntaxType.CARET_EQUALS_TOKEN:
                return "^=";
            case SyntaxType.PLUS_EQUALS_TOKEN:
                return "+=";
            case SyntaxType.MINUS_EQUALS_TOKEN:
                return "-=";
            case SyntaxType.SLASH_EQUALS_TOKEN:
                return "/=";
            case SyntaxType.ASTERISK_EQUALS_TOKEN:
                return "*=";
            case SyntaxType.ASTERISK_ASTERISK_EQUALS_TOKEN:
                return "**=";
            case SyntaxType.GREATER_THAN_GREATER_THAN_EQUALS_TOKEN:
                return ">>=";
            case SyntaxType.LESS_THAN_LESS_THAN_EQUALS_TOKEN:
                return "<<=";
            case SyntaxType.TRUE_KEYWORD:
                return "true";
            case SyntaxType.FALSE_KEYWORD:
                return "false";
            case SyntaxType.NULL_KEYWORD:
                return "null";
            case SyntaxType.VAR_KEYWORD:
                return "var";
            case SyntaxType.CONST_KEYWORD:
                return "const";
            case SyntaxType.REF_KEYWORD:
                return "ref";
            case SyntaxType.IF_KEYWORD:
                return "if";
            case SyntaxType.ELSE_KEYWORD:
                return "else";
            case SyntaxType.WHILE_KEYWORD:
                return "while";
            case SyntaxType.FOR_KEYWORD:
                return "for";
            case SyntaxType.DO_KEYWORD:
                return "do";
            case SyntaxType.BREAK_KEYWORD:
                return "break";
            case SyntaxType.CONTINUE_KEYWORD:
                return "continue";
            case SyntaxType.TRY_KEYWORD:
                return "try";
            case SyntaxType.CATCH_KEYWORD:
                return "catch";
            case SyntaxType.FINALLY_KEYWORD:
                return "finally";
            case SyntaxType.RETURN_KEYWORD:
                return "return";
            case SyntaxType.IS_KEYWORD:
                return "is";
            case SyntaxType.ISNT_KEYWORD:
                return "isnt";
            default:
                return null;
        }
    }

    internal static SyntaxType GetBinaryOperatorOfAssignmentOperator(SyntaxType type) {
        switch (type) {
            case SyntaxType.PLUS_EQUALS_TOKEN:
                return SyntaxType.PLUS_TOKEN;
            case SyntaxType.MINUS_EQUALS_TOKEN:
                return SyntaxType.MINUS_TOKEN;
            case SyntaxType.ASTERISK_EQUALS_TOKEN:
                return SyntaxType.ASTERISK_TOKEN;
            case SyntaxType.SLASH_EQUALS_TOKEN:
                return SyntaxType.SLASH_TOKEN;
            case SyntaxType.AMPERSAND_EQUALS_TOKEN:
                return SyntaxType.AMPERSAND_TOKEN;
            case SyntaxType.PIPE_EQUALS_TOKEN:
                return SyntaxType.PIPE_TOKEN;
            case SyntaxType.CARET_EQUALS_TOKEN:
                return SyntaxType.CARET_TOKEN;
            case SyntaxType.ASTERISK_ASTERISK_EQUALS_TOKEN:
                return SyntaxType.ASTERISK_ASTERISK_TOKEN;
            case SyntaxType.LESS_THAN_LESS_THAN_EQUALS_TOKEN:
                return SyntaxType.LESS_THAN_LESS_THAN_TOKEN;
            case SyntaxType.GREATER_THAN_GREATER_THAN_EQUALS_TOKEN:
                return SyntaxType.GREATER_THAN_GREATER_THAN_TOKEN;
            default:
                throw new Exception($"GetBinaryOperatorOfAssignmentOperator: unexpected syntax '{type}'");
        }
    }

    internal static IEnumerable<SyntaxType> GetUnaryOperatorTypes() {
        var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
        foreach (var type in types) {
            if (GetUnaryPrecedence(type) > 0)
                yield return type;
        }
    }

    internal static IEnumerable<SyntaxType> GetBinaryOperatorTypes() {
        var types = (SyntaxType[])Enum.GetValues(typeof(SyntaxType));
        foreach (var type in types) {
            if (GetBinaryPrecedence(type) > 0)
                yield return type;
        }
    }

    internal static bool IsKeyword(this SyntaxType type) {
        return type.ToString().EndsWith("KEYWORD");
    }

    internal static bool IsToken(this SyntaxType type) {
        return !type.IsTrivia() && (type.IsKeyword() || type.ToString().EndsWith("TOKEN"));
    }

    internal static bool IsTrivia(this SyntaxType type) {
        return type.ToString().EndsWith("TRIVIA");
    }

    internal static bool IsComment(this SyntaxType type) {
        return type == SyntaxType.SINGLELINE_COMMENT_TRIVIA || type == SyntaxType.MULTILINE_COMMENT_TRIVIA;
    }
}
