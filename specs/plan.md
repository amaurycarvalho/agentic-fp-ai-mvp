# Execution Plan

## Situação atual (MVP)

- Fase de bootstrap iniciada em 13/02/2026.

## Próxima fase (curto prazo) - Fase 1

- [x] Estrutura base do projeto: criação dos projetos C# dos serviços em `services/` com pastas `src/` e `tests/`, criação dos `Dockerfile` dos serviços e `docker-compose` do projeto;
- [x] Escrita dos `user-stories/` e `adr/` iniciais dos serviços.

## Fase seguinte (evolução)

- Serviço MCP de Contagem (mcp-service) + testes unitários;
- Serviço RAG com Vector Database (rag-service) + testes unitários;
- Agent Orchestrator (agent-service) + testes unitários;
- API REST + testes unitários;
- Testes integrados TDD/BDD;
- CLI;
- UI Web;
- Integração Ollama/OpenAI;
- Observabilidade completa;
- Hardening de segurança.
