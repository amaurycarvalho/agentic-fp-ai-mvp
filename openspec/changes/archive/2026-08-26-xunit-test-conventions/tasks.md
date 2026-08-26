# Tasks: xUnit Test Conventions

> Change de documentação de processo. A correção de código já foi aplicada
> (testes passam `TestContext.Current.CancellationToken`); aqui apenas se registra
> a convenção nas specs.

## 1. Verificação da correção existente

- [x] 1.1 Confirmar que os testes de integração (agent/mcp/rag) passam `TestContext.Current.CancellationToken`
- [x] 1.2 Confirmar que `make lint` e `make quality-gate` passam no CI

## 2. Registro nas specs

- [x] 2.1 Criar delta spec com o requisito "Testes async passam CancellationToken" em `build-release-tooling`
- [x] 2.2 Sincronizar o requisito para `openspec/specs/build-release-tooling/spec.md`

## 3. Arquivamento

- [x] 3.1 Arquivar a change em `openspec/changes/archive/`
