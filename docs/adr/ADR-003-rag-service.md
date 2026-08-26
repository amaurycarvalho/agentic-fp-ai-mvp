# ADR-003 – Decisões arquiteturais para o rag-service

---

## Status

Aceito

## Contexto

Serviço RAG com Vector Database para consulta de conhecimento normativo e apoio contextual à orquestração.

---

## Decisão

1. O `rag-service` será responsável apenas por ingestão/consulta de conhecimento, sem executar ações de negócio.
2. O armazenamento vetorial será desacoplado por porta de infraestrutura, permitindo troca de provedor sem alterar domínio.
3. A API inicial exporá operações mínimas de health-check e consulta, evoluindo para gRPC interno.
4. Respostas de recuperação devem incluir metadados de origem para auditabilidade.

---

## Racional

- Mantém o princípio constitucional de separar conhecimento de execução.
- Permite escalar recuperação sem impactar serviços de decisão.
- Facilita conformidade por evidência de fonte consultada.

---

## Decisões Relacionadas

- ADR-001
