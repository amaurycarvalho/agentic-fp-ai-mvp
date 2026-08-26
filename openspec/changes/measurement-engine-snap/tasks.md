# Tasks: Measurement Engine SNAP

## 1. Categorias

- [ ] 1.1 Definir schema de categorias de avaliação com SemVer (FR-015)
- [ ] 1.2 Validar definições de categoria no load
- [ ] 1.3 Configurar categorias iniciais (ex.: Presentation, Data Operations)

## 2. Motor SNAP

- [ ] 2.1 Implementar identificação de candidatos por metadados semânticos (FR-016)
- [ ] 2.2 Implementar merge de duplicados (FR-017)
- [ ] 2.3 Ignorar detalhes de tecnologia na avaliação (FR-018/019)
- [ ] 2.4 Implementar contribuição independente por item (FR-020)

## 3. Rule Packs e relatório

- [ ] 3.1 Aplicar exclusões de categorias/itens e políticas de inclusão (FR-025 a FR-029)
- [ ] 3.2 Reportar exclusões e warnings de não-resolvidos (FR-023/024)
- [ ] 3.3 Montar resultado agregado por categoria com evidências (FR-030 a FR-032)

## 4. Exposição e testes

- [ ] 4.1 Expor endpoint `POST /measure/snap`
- [ ] 4.2 Adicionar testes de identificação, merge e categorias
- [ ] 4.3 Adicionar testes de Rule Packs e determinismo byte-idêntico (SC-001)
- [ ] 4.4 Garantir evidência em 100% dos itens (SC-002)
