# Design: Vector Database RAG

## Context

O rag-service tem contrato de consulta (change `rag-service-query-contract`), mas
a implementação de infraestrutura usa repositório in-memory. ADR-003 exige
armazenamento vetorial desacoplado por porta. Esta change traz o provider Qdrant,
ingestão e infra de compose.

## Goals / Non-Goals

**Goals:**
- Provider Qdrant para a porta de recuperação.
- Pipeline de ingestão (chunking + embeddings + idempotência).
- Qdrant no compose com persistência.

**Non-Goals:**
- Provedor de embeddings remoto OpenAI — embeddings locais via Ollama
  (`llm-provider-integration`) ou modelo local; interface de embeddings desacoplada.
- Otimização de performance/scale — MVP.
- FPA/SFP/SNAP — changes próprias.

## Decisions

### D1 — Provider `QdrantRetrievalStore`
Implementa a porta `IRetrievalStore` usando o cliente Qdrant para .NET. Coleção
única `normative_chunks` com payload de metadados (document_id, section_id,
reference) e vetor de embedding. Swap por DI.
- **Alternativa**: Postgres+pgvector — Qdrant é a escolha documentada.

### D2 — Pipeline de ingestão idempotente
`DocumentIngestor` faz: leitura do documento → chunking por seção → geração de
embedding (`IEmbeddingProvider`) → upsert no Qdrant usando hash do conteúdo
(SHA-256) como ponto/ID — garante idempotência (FR de duplicados).
- **Alternativa**: duplicar trechos — rejeitada (edge case de duplicados nos docs).

### D3 — `IEmbeddingProvider` desacoplado
Interface com implementação local (modelo embutido) e futura Ollama/OpenAI
(change `llm-provider-integration`). Embedding dimensionado à coleção.

### D4 — Qdrant no compose
Serviço `qdrant` (imagem oficial) com volume nomeado, porta 6333, healthcheck.
`rag-service` declara `depends_on` com `condition: service_healthy`.

## Risks / Trade-offs

- **[Embeddings locais têm qualidade menor]** → Suficiente para MVP; troca de
  provider pela interface sem tocar no domínio.
- **[Qdrant é mais um serviço na stack]** → Necessário para recuperação real;
  documentado no README.

## Migration Plan

1. Adicionar Qdrant ao compose.
2. Criar `IEmbeddingProvider` e `DocumentIngestor`.
3. Implementar `QdrantRetrievalStore`.
4. Registrar no DI e substituir repositório in-memory.
5. Testes de integração.

## Open Questions

- Dimensão e modelo de embedding padrão para o MVP.
