## ADDED Requirements

### Requirement: Soluções referenciadas pelo Makefile existem
O `Makefile` SHALL operar apenas sobre a solução raiz `agentic-fp-ai-mvp.sln` e os
projetos dos serviços existentes (`agent-service`, `mcp-service`, `rag-service`),
sem referenciar `erp-acl-service` ou soluções por serviço inexistentes.

#### Scenario: Instalar dependências
- **WHEN** `make install` é executado
- **THEN** o restore é feito sobre `agentic-fp-ai-mvp.sln` e conclui sem erro

#### Scenario: Executar testes
- **WHEN** `make test` é executado
- **THEN** os projetos de teste dos três serviços são executados com coleta de cobertura

### Requirement: Script de verificação de cobertura
O projeto SHALL possuir `scripts/coverage_check.py`, chamado por
`make coverage-check`, que lê os relatórios Cobertura gerados pelos testes e
retorna falha quando a cobertura ficar abaixo do limiar (`COVERAGE_THRESHOLD`).

#### Scenario: Cobertura abaixo do limiar
- **WHEN** a cobertura coletada é menor que `COVERAGE_THRESHOLD`
- **THEN** `make coverage-check` falha indicando o limiar não atingido

#### Scenario: Cobertura no limiar ou acima
- **WHEN** a cobertura coletada é maior ou igual a `COVERAGE_THRESHOLD`
- **THEN** `make coverage-check` conclui com sucesso

### Requirement: Quality gate executável
O `make quality-gate` SHALL executar `install`, `lint`, `test`, `coverage-check`,
`metrics` e `security` sem referências a serviços ou soluções inexistentes.

#### Scenario: Quality gate completo
- **WHEN** `make quality-gate` é executado
- **THEN** as etapas de lint, teste, cobertura, métricas e segurança rodam sobre os três serviços reais

### Requirement: Workflow CI consistente com o projeto
O `.github/workflows/ci.yml` SHALL rodar o quality gate em push/PR e a análise
SonarCloud por serviço (`agent-service`, `mcp-service`, `rag-service`) em PRs, com
chaves de projeto usando o prefixo `agentic-fp-ai-`.

#### Scenario: Quality gate no CI
- **WHEN** um push/PR para `main` ocorre
- **THEN** o job `quality-gate` executa `make quality-gate`

#### Scenario: Análise SonarCloud nos três serviços
- **WHEN** um PR é aberto
- **THEN** cada serviço é analisado com chave `agentic-fp-ai-<service>` a partir da solução raiz

### Requirement: Workflow de Release consistente com o projeto
O `.github/workflows/release.yml` SHALL construir, taggear e publicar as três
imagens de serviço, empacotar os tarballs, anexar `docker-compose.release.yml` e
usar o nome do repositório `agentic-fp-ai-mvp`.

#### Scenario: Release a partir de tag v*
- **WHEN** uma tag `v*` é criada
- **THEN** as três imagens são exportadas como `*-service.tar.gz` e a Release usa o nome correto do repositório

### Requirement: docker-compose de release disponível
O projeto SHALL manter `docker-compose.release.yml`, usado pelo workflow de Release
e documentado no README para consumidores das imagens publicadas.

#### Scenario: Arquivo referenciado pela Release
- **WHEN** uma Release é criada
- **THEN** `docker-compose.release.yml` é anexado aos assets

### Requirement: README e CHANGELOGs consistentes
O `README.md` e os `CHANGELOG.md`/`CHANGELOG-ARCHIVE.md` SHALL refletir o projeto
`agentic-fp-ai-mvp`: três serviços, portas reais, comandos válidos e URL de
compare apontando para `github.com/amaurycarvalho/agentic-fp-ai-mvp`.

#### Scenario: Portas documentadas conferem com o compose
- **WHEN** um usuário segue o README para acessar os serviços
- **THEN** as portas correspondem às do `docker-compose.yml`

#### Scenario: Changelog aponta para o repositório correto
- **WHEN** um leitor abre o `CHANGELOG.md`
- **THEN** o link de compare usa o repositório `agentic-fp-ai-mvp`

### Requirement: docker-compose principal reflete os serviços reais
O `docker-compose.yml` SHALL declarar apenas os serviços existentes
(`agent-service`, `mcp-service`, `rag-service`) com portas e dependências coerentes
com o README.

#### Scenario: Serviços declarados existem
- **WHEN** `docker-compose config` é executado
- **THEN** os serviços listados são exatamente os três do MVP, sem `erp-acl-service`
