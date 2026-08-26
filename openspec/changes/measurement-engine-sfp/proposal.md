# Measurement Engine SFP

## Why

A RFC 017 (`docs/rfc/017-measurement-engine-sfp`) especifica o motor de Simple
Function Points: mede apenas dois componentes (Logical Functions e Functional
Processes), sem DET/RET/FTR, com valores fixos, Rule Packs e total determinístico.
Não existe implementação hoje.

## What Changes

- Implementar o motor SFP conforme a RFC 017: identificação de Logical Functions e
  Functional Processes por matching no modelo canônico (FR-011 a FR-018), valores
  fixos de contribuição (FR-019/FR-020), merge de duplicados por fingerprint
  (FR-017/018), sem classificação de complexidade (FR-021 a FR-027).
- Aplicar Rule Packs (exclusões) e relatório com evidência (FR-033 a FR-035).
- Observabilidade mínima (structured logs, FR-041) e exposição como capacidade.

## Capabilities

### New Capabilities

- `measurement-engine-sfp`: motor determinístico de Simple Function Points —
  medição baseada apenas em contagem de componentes funcionais reconhecidos, com
  merge de duplicados, Rule Packs e explicabilidade, conforme RFC 017.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `mcp-service` (domínio SFP e endpoint `POST /measure/sfp`) ou módulo
  compartilhado; testes.
- Não requer DET/RET/FTR nem LLM; totalmente determinístico.
