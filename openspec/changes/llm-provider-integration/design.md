# Design: LLM Provider Integration

## Context

Constituição: IA agêntica desacoplada via MCP e RAG; decisão separada de execução;
capacidades expostas à IA explícitas (MCP). O provedor LLM (Ollama ou OpenAI
Compliance API) apoia interpretação semântica e embeddings, mas NUNCA decide regras
de contagem (determinismo preservado).

## Goals / Non-Goals

**Goals:**
- Porta `ILLMProvider` (chat + embedding) com provedores Ollama e OpenAI.
- Enriquecimento semântico de histórias (rascunho revisável).
- Embeddings do RAG via provedor.

**Non-Goals:**
- Decisão de regra de contagem por LLM — determinismo é inegociável (Constitution).
- Agentes complexos/tool-calling — MVP simples.
- Treinamento/fine-tuning.

## Decisions

### D1 — `ILLMProvider` com `ChatAsync` e `EmbedAsync`
Contrato mínimo:
- `ChatAsync(messages, options) → string`;
- `EmbedAsync(texts) → float[][]`.
Implementações `OllamaProvider` e `OpenAIComplianceProvider` selecionadas por
configuração (`LLM:Provider`).

### D2 — Enriquecimento revisável
No `agent-service`, o LLM propõe um rascunho de modelo funcional (elementos
candidatos) que é retornado para **revisão humana**, nunca consumido como decisão
final de contagem.

### D3 — Embeddings desacoplados
O `rag-service` usa `ILLMProvider.EmbedAsync` via a interface `IEmbeddingProvider`
(change `vector-database-rag`), mantendo o domínio isolado.

### D4 — Ollama opcional no compose
Serviço `ollama` marcado como opcional (comentado/profiled) com volume para modelos;
README documenta o pull de modelo (ex.: `llama3`, `nomic-embed-text`).

## Risks / Trade-offs

- **[Qualidade e custo do provedor]** → Ollama é gratuito/local; OpenAI pago — 
  escolha por configuração; custos documentados.
- **[Vazamento de dados para provedor remoto]** → LGPD/hardening: apenas conteúdo
  mínimo necessário; OpenAI pode ser vetado em ambientes restritos (preferir Ollama).

## Migration Plan

1. Definir `ILLMProvider` e DTOs.
2. Implementar OllamaProvider.
3. Implementar OpenAIComplianceProvider.
4. Integrar enriquecimento no agente (revisável).
5. Integrar embeddings no RAG.
6. Ollama no compose; testes.

## Open Questions

- Modelo padrão de chat e de embedding do MVP.
