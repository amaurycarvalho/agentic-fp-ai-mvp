## ADDED Requirements

### Requirement: Configuração do runner de testes
Os projetos de teste SHALL NOT definir `OutputType=Exe` nem
`UseMicrosoftTestingPlatformRunner=true`, mantendo o `dotnet test` (VSTest) como
runner padrão — compatível com qualquer SDK (.NET 8, 9 ou 10). O MTP SHALL ser
acionado apenas pelo Stryker via `stryker-config.json` (`test-runner: mtp`).

#### Scenario: dotnet test via VSTest em qualquer SDK
- **WHEN** `make test` é executado no CI (incluindo runners com SDK .NET 10)
- **THEN** os testes rodam via VSTest sem o erro "Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK"

#### Scenario: Stryker com MTP configurado
- **WHEN** o Stryker é executado em um projeto de teste
- **THEN** ele usa o MTP conforme o `stryker-config.json` (test-runner `mtp`, coverage-analysis `off`)

### Requirement: stryker-config.json por projeto de teste
Cada projeto de teste SHALL possuir um `stryker-config.json` declarando
`test-runner: mtp`, `coverage-analysis: off`, `project` (csproj sob teste),
reporters e thresholds.

#### Scenario: Configuração presente e válida
- **WHEN** um projeto de teste é inspecionado
- **THEN** existe um `stryker-config.json` com `test-runner` e `project` definidos
