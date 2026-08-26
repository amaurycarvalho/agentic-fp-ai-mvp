# Tasks: Vector Database RAG

## 1. Infraestrutura vetorial

- [ ] 1.1 Adicionar serviço `qdrant` ao `docker-compose.yml` (volume persistente, healthcheck)
- [ ] 1.2 Declarar `depends_on` do rag-service com `condition: service_healthy`
- [ ] 1.3 Adicionar cliente Qdrant para .NET ao rag-service

## 2. Embeddings e ingestão

- [ ] 2.1 Criar `IEmbeddingProvider` (interface desacoplada)
- [ ] 2.2 Implementar `DocumentIngestor` (chunking por seção + embeddings + upsert)
- [ ] 2.3 Garantir idempotência da ingestão (hash SHA-256 como ID do ponto)
- [ ] 2.4 Preservar metadados de origem no payload dos pontos

## 3. Provider de recuperação

- [ ] 3.1 Implementar `QdrantRetrievalStore` implementando a porta de infraestrutura
- [ ] 3.2 Registrar no DI substituindo o repositório in-memory
- [ ] 3.3 Garantir consulta por relevância vetorial com metadados

## 4. Testes

- [ ] 4.1 Teste de integração da ingestão (idempotência)
- [ ] 4.2 Teste de integração da consulta por relevância
- [ ] 4.3 Manter testes unitários do contrato passando com fake
- [ ] 4.4 Validar `docker-compose up` completo com Qdrant ready
