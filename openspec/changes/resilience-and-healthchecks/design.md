# Design: Resilience and Health Checks

## Context

Os três serviços expõem apenas `/health` trivial. As chamadas do agente para
RAG/MCP não têm timeout nem retry. ADR-001 exige health checks e resiliência.

## Goals / Non-Goals

**Goals:**
- `/health/live` e `/health/ready` (readiness com dependências).
- Timeout + retry configurável nas chamadas entre serviços.

**Non-Goals:**
- Circuit breaker (pode evoluir depois).
- Observabilidade de falhas (OpenTelemetry) — change `observability`.

## Decisions

### D1 — `AddHealthChecks` + checks customizados
Usa `Microsoft.Extensions.Diagnostics.HealthChecks` com checks registrados por
dependência (URLs HTTP das dependências). Endpoints mapeados como `/health/live` e
`/health/ready`.
- **Alternativa**: implementar manualmente — rejeitada (padrão do ecossistema .NET).

### D2 — `Microsoft.Extensions.Http.Resilience` (Polly integrado)
Clientes HTTP configurados com `AddStandardResilienceHandler` ou políticas
explícitas: timeout (ex.: 3s) e retry com backoff exponencial (ex.: 3 tentativas).
Valores em configuração (`ResilienceOptions`).
- **Alternativa**: Polly clássico direto — aceitável; preferido o handler integrado.

### D3 — Readiness por serviço
- `mcp-service`: ready sem dependências internas (self).
- `rag-service`: ready conforme provider de recuperação (qdrant em `vector-database-rag`).
- `agent-service`: ready conforme RAG/MCP (checks de dependência via HTTP).

## Risks / Trade-offs

- **[Readiness do agente depende de serviços que sobem depois]** → Usar
  `depends_on` com health conditions no compose e `start_period`/tolerância no check.

## Migration Plan

1. Adicionar health checks customizados e endpoints nos 3 serviços.
2. Configurar resiliência (timeout/retry) nos clientes do agente.
3. Ajustar `docker-compose.yml` (healthchecks nos containers).
4. Testes.

## Open Questions

- Valores exatos de timeout/retry por ambiente (dev/prod).
