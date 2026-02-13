# ADR-004 – Decisões arquiteturais para o agent-service

---

## Status

Aceito

## Contexto

`agent-service` atua como orquestrador entre entrada do usuário, conhecimento RAG e capacidades MCP de contagem.

---

## Decisão

1. O `agent-service` centraliza fluxo de orquestração e não implementa regras de contagem no domínio interno.
2. O serviço chamará capacidades do `mcp-service` e contexto do `rag-service` por contratos explícitos.
3. O fluxo inicial será síncrono para o MVP, com evolução para processamento assíncrono em fases futuras.
4. Toda decisão de orquestração relevante deverá registrar trilha auditável (entrada, chamadas e resultado).

---

## Racional

- Preserva separação de responsabilidades entre orquestração e cálculo.
- Reduz risco de inconsistência entre regras normativas e fluxo agente.
- Acelera entrega incremental mantendo governança de decisão.

---

## Decisões Relacionadas

- ADR-001
