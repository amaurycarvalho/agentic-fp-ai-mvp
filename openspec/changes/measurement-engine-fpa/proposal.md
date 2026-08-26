# Measurement Engine FPA

## Why

A RFC 008 (`docs/rfc/008-measurement-engine-fpa`) especifica um motor de medição
IFPUG/FPA completo, determinístico e explicável. Hoje o `mcp-service` possui apenas
o protótipo `mcp-basic-count` (classificação por palavras-chave), sem as regras
formais de identificação, matrizes de complexidade RET/DET e DET/FTR, pesos IFPUG,
UFP/AFP/VAF e trilha de evidência.

## What Changes

- Implementar o motor FPA conforme a RFC 008: identificação de ILF/EIF/EI/EO/EQ,
  complexidade (matrizes), pesos IFPUG (FR-022), UFP/AFP/VAF (FR-023 a FR-027),
  regras de contagem (DET/RET/FTR, fronteira) e relatório com evidência.
- Aplicar Rule Packs (externalização de políticas) e resultado determinístico.
- Expor o motor como capacidade do `mcp-service` (via gRPC/HTTP) com testes
  (FR-008/017/018 sc: reprodução de exemplos IFPUG, matrizes, SC-008 a SC-014).

## Capabilities

### New Capabilities

- `measurement-engine-fpa`: motor determinístico de medição IFPUG/FPA — cinco tipos
  de função, classificação de complexidade, pesos, UFP/AFP/VAF, Rule Packs,
  explicabilidade e reprodutibilidade, conforme RFC 008.

### Modified Capabilities

- `mcp-basic-count`: o protótipo de contagem básica evolui para consumir o motor
  FPA; o endpoint `POST /count/basic` permanece como contrato legado/compatibilidade.

## Impact

- Código: `mcp-service` (novo domínio de medição FPA, portas, aplicação, endpoint/gRPC).
- Dependências: nenhuma externa nova além das já usadas (motor 100% determinístico).
- Testes: unitários por regra + reprodução de exemplos IFPUG CPM 4.3 (SC-008).
