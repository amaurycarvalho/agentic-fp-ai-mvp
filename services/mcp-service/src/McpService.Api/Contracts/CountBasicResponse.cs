namespace McpService.Api.Contracts;

public sealed record CountBasicResponse(
    IReadOnlyList<FunctionClassification> TransactionalFunctions,
    IReadOnlyList<FunctionClassification> DataFunctions,
    ComplexitySummary Summary,
    IReadOnlyList<string> AuditTrail);

public sealed record FunctionClassification(
    string Type,
    string Justification);

public sealed record ComplexitySummary(
    int Det,
    int Ftr,
    string Complexity,
    int TotalFunctionPoints);
