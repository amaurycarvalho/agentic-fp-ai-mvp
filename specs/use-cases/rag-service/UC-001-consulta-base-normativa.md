# UC-001 - Consulta à Base Normativa
---

## ID
`UC-RAG-001`

## História de Usuário
Como agente de orquestração, preciso consultar a base normativa para recuperar contexto técnico confiável antes da análise.

## Critérios de Aceite
1. Deve retornar trechos relevantes por consulta textual.
2. Deve retornar metadados mínimos da fonte consultada.
3. Não deve executar ações de negócio, apenas recuperar conhecimento.

---

## Cenários de Teste (BDD)

### TS-001: Recuperação de contexto por consulta
Dado uma consulta sobre regra de contagem
Quando o serviço RAG recebe a busca
Então retorna os trechos mais relevantes
E informa metadados de origem para auditoria.
