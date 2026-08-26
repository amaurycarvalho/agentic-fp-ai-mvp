# Design: xUnit Test Conventions

## Context

O quality gate roda `make lint` → `dotnet format --verify-no-changes`. Esse comando
falha quando qualquer analyzer produz aviso. No CI (SDK 8.0.424 + xunit.analyzers
mais recentes), o rule **xUnit1051** avisa que métodos async que aceitam
`CancellationToken` devem recebê-lo, para permitir cancelamento responsivo dos
testes. Os testes de integração recém-criados não passavam o token e quebraram o CI.

## Goals / Non-Goals

**Goals:**
- Registrar a convenção na spec `build-release-tooling` para guiar futuros testes.
- Evitar que novos testes reintroduzam o mesmo aviso.

**Non-Goals:**
- Alterar código (os testes já foram corrigidos).
- Introduzir novas ferramentas de lint/teste.

## Decisions

### D1 — Convenção como requisito de spec, não só comentário
A convenção entra como requisito com cenários verificáveis na capacidade
`build-release-tooling` (que já cobre o quality gate/lint), em vez de um comentário
em arquivo. Assim fica rastreável e auditável via OpenSpec.

### D2 — `TestContext.Current.CancellationToken` (xunit v3)
Como o projeto usa `xunit.v3.mtp-off`, a fonte de cancelamento recomendada é
`TestContext.Current.CancellationToken`, disponível no namespace `Xunit` (já
incluso via `global using Xunit;`).

## Risks / Trade-offs

- **[SDK local mais antigo não dispara o aviso]** → O requisito documenta o
  comportamento no CI (SDK mais novo); a convenção é aplicável independentemente.

## Migration Plan

1. Delta spec com o requisito ADDED em `build-release-tooling`.
2. Sincronizar para `openspec/specs/build-release-tooling/spec.md`.
3. Arquivar a change.

## Open Questions

- Nenhuma.
