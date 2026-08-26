# Non Functional Requirements

## Arquitetura
- IA Agêntica com orquestração autônoma;
- Serviços determinísticos via MCP (Model Context Protocol);
- RAG (Retrieval-Augmented Generation) com base em normas oficiais;
- C#, microserviços containerizados (docker), DDD e Clean Architecture.

## Comunicação
- API REST (externa) + gRPC (interna);
- Disponível como API, CLI e UI Web.

## Performance
- Tempo médio de resposta < 5 segundos.

## Escalabilidade
- Escalável horizontalmente via Docker.

## Segurança
- JWT para API pública;
- TLS obrigatório;
- Logs criptografados.

## Observabilidade
- OpenTelemetry;
- Logs estruturados e auditáveis.

## Compliance
- Compatível com LGPD;
- Compatível com auditoria TCU (IFPUG/SISP/SERPRO);
- Compatível com OpenAI Compliance API.
