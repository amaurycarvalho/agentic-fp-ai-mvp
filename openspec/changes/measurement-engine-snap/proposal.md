# Measurement Engine SNAP

## Why

A RFC 018 (`docs/rfc/018-measurement-engine-snap`) especifica o motor SNAP para
medir tamanho funcional não-funcional (formatação, apresentação, capacidades
operacionais, interação técnica), com categorias de avaliação, candidatos a partir
de metadados semânticos do modelo, Rule Packs e determinismo. Não existe
implementação hoje.

## What Changes

- Implementar o motor SNAP conforme a RFC 018: categorias de avaliação versionadas
  (FR-011 a FR-015), identificação de candidatos por metadados semânticos (FR-016 a
  FR-024), contribuição independente por item, Rule Packs (exclusões) e relatório
  com evidência por categoria.
- Merged de candidatos duplicados e warnings para não resolvidos (FR-017/023/024).
- Exposição como capacidade e testes (SC-001 a SC-007).

## Capabilities

### New Capabilities

- `measurement-engine-snap`: motor determinístico de avaliação SNAP — categorias de
  avaliação, itens avaliados a partir de metadados do modelo, Rule Packs e
  explicabilidade por categoria, conforme RFC 018.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `mcp-service` (domínio SNAP e endpoint `POST /measure/snap`) reusando a
  infraestrutura comum de medição; testes.
- Não requer LLM; determinístico e reprodutível.
