# Tasks: gRPC Internal Contracts

## 1. Protos e stubs

- [ ] 1.1 Criar proto do `mcp-service` (serviço de contagem)
- [ ] 1.2 Criar proto do `rag-service` (serviço de contexto)
- [ ] 1.3 Criar proto do `agent-service` (serviço de orquestração)
- [ ] 1.4 Adicionar pacotes gRPC (`Grpc.AspNetCore`, `Grpc.Net.Client`, `Google.Protobuf`) e gerar stubs

## 2. Servidores gRPC

- [ ] 2.1 Registrar `AddGrpc()` e mapear o serviço de contagem no mcp-service
- [ ] 2.2 Registrar e mapear o serviço de contexto no rag-service
- [ ] 2.3 Registrar e mapear o serviço de orquestração no agent-service
- [ ] 2.4 Configurar portas gRPC no Kestrel (HTTP/2) em cada serviço

## 3. Clientes e integração

- [ ] 3.1 Implementar clientes gRPC no agent-service para MCP e RAG
- [ ] 3.2 Propagar `correlation_id` via metadata gRPC
- [ ] 3.3 Expor portas gRPC no `docker-compose.yml`

## 4. Testes

- [ ] 4.1 Adicionar testes para os servidores gRPC de contagem e contexto
- [ ] 4.2 Adicionar teste de orquestração usando clientes gRPC fakes/reais
- [ ] 4.3 Garantir `dotnet test` passando e REST externo ainda funcional
