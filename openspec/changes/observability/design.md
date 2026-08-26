# Design: Observability

## Context

Constituição e NFRs exigem OpenTelemetry, logs estruturados e auditáveis. Os
serviços não emitem telemetria hoje.

## Goals / Non-Goals

**Goals:**
- Traces, métricas e logs estruturados nos 3 serviços.
- Métricas de negócio de medição e endpoint Prometheus.
- Configuração de exportadores por ambiente.

**Non-Goals:**
- Infra completa de observabilidade (collector/Jaeger/Grafana) — opcional e documentada.
- mTLS entre serviços — change `security-hardening`.

## Decisions

### D1 — OpenTelemetry .NET com `AddOpenTelemetry`
Configura recursos (service.name), traces (ASP.NET Core instrumentation),
métricas (HTTP server + custom meters), logs estruturados via `ILogger`.
- **Alternativa**: provedores próprios de log (Serilog) — aceitável; preferida a
  stack OTel nativa para coerência.

### D2 — Meters de negócio no mcp-service
`Meter("agentic-fp-ai.mcp")` com `Histogram` de duração de medição e `Gauge` de
contagens por componente/função.

### D3 — Endpoint `/metrics` Prometheus
Expoe via `OpenTelemetry.Exporter.Prometheus.AspNetCore` (ou ExporterHttpServer)
quando habilitado; OTLP exportador configurável por `OTEL_*` env vars.

### D4 — Correlação propagada
Uso do `Activity`/`TraceId` como identificador de correlação, propagado nas
chamadas HTTP (W3C traceparent) e registrado nos logs e na trilha de auditoria.

## Risks / Trade-offs

- **[Custo de telemetria]** → Amostragem configurável; baixo no MVP.
- **[Novas dependências]** → Pacotes OTel oficiais (maduros).

## Migration Plan

1. Adicionar pacotes OTel aos serviços.
2. Registrar instrumentation no DI + `/metrics`.
3. Adicionar meters de negócio.
4. Logs estruturados + propagação de correlação.
5. Testes de emissão de métricas/logs.

## Open Questions

- Destino padrão (OTLP collector local vs Prometheus direto) por ambiente.
