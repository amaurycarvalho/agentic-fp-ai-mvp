## ADDED Requirements

### Requirement: Armazenamento vetorial via Qdrant
O `rag-service` SHALL persistir trechos de conhecimento normativo em um
armazenamento vetorial Qdrant, acessado pela porta de infraestrutura de recuperação.

#### Scenario: Persistência no Qdrant
- **WHEN** um documento normativo é ingerido
- **THEN** seus trechos são indexados no Qdrant com metadados de origem

### Requirement: Ingestão de documentos normativos
O `rag-service` SHALL ingerir documentos (chunking por seções + geração de
embeddings), preservando metadados de origem (documento, seção, referência).

#### Scenario: Ingestão com metadados
- **WHEN** um documento é submetido à ingestão
- **THEN** cada trecho gerado carrega metadados de origem e é armazenado com embedding

#### Scenario: Documento duplicado
- **WHEN** um documento já ingerido é submetido novamente
- **THEN** a ingestão é idempotente (sem duplicação de trechos)

### Requirement: Consulta por relevância vetorial
A consulta ao rag-service SHALL retornar os trechos mais relevantes com base em
similaridade vetorial (embeddings da consulta × armazenados).

#### Scenario: Ranking por similaridade
- **WHEN** uma consulta é realizada
- **THEN** os trechos são ordenados por relevância vetorial e retornados com metadados

### Requirement: Qdrant no docker-compose
O `docker-compose.yml` SHALL incluir o serviço `qdrant` com volume persistente, e o
`rag-service` SHALL depender dele (healthcheck/ready).

#### Scenario: Stack sube com Qdrant
- **WHEN** `docker-compose up` é executado
- **THEN** o `qdrant` sobe com volume persistente e o `rag-service` fica ready

### Requirement: Testes da integração vetorial
O projeto SHALL possuir testes da ingestão e da consulta com o provider Qdrant
(integração) e do desacoplamento da porta (unitários com fake).

#### Scenario: Testes da ingestão/consulta
- **WHEN** os testes da integração vetorial são executados
- **THEN** a ingestão idempotente e a consulta por relevância são validadas
