## ADDED Requirements

### Requirement: Instrumentação OpenTelemetry
Os três serviços SHALL ser instrumentados com OpenTelemetry para emitir traces,
métricas e logs estruturados.

#### Scenario: Traces nas requisições
- **WHEN** uma requisição é processada
- **THEN** um trace é emitido com os spans das etapas internas

#### Scenario: Métricas de HTTP
- **WHEN** requisições HTTP são atendidas
- **THEN** métricas de duração e contagem são emitidas

### Requirement: Métricas de negócio
O `mcp-service` SHALL emitir métricas de duração de medição e contagens de
componentes/funções medidas.

#### Scenario: Métricas de medição
- **WHEN** uma medição é concluída
- **THEN** um histograma de duração e gauges de contagem são emitidos

### Requirement: Logs estruturados com correlação
Os serviços SHALL emitir logs estruturados (INFO/ERROR) com contexto de correlação
(trace/correlation id) para rastreabilidade entre serviços.

#### Scenario: Correlação entre serviços
- **WHEN** uma chamada cruza o agente, o MCP e o RAG
- **THEN** os logs das etapas compartilham o identificador de correlação

### Requirement: Endpoint de métricas Prometheus
Os serviços SHALL expor um endpoint de métricas no formato Prometheus (ex.: `/metrics`).

#### Scenario: Métricas exportáveis
- **WHEN** o endpoint de métricas é consultado
- **THEN** as métricas são retornadas no formato Prometheus

### Requirement: Configuração de exportadores
A instrumentação SHALL ser configurável via configuração (sem código), permitindo
habilitar exportador OTLP e/ou Prometheus por ambiente.

#### Scenario: Exportador por ambiente
- **WHEN** a configuração define um exportador
- **THEN** os sinais são enviados ao destino configurado sem alterar o código
