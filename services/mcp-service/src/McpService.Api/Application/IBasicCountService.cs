using McpService.Api.Contracts;

namespace McpService.Api.Application;

public interface IBasicCountService
{
    CountBasicResponse Analyze(CountBasicRequest request);
}
