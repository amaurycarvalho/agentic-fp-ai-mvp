# Changelog Archive

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-08-26

### [consolidate-implemented-mvp](openspec/changes/archive/2026-08-26-consolidate-implemented-mvp) O repositório acumula código e documentação que já materializam parte do MVP (bootstrap dos serviços, ADRs, RFCs, user stories e o endpoint de contagem básica do `mcp-service`), mas o `openspec/specs` estava vazio — não havia especificação consolidada do que já existia. Sem esse baseline, as changes futuras de evolução ficariam sem um contrato formal do estado atual.

#### Added
- Registrar como capacidades formais o que já foi implementado e verificado no código: fundação arquitetural (DDD, Clean Architecture, microserviços, C#/.NET, SDD); bootstrap dos três serviços (`agent-service`, `mcp-service`, `rag-service`), solução raiz `agentic-fp-ai-mvp.sln`, `Dockerfile` por serviço, `docker-compose.yml` e documentação (`docs/` com ADRs, RFCs e user stories); endpoint de contagem básica do `mcp-service` (`POST /count/basic`) com classificação determinística por palavras-chave, complexidade DET×FTR, trilha de auditoria e testes unitários.
- Consolidar esse baseline em `openspec/specs` e arquivar a change (não há código novo a implementar nesta change).

#### Changed
- Nenhuma alteração de código-fonte é introduzida aqui — apenas captura do estado atual.

[1.0.0]: https://github.com/amaurycarvalho/agentic-fp-ai-mvp/releases/tag/v1.0.0

See main [CHANGELOG](CHANGELOG.md) for newer releases.
