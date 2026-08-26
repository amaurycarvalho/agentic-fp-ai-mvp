# Security Hardening

## Why

Os requisitos não-funcionais preveem JWT para API pública, TLS obrigatório e logs
criptografados, além de compatibilidade LGPD. Hoje os endpoints são abertos, sem
autenticação, TLS ou proteções adicionais — inconsistente com a Constituição
(Security by design).

## What Changes

- Autenticação JWT na API pública (borda REST externa).
- TLS/HTTPS obrigatório nos serviços (e mTLS opcional na comunicação interna).
- Proteções básicas: CORS restrito, rate limiting, headers de segurança, validação
  de entrada e logging com saneamento (sem dados sensíveis).
- Conformidade LGPD: tratamento de dados pessoais em histórias/relatórios,
  mecanismo de retenção/exclusão e consentimento quando aplicável.
- Testes de segurança (auth, injeção básica, headers).

## Capabilities

### New Capabilities

- `security-hardening`: autenticação JWT na borda pública, TLS, rate limiting,
  headers de segurança, saneamento de logs e conformidade LGPD nos serviços do MVP.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: Program.cs e DI dos três serviços; `appsettings` (Issuer/audience/segredo,
  TLS, limites).
- Dependências: `Microsoft.AspNetCore.Authentication.JwtBearer` (ou similar).
- Infra: configuração de certificados/TLS no compose/CI quando aplicável.
