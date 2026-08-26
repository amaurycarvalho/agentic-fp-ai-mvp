## ADDED Requirements

### Requirement: Health checks de dependências
Cada serviço SHALL expor health checks de liveness e readiness, onde o readiness
verifica as dependências do serviço (serviços internos, banco/vector DB quando aplicável).

#### Scenario: Readiness com dependência disponível
- **WHEN** as dependências do serviço estão disponíveis
- **THEN** `/health/ready` retorna status `Healthy`

#### Scenario: Readiness com dependência indisponível
- **WHEN** uma dependência do serviço está indisponível
- **THEN** `/health/ready` retorna status indicando a falha da dependência

#### Scenario: Liveness do processo
- **WHEN** o processo do serviço está de pé
- **THEN** `/health/live` retorna status `Healthy`

### Requirement: Timeout em chamadas entre serviços
As chamadas HTTP/gRPC entre serviços SHALL possuir timeout configurável, evitando
que uma dependência lenta trave a chamada por tempo indeterminado.

#### Scenario: Timeout aplicado
- **WHEN** uma dependência não responde dentro do timeout
- **THEN** a chamada falha com erro de timeout e a orquestração trata o fallback

### Requirement: Retry com backoff
As chamadas a dependências SHALL possuir política de retry com backoff para falhas
transitórias, limitada a um número máximo de tentativas.

#### Scenario: Retry em falha transitória
- **WHEN** uma chamada falha por causa transitória
- **THEN** ela é retentada com backoff até o limite configurado

#### Scenario: Desistência após limite
- **WHEN** o número máximo de tentativas é atingido
- **THEN** a chamada falha e o erro é propagado/tratado

### Requirement: Testes de resiliência
O projeto SHALL possuir testes cobrindo health-checks com dependência indisponível
e o comportamento de timeout/retry.

#### Scenario: Testes de resiliência
- **WHEN** os testes de resiliência são executados
- **THEN** os cenários de readiness com falha e de retry são validados
