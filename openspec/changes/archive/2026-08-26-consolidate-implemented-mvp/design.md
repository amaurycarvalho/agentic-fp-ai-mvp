# Design: Consolidate Implemented MVP

## Context

O projeto `agentic-fp-ai-mvp` foi iniciado como um MVP de IA agêntica para
contagem de Pontos de Função. O bootstrap (13/02/2026) criou três serviços
(`agent-service`, `mcp-service`, `rag-service`), a solução raiz, Dockerfiles,
docker-compose, ADRs (001–004), RFCs de medição (008 FPA, 017 SFP, 018 SNAP) e
user stories. A primeira evolução implementou o endpoint `POST /count/basic` no
`mcp-service` com testes unitários. Porém `openspec/specs` está vazio: o estado
atual não está formalizado como especificações.

Esta change **consolida o estado atual** em especificações formais e arquiva o
baseline. Não há código novo.

## Goals / Non-Goals

**Goals:**
- Criar `openspec/specs` para as capacidades já implementadas.
- Estabelecer um baseline auditável que fundamente as changes de evolução.
- Sincronizar as delta specs para as specs principais ao arquivar.

**Non-Goals:**
- Não implementar funcionalidades novas (RAG, agent, gRPC, motores FPA/SFP/SNAP, etc.).
- Não modificar código-fonte dos serviços.
- Não alterar os arquivos de settings (Makefile, CI, etc.) — escopo da change `adapt-project-settings`.

## Decisions

### D1 — Registrar como specs principais (não como delta de longo prazo)
Como nada existe em `openspec/specs`, as três capacidades são criadas como
**ADDED Requirements** nas delta specs da change e, no arquivamento, sincronizadas
para `openspec/specs/<capability>/spec.md`.
- **Alternativas**: deixar as specs apenas na change arquivada — rejeitado porque o
  arquivamento aponta para as specs principais como fonte de verdade.

### D2 — Três capacidades no baseline
| Capability | Cobre |
|---|---|
| `architecture-foundation` | ADR-001–004, DDD, Clean Arch, microserviços, SDD, auditoria |
| `service-bootstrap` | estrutura dos 3 serviços, sln raiz, Dockerfiles, docker-compose, health-checks, docs |
| `mcp-basic-count` | `POST /count/basic`, classificação, DET×FTR, pesos IFPUG, auditoria, testes |
- **Alternativa**: uma capability única abrangente — rejeitada por dificultar a
  evolução incremental e a revisão por capacidade.

### D3 — FPA/SFP/SNAP não entram como capacidade existente
As RFCs 008/017/018 descrevem motores completos **não implementados**. O protótipo
atual do `mcp-service` é apenas a contagem básica (capability `mcp-basic-count`).
Os motores completos permanecem como changes futuras
(`measurement-engine-fpa/sfp/snap`).

### D4 — Health-checks e containerização vão em `service-bootstrap`
Em vez de criar uma capability separada para health-checks, o estado atual
(endpoints `/health` nos 3 serviços + Dockerfiles + compose) é agrupado em
`service-bootstrap`, refletindo a origem bootstrap desses artefatos.

## Risks / Trade-offs

- **[Baseline desatualizado rapidamente]** → As changes de evolução seguintes
  devem modificar as specs principais via delta specs, mantendo o baseline vivo.
- **[Protótipo da contagem pode dar impressão de conformidade IFPUG]** → A spec
  `mcp-basic-count` documenta a natureza determinística/keyword-based do protótipo;
  a README já qualifica FPA/SFP/SNAP como protótipos não conformes.

## Migration Plan

1. Criar proposal, delta specs e design desta change.
2. Criar `tasks.md` (tudo já concluído).
3. Arquivar a change com sincronização das delta specs para `openspec/specs/`.

## Open Questions

- Nenhuma — escopo validado junto ao dono do produto (consolidar apenas o implementado).
