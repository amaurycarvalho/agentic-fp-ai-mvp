# Tasks: Measurement Engine SFP

## 1. Infraestrutura comum

- [ ] 1.1 Criar base compartilhada de medição (modelo canônico, `RulePack`, `MeasurementResult`)
- [ ] 1.2 Definir contrato de evidência imutável por componente

## 2. Motor SFP

- [ ] 2.1 Implementar identificação de Logical Functions (FR-011 a FR-013)
- [ ] 2.2 Implementar identificação de Functional Processes (FR-014 a FR-016)
- [ ] 2.3 Implementar merge de duplicados por fingerprint (FR-017/018)
- [ ] 2.4 Implementar valores fixos e total SFP (FR-019/020, FR-027)
- [ ] 2.5 Garantir ausência de complexidade/DET/RET/FTR (FR-021 a FR-026)

## 3. Rule Packs e relatório

- [ ] 3.1 Aplicar exclusões e inclusões de Rule Packs (FR-028 a FR-032)
- [ ] 3.2 Montar resultado com evidências e warnings (FR-033 a FR-035)

## 4. Exposição e testes

- [ ] 4.1 Expor endpoint `POST /measure/sfp`
- [ ] 4.2 Adicionar testes de identificação e merge
- [ ] 4.3 Adicionar testes de Rule Packs e determinismo (SC-001)
- [ ] 4.4 Garantir evidência em 100% dos componentes (SC-002)
