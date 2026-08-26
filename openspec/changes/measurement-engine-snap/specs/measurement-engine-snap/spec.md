## ADDED Requirements

### Requirement: Categorias de avaliação SNAP
O motor SHALL organizar itens avaliados por categorias de avaliação, cada item
pertencendo a exatamente uma categoria, com categorias independentes e extensíveis
por Rule Packs (FR-011 a FR-014).

#### Scenario: Itens por categoria
- **WHEN** um item é avaliado
- **THEN** ele pertence a exatamente uma categoria de avaliação

#### Scenario: Categorias versionadas
- **WHEN** o motor carrega definições de categoria
- **THEN** as categorias são validadas por versão semântica do schema (FR-015)

### Requirement: Identificação de candidatos a avaliação
O motor SHALL identificar candidatos a partir de metadados semânticos do modelo
(marcadores/tags produzidos por estágios anteriores) (FR-016), mesclando duplicados
(FR-017) e ignorando detalhes de tecnologia (FR-018/019).

#### Scenario: Candidato identificado por metadado
- **WHEN** o modelo contém metadados semânticos de apresentação/operação
- **THEN** o motor identifica itens de avaliação correspondentes

#### Scenario: Duplicados mesclados
- **WHEN** candidatos duplicados são detectados
- **THEN** eles são mesclados em um único item avaliado

### Requirement: Contribuição e relatório
Cada item avaliado SHALL contribuir independentemente para o total SNAP, com
exclusões reportadas e não-resolvidos como warnings (FR-020 a FR-024), e o resultado
SHALL ser agregado por categoria.

#### Scenario: Total por categoria
- **WHEN** a avaliação é concluída
- **THEN** o resultado informa o total SNAP e o valor por categoria

#### Scenario: Candidato não resolvido
- **WHEN** um candidato não pode ser resolvido
- **THEN** ele é reportado como warning sem interromper a avaliação

### Requirement: Rule Packs para SNAP
Rule Packs SHALL poder excluir categorias ou itens individuais e redefinir políticas
de inclusão (FR-025 a FR-027), sem alterar a execução determinística (FR-028), com
ajustes reportados (FR-029).

#### Scenario: Exclusão de categoria
- **WHEN** um Rule Pack exclui uma categoria
- **THEN** os itens dessa categoria não contribuem e a exclusão é reportada

### Requirement: Explicabilidade por item
Cada item avaliado SHALL expor elemento de origem, categoria, Rule Pack aplicado,
contribuição e evidências (FR-030), com evidência imutável que sobrevive à
exportação (FR-031/032).

#### Scenario: Evidência por item
- **WHEN** um resultado é inspecionado
- **THEN** cada item inclui origem, categoria, contribuição e evidências

### Requirement: Determinismo e reprodutibilidade
Execuções repetidas SHALL produzir resultados byte-idênticos (SC-001) e 100% dos
itens avaliados SHALL conter evidências (SC-002).

#### Scenario: Resultado byte-idêntico
- **WHEN** a mesma avaliação é executada duas vezes
- **THEN** os resultados são byte-idênticos
