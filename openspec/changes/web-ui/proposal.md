# Web UI

## Why

A Constitution e os requisitos preveem o entregável como API, CLI e UI Web
(requisito funcional 11). Hoje não existe interface web para submeter histórias,
ver contagens e acompanhar a trilha de auditoria.

## What Changes

- Criar uma UI Web (SPA) consumindo a API pública:
  - Formulário de submissão de história de usuário com resultado de contagem;
  - Consulta de contexto normativo (RAG);
  - Visualização de medições (fpa/sfp/snap) e trilha de auditoria;
  - Health/status dos serviços.
- Autenticação JWT (login/token) e responsividade.
- Testes unitários dos componentes/páginas principais.

## Capabilities

### New Capabilities

- `web-ui`: interface web (SPA) para contagem de pontos de função, consulta de
  contexto, medições e status, consumindo a API pública com autenticação JWT.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: novo frontend (ex.: `services/web-ui` com React/Blazor ou estático);
  CI/build integrado.
- Dependências: framework frontend a definir (Blazor mantém stack .NET; React é opção).
- API: CORS configurado para origem da UI.
