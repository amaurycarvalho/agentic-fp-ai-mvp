# Tasks: CLI Tool

## 1. Estrutura

- [ ] 1.1 Criar projeto `tools/agentic-fp-cli` (console) e `tools/agentic-fp-cli.Tests`
- [ ] 1.2 Adicionar os projetos à solução raiz
- [ ] 1.3 Adicionar `System.CommandLine` e referências necessárias

## 2. Cliente de API

- [ ] 2.1 Criar `ApiClient` (HttpClient tipado) com base URL e token configuráveis
- [ ] 2.2 Implementar métodos de contagem, contexto, medição e health
- [ ] 2.3 Tratar erros de API e códigos de saída não-zero

## 3. Comandos e render

- [ ] 3.1 Implementar comando `count`
- [ ] 3.2 Implementar comando `context`
- [ ] 3.3 Implementar comando `measure` (com `--method`)
- [ ] 3.4 Implementar comando `health`
- [ ] 3.5 Implementar output legível e modo `--json`

## 4. Configuração e testes

- [ ] 4.1 Configurar URL base e token via argumentos/env
- [ ] 4.2 Adicionar testes das operações (fakes do HttpClient)
- [ ] 4.3 Adicionar testes de parsing de saída/erros
- [ ] 4.4 Garantir `dotnet test` passando
