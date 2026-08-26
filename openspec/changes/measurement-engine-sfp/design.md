# Design: Measurement Engine SFP

## Context

RFC 017 especifica o SFP: modelo reduzido com dois componentes (Logical Function e
Functional Process), sem complexidade, valores fixos, merge por fingerprint,
Rule Packs e determinismo. Compartilha infraestrutura comum com os demais motores
(FPA, SNAP), diferindo em regras e algoritmo.

## Goals / Non-Goals

**Goals:**
- Motor SFP determinístico com merge de duplicados.
- Rule Packs e relatório explicável.
- Reuso da infraestrutura comum de medição.

**Non-Goals:**
- FPA e SNAP — changes próprias.
- Extração semântica/LLM — o motor não invoca LLMs.
- Exportação e estimativa de esforço/custo (RFC 017 Non-Goals).

## Decisions

### D1 — Infraestrutura comum de medição
Criar base compartilhada (modelo canônico, `RulePack`, `MeasurementResult`,
evidência) reutilizável por FPA/SFP/SNAP. SFP implementa apenas identificação e
valores fixos.
- **Alternativa**: duplicar por motor — rejeitada (RFC 017 "shared infrastructure").

### D2 — Valores fixos configuráveis
Valores de contribuição de Logical Function e Functional Process definidos como
configuração/constantes (não no algoritmo), conforme FR-019/FR-020.

### D3 — Fingerprint para merge
Fingerprint = SHA-256 de `document_id|section_id|text|semantic_type`; merge por
`node_id` + fingerprint (FR-017/018) antes do cálculo.

### D4 — Endpoint `POST /measure/sfp`
Expõe o motor como capacidade do `mcp-service` (mesmo padrão do FPA). Retorna o
resultado com contagens, total, rule packs, evidências e warnings.

## Risks / Trade-offs

- **[Qualidade depende da extração semântica]** → Motor é determinístico; precisão
  do CFM afeta a medição (assumido na RFC).
- **[Total com decimais no exemplo da RFC]** → Suportar valores fixos com precisão
  configurável (ex.: 319.2 no exemplo).

## Migration Plan

1. Criar infraestrutura comum de medição.
2. Implementar identificação + merge.
3. Implementar valores fixos e total.
4. Integrar Rule Packs.
5. Endpoint + testes (SC-001/SC-002/SC-004).

## Open Questions

- Valores fixos padrão (o exemplo usa totais com decimal).
