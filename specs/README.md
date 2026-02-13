# Agentic Function Point AI

Projeto baseado em Spec Driven Development (SDD) para construção de um agente autônomo em C#/.NET para contagem de Pontos de Função (IFPUG) e Simple Function Point.

## Arquitetura

O projeto segue os seguintes princípios:

- Domain-Driven Design (DDD);
- Clean Architecture;
- Microserviços independentes;
- Comunicação interna via gRPC;
- API externa REST;
- Containers isolados por serviço;
- Spec Driven Development (SDD);
- TDD e BDD para histórias de usuário;
- IA desacoplada via MCP e RAG.

### Componentes principais

- **Agent Orchestrator** (agent-service);
- **Serviço MCP de Contagem** (mcp-service);
- **Serviço RAG com Vector Database** (rag-service);
- **Provedor LLM (Ollama ou OpenAI Compliance API)**.

### Execução com Docker

```bash
docker-compose up --build
```

### Serviços:

- agent;
- mcp-server;
- rag-service;
- qdrant;
- ollama (opcional).

### Interfaces Disponíveis

- REST API pública;
- gRPC interno;
- CLI;
- UI Web.

