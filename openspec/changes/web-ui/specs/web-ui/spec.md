## ADDED Requirements

### Requirement: Submissão de história de usuário
A UI SHALL oferecer um formulário para submeter uma história de usuário e exibir o
resultado da contagem (funções, complexidade, pontos e trilha de auditoria).

#### Scenario: Contagem pela UI
- **WHEN** o usuário submete uma história de usuário
- **THEN** a UI exibe o resultado da contagem com a trilha de auditoria

#### Scenario: Erro de validação
- **WHEN** o formulário é submetido sem história
- **THEN** a UI exibe mensagem de validação e não chama a API

### Requirement: Consulta de contexto normativo
A UI SHALL permitir consultar o contexto normativo (RAG) e exibir os trechos com
metadados de origem.

#### Scenario: Consulta pela UI
- **WHEN** o usuário consulta um termo/regra
- **THEN** a UI exibe os trechos relevantes e os metadados de origem

### Requirement: Visualização de medições
A UI SHALL exibir o resultado de medições (fpa/sfp/snap) com totais, distribuição e
evidências.

#### Scenario: Medição exibida
- **WHEN** o usuário visualiza uma medição
- **THEN** a UI exibe totais, distribuição por tipo/categoria e evidências

### Requirement: Autenticação na UI
A UI SHALL suportar autenticação JWT (login ou token) e enviar o token nas chamadas
à API.

#### Scenario: Acesso autenticado
- **WHEN** o usuário autentica na UI
- **THEN** as chamadas à API incluem o token e expirações são tratadas (re-login)

### Requirement: Status dos serviços
A UI SHALL exibir o status dos serviços (health) de forma clara.

#### Scenario: Status exibido
- **WHEN** a UI consulta o health dos serviços
- **THEN** o status de cada serviço é exibido

### Requirement: Testes da UI
A UI SHALL possuir testes dos componentes/páginas principais (contagem, contexto,
autenticação e status).

#### Scenario: Testes da interface
- **WHEN** os testes da UI são executados
- **THEN** os cenários de contagem, contexto, autenticação e status passam
