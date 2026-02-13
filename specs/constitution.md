# Constitution

Este documento define os princípios imutáveis do sistema.

---

## Princípios gerais

1. Toda automação deve ser reversível e auditável.
2. Decisão é separada de execução.
3. Conhecimento (RAG) não executa ações.
4. Capacidades expostas à IA devem ser explícitas (MCP).
5. Especificações precedem código.

Estes princípios não são opcionais.

## Princípios Arquiteturais

1. Domain-Driven Design (DDD)
2. Clean Architecture
3. Microservices independentes
4. C#/.NET como stack principal
5. Spec Driven Development (SDD)
6. IA agêntica desacoplada via MCP e RAG
7. TDD + BDD para histórias de usuário
8. Containers isolados por serviço
9. REST externo e gRPC interno
10. Entregável como API, CLI e UI

## Valores

- Auditabilidade
- Rastreabilidade
- Reprodutibilidade
- Aderência às normas oficiais 

---

## Padrão obrigatório de use case

Todos os casos de uso devem conter:

1. História de Usuário no formato: "Como [ator], preciso [funcionalidade] para que [benefício/valor]".
2. Critérios de Aceite explícitos.
3. Cenários comportamentais em BDD (`Dado / Quando / Então`).

---

## Diretrizes técnicas obrigatórias

- Consultar a pasta `specs/adr` para decisões arquiteturais vigentes.

### Restrições tecnológicas

- Não utilizar frameworks, SDKs, bibliotecas, componentes ou serviços de terceiros pouco conhecidos, não validados profissionalmente ou sem manutenção ativa.

### Segurança e qualidade

- Alertar se houver vulnerabilidades conhecidas (OWASP, CISA) e propor mitigação.
- Propor remoção de pacotes não utilizados.

---

## Estratégia de testes

- Uso de TDD.
- Todo endpoint deve ter pelo menos um teste unitário associado.
- Cobertura mínima de 100% para código de média/alta criticidade.
- Cobertura mínima de 90% para código de baixa criticidade.

