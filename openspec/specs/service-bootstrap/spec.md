# Service Bootstrap

## Purpose

Descreve a estrutura base do MVP: os três serviços (`agent-service`,
`mcp-service`, `rag-service`), a solução raiz, containerização, endpoints de
health-check e a documentação das decisões.

## Requirements

### Requirement: Estrutura dos serviços MVP
O sistema SHALL manter os serviços `agent-service`, `mcp-service` e `rag-service`
em `services/<nome>/`, cada um com projeto de API em `src/` e projeto de testes em
`tests/`, todos referenciados pela solução raiz `agentic-fp-ai-mvp.sln`.

#### Scenario: Solução raiz contém todos os projetos
- **WHEN** a solução raiz `agentic-fp-ai-mvp.sln` é aberta
- **THEN** os projetos de API e de testes dos três serviços estão incluídos

#### Scenario: Estrutura de pastas por serviço
- **WHEN** um serviço é inspecionado
- **THEN** ele possui `src/<Nome>.Api/` e `tests/<Nome>.Api.Tests/`

### Requirement: Containerização dos serviços
Cada serviço SHALL possuir um `Dockerfile` próprio e o projeto SHALL fornecer um
`docker-compose.yml` capaz de subir os três serviços do MVP.

#### Scenario: Build de imagem por serviço
- **WHEN** uma imagem é construída a partir do `Dockerfile` do serviço
- **THEN** o container resultante executa a API do serviço

#### Scenario: Stack local completa
- **WHEN** `docker-compose up` é executado
- **THEN** `agent-service`, `mcp-service` e `rag-service` sobem com as portas mapeadas

### Requirement: Health-check por serviço
Cada serviço SHALL expor um endpoint `GET /health` que retorna o nome do serviço,
o status e o timestamp UTC.

#### Scenario: Health-check do mcp-service
- **WHEN** `GET /health` é chamado no mcp-service
- **THEN** retorna `service=mcp-service`, `status=healthy` e `timestampUtc` atual

#### Scenario: Health-check do rag-service
- **WHEN** `GET /health` é chamado no rag-service
- **THEN** retorna `service=rag-service`, `status=healthy` e `timestampUtc` atual

#### Scenario: Health-check do agent-service
- **WHEN** `GET /health` é chamado no agent-service
- **THEN** retorna `service=agent-service`, `status=healthy` e `timestampUtc` atual

### Requirement: Documentação das decisões
O projeto SHALL manter documentação das decisões arquiteturais em `docs/adr/`
(ADR-001 a ADR-004) e das especificações de medição em `docs/rfc/`
(FPA, SFP e SNAP).

#### Scenario: ADRs disponíveis
- **WHEN** um desenvolvedor consulta `docs/adr/`
- **THEN** encontra os ADRs 001 a 004 com status e decisão

#### Scenario: RFCs de medição disponíveis
- **WHEN** um desenvolvedor consulta `docs/rfc/`
- **THEN** encontra as especificações dos motores FPA, SFP e SNAP
