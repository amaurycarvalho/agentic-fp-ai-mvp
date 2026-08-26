# Observability

## Why

A Constituição exige observabilidade completa e os requisitos não-funcionais
preveem OpenTelemetry e logs estruturados e auditáveis. Hoje os serviços apenas
logam implicitamente no console e não emitem métricas/traces.

## What Changes

- Instrumentar os três serviços com OpenTelemetry (traces, métricas e logs).
- Emitir métricas de negócio (duração de medição, contagens por componente) e de
  infraestrutura (HTTP).
- Logs estruturados com correlação (trace/correlation id).
- Adicionar endpoint de métricas Prometheus (ex.: `/metrics`) e integração com
  collector/exportador configurável.

## Capabilities

### New Capabilities

- `observability`: telemetria (traces, métricas, logs estruturados) via
  OpenTelemetry nos três serviços, com correlação de requisições e endpoint de
  métricas Prometheus.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `Program.cs` e DI dos três serviços; `appsettings` para configuração de
  exportadores.
- Dependências: `OpenTelemetry.Extensions.Hosting`, `OpenTelemetry.Exporter.OpenTelemetryProtocol`
  (ou Prometheus), `Serilog`/`OpenTelemetry` logs.
- Infra: (opcional) collector/jaeger/prometheus — documentado, sem obrigatoriedade no MVP.
