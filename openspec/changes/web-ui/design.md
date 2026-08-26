# Design: Web UI

## Context

Requisito funcional 11 e Constitution: entregável como API, CLI e UI. A UI consome
a borda REST pública (com JWT). É a interface voltada a analistas de métricas.

## Goals / Non-Goals

**Goals:**
- Submeter histórias, ver contagem + trilha.
- Consultar contexto RAG; visualizar medições; status.
- Auth JWT.

**Non-Goals:**
- Reimplementar lógica de contagem no frontend.
- Transporte gRPC no frontend.
- PWA/offline no MVP.

## Decisions

### D1 — Stack: Blazor (mantém C#/.NET) vs SPA (React)
Recomendado **Blazor WebAssembly/Server** para manter a stack .NET e reaproveitar
contratos/DTOs; alternativa React é possível se o time preferir. **Decisão aberta**.
- **Alternativa**: React/TypeScript — independência do front; custo de dupla stack.

### D2 — Cliente HTTP com token
`HttpClient` configurado com interceptor de auth (adiciona Bearer do token);
expiração → redireciona ao login. CORS configurado nos serviços para a origem da UI.

### D3 — Páginas
- `Contagem` (form + resultado + trilha);
- `Contexto` (busca + resultados com metadados);
- `Medições` (método + resultado/evidências);
- `Status` (health dos serviços);
- `Login` (token/credenciais).

## Risks / Trade-offs

- **[Escolha de stack]** → Blazor alinha à stack; SPA facilita recrutamento. Decidir
  antes da implementação.
- **[CORS/segurança]** → Origem restrita da UI; tokens nunca em logs (ver
  `security-hardening`).

## Migration Plan

1. Decidir stack; criar app base.
2. Cliente API + auth.
3. Páginas de contagem/contexto.
4. Medições e status.
5. Testes.

## Open Questions

- Stack final (Blazor vs React).
