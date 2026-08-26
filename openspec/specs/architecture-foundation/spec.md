# Architecture Foundation

## Purpose

Define os princípios arquiteturais imutáveis do sistema (DDD, Clean Architecture,
microserviços, C#/.NET, SDD, IA agêntica desacoplada via MCP/RAG) e as regras de
auditoria e rastreabilidade aplicáveis a todos os serviços.

## Requirements

### Requirement: Decisões arquiteturais fundamentais
O sistema SHALL seguir os princípios arquiteturais definidos no ADR-001 e na
Constituição do projeto: DDD, Clean Architecture, microserviços independentes,
C#/.NET como stack principal, Spec Driven Development (SDD), IA agêntica
desacoplada via MCP e RAG, TDD/BDD para histórias de usuário, containers isolados
por serviço, REST externo e gRPC interno.

#### Scenario: Princípios aplicados na estrutura do projeto
- **WHEN** um novo serviço ou módulo é criado
- **THEN** ele segue as camadas DDD/Clean Architecture e as decisões dos ADRs vigentes

#### Scenario: Conhecimento não executa ações
- **WHEN** o rag-service fornece contexto a outro serviço
- **THEN** ele apenas recupera conhecimento e não executa ações de negócio

### Requirement: Separar serviços por responsabilidade
O sistema SHALL manter o `agent-service` (orquestração), o `mcp-service`
(capacidades determinísticas de contagem) e o `rag-service` (consulta de
conhecimento normativo) como microserviços independentes, cada um com projeto de
aplicação (`src/`) e projeto de testes (`tests/`) próprios.

#### Scenario: Estrutura de projeto por serviço
- **WHEN** um desenvolvedor abre o repositório
- **THEN** cada serviço em `services/<nome>/` contém `src/` e `tests/`

### Requirement: Especificações precedem código
O sistema SHALL seguir Spec Driven Development: toda funcionalidade é precedida
por especificação (user story com critérios de aceite e cenários BDD).

#### Scenario: Histórias de usuário documentadas
- **WHEN** uma funcionalidade é planejada
- **THEN** existe uma user story com história, critérios de aceite e cenários BDD em `docs/rfc/user-stories/`

### Requirement: Auditoria e rastreabilidade de decisão
Serviços SHALL registrar trilha auditável de decisões relevantes para permitir
auditoria (Constitution: auditabilidade, rastreabilidade, reprodutibilidade).

#### Scenario: Decisão registrada
- **WHEN** um serviço toma uma decisão de contagem ou orquestração
- **THEN** a decisão fica registrada em trilha auditável com justificativa
