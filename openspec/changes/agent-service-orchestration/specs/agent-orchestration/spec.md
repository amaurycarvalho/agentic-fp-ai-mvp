## ADDED Requirements

### Requirement: Fluxo orquestrado de contagem
O `agent-service` SHALL expor um endpoint que recebe uma solicitação única de
contagem e SHALL orquestrar: consulta de contexto no `rag-service` quando
necessário e acionamento do `mcp-service` para o cálculo determinístico.

#### Scenario: Contagem orquestrada
- **WHEN** uma solicitação de contagem é recebida pelo agente
- **THEN** o agente consulta contexto no RAG e executa o cálculo no MCP

#### Scenario: Resposta consolidada
- **WHEN** a orquestração é concluída
- **THEN** o agente retorna um resultado consolidado com as evidências das etapas

### Requirement: Consulta de contexto opcional
O `agent-service` SHALL consultar o `rag-service` quando o contexto for necessário
para a análise, sem tornar a contagem dependente da recuperação quando não houver contexto.

#### Scenario: Contexto indisponível
- **WHEN** o RAG não retorna contexto relevante
- **THEN** a orquestração continua com o cálculo determinístico do MCP

### Requirement: Trilha auditável da orquestração
O `agent-service` SHALL registrar trilha auditável com a entrada recebida, as
chamadas realizadas (RAG/MCP) e o resultado final.

#### Scenario: Trilha registrada
- **WHEN** uma orquestração é executada
- **THEN** a resposta inclui rastreabilidade de entrada, chamadas e resultado

### Requirement: Clientes desacoplados
O `agent-service` SHALL consumir o RAG e o MCP por interfaces definidas em sua
camada de aplicação, com URLs base configuráveis, permitindo testes com fakes.

#### Scenario: URLs configuráveis
- **WHEN** o agente é configurado com as URLs base dos serviços
- **THEN** ele usa essas URLs para as chamadas de contexto e contagem

### Requirement: Testes unitários da orquestração
O `agent-service` SHALL possuir testes unitários reais cobrindo o fluxo completo e
o cenário de contexto indisponível.

#### Scenario: Testes do orquestrador
- **WHEN** os testes unitários do agent-service são executados
- **THEN** os cenários de orquestração completa e de contexto indisponível passam
