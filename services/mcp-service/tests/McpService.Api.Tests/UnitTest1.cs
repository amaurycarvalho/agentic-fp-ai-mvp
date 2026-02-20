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
}
