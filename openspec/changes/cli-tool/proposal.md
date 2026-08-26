# CLI Tool

## Why

A Constituição e os requisitos preveem o entregável como API, CLI e UI (requisito
funcional 10: disponibilizar CLI). Hoje o sistema só expõe HTTP, sem uma interface
de linha de comando para submissão de histórias e contagem.

## What Changes

- Criar um CLI (projeto .NET console) que consome a API pública (agente/RAG/MCP):
  - `count` — submeter história de usuário e exibir o resultado de contagem;
  - `context` — consultar contexto normativo no RAG;
  - `measure` — invocar motores (fpa/sfp/snap) quando disponíveis;
  - `health` — checar status dos serviços.
- Output legível + formatos estruturados (JSON).
- Autenticação via token JWT (configurável).
- Testes unitários das operações do CLI.

## Capabilities

### New Capabilities

- `cli-tool`: interface de linha de comando (.NET) para contagem, consulta de
  contexto, medição e health-check contra a API pública, com output legível/JSON.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: novo projeto `services/cli-tool` (ou `tools/cli`) + testes; referência na
  solução raiz.
- Dependências: `System.CommandLine` (ou equivalente) e `HttpClient`.
- Build/CI: inclusão do novo projeto no Makefile/sln (alinhado a `adapt-project-settings`).
