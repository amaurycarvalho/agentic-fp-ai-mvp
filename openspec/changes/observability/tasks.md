# Tasks: Observability

## 1. Instrumentação base

- [ ] 1.1 Adicionar pacotes OpenTelemetry aos três serviços
- [ ] 1.2 Registrar recursos e instrumentation (traces/métricas HTTP)
- [ ] 1.3 Configurar exportadores via configuração (OTLP/Prometheus)

## 2. Métricas de negócio

- [ ] 2.1 Criar `Meter` de negócio no mcp-service
- [ ] 2.2 Emitir histograma de duração de medição
- [ ] 2.3 Emitir gauges de contagens por componente/função

## 3. Logs e correlação

- [ ] 3.1 Garantir logs estruturados (INFO/ERROR) com contexto
- [ ] 3.2 Propagar identificador de correlação entre serviços e registrar em trilhas

## 4. Endpoint e testes

- [ ] 4.1 Expor endpoint `/metrics` (formato Prometheus)
- [ ] 4.2 Adicionar testes de emissão de métricas/logs
- [ ] 4.3 Garantir `dotnet test` passando
