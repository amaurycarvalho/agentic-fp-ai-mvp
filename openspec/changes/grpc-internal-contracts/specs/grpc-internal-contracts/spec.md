## ADDED Requirements

### Requirement: Contratos gRPC para capacidades internas
O sistema SHALL expor as capacidades internas por contratos gRPC: contagem
determinística (`mcp-service`), recuperação de contexto (`rag-service`) e
orquestração (`agent-service`).

#### Scenario: Contagem via gRPC
- **WHEN** o agent-service chama o mcp-service via gRPC
- **THEN** a contagem determinística é executada e o resultado é retornado no mesmo transporte

#### Scenario: Contexto via gRPC
- **WHEN** o agent-service chama o rag-service via gRPC
- **THEN** a recuperação de contexto é executada e os trechos são retornados com metadados

### Requirement: Borda externa permanece REST
O sistema SHALL manter REST (HTTP) para a borda externa pública, usando gRPC apenas
na comunicação interna entre serviços.

#### Scenario: REST na borda
- **WHEN** um consumidor externo chama a API pública
- **THEN** a comunicação é REST, independentemente dos contratos internos gRPC

### Requirement: Contratos versionados e auditable
Os protos SHALL declarar versão do serviço e campos de correlação para
rastreabilidade das chamadas internas.

#### Scenario: Correlação nas chamadas internas
- **WHEN** uma chamada gRPC interna é feita
- **THEN** o metadado de correlação é propagado e registrado na trilha

### Requirement: Testes dos contratos gRPC
Os serviços gRPC SHALL possuir testes cobrindo as chamadas de contagem e de
recuperação de contexto.

#### Scenario: Testes dos servidores gRPC
- **WHEN** os testes dos serviços gRPC são executados
- **THEN** as chamadas de contagem e de contexto retornam resultados esperados
