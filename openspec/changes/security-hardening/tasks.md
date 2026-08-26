# Tasks: Security Hardening

## 1. Autenticação JWT

- [ ] 1.1 Adicionar pacote JwtBearer e configurar `AddAuthentication` nos três serviços
- [ ] 1.2 Proteger endpoints de negócio; manter health-checks públicos
- [ ] 1.3 Configurar Authority/Audience (ou chave dev) via `appsettings`

## 2. TLS e proxy

- [ ] 2.1 Configurar `HttpsRedirection` e `ForwardedHeaders`
- [ ] 2.2 Documentar e configurar mTLS opcional na comunicação interna
- [ ] 2.3 Configurar certificados por ambiente

## 3. Rate limiting e headers

- [ ] 3.1 Adicionar `AddRateLimiter` com limites configuráveis
- [ ] 3.2 Adicionar middleware de headers de segurança (CSP, nosniff, HSTS)
- [ ] 3.3 Garantir resposta `429` quando o limite é excedido

## 4. Saneamento e LGPD

- [ ] 4.1 Implementar redatores de dados sensíveis nos logs
- [ ] 4.2 Sanear entradas antes do log
- [ ] 4.3 Definir política de retenção e mecanismo de exclusão de dados pessoais (LGPD)

## 5. Testes

- [ ] 5.1 Testes de autenticação (401 e autorizado)
- [ ] 5.2 Testes de rate limiting (429)
- [ ] 5.3 Testes de headers de segurança
- [ ] 5.4 Garantir `dotnet test` passando
