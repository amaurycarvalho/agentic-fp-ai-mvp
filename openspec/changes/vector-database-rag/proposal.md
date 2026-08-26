# Vector Database RAG

## Why

O `rag-service` (ADR-003) prevê armazenamento vetorial desacoplado por porta de
infraestrutura. A US-RAG-001 exige recuperação por relevância; hoje não há base
vetorial, ingestão de documentos normativos nem o Qdrant no `docker-compose.yml`
(que docs/README prometem).

## What Changes

- Integrar um provider vetorial concreto (Qdrant) via a porta de infraestrutura do
  rag-service (definida na change `rag-service-query-contract`).
- Implementar ingestão de documentos normativos (chunking + embeddings).
- Adicionar o Qdrant (e provedor de embeddings) ao `docker-compose.yml`.
- Testes da ingestão e da consulta com o provider real (ou teste de integração).

## Capabilities

### New Capabilities

- `vector-database-rag`: armazenamento e recuperação vetorial do `rag-service` —
  ingestão de conhecimento normativo (chunking/embeddings) e consulta por relevância
  usando Qdrant, desacoplado por porta de infraestrutura.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `rag-service` (provider Qdrant, pipeline de ingestão, DI) e `docker-compose.yml`.
- Dependências: Qdrant (Docker), cliente Qdrant para .NET, provedor de embeddings
  (local/Ollama — ver `llm-provider-integration`).
- Infra: serviço `qdrant` no compose com volume persistente.
