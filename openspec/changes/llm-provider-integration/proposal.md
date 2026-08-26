# LLM Provider Integration

## Why

A visão do projeto prevê IA desacoplada via MCP e RAG, com provedor LLM
(Ollama ou OpenAI Compliance API) para interpretação de histórias em linguagem
natural e enriquecimento semântico — sem decisão de regra. Hoje não há integração
com nenhum provedor LLM.

## What Changes

- Definir porta de provedor LLM (`ILLMProvider`) desacoplada (chat/embedding).
- Implementar provedores: Ollama (local) e OpenAI Compliance API, selecionáveis por
  configuração.
- Usar o LLM para interpretar/enriquecer histórias (draft de modelo funcional para
  revisão), SEM classificar regras determinísticas (decisão permanece no motor).
- Configurar servidor Ollama opcional no `docker-compose.yml`.
- Testes com mocks/fakes do provedor.

## Capabilities

### New Capabilities

- `llm-provider-integration`: integração de provedores LLM (Ollama e OpenAI
  Compliance API) por porta desacoplada, para interpretação e enriquecimento
  semântico de histórias sem decisão de regras de contagem.

### Modified Capabilities

_(nenhuma)_

## Impact

- Código: `agent-service` (orquestração com LLM), `rag-service` (embeddings),
  porta `ILLMProvider` + implementações.
- Dependências: clientes HTTP para Ollama/OpenAI (sem SDKs pesados).
- Infra: serviço `ollama` opcional no compose (docs já o citam).
