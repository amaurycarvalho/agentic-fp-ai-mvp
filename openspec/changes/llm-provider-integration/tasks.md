# Tasks: LLM Provider Integration

## 1. Porta e contratos

- [ ] 1.1 Definir `ILLMProvider` (ChatAsync/EmbedAsync) e DTOs
- [ ] 1.2 Definir configuração `LLM:Provider` (ollama/openai)

## 2. Provedores

- [ ] 2.1 Implementar `OllamaProvider` (URL configurável)
- [ ] 2.2 Implementar `OpenAIComplianceProvider` (chave/URL configuráveis)
- [ ] 2.3 Registrar provedores no DI com seleção por configuração

## 3. Integrações

- [ ] 3.1 Integrar enriquecimento semântico revisável no `agent-service`
- [ ] 3.2 Garantir que a contagem final permanece determinística (LLM não decide regras)
- [ ] 3.3 Integrar embeddings no `rag-service` via `ILLMProvider`
- [ ] 3.4 Adicionar serviço `ollama` opcional no `docker-compose.yml`

## 4. Testes

- [ ] 4.1 Testes de chat e embedding com fakes/mocks do provedor
- [ ] 4.2 Testes de independência da contagem em relação ao LLM
- [ ] 4.3 Garantir `dotnet test` passando
