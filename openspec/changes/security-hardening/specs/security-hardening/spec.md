## ADDED Requirements

### Requirement: Autenticação JWT na API pública
A borda REST externa SHALL exigir JWT válido para acesso aos endpoints de negócio
(exceto health-checks públicos), com emissor/audiência e expiração configuráveis.

#### Scenario: Requisição sem token
- **WHEN** um endpoint de negócio é chamado sem JWT válido
- **THEN** a resposta é `401 Unauthorized`

#### Scenario: Token válido
- **WHEN** um endpoint de negócio é chamado com JWT válido
- **THEN** a requisição é autorizada conforme o claim de role/scope

### Requirement: TLS obrigatório
Os serviços SHALL operar sobre TLS/HTTPS (borda externa), com certificados
configuráveis; a comunicação interna SHALL poder exigir mTLS quando habilitado.

#### Scenario: HTTPS na borda
- **WHEN** um consumidor acessa a API pública
- **THEN** a comunicação é criptografada via TLS

### Requirement: Rate limiting e headers de segurança
Os serviços SHALL aplicar rate limiting configurável e headers de segurança
(Content-Security-Policy, X-Content-Type-Options, etc.) nas respostas.

#### Scenario: Rate limit excedido
- **WHEN** um cliente excede o limite de requisições
- **THEN** a resposta é `429 Too Many Requests`

#### Scenario: Headers de segurança presentes
- **WHEN** uma resposta HTTP é emitida
- **THEN** os headers de segurança configurados estão presentes

### Requirement: Saneamento de logs e dados sensíveis
Os serviços SHALL evitar registrar dados sensíveis (credenciais, dados pessoais)
em logs e SHALL sanear entradas antes do log.

#### Scenario: Dados sensíveis não logados
- **WHEN** uma requisição contém dados sensíveis
- **THEN** os logs não contêm esses dados em claro

### Requirement: Conformidade LGPD
O sistema SHALL tratar dados pessoais presentes em histórias/relatórios conforme
LGPD: minimização, retenção definida e mecanismo de exclusão quando aplicável.

#### Scenario: Dados pessoais minimizados
- **WHEN** um relatório é gerado com dados pessoais
- **THEN** apenas os dados necessários são usados, com política de retenção/exclusão definida

### Requirement: Testes de segurança
O projeto SHALL possuir testes de autenticação (401/autorizado), rate limiting e
headers de segurança.

#### Scenario: Testes de segurança
- **WHEN** os testes de segurança são executados
- **THEN** os cenários de 401, autorização, 429 e headers passam
