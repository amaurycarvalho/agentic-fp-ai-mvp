# Tasks Backlog

## Concluído

- [x] Criar estrutura inicial dos serviços em `services/agent-service`, `services/mcp-service` e `services/rag-service`.
- [x] Criar projetos .NET em `src/` para os três serviços.
- [x] Criar projetos de testes em `tests/` para os três serviços.
- [x] Criar solução raiz `agentic-fp-ai-mvp.sln` e adicionar todos os projetos.
- [x] Criar `Dockerfile` para cada serviço.
- [x] Criar `docker-compose.yml` com os três serviços do MVP.
- [x] Preencher ADRs iniciais: `ADR-002`, `ADR-003`, `ADR-004`.
- [x] Completar caso de uso inicial `US-MCP-001` e adicionar USs iniciais de `rag-service` e `agent-service`.

## Próximos passos

- [x] Implementar o endpoint de negócio inicial do `mcp-service` para contagem básica (além de health-check).
- [ ] Implementar consulta inicial no `rag-service` com contrato de recuperação.
- [ ] Implementar fluxo inicial de orquestração no `agent-service`.
- [ ] Adicionar testes unitários reais para regras de domínio de cada serviço.
- [ ] Adicionar contratos internos (gRPC) entre serviços.
- [ ] Implementar health checks e cenários de resiliência (timeout/retry).
- [ ] Evoluir `docker-compose.yml` com dependências de infraestrutura (ex.: vector DB).
