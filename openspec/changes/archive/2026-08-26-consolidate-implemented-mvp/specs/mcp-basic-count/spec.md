## ADDED Requirements

### Requirement: Contagem básica de pontos de função
O `mcp-service` SHALL expor o endpoint `POST /count/basic` que, dado um texto de
história de usuário, classifica funções transacionais (EI, EO, EQ) e funções de
dados (ILF, EIF) de forma determinística, calcula a complexidade DET×FTR, computa
os pontos de função e retorna uma trilha de auditoria com as justificativas.

#### Scenario: Classificação de transação EI e dado ILF
- **WHEN** uma história de usuário contém verbos de entrada (ex.: cadastrar) e termos de persistência (ex.: banco)
- **THEN** o resultado classifica EI entre as funções transacionais e ILF entre as funções de dados

#### Scenario: Classificação de transação EQ e dado EIF
- **WHEN** uma história de usuário contém verbos de consulta (ex.: consultar) e termos externos (ex.: sistema externo)
- **THEN** o resultado classifica EQ entre as funções transacionais e EIF entre as funções de dados

#### Scenario: Fallback de classificação
- **WHEN** nenhuma palavra-chave é detectada
- **THEN** o serviço aplica fallback (EQ para transacional e ILF para dados) e registra na trilha de auditoria

### Requirement: Complexidade DET×FTR
O `mcp-service` SHALL calcular a complexidade usando a matriz DET×FTR para funções
transacionais, usando DET e FTR informados no request ou inferidos do texto quando
não informados.

#### Scenario: DET e FTR fornecidos explicitamente
- **WHEN** o request informa `det=20` e `ftr=3`
- **THEN** o resumo usa esses valores e retorna complexidade `High`

#### Scenario: DET e FTR inferidos
- **WHEN** o request não informa DET/FTR
- **THEN** o serviço infere DET a partir de tokens únicos do texto e FTR a partir das funções de dados identificadas

### Requirement: Pontos de função transacionais
O `mcp-service` SHALL atribuir pesos IFPUG às funções transacionais conforme a
complexidade (EI 3/4/6, EO 4/5/7, EQ 3/4/6) e SHALL retornar o total no resumo.

#### Scenario: Total de pontos maior que zero
- **WHEN** uma contagem é executada com funções identificadas
- **THEN** o `totalFunctionPoints` no resumo é maior que zero

### Requirement: Trilha de auditoria da contagem
O `mcp-service` SHALL retornar uma trilha de auditoria (lista de justificativas)
explicando as classificações, inferências e cálculos aplicados.

#### Scenario: Trilha não vazia
- **WHEN** uma contagem é executada
- **THEN** a resposta contém uma trilha de auditoria não vazia com as etapas executadas

### Requirement: Validação do request de contagem
O endpoint `POST /count/basic` SHALL rejeitar requests sem `userStory` com
`400 Bad Request` e mensagem de erro indicando o campo obrigatório.

#### Scenario: User story ausente
- **WHEN** um request é enviado sem `userStory`
- **THEN** a resposta é `400 Bad Request` com mensagem informando que o campo `userStory` é obrigatório

### Requirement: Testes unitários da contagem básica
O `mcp-service` SHALL possuir testes unitários que cobrem a classificação de
funções, o uso de DET/FTR explícitos e a geração de relatório justificável.

#### Scenario: Testes do serviço de contagem
- **WHEN** os testes unitários do `mcp-service` são executados
- **THEN** os cenários de classificação EI/ILF, EQ/EIF e DET/FTR explícitos passam
