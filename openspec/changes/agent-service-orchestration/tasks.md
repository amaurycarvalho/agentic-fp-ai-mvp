# Tasks: Agent Service Orchestration

## 1. Contratos e clientes

- [ ] 1.1 Criar `Contracts/OrchestrationRequest.cs`, `OrchestrationResponse.cs` e `AuditEntry.cs`
- [ ] 1.2 Criar `IRagClient` e `IMcpClient` (interfaces de aplicação)
- [ ] 1.3 Criar implementações HTTP (Typed Clients) com URLs base configuráveis (`RagBaseUrl`, `McpBaseUrl`)

## 2. Orquestrador

- [ ] 2.1 Criar `ICountOrchestrator` e implementação com trilha auditável
- [ ] 2.2 Consultar contexto no RAG quando necessário, sem bloquear na ausência
- [ ] 2.3 Acionar o `mcp-service` para o cálculo determinístico

## 3. Endpoint

- [ ] 3.1 Expor `POST /count/orchestrated` retornando resultado consolidado com evidências
- [ ] 3.2 Rejeitar payload inválido com 400
- [ ] 3.3 Manter `GET /health` funcionando

## 4. Testes

- [ ] 4.1 Remover placeholder `UnitTest1.cs` do agent-service
- [ ] 4.2 Adicionar testes do fluxo completo (com fakes de RAG/MCP)
- [ ] 4.3 Adicionar testes de contexto indisponível e de payload inválido
- [ ] 4.4 Garantir que os testes passam em `dotnet test`
