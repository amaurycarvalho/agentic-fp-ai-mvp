# Design: Measurement Engine FPA

## Context

RFC 008 especifica o motor FPA completo: consumo de modelo funcional canônico,
identificação das cinco funções, matrizes de complexidade, pesos IFPUG, UFP/AFP,
Rule Packs, determinismo e explicabilidade. O `mcp-service` hoje tem apenas o
protótipo keyword-based (`mcp-basic-count`).

## Goals / Non-Goals

**Goals:**
- Motor determinístico com as regras FPA da RFC 008 (FR-001 a FR-033).
- Relatório com evidência e reprodutibilidade (SC-008 a SC-014).
- Exposição via capacidade do mcp-service (HTTP e depois gRPC).

**Non-Goals:**
- SFP e SNAP — changes próprias (`measurement-engine-sfp/snap`).
- Exportação (JSON/CSV/XML) — fora do escopo da RFC (F10).
- Semântica/extração de LLM — o motor é puramente determinístico.

## Decisions

### D1 — Domínio de medição dedicado no mcp-service
Criar `Domain` de medição FPA (`FunctionType`, `ComplexityMatrix`, `ElementaryProcess`,
`DataGroup`, `WeightTable`, `FpaMeasurement`), seguindo Clean Architecture e DDD.
- **Alternativa**: estender o protótipo — rejeitada (regras completas exigem modelo rico).

### D2 — Matrizes e pesos como dados, não código
Matrizes RET×DET / DET×FTR e a tabela de pesos são dados declarativos
(constantes/tabelas) para auditoria e reprodução de exemplos IFPUG (SC-008).

### D3 — `RulePack` externalizado
Contrato `IRulePack` com políticas (exclusões, limiares customizados, habilitação de
VAF/GSC). Engine aplica antes do resultado; ajustes documentados no relatório.
- **Alternativa**: regras hardcoded — rejeitada (FR-004/IX).

### D4 — Porta de entrada do motor
Camada de aplicação expõe `IFpaMeasurementService` consumindo `CanonicalFunctionalModel`
+ opcional `RulePack`. Endpoints HTTP existentes ganham um novo endpoint
`POST /measure/fpa`; o protótipo `POST /count/basic` é mantido como compatibilidade.

## Risks / Trade-offs

- **[Complexidade da RFC]** → Implementação incremental por FR, cada uma com testes;
  reprodução dos exemplos oficiais como critério de saída.
- **[Modelo canônico ainda não formalizado]** → Definir modelo mínimo (`CanonicalFunctionalModel`)
  no domínio; evolução conforme necessidade.

## Migration Plan

1. Criar domínio FPA (tipos, matrizes, pesos, modelo canônico mínimo).
2. Implementar identificação das cinco funções.
3. Implementar complexidade e UFP.
4. Implementar VAF/AFP e Rule Packs.
5. Expor endpoint e testes (incluindo exemplos IFPUG).

## Open Questions

- Formato do `CanonicalFunctionalModel` mínimo aceito como entrada.
