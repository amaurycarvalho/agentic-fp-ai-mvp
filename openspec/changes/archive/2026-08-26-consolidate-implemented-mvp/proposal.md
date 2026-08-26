# Consolidate Implemented MVP

## Why

O repositório acumula código e documentação que já materializam parte do MVP
(bootstrap dos serviços, ADRs, RFCs, user stories e o endpoint de contagem básica
do `mcp-service`), mas o `openspec/specs` está vazio — não há especificação
consolidada do que já existe. Sem esse baseline, as changes futuras de evolução
ficam sem um contrato formal do estado atual.

## What Changes

- Registrar como capacidades formais o que já foi implementado e verificado no código:
  - Fundação arquitetural (DDD, Clean Architecture, microserviços, C#/.NET, SDD);
  - Bootstrap dos três serviços (`agent-service`, `mcp-service`, `rag-service`),
    solução raiz `agentic-fp-ai-mvp.sln`, `Dockerfile` por serviço, `docker-compose.yml`
    e documentação (`docs/` com ADRs, RFCs e user stories);
  - Endpoint de contagem básica do `mcp-service` (`POST /count/basic`) com
    classificação determinística por palavras-chave, complexidade DET×FTR,
    trilha de auditoria e testes unitários.
- Consolidar esse baseline em `openspec/specs` e arquivar a change (não há código
  novo a implementar nesta change).
- Nenhuma alteração de código-fonte é introduzida aqui — apenas captura do estado atual.

## Capabilities

### New Capabilities

- `architecture-foundation`: princípios imutáveis e decisões arquiteturais vigentes
  (ADR-001 a ADR-004): DDD, Clean Architecture, microserviços, C#/.NET, SDD,
  IA agêntica desacoplada via MCP/RAG, REST externo + gRPC interno, containers isolados.
- `service-bootstrap`: estrutura dos serviços `agent-service`, `mcp-service` e
  `rag-service` (projetos `src/` + `tests/`), solução raiz, `Dockerfile` por serviço,
  `docker-compose.yml` e endpoints de health-check (`GET /health`).
- `mcp-basic-count`: capacidade de contagem básica do `mcp-service` — classificação
  EI/EO/EQ e ILF/EIF por análise determinística de palavras-chave, complexidade
  DET×FTR, pontos de função transacionais, trilha de auditoria e testes unitários.

### Modified Capabilities

_(nenhuma — não há specs principais existentes a modificar)_

## Impact

- Código: nenhum — change de consolidação/arquivamento.
- Especificações: criação de `openspec/specs/architecture-foundation/spec.md`,
  `openspec/specs/service-bootstrap/spec.md` e `openspec/specs/mcp-basic-count/spec.md`.
- Processo: estabelece o baseline para as changes de evolução subsequentes
  (rag-service, agent-service, gRPC, resiliência, motores de medição, etc.).
