# Design: Security Hardening

## Context

NFRs exigem JWT na API pública, TLS obrigatório e logs criptografados, com
compatibilidade LGPD. Os serviços estão abertos hoje. Constituição: Security by
design, alertar sobre vulnerabilidades conhecidas (OWASP/CISA).

## Goals / Non-Goals

**Goals:**
- JWT na borda pública; health-checks públicos.
- TLS (e mTLS opcional interno); rate limiting; headers de segurança.
- Saneamento de logs e conformidade LGPD mínima.

**Non-Goals:**
- Implementar um IdP/authorization server — o MVP autentica JWT emitido por IdP existente.
- Criptografia completa de logs em repouso — documentada como requisito de ambiente;
  garantida sanitização e TLS em trânsito.
- AuthZ complexa (RBAC completo) — claims básicos de role/scope.

## Decisions

### D1 — JWT Bearer no ASP.NET Core
`AddAuthentication(JwtBearerDefaults)` configurado com `Authority`/`Audience`
(ou chave simétrica para dev). Health-checks (`/health*`) marcados como `AllowAnonymous`.
- **Alternativa**: API Key — rejeitada (NFR exige JWT).

### D2 — TLS configurável
`HttpsRedirection` + `ForwardedHeaders` atrás de proxy; certificados via
`appsettings`/env. mTLS opcional (interno) documentado e desligado por default.

### D3 — Rate limiting via middleware nativo (`AddRateLimiter`)
Limites por IP/chave em `appsettings`; política global com `429` default.

### D4 — Headers de segurança via middleware
`UseSecurityHeaders` customizado (CSP, nosniff, referrer-policy, HSTS em prod).

### D5 — Saneamento de logs
Convenção de logging: redactores para campos sensíveis (`password`, `token`,
`cpf`, e-mail) e sanitização de entradas antes do log.

### D6 — LGPD mínima
Metadados de retenção nos relatórios; endpoint/mecanismo de exclusão de dados
pessoais quando aplicável; minimização por design.

## Risks / Trade-offs

- **[JWT exige IdP]** → No MVP, dev usa chave local (dev token); prod aponta
  Authority real. Documentar.
- **[TLS local com certificado autoassinado]** → Ambientes dev usam http com
  override documentado; prod exige TLS.

## Migration Plan

1. JWT nos 3 serviços; proteger endpoints de negócio.
2. TLS/HttpsRedirection + ForwardedHeaders.
3. Rate limiting + headers.
4. Saneamento de logs.
5. LGPD: retenção/exclusão.
6. Testes de segurança.

## Open Questions

- IdP real a ser integrado (Ollama/OpenAI não cobre; avaliar Keycloak local).
