# Agent Service Orchestration

## Why

O `agent-service` expõe apenas `GET /health`. A US-AGENT-001 exige o fluxo
orquestrado de contagem: receber uma solicitação única, consultar contexto no
`rag-service`, acionar o `mcp-service` para cálculo determinístico e retornar um
resultado consolidado com rastreabilidade das etapas.

## What Changes

- Implementar o fluxo de orquestração do `agent-service` (US-AGENT-001).
- Definir clientes HTTP para `rag-service` (busca de contexto) e `mcp-service`
  (contagem), com interfaces desacopladas para testes.
- Registrar trilha auditável da orquestração (entrada, chamadas e resultado).
- Adicionar testes unitários reais (substituir placeholder).

## Capabilities

### New Capabilities

- `agent-orchestration`: orquestração da contagem pelo `agent-service` — entrada do
  usuário, consulta de contexto RAG, acionamento determinístico MCP e resposta
  consolidada com rastreabilidade.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `services/agent-service/src/AgentService.Api` (contratos, orquestrador,
  clientes HTTP, endpoint) e `services/agent-service/tests`.
- Dependências: consome os contratos de `rag-service` e `mcp-service` (endpoints
  HTTP existentes/definidos em changes anteriores).
- Interfaces: novo endpoint de orquestração; clientes configuráveis por URL base.
