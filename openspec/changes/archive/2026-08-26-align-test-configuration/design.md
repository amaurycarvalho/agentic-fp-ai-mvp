# Design: Align Test Configuration

## Context

O CI falhou com o hard error do MTP 2.3.3 no SDK .NET 10: `Testing with VSTest
target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and
later`. Causa: os projetos de teste usam `xunit.v3` 4.0.0 (mtp-v2 → MTP 2.3.3) com
`OutputType=Exe` + `UseMicrosoftTestingPlatformRunner=true`, o que ativa o MTP como
runner do `dotnet test`. O projeto de referência (`agentic-erp-platform-mvp`) usa
`xunit.v3` 3.2.2 (mtp-v1 → MTP 1.9.1) sem essas props — `dotnet test` segue VSTest e
o Stryker usa MTP via `stryker-config.json`.

## Goals / Non-Goals

**Goals:**
- `dotnet test` (VSTest) funcional em qualquer SDK, incluindo .NET 10 (fix do CI).
- Stryker com MTP via `stryker-config.json` (padrão do projeto de referência).

**Non-Goals:**
- Alterar o Makefile `test-sln` (formato VSTest já aprovado).
- Alterar os testes em si (mantém `*.Api.Tests` com `WebApplicationFactory`).

## Decisions

### D1 — Alinhar pacotes ao projeto de referência
`xunit.v3` 3.2.2, `xunit.runner.visualstudio` 3.1.5, `Microsoft.NET.Test.Sdk` 17.10.0,
`coverlet.collector` 6.0.0. Manter `Microsoft.AspNetCore.Mvc.Testing` 8.0.30 (usado
pelos testes de integração).
- **Alternativa**: manter versões novas (4.0.0/4.0.0/18.9.0/10.0.1) — rejeitada:
  o xunit.v3 4.0.0 (MTP 2.3.3) é justamente o gatilho do erro no SDK .NET 10.

### D2 — Remover props de MTP dos csproj
Remover `<OutputType>Exe</OutputType>` e `<UseMicrosoftTestingPlatformRunner>` dos
3 projetos de teste. O MTP (v1, via xunit.v3 3.2.2) permanece disponível para o
Stryker mas fica dormante no build do `dotnet test`.

### D3 — stryker-config.json por projeto
`project` aponta para o csproj sob teste (`*Service.Api.csproj`), `test-runner: mtp`,
`coverage-analysis: off`, thresholds `{high:80, low:70, break:60}`, reporters
`html/json/progress` — espelhando o projeto de referência.

## Risks / Trade-offs

- **[Versões "outdated" no `make security`]** → `--outdated` é informativo e não
  falha o gate; `xunit.v3` 3.2.2 não é deprecated.
- **[Stryker MTP depende do comportamento do projeto de referência]** → Mesma
  combinação comprovada (xunit.v3 3.2.2 + MTP 1.9.1 + stryker-config).

## Migration Plan

1. Ajustar os 3 csproj de teste (props + pacotes).
2. Criar os 3 `stryker-config.json`.
3. Validar localmente `make lint`, `make test`, `make coverage-check`.

## Open Questions

- Nenhuma.
