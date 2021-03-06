
namespace Buckle.Diagnostics;

internal enum DiagnosticCode : int {
    Unknown = 0,

    WRN_AlwaysValue = 1,

    ERR_InvalidReference = 3,
    ERR_InvalidType = 4,
    ERR_BadCharacter = 5,
    ERR_UnexpectedToken = 6,
    ERR_CannotConvertImplicitly = 7,
    ERR_InvalidUnaryOperatorUse = 8,
    ERR_RequiredTypeNotFound = 9,
    ERR_RequiredTypeAmbiguous = 10,
    ERR_InvalidBinaryOperatorUse = 11,
    ERR_GlobalStatementsInMultipleFiles = 12,
    ERR_ParameterAlreadyDeclared = 13,
    ERR_InvalidMain = 14,
    ERR_RequiredMethodNotFound = 15,
    ERR_MainAndGlobals = 16,
    ERR_UndefinedName = 17,
    ERR_FunctionAlreadyDeclared = 18,
    ERR_NotAllPathsReturn = 19,
    ERR_CannotConvert = 20,
    ERR_AlreadyDeclared = 21,
    ERR_ConstantAssignment = 22,
    ERR_AmbiguousElse = 23,
    ERR_NoValue = 24,
    ERR_CannotApplyIndexing = 25,
    WRN_UnreachableCode = 26,
    ERR_UnterminatedString = 27,
    ERR_UndefinedFunction = 28,
    ERR_IncorrectArgumentCount = 29,
    ERR_UnexpectedType = 30,
    ERR_InvalidArgumentType = 31,
    ERR_CannotCallNonFunction = 32,
    ERR_InvalidExpressionStatement = 33,
    ERR_UnknownType = 34,
    ERR_InvalidBreakOrContinue = 35,
    ERR_ReturnOutsideFunction = 36,
    ERR_UnexpectedReturnValue = 37,
    ERR_MissingReturnValue = 38,
    ERR_NotAVariable = 39,
    ERR_NoInitOnImplicit = 40,
    ERR_UnterminatedComment = 41,
    ERR_NullAssignOnImplicit = 42,
    ERR_EmptyInitializerListOnImplicit = 43,
    ERR_ImpliedDimensions = 44,
    ERR_CannotUseImplicit = 45,
    ERR_NoCatchOrFinally = 46,
    ERR_ExpectedMethodName = 47,
    ERR_ReferenceNoInitialization = 48,
    ERR_ReferenceWrongInitialization = 49,
    ERR_WrongInitializationReference = 50,
    ERR_UnknownAttribute = 51,
    ERR_NullAssignNotNull = 52,
    ERR_InconsistentReturnTypes = 53,
    ERR_MissingReturnStatement = 54,
    ERR_VoidVariable = 55,
    ERR_ExpectedToken = 56,

    // carving out >=9000 for unsupported errors
    UNS_GlobalReturnValue = 9000,
    UNS_Assembling = 9001,
    UNS_Linking = 9002,
    UNS_IndependentCompilation = 9003,
    UNS_IsWithoutNull = 9004,
}
