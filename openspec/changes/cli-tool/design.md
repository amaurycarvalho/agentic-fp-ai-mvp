# Design: CLI Tool

## Context

Requisito funcional 10 e Constitution (entregável como API, CLI e UI). O CLI
complementa a API, consumindo a borda REST pública (agente/MCP/RAG). Deve ser
simples, testável e consumível por humanos e scripts.

## Goals / Non-Goals

**Goals:**
- Comandos `count`, `context`, `measure`, `health`.
- Output legível + `--json`.
- Auth JWT configurável.

**Non-Goals:**
- UI Web — change `web-ui`.
- Lógica de negócio de contagem no CLI (apenas cliente da API).
- Transporte gRPC direto no CLI — usa a borda REST.

## Decisions

### D1 — Projeto .NET console `tools/agentic-fp-cli`
Novo projeto console em `tools/agentic-fp-cli` + testes em
`tools/agentic-fp-cli.Tests`, adicionado à solução raiz. Usa `System.CommandLine`
para parsing.
- **Alternativa**: projetos separados por serviço — desnecessário; um CLI único.

### D2 — Cliente de API reutilizável
Camada `ApiClient` (HttpClient tipado) com métodos para contagem, contexto,
medição e health; base URL e token configuráveis via args/env (`AGENTIC_FP_API_URL`,
`AGENTIC_FP_TOKEN`).

### D3 — Output em dois modos
Renderer com modo legível (tabela/texto) e `--json` (serialização dos DTOs).

## Risks / Trade-offs

- **[Dependência de `System.CommandLine`]** → Bibliotecas estáveis/ativas
  (pré-release da MS é aceitável); fallback: parsing manual simples.
- **[CLI espelha a API]** → Documentar alinhamento; evolução conjunta.

## Migration Plan

1. Criar projeto + solução.
2. Implementar ApiClient e comandos.
3. Implementar renderers.
4. Auth/config.
5. Testes.

## Open Questions

- Nome do binário/comando raiz (`agentic-fp` vs `afp`).
