## ADDED Requirements

### Requirement: Comando de contagem
O CLI SHALL permitir submeter uma história de usuário via comando `count` e exibir
o resultado da contagem (funções, complexidade, pontos e trilha).

#### Scenario: Contagem via CLI
- **WHEN** o usuário executa o comando `count` com uma história de usuário
- **THEN** o CLI chama a API e exibe o resultado da contagem

#### Scenario: Erro da API exibido
- **WHEN** a API retorna erro
- **THEN** o CLI exibe mensagem de erro clara e código de saída não-zero

### Requirement: Comando de contexto
O CLI SHALL permitir consultar contexto normativo no RAG via comando `context`.

#### Scenario: Consulta de contexto
- **WHEN** o usuário executa o comando `context` com uma consulta
- **THEN** o CLI exibe os trechos relevantes com metadados de origem

### Requirement: Comando de medição
O CLI SHALL permitir invocar os motores de medição via comando `measure` com o
método desejado (ex.: `fpa`, `sfp`, `snap`).

#### Scenario: Medição via CLI
- **WHEN** o usuário executa o comando `measure --method fpa`
- **THEN** o CLI exibe o resultado da medição do motor solicitado

### Requirement: Comando de health-check
O CLI SHALL permitir verificar o status dos serviços via comando `health`.

#### Scenario: Health-check via CLI
- **WHEN** o usuário executa o comando `health`
- **THEN** o CLI exibe o status dos serviços configurados

### Requirement: Output legível e JSON
O CLI SHALL oferecer output legível por padrão e formato JSON (`--json`) para
consumo programático.

#### Scenario: Saída JSON
- **WHEN** o usuário executa um comando com `--json`
- **THEN** a saída é um documento JSON estruturado

### Requirement: Autenticação configurável
O CLI SHALL suportar token JWT configurável (via argumento/env) para autenticar nas
chamadas à API pública.

#### Scenario: Token informado
- **WHEN** o usuário informa um token (argumento/env)
- **THEN** o CLI inclui o token nas chamadas autenticadas

### Requirement: Testes do CLI
O CLI SHALL possuir testes unitários das operações (contagem, contexto, medição,
health e parsing de saída).

#### Scenario: Testes das operações
- **WHEN** os testes do CLI são executados
- **THEN** os cenários de contagem, contexto, medição e health passam
