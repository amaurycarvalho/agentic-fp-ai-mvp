# gRPC Internal Contracts

## Why

A arquitetura (ADR-001, ADR-002, ADR-003) define comunicação interna via gRPC,
com REST apenas na borda externa. Hoje os serviços conversam por HTTP (ou ainda
não conversam). Os contratos gRPC internos precisam ser formalizados e
implementados para alinhar a comunicação entre `agent-service`, `mcp-service` e
`rag-service`.

## What Changes

- Definir contratos gRPC (protos) para as capacidades internas:
  - Contagem determinística (`mcp-service`);
  - Recuperação de contexto (`rag-service`);
  - Orquestração (`agent-service`).
- Implementar servidores e clientes gRPC nos serviços (mantendo REST na borda).
- Adicionar testes dos contratos/serviços gRPC.

## Capabilities

### New Capabilities

- `grpc-internal-contracts`: contratos e transporte gRPC para comunicação interna
  entre `agent-service`, `mcp-service` e `rag-service`, mantendo REST para a borda externa.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: projetos dos três serviços (protos, serviços gRPC, clientes gRPC, DI).
- Dependências: `Grpc.AspNetCore`, `Google.Protobuf`, `Grpc.Net.Client`.
- Interfaces: portas gRPC dedicadas nos serviços e no `docker-compose.yml`.
