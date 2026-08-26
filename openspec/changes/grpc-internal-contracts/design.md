# Design: gRPC Internal Contracts

## Context

ADR-001/002/003 definem gRPC como transporte interno. O MVP começou com REST
mínimo. Esta change introduz gRPC entre os serviços sem remover a borda REST
externa. O agente passa a consumir MCP e RAG por gRPC.

## Goals / Non-Goals

**Goals:**
- Contratos gRPC (protos) para contagem, contexto e orquestração.
- Servidores gRPC nos serviços e clientes gRPC no agente.
- Manter REST externo intacto.

**Non-Goals:**
- Remover endpoints REST existentes.
- Streaming/duplex — chamadas unárias no MVP.
- TLS/mTLS entre serviços — hardening em `security-hardening`.

## Decisions

### D1 — Protos por serviço
- `mcp-service`: `CountBasic` (request: `user_story`, `det`, `ftr`; response: classificações + resumo + trilha).
- `rag-service`: `SearchContext` (request: `query`, `correlation_id`; response: trechos + metadados).
- `agent-service`: `OrchestrateCount` (request: solicitação; response: consolidado + trilha).
Cada proto declara `service_version` e aceita `correlation_id` em metadata.

### D2 — Portas gRPC dedicadas
Cada serviço expõe gRPC em porta própria (ex.: 50051/50052/50053), exposta também
no `docker-compose.yml`. Kestrel configura HTTP + HTTP/2 (gRPC) na mesma instância
com endpoints distintos.

### D3 — Clientes gRPC no agente
O `agent-service` consome MCP/RAG por clientes gRPC gerados, sob as mesmas
interfaces (`IMcpClient`/`IRagClient`) já definidas na change
`agent-service-orchestration` — implementação alternativa de contrato.

## Risks / Trade-offs

- **[Complexidade de configuração HTTP/2]** → Kestrel com endpoints nomeados;
  testes de integração no CI validam o caminho gRPC.
- **[Duplicação HTTP vs gRPC no MVP]** → Aceita: REST na borda + gRPC interno;
  eliminação gradual em fases futuras.

## Migration Plan

1. Criar protos e gerar stubs.
2. Registrar serviços gRPC (AddGrpc) e mapear handlers para a camada de aplicação.
3. Configurar portas gRPC no Kestrel e no compose.
4. Implementar clientes gRPC no agente.
5. Testes.

## Open Questions

- Quais portas exatas de gRPC adotar (evitar conflito com 8081/8082/8083).
