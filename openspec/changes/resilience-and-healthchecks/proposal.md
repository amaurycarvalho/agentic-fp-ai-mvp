# Resilience and Health Checks

## Why

O ADR-001 prevê health checks e cenários de resiliência (timeout/retry) como parte
da fundação. Hoje os health-checks são apenas `GET /health` trivial, sem verificar
dependências, e as chamadas entre serviços não possuem timeouts/retries — deixando
a stack frágil a falhas de dependência.

## What Changes

- Health checks de dependências (liveness + readiness) nos três serviços.
- Política de timeout/retry para chamadas HTTP/gRPC entre serviços
  (Polly ou middleware equivalente).
- Configuração de timeouts em clientes e opções de resiliência.
- Testes de resiliência e de health-check com dependência indisponível.

## Capabilities

### New Capabilities

- `service-resilience`: health checks de dependências (liveness/readiness) e
  resiliência de chamadas entre serviços (timeout/retry) para os três serviços do MVP.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: Program.cs dos três serviços, clientes HTTP/gRPC, `appsettings`.
- Dependências: Polly (ou `Microsoft.Extensions.Http.Resilience`).
- Infra: novos endpoints `/health/live` e `/health/ready`.
