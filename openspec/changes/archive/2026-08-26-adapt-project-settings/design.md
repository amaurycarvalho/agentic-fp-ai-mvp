# Design: Adapt Project Settings

## Context

O repositório foi iniciado com bootstrap e importou o ferramental de build/release
de um projeto anterior (`agentic-erp-platform-mvp`) que possuía 4 serviços e
soluções por serviço. Neste MVP existem apenas 3 serviços e uma solução raiz única
(`agentic-fp-ai-mvp.sln`). As referências divergentes quebram `make`, CI e Release:
- `Makefile`: `SOLUTIONS`/`IMAGES`/`SERVICE_DIRS`/`TEST_RESULT_DIRS` citam `erp-acl-service`
  e slns `Agent.sln`/`Mcp.sln`/`Rag.sln`/`ErpAcl.sln`; `coverage-check` invoca
  `scripts/coverage_check.py` inexistente; `mutation` espera `*.Application.Tests`.
- `ci.yml`: matrix SonarCloud com 4 serviços e slns inexistentes.
- `release.yml`: exporta 4 imagens, nome `agentic-erp-platform-mvp`, usa
  `docker-compose.release.yml` inexistente.
- `README.md`/`CHANGELOG.md`: portas divergentes (`agent` 8080 vs 8081), cita
  `erp-acl-service`, `rag/search`, URL de compare do repo errado.
- `docker-compose.yml`: mapeia host 8081→8080 do agent; não possui qdrant/ollama
  (infra fica para a change `vector-database-rag`).

## Goals / Non-Goals

**Goals:**
- Fazer `make install/test/build/lint/coverage/quality-gate` funcionar com os 3 serviços reais.
- Alinhar CI/Release/README/CHANGELOG/compose ao projeto `agentic-fp-ai-mvp`.
- Criar os dois artefatos faltantes referenciados pelo tooling.

**Non-Goals:**
- Implementar features de negócio (RAG, agent, gRPC, motores) — changes próprias.
- Adicionar infra qdrant/ollama ao compose — escopo de `vector-database-rag`.
- Criar slns por serviço (decisão do dono: manter solução raiz única).

## Decisions

### D1 — Manter solução raiz única; adaptar o Makefile a ela
Em vez de recriar `Agent.sln`/`Mcp.sln`/`Rag.sln`, o Makefile passa a iterar sobre
`agentic-fp-ai-mvp.sln` uma única vez (restore/test/build/lint). A separação
per-service continua possível via `dotnet test <sln> --filter` no CI SonarCloud
(matrix), que usa a mesma solução raiz.
- **Alternativa**: recriar slns por serviço — rejeitada (decisão do dono).

### D2 — `scripts/coverage_check.py` novo
Script Python mínimo que varre `TestResults/**/coverage.cobertura.xml`, soma
linhas cobertas/totais e falha se < `COVERAGE_THRESHOLD`. Sem dependências externas.
- **Alternativa**: shell/awk para somar XML — frágil; Python+`xml.etree` é stdlib.

### D3 — `docker-compose.release.yml` novo
Compose de consumo para imagens publicadas (3 serviços, `image:` apontando tags de
release), documentado no README e anexado pela Release.

### D4 — Portas alinhadas
`docker-compose.yml` passa a mapear `8080→8080` do agent (coerente com README);
`mcp-service` 8082 e `rag-service` 8083 permanecem. Semântica de release (portas
de produção) definida no `docker-compose.release.yml`.

### D5 — SonarCloud matrix de 3 serviços
`ci.yml` mantém 3 jobs SonarCloud (um por serviço) usando `make test-sln
SLN=agentic-fp-ai-mvp.sln` com prefixo de chave `agentic-fp-ai-`. O projeto
SonarCloud precisa dos projetos criados com essas chaves (ação de operação manual).

## Risks / Trade-offs

- **[Chaves SonarCloud novas no SonarCloud]** → Criar os 3 projetos
  (`agentic-fp-ai-agent-service`, etc.) no SonarCloud; o `make sonar-up` local
  continua auto-criando com prefixo `agentic-fp-ai-`.
- **[Cobertura abaixo do limiar inicial]** → O `COVERAGE_THRESHOLD` pode ser
  ajustado (default 80) conforme baseline medido; manter 80 e revisar após testes reais.
- **[Mudança de portas quebra consumidores]** → Porta do agent muda de 8081 para
  8080; impacto local apenas (dev), documentado no README.

## Migration Plan

1. Ajustar `Makefile` (solução raiz única, remover `erp-acl-service`, prefixo Sonar `agentic-fp-ai-`).
2. Criar `scripts/coverage_check.py`.
3. Criar `docker-compose.release.yml`.
4. Ajustar `ci.yml` e `release.yml`.
5. Ajustar `docker-compose.yml` (portas/serviços) e `sonarqube/docker-compose.yml` se necessário.
6. Ajustar `README.md` e `CHANGELOG*.md`.
7. Validar `make install && make build && make lint && make test` e `docker-compose config`.

## Open Questions

- Nenhuma — escopo confirmado (criar `coverage_check.py` e `docker-compose.release.yml`;
  sem slns por serviço).
