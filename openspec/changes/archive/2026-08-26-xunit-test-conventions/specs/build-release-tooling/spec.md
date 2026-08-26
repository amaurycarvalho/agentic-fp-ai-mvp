## ADDED Requirements

### Requirement: Testes async passam CancellationToken
Testes que chamam métodos async que aceitam um `CancellationToken` (ex.:
`HttpClient.GetAsync`, `PostAsJsonAsync`, `ReadFromJsonAsync`, `ReadAsStringAsync`)
SHALL passar `TestContext.Current.CancellationToken`, evitando o aviso do analyzer
xUnit1051 que falha o `make lint` (dotnet format --verify-no-changes) no CI.

#### Scenario: Lint sem avisos xUnit1051
- **WHEN** `make lint` é executado no CI
- **THEN** não há avisos xUnit1051 nos projetos de teste

#### Scenario: Chamada async com cancellation token
- **WHEN** um teste faz uma chamada HTTP async que aceita `CancellationToken`
- **THEN** a chamada passa `TestContext.Current.CancellationToken`
