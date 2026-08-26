## ADDED Requirements

### Requirement: Consulta à base normativa
O `rag-service` SHALL expor um endpoint de busca que recebe uma consulta textual e
retorna os trechos mais relevantes recuperados da base normativa.

#### Scenario: Recuperação por consulta textual
- **WHEN** uma consulta sobre uma regra de contagem é enviada
- **THEN** o serviço retorna os trechos mais relevantes para a consulta

#### Scenario: Consulta sem resultados
- **WHEN** nenhum trecho relevante é encontrado
- **THEN** a resposta indica resultado vazio sem erro

### Requirement: Metadados de origem para auditoria
Cada trecho retornado SHALL incluir metadados mínimos da fonte consultada
(documento, seção e referência) para permitir auditoria.

#### Scenario: Metadados na resposta
- **WHEN** trechos são retornados
- **THEN** cada trecho inclui metadados de origem da fonte consultada

### Requirement: Conhecimento não executa ações
O `rag-service` SHALL apenas recuperar conhecimento e SHALL NOT executar ações de
negócio (princípio constitucional: conhecimento não executa ações).

#### Scenario: Serviço não executa ações
- **WHEN** o rag-service recebe uma consulta
- **THEN** ele apenas recupera e retorna conhecimento, sem efeitos colaterais de negócio

### Requirement: Porta de infraestrutura desacoplada
O `rag-service` SHALL definir uma porta de infraestrutura (interface) para a
recuperação vetorial, permitindo trocar o provedor sem alterar o domínio.

#### Scenario: Provedor substituível
- **WHEN** o provedor de recuperação é trocado
- **THEN** a camada de aplicação não é alterada, apenas a implementação da porta

### Requirement: Testes unitários do contrato de consulta
O `rag-service` SHALL possuir testes unitários reais cobrindo a consulta, os
metadados e a ausência de resultados.

#### Scenario: Testes do serviço RAG
- **WHEN** os testes unitários do rag-service são executados
- **THEN** os cenários de consulta com e sem resultados e de metadados passam
