namespace McpService.Api.Contracts;

public sealed record CountBasicRequest(
    string UserStory,
    int? Det = null,
    int? Ftr = null);
