# Design: RAG Service Query Contract

## Context

O `rag-service` possui apenas health-check. A US-RAG-001 exige um contrato de
recuperação com metadados de origem e sem execução de ações. O ADR-003 determina:
recuperação desacoplada por porta de infraestrutura, resposta com metadados de
origem, evolução para gRPC. A base vetorial concreta (qdrant) entra em outra change.

## Goals / Non-Goals

**Goals:**
- Entregar contrato HTTP de busca com metadados de origem.
- Manter o domínio desacoplado do provedor de recuperação.
- Testes unitários reais para o contrato.

**Non-Goals:**
- Provedor vetorial concreto (qdrant/embeddings) — change `vector-database-rag`.
- Contratos gRPC — change `grpc-internal-contracts`.
- Orquestração (uso do RAG pelo agente) — change `agent-service-orchestration`.

## Decisions

### D1 — Contrato de aplicação `IRetrievalService`
A camada de aplicação define `IRetrievalService` com
`SearchAsync(query, correlationId) → IReadOnlyList<RetrievedChunk>`, onde
`RetrievedChunk` carrega `Content` + `SourceMetadata` (documento/seção/referência).
- **Alternativa**: acoplar direto ao SDK do vetor DB — rejeitada (viola ADR-003).

### D2 — Endpoint HTTP `POST /rag/search`
Reusa o padrão do `mcp-service` (minimal API). Response: `{ query, results: [...], }`.
- **Alternativa**: GET com query string — POST é mais adequado a payload de busca.

### D3 — Implementação da porta de infraestrutura
A implementação concreta da porta usa um repositório em memória no MVP e é
substituída pelo provider qdrant na change `vector-database-rag` via DI.

## Risks / Trade-offs

- **[Resultados dependem do provider]** → Contrato definido primeiro; provider real
  implementa a mesma porta (swap por DI).
- **[Sem pontuação de relevância real]** → MVP retorna por correspondência básica;
  embeddings/rank chegam com `vector-database-rag`.

## Migration Plan

1. Definir contratos (`RetrievedChunk`, `SourceMetadata`) e `IRetrievalService`.
2. Implementar service com repositório in-memory.
3. Expor `POST /rag/search`.
4. Substituir placeholder por testes reais.

## Open Questions

- Nenhuma.
