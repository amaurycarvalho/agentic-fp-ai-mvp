## ADDED Requirements

### Requirement: Identificação de componentes SFP
O motor SFP SHALL identificar Logical Functions e Functional Processes a partir do
modelo funcional por matching de tipo/atributo (FR-011 a FR-016).

#### Scenario: Identificação dos dois componentes
- **WHEN** o modelo contém grupos de dados e processos elementares
- **THEN** o motor identifica Logical Functions e Functional Processes como componentes medidos

#### Scenario: Processos parciais não medidos
- **WHEN** uma operação de negócio é parcial
- **THEN** ela não é medida como componente independente (FR-016)

### Requirement: Merge de duplicados por fingerprint
O motor SHALL mesclar componentes duplicados usando ID do nó e fingerprint de
conteúdo (SHA-256 de document_id, section_id, text, semantic_type) (FR-017/018).

#### Scenario: Componentes duplicados
- **WHEN** dois nós equivalentes representam o mesmo componente
- **THEN** eles são mesclados em um único componente medido

### Requirement: Medição sem complexidade
O motor SHALL contribuir um valor fixo por componente identificado e SHALL NOT
classificar complexidade nem calcular DET/RET/FTR (FR-019 a FR-027).

#### Scenario: Total por contagem de componentes
- **WHEN** a medição é executada
- **THEN** o total SFP depende apenas do número de componentes reconhecidos

#### Scenario: Sem cálculo de DET/RET/FTR
- **WHEN** um componente é medido
- **THEN** nenhum DET, RET ou FTR é calculado

### Requirement: Rule Packs para exclusões
Rule Packs SHALL poder excluir Functional Processes e Logical Functions (FR-028/029)
e redefinir critérios de inclusão (FR-030), sem modificar o algoritmo determinístico
(FR-031), com ajustes reportados (FR-032).

#### Scenario: Exclusão via Rule Pack
- **WHEN** um Rule Pack exclui determinados processos
- **THEN** eles não contribuem e a exclusão é reportada

### Requirement: Explicabilidade e evidência
Cada componente medido SHALL expor o nó de origem, a especificação, a regra aplicada
e a contribuição (FR-033), com evidência imutável que sobrevive à exportação
(FR-034/035).

#### Scenario: Evidência por componente
- **WHEN** um resultado é inspecionado
- **THEN** cada componente inclui origem, regra e contribuição

### Requirement: Resultado e reprodutibilidade
O resultado SHALL incluir método, versão, timestamp, contagens, total, Rule Packs,
evidências, warnings e estatísticas; execuções repetidas SHALL produzir resultados
idênticos (SC-001).

#### Scenario: Resultado completo e determinístico
- **WHEN** a mesma medição é executada duas vezes
- **THEN** os resultados são idênticos e contêm os campos do resultado SFP
