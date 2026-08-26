using McpService.Api.Application;
using McpService.Api.Contracts;

namespace McpService.Api.Tests;

public class BasicCountServiceTests
{
    private readonly BasicCountService _service = new();

    [Fact]
    public void Analyze_ShouldClassifyEiAndIlf_WhenStoryMentionsCreateAndPersistence()
    {
        var request = new CountBasicRequest("Como analista, preciso cadastrar cliente e armazenar dados no banco.");

        var result = _service.Analyze(request);

        Assert.Contains(result.TransactionalFunctions, x => x.Type == "EI");
        Assert.Contains(result.DataFunctions, x => x.Type == "ILF");
        Assert.NotEmpty(result.AuditTrail);
        Assert.True(result.Summary.TotalFunctionPoints > 0);
    }

    [Fact]
    public void Analyze_ShouldClassifyEqAndEif_WhenStoryMentionsExternalQuery()
    {
        var request = new CountBasicRequest("Quero consultar dados de sistema externo para visualização.");

        var result = _service.Analyze(request);

        Assert.Contains(result.TransactionalFunctions, x => x.Type == "EQ");
        Assert.Contains(result.DataFunctions, x => x.Type == "EIF");
    }

    [Fact]
    public void Analyze_ShouldClassifyEo_WhenStoryMentionsOutputVerb()
    {
        var request = new CountBasicRequest("Preciso gerar relatório mensal para envio.");

        var result = _service.Analyze(request);

        Assert.Contains(result.TransactionalFunctions, x => x.Type == "EO");
    }

    [Fact]
    public void Analyze_ShouldApplyFallbackClassifications_WhenNoKeywordsDetected()
    {
        var request = new CountBasicRequest("abc def");

        var result = _service.Analyze(request);

        Assert.Contains(result.TransactionalFunctions, x => x.Type == "EQ");
        Assert.Contains(result.DataFunctions, x => x.Type == "ILF");
        Assert.Contains(result.AuditTrail, line => line.Contains("fallback", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Analyze_ShouldUseDetAndFtrFromRequest_WhenProvided()
    {
        var request = new CountBasicRequest(
            "Quero cadastrar informações do cliente.",
            Det: 20,
            Ftr: 3);

        var result = _service.Analyze(request);

        Assert.Equal(20, result.Summary.Det);
        Assert.Equal(3, result.Summary.Ftr);
        Assert.Equal("High", result.Summary.Complexity);
    }

    [Fact]
    public void Analyze_ShouldInferDetFromUniqueTokens_WhenNotProvided()
    {
        var request = new CountBasicRequest("cadastrar cliente armazenar banco");

        var result = _service.Analyze(request);

        Assert.Equal(4, result.Summary.Det);
        Assert.Contains(result.AuditTrail, line => line.Contains("DET inferred", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Analyze_ShouldInferFtrFromDataFunctions_WhenNotProvided()
    {
        var request = new CountBasicRequest("cadastrar cliente e armazenar no banco, integrando sistema externo");

        var result = _service.Analyze(request);

        Assert.Equal(2, result.Summary.Ftr);
        Assert.Contains(result.AuditTrail, line => line.Contains("FTR inferred", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(1, 10, "Low")]
    [InlineData(1, 16, "Average")]
    [InlineData(2, 3, "Low")]
    [InlineData(2, 10, "Average")]
    [InlineData(2, 20, "High")]
    [InlineData(3, 3, "Average")]
    [InlineData(3, 10, "High")]
    public void Analyze_ShouldResolveComplexity_ByDetFtrMatrix(int ftr, int det, string expected)
    {
        var request = new CountBasicRequest("cadastrar cliente", Det: det, Ftr: ftr);

        var result = _service.Analyze(request);

        Assert.Equal(expected, result.Summary.Complexity);
    }

    [Theory]
    [InlineData(1, 10, 3)]
    [InlineData(2, 10, 4)]
    [InlineData(3, 10, 6)]
    public void Analyze_ShouldApplyEiWeights_ByComplexity(int ftr, int det, int expectedPoints)
    {
        var request = new CountBasicRequest("cadastrar cliente", Det: det, Ftr: ftr);

        var result = _service.Analyze(request);

        Assert.Equal(expectedPoints, result.Summary.TotalFunctionPoints);
    }

    [Theory]
    [InlineData(1, 10, 4)]
    [InlineData(2, 10, 5)]
    [InlineData(3, 10, 7)]
    public void Analyze_ShouldApplyEoWeights_ByComplexity(int ftr, int det, int expectedPoints)
    {
        var request = new CountBasicRequest("gerar relatório", Det: det, Ftr: ftr);

        var result = _service.Analyze(request);

        Assert.Equal(expectedPoints, result.Summary.TotalFunctionPoints);
    }

    [Theory]
    [InlineData(1, 10, 3)]
    [InlineData(2, 10, 4)]
    [InlineData(3, 10, 6)]
    public void Analyze_ShouldApplyEqWeights_ByComplexity(int ftr, int det, int expectedPoints)
    {
        var request = new CountBasicRequest("consultar cliente", Det: det, Ftr: ftr);

        var result = _service.Analyze(request);

        Assert.Equal(expectedPoints, result.Summary.TotalFunctionPoints);
    }

    [Fact]
    public void Analyze_ShouldSumPoints_WhenMultipleTransactionsIdentified()
    {
        var request = new CountBasicRequest(
            "cadastrar cliente, gerar relatório, consultar dados",
            Det: 10,
            Ftr: 3);

        var result = _service.Analyze(request);

        Assert.Equal(3, result.TransactionalFunctions.Count);
        Assert.Equal(19, result.Summary.TotalFunctionPoints);
    }

    [Fact]
    public void Analyze_ShouldExposeAuditTrail_WithComplexityLine()
    {
        var request = new CountBasicRequest("cadastrar cliente", Det: 10, Ftr: 2);

        var result = _service.Analyze(request);

        Assert.Contains(result.AuditTrail, line => line.Contains("Complexity resolved", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(result.AuditTrail, line => line.Contains("Total transactional function points", StringComparison.OrdinalIgnoreCase));
    }
}
