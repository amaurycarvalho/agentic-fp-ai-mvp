# Align Test Configuration

## Why

O CI falhou no job `quality-gate` com o erro:
`Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later`.
O runner image do GitHub tem o SDK .NET 10 como padrão. Neste projeto, os projetos de
teste foram configurados com `xunit.v3` 4.0.0 (MTP 2.3.3) + `OutputType=Exe` +
`UseMicrosoftTestingPlatformRunner=true`, o que ativa o MTP como runner do
`dotnet test` e dispara esse hard error no SDK .NET 10. O projeto de referência
(`agentic-erp-platform-mvp`) usa `xunit.v3` 3.2.2 (MTP 1.9.1) **sem** essas props,
mantendo `dotnet test` via VSTest e acionando o MTP apenas pelo Stryker via
`stryker-config.json`.

## What Changes

- Remover `OutputType=Exe` e `UseMicrosoftTestingPlatformRunner` dos projetos de teste
  (`agent-service`, `mcp-service`, `rag-service`).
- Rebaixar `xunit.v3` de `4.0.0` para `3.2.2` (volta ao MTP v1, sem o hard error de SDK).
- Alinhar pacotes de teste ao projeto de referência: `xunit.runner.visualstudio` 3.1.5,
  `Microsoft.NET.Test.Sdk` 17.10.0, `coverlet.collector` 6.0.0.
- Criar `stryker-config.json` em cada projeto de teste com
  `"test-runner": "mtp"`, `"coverage-analysis": "off"`, `"project"` apontando para o
  csproj sob teste e thresholds `{high: 80, low: 70, break: 60}`.

## Capabilities

### New Capabilities

_(nenhuma — requisito adicionado a uma capacidade existente)_

### Modified Capabilities

- `build-release-tooling`: adiciona requisito de configuração do runner de testes —
  projetos de teste SHALL NOT usar `OutputType=Exe`/`UseMicrosoftTestingPlatformRunner`
  (mantendo VSTest no `dotnet test`), e o Stryker SHALL ser configurado via
  `stryker-config.json` com `test-runner: mtp`.

## Impact

- Código: 3 csproj de teste (pacotes + props) e 3 novos `stryker-config.json`.
- CI: corrige o erro ".NET 10 SDK" no `quality-gate`.
- Processo: alinha a configuração de testes ao padrão comprovado do projeto de referência.
