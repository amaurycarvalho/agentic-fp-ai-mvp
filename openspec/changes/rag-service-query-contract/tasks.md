# Tasks: RAG Service Query Contract

## 1. Contratos e domínio

- [ ] 1.1 Criar `Contracts/RetrievedChunk.cs` e `Contracts/SourceMetadata.cs`
- [ ] 1.2 Criar interface de aplicação `IRetrievalService` com método `SearchAsync`
- [ ] 1.3 Criar implementação do service usando repositório in-memory (porta de infraestrutura)

## 2. Endpoint

- [ ] 2.1 Expor `POST /rag/search` recebendo a consulta textual
- [ ] 2.2 Retornar trechos com metadados de origem (ou vazio quando sem resultados)
- [ ] 2.3 Manter `GET /health` funcionando

## 3. Testes

- [ ] 3.1 Remover placeholder `UnitTest1.cs` do rag-service
- [ ] 3.2 Adicionar testes de consulta com resultados
- [ ] 3.3 Adicionar testes de consulta sem resultados e de metadados
- [ ] 3.4 Garantir que os testes passam em `dotnet test`
