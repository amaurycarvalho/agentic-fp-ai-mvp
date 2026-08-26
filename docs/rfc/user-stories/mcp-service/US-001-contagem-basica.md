# US-001 - Contagem Básica
---

## ID
`US-MCP-001`

## História de Usuário
Como analista de métricas, preciso submeter histórias de usuário para obter a contagem automática de Pontos de Função.

## Critérios de Aceite
1. Deve identificar EI/EO/EQ.
2. Deve identificar ILF/EIF.
3. Deve calcular complexidade baseada em DET x FTR.
4. Deve gerar relatório técnico justificável.

---

## Cenários de Teste (BDD)

### TS-001: Contagem básica de transações
Dado que o analista envia uma história de usuário válida
Quando o serviço processa a solicitação de contagem
Então o sistema classifica EI/EO/EQ e ILF/EIF
E calcula a complexidade
E retorna o relatório com justificativa da classificação.
