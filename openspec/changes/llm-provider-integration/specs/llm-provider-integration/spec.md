## ADDED Requirements

### Requirement: Porta de provedor LLM desacoplada
O sistema SHALL definir uma porta `ILLMProvider` com operações de chat e
embedding, permitindo trocar de provedor (Ollama/OpenAI) por configuração sem
alterar o domínio.

#### Scenario: Troca de provedor por configuração
- **WHEN** o provedor é selecionado via configuração
- **THEN** a mesma porta é usada sem alteração na camada de aplicação

### Requirement: Provedor Ollama
O sistema SHALL suportar Ollama como provedor local, com servidor opcional no
`docker-compose.yml` e URL configurável.

#### Scenario: Provedor local
- **WHEN** Ollama é configurado
- **THEN** o sistema usa o Ollama para chat/embedding

### Requirement: Provedor OpenAI Compliance API
O sistema SHALL suportar a OpenAI Compliance API como provedor alternativo, com
chave e URL configuráveis.

#### Scenario: Provedor OpenAI
- **WHEN** a OpenAI Compliance API é configurada
- **THEN** o sistema usa o provedor remoto para chat/embedding

### Requirement: LLM não decide regras de contagem
O LLM SHALL ser usado apenas para interpretação e enriquecimento semântico de
histórias; a decisão determinística de contagem SHALL permanecer no motor
(`mcp-service`), sem inferência de regra no LLM.

#### Scenario: Decisão permanece determinística
- **WHEN** uma história é enriquecida pelo LLM
- **THEN** o resultado é tratado como rascunho para revisão e a contagem final é determinística

### Requirement: Integração com embeddings do RAG
O `rag-service` SHALL usar o provedor LLM para gerar embeddings de ingestão e de
consulta, mantendo a porta de embeddings desacoplada.

#### Scenario: Embeddings pelo provedor
- **WHEN** um documento é ingerido ou uma consulta é feita
- **THEN** os embeddings são gerados pelo provedor configurado

### Requirement: Testes do provedor LLM
O projeto SHALL possuir testes com fakes/mocks do `ILLMProvider` cobrindo chat,
embedding e a não-dependência da contagem do LLM.

#### Scenario: Testes do provedor
- **WHEN** os testes de integração do LLM são executados
- **THEN** os cenários de chat, embedding e independência da contagem passam
