# Adapt Project Settings

## Why

Os arquivos de settings (Makefile, README, CHANGELOGs, workflows CI/Release,
SonarQube, docker-compose, props de build) foram importados de outro projeto
(`agentic-erp-platform-mvp`) e referenciam artefatos que **não existem** neste
repositório: o serviço `erp-acl-service`, soluções por serviço (`Agent.sln`,
`Mcp.sln`, `Rag.sln`), `scripts/coverage_check.py` e `docker-compose.release.yml`.
Resultado: `make install/test/build`, CI e Release estão quebrados ou inconsistentes
com a realidade do `agentic-fp-ai-mvp`.

## What Changes

- **Adaptar** `Makefile` à realidade de 3 serviços + solução raiz única
  (`agentic-fp-ai-mvp.sln`), removendo `erp-acl-service` e as soluções inexistentes;
- **Adaptar** `README.md` (portas, serviços, comandos) e os `CHANGELOG*.md`
  (URL de compare do repositório correto);
- **Adaptar** `.github/workflows/ci.yml` (matrix de 3 serviços, sem `erp-acl-service`,
  usando a solução raiz) e `.github/workflows/release.yml` (3 imagens, nome e repo corretos);
- **Adaptar** `docker-compose.yml` para alinhar portas ao README e refletir os 3 serviços;
- **Adaptar** `sonarqube/docker-compose.yml` apenas se necessário à stack local;
- **Criar** `scripts/coverage_check.py` (verificação de cobertura que o Makefile invoca);
- **Criar** `docker-compose.release.yml` (referenciado pelo workflow de Release);
- Ajustar `Directory.Build.props`, `CodeCoverage.runsettings`, `.gitignore` e demais
  settings conforme necessário.

## Capabilities

### New Capabilities

- `build-release-tooling`: tooling de build, teste, qualidade, CI/CD e release
  alinhado à estrutura real do projeto (3 serviços, solução raiz única, nome
  `agentic-fp-ai-mvp`, sem referências a `erp-acl-service`).

### Modified Capabilities

_(nenhuma — as specs de `build-release-tooling` são novas)_

## Impact

- Código: nenhum serviço é alterado; apenas ferramentas/scripts de build e configuração.
- Arquivos: `Makefile`, `README.md`, `CHANGELOG.md`, `CHANGELOG-ARCHIVE.md`,
  `.github/workflows/ci.yml`, `.github/workflows/release.yml`,
  `sonarqube/docker-compose.yml`, `docker-compose.yml`, `Directory.Build.props`,
  `CodeCoverage.runsettings`, `.gitignore`; novos `scripts/coverage_check.py` e
  `docker-compose.release.yml`.
- Processo: destrava `make install/test/build/quality-gate`, CI e Release para o MVP real.
