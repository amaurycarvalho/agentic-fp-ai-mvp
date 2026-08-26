# RAG Service Query Contract

## Why

O `rag-service` hoje expõe apenas `GET /health`. A US-RAG-001 (consulta à base
normativa) define que o agente precisa recuperar contexto técnico confiável antes
da análise, com metadados de origem para auditoria — sem executar ações de negócio.

## What Changes

- Implementar o contrato de consulta do `rag-service` (US-RAG-001): endpoint de
  busca por texto que retorna trechos relevantes e metadados mínimos da fonte.
- Definir porta de infraestrutura para o armazenamento (desacoplada do domínio),
  permitindo troca de provedor de recuperação.
- Adicionar testes unitários reais (substituir o `UnitTest1.cs` placeholder).

## Capabilities

### New Capabilities

- `rag-query-contract`: consulta à base normativa do `rag-service` — recuperação
  de trechos relevantes por consulta textual com metadados de origem, sem execução
  de ações de negócio.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `services/rag-service/src/RagService.Api` (contratos, porta de
  infraestrutura, service de consulta, endpoint) e `services/rag-service/tests`.
- Dependência: porta de recuperação desacoplada (provider concreto entra na change
  `vector-database-rag`).
- Interfaces: novo endpoint HTTP de busca; contrato de recuperação consumível por gRPC depois.
