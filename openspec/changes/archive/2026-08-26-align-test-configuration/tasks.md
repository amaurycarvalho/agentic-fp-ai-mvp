# Tasks: Align Test Configuration

## 1. Ajustar projetos de teste

- [x] 1.1 Remover `<OutputType>Exe</OutputType>` e `<UseMicrosoftTestingPlatformRunner>` do csproj de teste do `agent-service`
- [x] 1.2 Remover `<OutputType>Exe</OutputType>` e `<UseMicrosoftTestingPlatformRunner>` do csproj de teste do `mcp-service`
- [x] 1.3 Remover `<OutputType>Exe</OutputType>` e `<UseMicrosoftTestingPlatformRunner>` do csproj de teste do `rag-service`
- [x] 1.4 Atualizar pacotes dos 3 csproj: `xunit.v3` 3.2.2, `xunit.runner.visualstudio` 3.1.5, `Microsoft.NET.Test.Sdk` 17.10.0, `coverlet.collector` 6.0.0 (mantendo `Microsoft.AspNetCore.Mvc.Testing` 8.0.30)

## 2. Configuração do Stryker

- [x] 2.1 Criar `stryker-config.json` no projeto de teste do `agent-service` (`project: AgentService.Api.csproj`)
- [x] 2.2 Criar `stryker-config.json` no projeto de teste do `mcp-service` (`project: McpService.Api.csproj`)
- [x] 2.3 Criar `stryker-config.json` no projeto de teste do `rag-service` (`project: RagService.Api.csproj`)

## 3. Validação

- [x] 3.1 `dotnet restore` e `make lint` com sucesso
- [x] 3.2 `make test` (33 testes) com sucesso
- [x] 3.3 `make coverage-check` >= 90% com sucesso
