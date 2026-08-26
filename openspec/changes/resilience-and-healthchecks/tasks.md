# Tasks: Resilience and Health Checks

## 1. Health checks

- [ ] 1.1 Adicionar `AddHealthChecks` nos três serviços
- [ ] 1.2 Registrar checks de dependências (RAG/MCP no agente; provider no RAG)
- [ ] 1.3 Expor `/health/live` e `/health/ready`
- [ ] 1.4 Adicionar healthchecks no `docker-compose.yml` (com `condition: service_healthy`)

## 2. Resiliência

- [ ] 2.1 Adicionar pacote de resiliência (Exponential Backoff/Polly) aos clientes
- [ ] 2.2 Configurar timeout configurável nas chamadas RAG/MCP
- [ ] 2.3 Configurar retry com backoff limitado
- [ ] 2.4 Centralizar opções em `appsettings` (`ResilienceOptions`)

## 3. Testes

- [ ] 3.1 Testar readiness com dependência indisponível
- [ ] 3.2 Testar comportamento de timeout e retry
- [ ] 3.3 Garantir `dotnet test` passando
