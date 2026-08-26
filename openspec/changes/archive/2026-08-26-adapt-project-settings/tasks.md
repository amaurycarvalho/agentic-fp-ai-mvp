# Tasks: Adapt Project Settings

## 1. Makefile e scripts

- [x] 1.1 Remover `erp-acl-service` de `SOLUTIONS`, `IMAGES`, `SERVICE_DIRS` e `TEST_RESULT_DIRS` no Makefile
- [x] 1.2 Substituir as soluções por serviço pela solução raiz única `agentic-fp-ai-mvp.sln`
- [x] 1.3 Atualizar targets de imagem (`build-images`) para as 3 imagens reais
- [x] 1.4 Atualizar prefixo Sonar (`SONAR_PROJECT_KEY_PREFIX`) para `agentic-fp-ai-`
- [x] 1.5 Ajustar target `mutation` (padrão de projeto de testes real: `*.Api.Tests`)
- [x] 1.6 Criar `scripts/coverage_check.py` (lê `TestResults/**/coverage.cobertura.xml`, compara com `COVERAGE_THRESHOLD`)
- [x] 1.7 Atualizar `help` e exemplos do Makefile conforme nova realidade

## 2. Docker Compose

- [x] 2.1 Ajustar `docker-compose.yml` para os 3 serviços reais com portas coerentes (agent em `8080:8080`)
- [x] 2.2 Criar `docker-compose.release.yml` para consumo das imagens publicadas
- [x] 2.3 Validar `docker-compose config` (compose principal) sem serviços inexistentes
- [x] 2.4 Revisar `sonarqube/docker-compose.yml` e ajustar se necessário à stack local

## 3. CI e Release

- [x] 3.1 Ajustar `.github/workflows/ci.yml` (matrix SonarCloud de 3 serviços usando a solução raiz)
- [x] 3.2 Ajustar `.github/workflows/release.yml` (3 imagens, nome `agentic-fp-ai-mvp`, `docker-compose.release.yml`)
- [x] 3.3 Garantir que o job `integration-test` usa apenas serviços existentes

## 4. Documentação

- [x] 4.1 Atualizar `README.md` (serviços, portas, comandos, remover referências a `erp-acl-service`/`rag/search` inexistentes)
- [x] 4.2 Corrigir URL de compare em `CHANGELOG.md` para `amaurycarvalho/agentic-fp-ai-mvp`
- [x] 4.3 Revisar `CHANGELOG-ARCHIVE.md` quanto a estrutura/links

## 5. Settings auxiliares e validação final

- [x] 5.1 Revisar `Directory.Build.props`, `CodeCoverage.runsettings` e `.gitignore` conforme necessidade
- [x] 5.2 Executar `make install && make build && make lint && make test` com sucesso
- [x] 5.3 Executar `make coverage-check` com o novo script
- [x] 5.4 Validar `docker-compose config` no compose de release
