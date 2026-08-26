# Tasks: Measurement Engine FPA

## 1. Domínio FPA

- [ ] 1.1 Criar tipos de domínio (`FunctionType`, `ElementaryProcess`, `DataGroup`, `DET/RET/FTR`)
- [ ] 1.2 Definir modelo canônico mínimo (`CanonicalFunctionalModel`)
- [ ] 1.3 Declarar matrizes de complexidade RET×DET e DET×FTR
- [ ] 1.4 Declarar tabela de pesos IFPUG (FR-022)

## 2. Identificação e classificação

- [ ] 2.1 Implementar identificação ILF/EIF (FR-014/015)
- [ ] 2.2 Implementar identificação de processos elementares (FR-016)
- [ ] 2.3 Implementar classificação EI/EO/EQ exclusiva (FR-017/018/019, FR-032)
- [ ] 2.4 Implementar complexidade pelas matrizes (FR-020/021)

## 3. Cálculo e ajuste

- [ ] 3.1 Implementar UFP (FR-023)
- [ ] 3.2 Implementar GSC/TDI/VAF (FR-024/025/026) e AFP (FR-027)
- [ ] 3.3 Implementar regras de contagem DET/RET/FTR e fronteira (FR-028 a FR-031)

## 4. Rule Packs e relatório

- [ ] 4.1 Definir contrato `IRulePack` e integração (FR-004/005)
- [ ] 4.2 Implementar relatório de medição com evidência (FR-033)
- [ ] 4.3 Tratar modelo vazio e referências pendentes (FR-011/012)

## 5. Exposição e testes

- [ ] 5.1 Expor `IFpaMeasurementService` e endpoint `POST /measure/fpa`
- [ ] 5.2 Manter `POST /count/basic` como compatibilidade
- [ ] 5.3 Adicionar testes por regra (matrizes, pesos, VAF, Rule Packs)
- [ ] 5.4 Adicionar testes de reprodução de exemplos IFPUG CPM 4.3 (SC-008)
- [ ] 5.5 Garantir determinismo (SC-001/SC-004) e evidência (SC-002)
