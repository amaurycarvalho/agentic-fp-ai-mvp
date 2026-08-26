# Design: Measurement Engine SNAP

## Context

RFC 018 especifica o SNAP para tamanho funcional não-funcional, com categorias de
avaliação e itens a partir de metadados semânticos do modelo. Complementa FPA/SFP
sem substituí-los. Reusa a infraestrutura comum de medição (modelo, RulePacks,
resultado, evidência).

## Goals / Non-Goals

**Goals:**
- Categorias de avaliação versionadas e itens avaliados.
- Rule Packs, warnings e explicabilidade.
- Determinismo (byte-idêntico).

**Non-Goals:**
- FPA e SFP — changes próprias.
- Extração semântica/LLM — consome metadados já produzidos.
- Estimativa de esforço/custo (RFC 018 Non-Goals).

## Decisions

### D1 — Categorias versionadas (SemVer)
Definições de categoria com schema versionado e validado no load (FR-015). Regras
de contribuição por categoria declaradas como dados.
- **Alternativa**: categorias hardcoded — rejeitada (FR-014 extensibilidade).

### D2 — Candidatos por metadados semânticos
Identificação por `semantic_type`/tags do modelo (FR-016). Merge por ID+fingerprint
reusando o utilitário da infraestrutura comum (mesmo padrão do SFP).

### D3 — Endpoint `POST /measure/snap`
Expõe o motor no `mcp-service`. Resultado agrega por categoria e reporta exclusões
e warnings.

## Risks / Trade-offs

- **[Dependência de metadados semânticos do CFM]** → O motor valida a presença dos
  marcadores e reporta warnings (FR-024); sem metadados, avaliação vazia sem erro.

## Migration Plan

1. Criar modelo de categorias versionadas.
2. Implementar identificação/merge de candidatos.
3. Implementar contribuição por categoria e relatório.
4. Integrar Rule Packs.
5. Endpoint + testes (SC-001/SC-002).

## Open Questions

- Conjunto inicial de categorias (ex.: Presentation, Data Operations) e valores.
