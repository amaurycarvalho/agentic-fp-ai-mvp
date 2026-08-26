# Tasks: Consolidate Implemented MVP

> Esta change é de consolidação/arquivamento. Toda a implementação referenciada
> já existe no repositório. As tarefas abaixo refletem a verificação do baseline.

## 1. Verificação do baseline implementado

- [x] 1.1 Verificar que os serviços `agent-service`, `mcp-service` e `rag-service` existem com `src/` e `tests/`
- [x] 1.2 Verificar que `agentic-fp-ai-mvp.sln` referencia os projetos de API e testes dos três serviços
- [x] 1.3 Verificar que cada serviço possui `Dockerfile` e que `docker-compose.yml` sobe os três serviços
- [x] 1.4 Verificar que os endpoints `GET /health` existem nos três serviços
- [x] 1.5 Verificar que o `mcp-service` expõe `POST /count/basic` com trilha de auditoria
- [x] 1.6 Verificar que os testes unitários do `mcp-service` cobrem EI/ILF, EQ/EIF e DET/FTR explícitos
- [x] 1.7 Verificar que `docs/adr/` (001–004), `docs/rfc/` (008/017/018) e as user stories existem

## 2. Especificação do baseline

- [x] 2.1 Criar delta spec `architecture-foundation` (ADDED)
- [x] 2.2 Criar delta spec `service-bootstrap` (ADDED)
- [x] 2.3 Criar delta spec `mcp-basic-count` (ADDED)

## 3. Arquivamento

- [x] 3.1 Sincronizar as delta specs para `openspec/specs/<capability>/spec.md`
- [x] 3.2 Arquivar a change em `openspec/changes/archive/`
