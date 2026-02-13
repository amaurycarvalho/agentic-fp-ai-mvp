# UC-001 - Orquestrar Contagem
---

## ID
`UC-AGENT-001`

## História de Usuário
Como analista de métricas, preciso enviar uma solicitação única para que o agente orquestre consulta de contexto e cálculo de contagem.

## Critérios de Aceite
1. Deve consultar o `rag-service` quando necessário para contexto.
2. Deve acionar o `mcp-service` para cálculo determinístico.
3. Deve retornar resultado consolidado com rastreabilidade das etapas.

---

## Cenários de Teste (BDD)

### TS-001: Fluxo orquestrado de contagem
Dado uma solicitação de contagem recebida pelo agente
Quando o fluxo de orquestração é executado
Então o agente consulta contexto no RAG
E executa cálculo no MCP
E retorna resposta consolidada com evidências.
