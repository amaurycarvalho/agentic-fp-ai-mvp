# Design: Agent Service Orchestration

## Context

O `agent-service` (ADR-004) centraliza a orquestração sem implementar regras de
contagem no domínio. Ele chama capacidades do `mcp-service` e contexto do
`rag-service` por contratos explícitos, registra trilha auditável e, no MVP, o
fluxo é síncrono. O serviço hoje é só um health-check.

## Goals / Non-Goals

**Goals:**
- Endpoint de orquestração de contagem com resultado consolidado.
- Dependências externas desacopladas por interfaces (testabilidade).
- Trilha auditável da orquestração.

**Non-Goals:**
- Implementar regras de contagem no agente (pertence ao mcp-service).
- Processamento assíncrono/fila — fluxo síncrono no MVP (ADR-004).
- gRPC entre serviços — change `grpc-internal-contracts`.

## Decisions

### D1 — Orquestrador `ICountOrchestrator`
Camada de aplicação define `ICountOrchestrator.Orchestrate(request)` que executa
as etapas e monta o resultado consolidado com `AuditTrail`.
- **Alternativa**: lógica inline no endpoint — rejeitada (testabilidade).

### D2 — Clientes HTTP via interfaces `IRagClient` e `IMcpClient`
`IRagClient.SearchContextAsync` e `IMcpClient.CountAsync`, implementados com
`HttpClient` (Typed Client) apontando para URLs base configuráveis
(`RagBaseUrl`, `McpBaseUrl`). Testes usam fakes.
- **Alternativa**: ServiceDiscovery/gRPC — fora do escopo do MVP síncrono.

### D3 — Endpoint `POST /count/orchestrated`
Recebe a solicitação, chama o orquestrador e retorna o resultado consolidado com
evidências e trilha. Rejeita payload inválido com 400.

## Risks / Trade-offs

- **[Dependência de rede com MCP/RAG]** → Timeouts configuráveis e continuidade
  quando o contexto RAG está indisponível (resiliência ampliada em `resilience-and-healthchecks`).
- **[Latência síncrona]** → Aceito no MVP; evolução assíncrona prevista em ADR-004.

## Migration Plan

1. Definir contratos (`OrchestrationRequest`, `OrchestrationResponse`, `AuditEntry`).
2. Criar `IRagClient`/`IMcpClient` + implementações HTTP (Typed Clients).
3. Criar `ICountOrchestrator` e implementação com trilha.
4. Expor `POST /count/orchestrated`.
5. Testes unitários com fakes.

## Open Questions

- Nenhuma.
