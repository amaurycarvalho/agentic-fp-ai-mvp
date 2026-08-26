# Agentic Function Point AI

MVP de IA agêntica para contagem automatizada de Pontos de Função (IFPUG) e Pontos de Função Simplificados (SFP) a partir de histórias de usuário ou documentação técnica no padrão SDD.

[![Spec-Driven Development](https://img.shields.io/badge/SDD-OpenSpec-yellow)](openspec/specs/architecture-foundation/spec.md)

---

## Visão Geral

O Agentic Function Point AI é um protótipo construído em C#/.NET que utiliza:

- IA Agêntica com orquestração autônoma;
- Serviços determinísticos via MCP (Model Context Protocol);
- RAG (Retrieval-Augmented Generation) com base em normas oficiais;
- DDD + Clean Architecture;
- Microserviços containerizados;
- Containers Docker;
- API REST (externa) + gRPC (interna);
- Disponível como API, CLI e UI Web (CLI e UI em roadmap).

O objetivo principal do projeto é disponibilizar para interessados no tema um protótipo de assistente de IA especialista em contagem de pontos de função.

> **Nota:**
>
> 1. As implementações atuais de **FPA**, **SFP** e **SNAP** são **protótipos preliminares** destinados exclusivamente a fins de demonstração e validação. Elas oferecem uma aproximação altamente simplificada de suas respectivas metodologias de medição e **não constituem implementações completas ou em conformidade com as normas**. A conformidade integral com as especificações oficiais requer regras de contagem, lógica de validação e detalhes metodológicos adicionais que extrapolam o escopo destas implementações de protótipo.

### Público-Alvo

- Analistas de métricas;
- Fábricas de software;
- Órgãos públicos;
- Consultorias de estimativa;
- Times ágeis que precisam medir escopo com precisão.

---

## Contribuição

Contribuições são bem-vindas via Pull Request.

---

## Licença

Uso livre para fins educacionais e experimentais.

> Adapte, evolua e questione.

---

## 🧑‍💻 Para Usuários

### Como Instalar

A forma mais simples é usar as imagens de container publicadas nas
[Releases](https://github.com/amaurycarvalho/agentic-fp-ai-mvp/releases):

1. Baixe os tarballs dos serviços desejados (`*-service.tar.gz`);
2. Baixe também o `docker-compose.release.yml` da mesma Release;
3. Carregue cada tarball com `docker load` e retague a imagem para `latest`;
4. Suba a stack com `docker-compose -f docker-compose.release.yml up -d`.

```bash
# Download: pegue os tarballs (*-service.tar.gz) e o docker-compose.release.yml da Release
for img in agent-service mcp-service rag-service; do
  gunzip -c "$img.tar.gz" | docker load
  docker tag "$img:<versão>" "$img:latest"
done
docker-compose -f docker-compose.release.yml up -d
```

Também é possível construir tudo do código-fonte:

```bash
git clone https://github.com/amaurycarvalho/agentic-fp-ai-mvp.git
cd agentic-fp-ai-mvp
make install
make build-images
docker-compose up -d --build
```

### Como Usar

Após subir a stack, a API de cada serviço fica disponível:

- `agent-service`: http://localhost:8080
- `mcp-service`: http://localhost:8082
- `rag-service`: http://localhost:8083

Exemplo de chamada de contagem básica no `mcp-service`:

```bash
curl -X POST http://localhost:8082/count/basic \
  -H "Content-Type: application/json" \
  -d '{"userStory":"Como analista, preciso cadastrar clientes e armazenar dados no banco."}'
```

Para encerrar os serviços:

```bash
docker-compose down
```

---

## 👨‍🔧 Para Desenvolvedores

### Como Instalar

#### Baixando o codigo fonte

```bash
git clone https://github.com/amaurycarvalho/agentic-fp-ai-mvp.git
```

#### Como Compilar

```bash
make install
make build
```

Requisitos:

- .NET SDK 8.0
- Docker (para `make build-images` e `docker-compose`)

#### Testes unitários

```bash
make test
```

Executa os testes unitários e coleta cobertura por serviço (exclui a integração MCP
que exige stack ativa). Para validar os testes de integração do MCP ponta a ponta,
suba a stack e rode a integração:

```bash
docker-compose up -d --build
MCP_BASE_URL=http://localhost:8082 make test-integration
```

#### Quality Gate

O _quality gate_ executa lint + testes (com cobertura) + verificação de cobertura +
métricas + segurança:

```bash
make quality-gate
```

Verificações individuais:

```bash
make lint               # formato/análise (dotnet format --verify-no-changes)
make test               # testes + cobertura
make coverage-check     # cobertura contra COVERAGE_THRESHOLD (default 90)
make metrics            # linhas de código (LOC) por serviço
make security           # pacotes vulnerables/deprecated/outdated + Semgrep SAST
```

Análise estática, complexidade, code smells, dívida técnica e rating de
manutenibilidade são coordenados pelo **SonarCloud** no CI, com um projeto por
serviço (análise per-service), _Leak Period_ sobre código novo e decoração de
Pull Requests. A cobertura é encaminhada via `TestResults/**/coverage.cobertura.xml`.

> **Jobs no CI:** o job `sonarcloud` (SonarCloud) e o job `integration-test`
> (integração MCP ponta a ponta) rodam **apenas em pull requests**. O job
> `quality-gate` (lint + test + coverage + metrics + security) roda em push para
> `main` e em pull requests.

#### Análise SonarCloud no pipeline CI

O SonarCloud requer a configuração dos secrets abaixo no Github:

```
SONAR_PROJECT_KEY_PREFIX
SONAR_ORG
SONAR_TOKEN
```

A chave do projeto no SonarCloud deverá seguir o padrão:

```
SONAR_PROJECT_KEY_PREFIX-service_name
```

Exemplo:

```
agentic-fp-ai-agent-service
agentic-fp-ai-mcp-service
agentic-fp-ai-rag-service
```

#### Análise SonarQube local (self-hosted)

Para analisar os serviços localmente contra um servidor **SonarQube
self-hosted** em execução (ex.: `http://localhost:9000`), instale o scanner e
rode a análise per-service:

```bash
make sonar-install
SONAR_TOKEN=<seu-token> make sonar-check
```

O `sonar-check` executa `begin → build + test (com cobertura) → end` para cada
um dos três serviços, um projeto SonarQube por serviço (chave =
`SONAR_PROJECT_KEY_PREFIX` + nome do serviço, ex.: `agentic-fp-ai-agent-service`).

##### Subindo um servidor SonarQube local (Docker Compose)

O repositório inclui uma stack local reproduzível (SonarQube Community +
PostgreSQL, com volumes persistentes) em `sonarqube/docker-compose.yml`,
baseada na referência oficial da SonarSource e com o mesmo hardening
(`read_only`, `tmpfs`, volumes nomeados). Fluxo completo:

```bash
make sonar-up        # sobe a stack e aguarda o SonarQube ficar pronto
# 1) Acesse http://localhost:9000 e faça login com admin / admin
# 2) Troque a senha no primeiro login (obrigatório)
# 3) My Account -> Security -> Tokens -> Generate (token de um usuário admin)
SONAR_TOKEN=<seu-token> make sonar-check   # analisa os 3 serviços
make sonar-down      # para a stack preservando os volumes
```

Com um token de um usuário **admin**, os três projetos per-service (as chaves
exibidas pelo `make sonar-up`, uma por serviço) são **criados automaticamente**
na primeira análise.

**Requisitos de host:**

- **Linux:** o Elasticsearch embutido exige `vm.max_map_count` maior; aplique
  `sudo sysctl -w vm.max_map_count=262144` (persista em `/etc/sysctl.conf`).
- **Docker Desktop (Windows/Mac):** reserve pelo menos 2–4 GB de memória para o
  engine (o compose define `SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true` para evitar
  a falha do `max_map_count`, que não é diretamente configurável nesses hosts).
- **Reset completo** (apaga dados da stack): `docker-compose -f sonarqube/docker-compose.yml down -v`.
- As credenciais `admin`/`admin` são padrão de desenvolvimento local — não use
  em produção.

Variáveis de ambiente:

- `SONAR_HOST_URL` — URL do servidor SonarQube (default `http://localhost:9000`);
- `SONAR_TOKEN` — token de autenticação (obrigatório);
- `SONAR_PROJECT_KEY_PREFIX` — prefixo das chaves de projeto (default `agentic-fp-ai-`).

> O estado local do scanner (`/.sonarqube`) é ignorado pelo git. A análise local
> usa os mesmos relatórios de cobertura cobertura (`TestResults/**/coverage.cobertura.xml`)
> do `make test`, com exclusão de fontes de teste.

#### Mutation testing (opcional)

O teste de mutação com **Stryker.NET** é manual e não entra no gate do CI:

```bash
make install-quality-tools
make mutation
```

Os reportes de mutação são gerados com threshold `high/low/break` (`80/70/60`).

Leve os reports disponíveis em `services/**/tests/**/StrykerOutput/**/reports/mutation-report.json` e `services/**/tests/**/StrykerOutput/**/reports/mutation-report.html` para análise do seu agente de codificação e solicite a criação de testes para matar os mutantes sobreviventes. Depois, rode o mutation testing novamente.

#### Testes de integração

Ative os serviços com `Docker Compose`, configure a variável de ambiente com a url base e depois rode o teste.

```bash
sudo docker-compose up -d --build --timeout 120
MCP_BASE_URL=http://localhost:8082 make test-integration
sudo docker-compose down
```

### Como Usar

#### Docker Compose

Suba os serviços (constrói as imagens a partir dos `Dockerfile`):

```bash
sudo docker-compose up -d --build
```

Acesse via:

- `agent-service`: http://localhost:8080
- `mcp-service`: http://localhost:8082
- `rag-service`: http://localhost:8083

Derrube os serviços:

```bash
sudo docker-compose down
```

#### Usando Imagens Pré-compiladas

Para usar uma imagem publicada em uma Release, baixe o tarball do serviço, carregue
no Docker e suba com o `docker-compose` (ou outro orquestrador):

```bash
# Download: pegue agent-service.tar.gz da Release
gunzip -c agent-service.tar.gz | docker load
docker tag agent-service:<versão> agent-service:latest
# Volte a referenciar a imagem no docker-compose.yml (ex.: image: agent-service:<versão>)
docker-compose up -d
```

> O mesmo procedimento se aplica a `mcp-service.tar.gz` e `rag-service.tar.gz`.

---

## Saiba Mais

- [Repositório do projeto](https://github.com/amaurycarvalho/agentic-fp-ai-mvp);
- [Releases com binários pré-compilados](https://github.com/amaurycarvalho/agentic-fp-ai-mvp/releases);
- [Function Point Analysis (IFPUG/FPA)](https://ifpug.org/ifpug-standards/fpa);
- [Simplified Function Point (IFPUG/SFP)](https://ifpug.org/ifpug-standards/sfp);
- [Software Non-Functional Assessment Process (IFPUG/SNAP)](https://ifpug.org/ifpug-standards/snap).
