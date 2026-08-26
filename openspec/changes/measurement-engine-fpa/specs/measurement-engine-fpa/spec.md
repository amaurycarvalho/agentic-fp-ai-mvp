## ADDED Requirements

### Requirement: Identificação das cinco funções IFPUG
O motor FPA SHALL identificar os cinco tipos de função: ILF, EIF, EI, EO e EQ,
segundo as regras da RFC 008 (FR-014 a FR-019), consumindo um modelo funcional
como entrada semântica (FR-001).

#### Scenario: Identificação de ILF e EIF
- **WHEN** o modelo funcional contém grupos de dados mantidos pela aplicação e referenciados de outro sistema
- **THEN** o motor identifica ILF para os mantidos e EIF para os referenciados, sem classificação simultânea

#### Scenario: Classificação exclusiva de transações
- **WHEN** um processo elementar é analisado
- **THEN** ele é classificado como exatamente um de EI, EO ou EQ (FR-032)

### Requirement: Complexidade pelas matrizes IFPUG
O motor SHALL classificar complexidade (Low/Average/High) usando as matrizes
RET×DET (dados) e DET×FTR (transações) da RFC 008 (FR-020, FR-021).

#### Scenario: Complexidade de função de dados
- **WHEN** um ILF tem 1 RET e 25 DETs
- **THEN** a complexidade é `Low` conforme a matriz ILF

#### Scenario: Complexidade de transação
- **WHEN** um EI tem 2 FTRs e 20 DETs
- **THEN** a complexidade é `High` conforme a matriz EI

### Requirement: Pesos e cálculo UFP
O motor SHALL atribuir pesos IFPUG (EI 3/4/6, EO 4/5/7, EQ 3/4/6, ILF 7/10/15,
EIF 5/7/10) e calcular UFP como a soma dos pesos (FR-022, FR-023).

#### Scenario: UFP com funções conhecidas
- **WHEN** um modelo com 5 ILFs e 10 EIs é medido
- **THEN** o relatório informa 5 ILFs e 10 EIs com pesos e o UFP igual à soma

### Requirement: VAF e pontos ajustados
Quando habilitado, o motor SHALL calcular TDI (14 GSCs, FR-024/FR-025),
`VAF = 0.65 + 0.01 × TDI` (FR-026) e `AFP = UFP × VAF` (FR-027), indicando
explicitamente se VAF foi aplicado.

#### Scenario: VAF aplicado
- **WHEN** um Rule Pack habilita VAF com TDI=40
- **THEN** VAF=1.05 e AFP=UFP×1.05, com indicação explícita no relatório

#### Scenario: VAF não aplicado
- **WHEN** nenhum Rule Pack habilita VAF
- **THEN** AFP=UFP e o relatório indica que VAF não foi aplicado

### Requirement: Rule Packs externalizados
O motor SHALL aplicar Rule Packs organizacionais antes do resultado final (FR-004),
usando regras IFPUG padrão quando nenhum Rule Pack é fornecido (FR-005).

#### Scenario: Exclusão via Rule Pack
- **WHEN** um Rule Pack exclui EQs da contagem
- **THEN** o resultado exclui as contribuições de EQ sem alterar o modelo de entrada

#### Scenario: Regras padrão
- **WHEN** nenhum Rule Pack é fornecido
- **THEN** o motor aplica as regras IFPUG padrão

### Requirement: Determinismo e explicabilidade
O motor SHALL produzir resultados determinísticos (mesma entrada → mesma saída,
FR-006/FR-008) e preservar referências de evidência para cada função medida
(FR-007), sem executar inferência de LLM.

#### Scenario: Resultado reprodutível
- **WHEN** a mesma medição é executada duas vezes
- **THEN** os resultados são idênticos

#### Scenario: Evidência por função
- **WHEN** um resultado é inspecionado
- **THEN** cada função medida referencia o elemento de origem e a regra aplicada

### Requirement: Tratamento de modelos vazios e referências pendentes
O motor SHALL retornar contagem zero sem erro para modelos vazios (FR-011) e SHALL
reportar warnings para referências não resolvidas, continuando a medição (FR-012).

#### Scenario: Modelo vazio
- **WHEN** o modelo funcional não contém funções
- **THEN** o motor retorna contagem zero sem erro

#### Scenario: Referência não resolvida
- **WHEN** o modelo referencia um grupo de dados inexistente
- **THEN** o motor emite warning e continua com os elementos disponíveis

### Requirement: Relatório de medição
O relatório SHALL incluir fronteira, total UFP, AFP, VAF/TDI quando aplicável,
contagens por tipo, distribuição de complexidade, DET/RET/FTR por função e trilha
de evidência (FR-033).

#### Scenario: Relatório completo
- **WHEN** uma medição é concluída
- **THEN** o relatório contém os campos obrigatórios do FR-033
