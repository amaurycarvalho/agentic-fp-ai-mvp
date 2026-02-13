# ADR-001 – Arquitetura Base do agentic-fp-ai-mvp

---

## Status

Aceito

## Contexto

Definições gerais aplicáveis a todo o projeto.

---

## Decisão

Adotar como fundação:
1. DDD;
2. Clean Architecture;
3. Microserviços;
4. C#/.NET como stack principal;
5. Spec Driven Development (SDD);
6. IA agêntica desacoplada por MCP e RAG;
7. Histórias de usuário com TDD/BDD;
8. Containers por serviço com Docker Compose;
9. Comunicação interna priorizando gRPC e bordas HTTP conforme contexto.

---

## Estrutura de referência

```
agentic-fp-ai-mvp
|
+--agent-service
   |
   +--mcp-service
   |
   +--rag-service
   |
   +--LLM
```

---

## Racional

Separação clara de responsabilidades,
facilidade de testes,
evolução independente dos serviços.

---

## Decisões Relacionadas

