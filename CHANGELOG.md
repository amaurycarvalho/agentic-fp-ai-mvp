# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### [agent-service-orchestration](openspec/changes/agent-service-orchestration) O `agent-service` expõe apenas `GET /health`. A US-AGENT-001 exige o fluxo orquestrado de contagem: receber uma solicitação única, consultar contexto no `rag-service`, acionar o `mcp-service` para cálculo determinístico e retornar um resultado consolidado com rastreabilidade das etapas.

#### Added
- Implementar o fluxo de orquestração do `agent-service` (US-AGENT-001).
- Definir clientes HTTP para `rag-service` (busca de contexto) e `mcp-service` (contagem), com interfaces desacopladas para testes.
- Registrar trilha auditável da orquestração (entrada, chamadas e resultado).
- Adicionar testes unitários reais (substituir placeholder).

### [cli-tool](openspec/changes/cli-tool) A Constituição e os requisitos preveem o entregável como API, CLI e UI (requisito funcional 10: disponibilizar CLI). Hoje o sistema só expõe HTTP, sem uma interface de linha de comando para submissão de histórias e contagem.

#### Added
- Criar um CLI (projeto .NET console) que consome a API pública (agente/RAG/MCP).
- `count` — submeter história de usuário e exibir o resultado de contagem.
- `context` — consultar contexto normativo no RAG.
- `measure` — invocar motores (fpa/sfp/snap) quando disponíveis.
- `health` — checar status dos serviços.
- Output legível + formatos estruturados (JSON).
- Autenticação via token JWT (configurável).
- Testes unitários das operações do CLI.

### [grpc-internal-contracts](openspec/changes/grpc-internal-contracts) A arquitetura (ADR-001, ADR-002, ADR-003) define comunicação interna via gRPC, com REST apenas na borda externa. Hoje os serviços conversam por HTTP (ou ainda não conversam). Os contratos gRPC internos precisam ser formalizados e implementados para alinhar a comunicação entre `agent-service`, `mcp-service` e `rag-service`.

#### Added
- Definir contratos gRPC (protos) para as capacidades internas: contagem determinística (`mcp-service`), recuperação de contexto (`rag-service`) e orquestração (`agent-service`).
- Implementar servidores e clientes gRPC nos serviços (mantendo REST na borda).
- Adicionar testes dos contratos/serviços gRPC.

### [llm-provider-integration](openspec/changes/llm-provider-integration) A visão do projeto prevê IA desacoplada via MCP e RAG, com provedor LLM (Ollama ou OpenAI Compliance API) para interpretação de histórias em linguagem natural e enriquecimento semântico — sem decisão de regra. Hoje não há integração com nenhum provedor LLM.

#### Added
- Definir porta de provedor LLM (`ILLMProvider`) desacoplada (chat/embedding).
- Implementar provedores: Ollama (local) e OpenAI Compliance API, selecionáveis por configuração.
- Usar o LLM para interpretar/enriquecer histórias (draft de modelo funcional para revisão), SEM classificar regras determinísticas (decisão permanece no motor).
- Configurar servidor Ollama opcional no `docker-compose.yml`.
- Testes com mocks/fakes do provedor.

### [measurement-engine-fpa](openspec/changes/measurement-engine-fpa) A RFC 008 (`docs/rfc/008-measurement-engine-fpa`) especifica um motor de medição IFPUG/FPA completo, determinístico e explicável. Hoje o `mcp-service` possui apenas o protótipo `mcp-basic-count` (classificação por palavras-chave), sem as regras formais de identificação, matrizes de complexidade RET/DET e DET/FTR, pesos IFPUG, UFP/AFP/VAF e trilha de evidência.

#### Added
- Implementar o motor FPA conforme a RFC 008: identificação de ILF/EIF/EI/EO/EQ, complexidade (matrizes), pesos IFPUG (FR-022), UFP/AFP/VAF (FR-023 a FR-027), regras de contagem (DET/RET/FTR, fronteira) e relatório com evidência.
- Aplicar Rule Packs (externalização de políticas) e resultado determinístico.
- Expor o motor como capacidade do `mcp-service` (via gRPC/HTTP) com testes (reprodução de exemplos IFPUG, matrizes, SC-008 a SC-014).

### [measurement-engine-sfp](openspec/changes/measurement-engine-sfp) A RFC 017 (`docs/rfc/017-measurement-engine-sfp`) especifica o motor de Simple Function Points: mede apenas dois componentes (Logical Functions e Functional Processes), sem DET/RET/FTR, com valores fixos, Rule Packs e total determinístico. Não existe implementação hoje.

#### Added
- Implementar o motor SFP conforme a RFC 017: identificação de Logical Functions e Functional Processes por matching no modelo canônico (FR-011 a FR-018), valores fixos de contribuição (FR-019/FR-020), merge de duplicados por fingerprint (FR-017/018), sem classificação de complexidade (FR-021 a FR-027).
- Aplicar Rule Packs (exclusões) e relatório com evidência (FR-033 a FR-035).
- Observabilidade mínima (structured logs, FR-041) e exposição como capacidade.

### [measurement-engine-snap](openspec/changes/measurement-engine-snap) A RFC 018 (`docs/rfc/018-measurement-engine-snap`) especifica o motor SNAP para medir tamanho funcional não-funcional (formatação, apresentação, capacidades operacionais, interação técnica), com categorias de avaliação, candidatos a partir de metadados semânticos do modelo, Rule Packs e determinismo. Não existe implementação hoje.

#### Added
- Implementar o motor SNAP conforme a RFC 018: categorias de avaliação versionadas (FR-011 a FR-015), identificação de candidatos por metadados semânticos (FR-016 a FR-024), contribuição independente por item, Rule Packs (exclusões) e relatório com evidência por categoria.
- Merged de candidatos duplicados e warnings para não resolvidos (FR-017/023/024).
- Exposição como capacidade e testes (SC-001 a SC-007).

### [observability](openspec/changes/observability) A Constituição exige observabilidade completa e os requisitos não-funcionais preveem OpenTelemetry e logs estruturados e auditáveis. Hoje os serviços apenas logam implicitamente no console e não emitem métricas/traces.

#### Added
- Instrumentar os três serviços com OpenTelemetry (traces, métricas e logs).
- Emitir métricas de negócio (duração de medição, contagens por componente) e de infraestrutura (HTTP).
- Logs estruturados com correlação (trace/correlation id).
- Adicionar endpoint de métricas Prometheus (ex.: `/metrics`) e integração com collector/exportador configurável.

### [rag-service-query-contract](openspec/changes/rag-service-query-contract) O `rag-service` hoje expõe apenas `GET /health`. A US-RAG-001 (consulta à base normativa) define que o agente precisa recuperar contexto técnico confiável antes da análise, com metadados de origem para auditoria — sem executar ações de negócio.

#### Added
- Implementar o contrato de consulta do `rag-service` (US-RAG-001): endpoint de busca por texto que retorna trechos relevantes e metadados mínimos da fonte.
- Definir porta de infraestrutura para o armazenamento (desacoplada do domínio), permitindo troca de provedor de recuperação.
- Adicionar testes unitários reais (substituir o `UnitTest1.cs` placeholder).

### [resilience-and-healthchecks](openspec/changes/resilience-and-healthchecks) O ADR-001 prevê health checks e cenários de resiliência (timeout/retry) como parte da fundação. Hoje os health-checks são apenas `GET /health` trivial, sem verificar dependências, e as chamadas entre serviços não possuem timeouts/retries — deixando a stack frágil a falhas de dependência.

#### Added
- Health checks de dependências (liveness + readiness) nos três serviços.
- Política de timeout/retry para chamadas HTTP/gRPC entre serviços (Polly ou middleware equivalente).
- Configuração de timeouts em clientes e opções de resiliência.
- Testes de resiliência e de health-check com dependência indisponível.

### [security-hardening](openspec/changes/security-hardening) Os requisitos não-funcionais preveem JWT para API pública, TLS obrigatório e logs criptografados, além de compatibilidade LGPD. Hoje os endpoints são abertos, sem autenticação, TLS ou proteções adicionais — inconsistente com a Constituição (Security by design).

#### Added
- Autenticação JWT na API pública (borda REST externa).
- TLS/HTTPS obrigatório nos serviços (e mTLS opcional na comunicação interna).
- Proteções básicas: CORS restrito, rate limiting, headers de segurança, validação de entrada e logging com saneamento (sem dados sensíveis).
- Conformidade LGPD: tratamento de dados pessoais em histórias/relatórios, mecanismo de retenção/exclusão e consentimento quando aplicável.
- Testes de segurança (auth, injeção básica, headers).

### [vector-database-rag](openspec/changes/vector-database-rag) O `rag-service` (ADR-003) prevê armazenamento vetorial desacoplado por porta de infraestrutura. A US-RAG-001 exige recuperação por relevância; hoje não há base vetorial, ingestão de documentos normativos nem o Qdrant no `docker-compose.yml` (que docs/README prometem).

#### Added
- Integrar um provider vetorial concreto (Qdrant) via a porta de infraestrutura do rag-service (definida na change `rag-service-query-contract`).
- Implementar ingestão de documentos normativos (chunking + embeddings).
- Adicionar o Qdrant (e provedor de embeddings) ao `docker-compose.yml`.
- Testes da ingestão e da consulta com o provider real (ou teste de integração).

### [web-ui](openspec/changes/web-ui) A Constitution e os requisitos preveem o entregável como API, CLI e UI Web (requisito funcional 11). Hoje não existe interface web para submeter histórias, ver contagens e acompanhar a trilha de auditoria.

#### Added
- Criar uma UI Web (SPA) consumindo a API pública.
- Formulário de submissão de história de usuário com resultado de contagem.
- Consulta de contexto normativo (RAG).
- Visualização de medições (fpa/sfp/snap) e trilha de auditoria.
- Health/status dos serviços.
- Autenticação JWT (login/token) e responsividade.
- Testes unitários dos componentes/páginas principais.

## [1.0.1] - 2026-08-26

### [adapt-project-settings](openspec/changes/archive/2026-08-26-adapt-project-settings) Os arquivos de settings (Makefile, README, CHANGELOGs, workflows CI/Release, SonarQube, docker-compose, props de build) foram importados de outro projeto (`agentic-erp-platform-mvp`) e referenciam artefatos que não existem neste repositório: o serviço `erp-acl-service`, soluções por serviço (`Agent.sln`, `Mcp.sln`, `Rag.sln`), `scripts/coverage_check.py` e `docker-compose.release.yml`. Resultado: `make install/test/build`, CI e Release estavam quebrados ou inconsistentes com a realidade do `agentic-fp-ai-mvp`.

#### Changed
- Adaptar `Makefile` à realidade de 3 serviços + solução raiz única (`agentic-fp-ai-mvp.sln`), removendo `erp-acl-service` e as soluções inexistentes.
- Adaptar `README.md` (portas, serviços, comandos) e os `CHANGELOG*.md` (URL de compare do repositório correto).
- Adaptar `.github/workflows/ci.yml` (matrix de 3 serviços, sem `erp-acl-service`, usando a solução raiz) e `.github/workflows/release.yml` (3 imagens, nome e repo corretos).
- Adaptar `docker-compose.yml` para alinhar portas ao README e refletir os 3 serviços.
- Adaptar `sonarqube/docker-compose.yml` apenas se necessário à stack local.
- Ajustar `Directory.Build.props`, `CodeCoverage.runsettings`, `.gitignore` e demais settings conforme necessário.

#### Added
- Criar `scripts/coverage_check.py` (verificação de cobertura que o Makefile invoca).
- Criar `docker-compose.release.yml` (referenciado pelo workflow de Release).

### [2026-08-26-align-test-configuration](openspec/changes/archive/2026-08-26-align-test-configuration) O CI falhou no job `quality-gate` com o erro: `Testing with VSTest target is no longer supported by Microsoft.Testing.Platform on .NET 10 SDK and later`.

#### Added
- Criar `stryker-config.json` em cada projeto de teste com `"test-runner": "mtp"`, `"coverage-analysis": "off"`, `"project"` apontando para o csproj sob teste e thresholds `{high: 80, low: 70, break: 60}`.

#### Changed
- Rebaixar `xunit.v3` de `4.0.0` para `3.2.2` (volta ao MTP v1, sem o hard error de SDK).
- Alinhar pacotes de teste ao projeto de referência: `xunit.runner.visualstudio` 3.1.5, `Microsoft.NET.Test.Sdk` 17.10.0, `coverlet.collector` 6.0.0.

#### Removed
- Remover `OutputType=Exe` e `UseMicrosoftTestingPlatformRunner` dos projetos de teste (`agent-service`, `mcp-service`, `rag-service`).

### [xunit-test-conventions](openspec/changes/archive/2026-08-26-xunit-test-conventions) O CI falhou no `make lint` porque o analyzer xUnit1051 emite avisos quando uma chamada async que aceita `CancellationToken` (ex.: `HttpClient.GetAsync`, `PostAsJsonAsync`, `ReadFromJsonAsync`) não recebe `TestContext.Current.CancellationToken`. `dotnet format --verify-no-changes` falha com qualquer aviso de analyzer, então novos testes escritos sem essa convenção quebrariam o quality gate novamente.

#### Added
- Registrar como requisito de spec a convenção: chamadas async em testes que aceitam `CancellationToken` SHALL passar `TestContext.Current.CancellationToken`.

#### Changed
- Escopo: convenção documental/processo — os testes atuais já seguem a convenção (corrigidos previamente); não há alteração de código nesta change.

[Unreleased]: https://github.com/amaurycarvalho/agentic-fp-ai-mvp/compare/v1.0.1...HEAD
[1.0.1]: https://github.com/amaurycarvalho/agentic-fp-ai-mvp/releases/tag/v1.0.1

See [CHANGELOG Archive](CHANGELOG-ARCHIVE.md) for older releases.
