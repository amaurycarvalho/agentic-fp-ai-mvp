# xUnit Test Conventions

## Why

O CI falhou no `make lint` porque o analyzer **xUnit1051** emite avisos quando uma
chamada async que aceita `CancellationToken` (ex.: `HttpClient.GetAsync`,
`PostAsJsonAsync`, `ReadFromJsonAsync`) não recebe `TestContext.Current.CancellationToken`.
`dotnet format --verify-no-changes` falha com qualquer aviso de analyzer, então
novos testes escritos sem essa convenção quebrariam o quality gate novamente.

## What Changes

- Registrar como requisito de spec a convenção: chamadas async em testes que
  aceitam `CancellationToken` SHALL passar `TestContext.Current.CancellationToken`.
- Escopo: convenção documental/processo — os testes atuais já seguem a convenção
  (corrigidos previamente); não há alteração de código nesta change.

## Capabilities

### New Capabilities

_(nenhuma — requisito adicionado a uma capacidade existente)_

### Modified Capabilities

- `build-release-tooling`: adiciona requisito de convenção de escrita de testes
  (passar `CancellationToken`), garantindo que o `make lint` no CI permaneça verde.

## Impact

- Código: nenhum.
- Especificações: novo requisito em `openspec/specs/build-release-tooling/spec.md`.
- Processo: orienta futuros testes a não reintroduzirem avisos xUnit1051.
