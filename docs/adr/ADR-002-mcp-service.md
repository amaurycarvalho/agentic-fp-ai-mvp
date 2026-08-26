# ADR-002 – Decisões arquiteturais para o mcp-service

---

## Status

Aceito

## Contexto

Serviço MCP de Contagem responsável por expor capacidades determinísticas para classificação e cálculo de Pontos de Função.

---

## Decisão

1. O `mcp-service` será um microserviço dedicado a regras determinísticas de contagem (sem dependência de LLM para decisão de regra).
2. A primeira entrega terá API HTTP mínima para bootstrap e health-check, evoluindo para contratos gRPC internos.
3. O núcleo de contagem seguirá separação em camadas (Domain/Application/Infrastructure), preservando testabilidade.
4. O serviço deverá produzir justificativas auditáveis por regra aplicada (rastreabilidade de decisão).

---

## Racional

- Reduz acoplamento entre IA generativa e cálculo normativo.
- Mantém previsibilidade e reprodutibilidade para auditoria.
- Facilita evolução incremental de regras com TDD.

---

## Decisões Relacionadas

- ADR-001
