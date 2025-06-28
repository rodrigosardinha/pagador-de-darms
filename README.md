# 🏛️ Pagador de DARMs

**Sistema automatizado para processamento de DARMs (Documento de Arrecadação de Receitas Municipais) em PDF e geração de scripts SQL para inserção em banco de dados.**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/csharp/)
[![Windows Forms](https://img.shields.io/badge/Windows%20Forms-0078D4?logo=windows&logoColor=white)](https://docs.microsoft.com/pt-br/dotnet/desktop/winforms/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

## 📋 Descrição

O **Pagador de DARMs** é uma aplicação desktop desenvolvida em C# que automatiza o processamento de documentos de arrecadação municipal (DARMs) em formato PDF. O sistema extrai dados relevantes dos PDFs e gera scripts SQL prontos para inserção em banco de dados, facilitando a gestão de receitas municipais.

### 🎯 Principais Funcionalidades

- ✅ **Processamento em Lote**: Processa múltiplos arquivos PDF simultaneamente
- ✅ **Extração Inteligente**: Extrai automaticamente dados como guia, inscrição, valores, vencimentos
- ✅ **Geração de SQL**: Cria scripts INSERT individuais e consolidados
- ✅ **Interface Intuitiva**: Interface gráfica moderna e responsiva
- ✅ **Relatórios Detalhados**: Gera relatórios PDF com estatísticas completas
- ✅ **Logs em Tempo Real**: Acompanhamento detalhado do processamento
- ✅ **Validação de Dados**: Verificação automática de integridade dos dados extraídos

## 🚀 Tecnologias Utilizadas

- **.NET 8.0** - Framework de desenvolvimento
- **C#** - Linguagem de programação
- **Windows Forms** - Interface gráfica
- **iTextSharp 5.5.13.4** - Biblioteca para manipulação de PDFs
- **PDFsharp 6.2.0** - Processamento adicional de PDFs
- **BouncyCastle.Cryptography** - Criptografia e segurança

## 📦 Instalação

### Pré-requisitos

- Windows 10/11
- .NET 8.0 Runtime ou SDK
- Visual Studio 2022 (para desenvolvimento) ou VS Code

### Passos para Instalação

1. **Clone o repositório**
   ```bash
   git clone https://github.com/rodrigosardinha/pagador-de-darms.git
   cd pagador-de-darms
   ```

2. **Restaura as dependências**
   ```bash
   dotnet restore
   ```

3. **Compila o projeto**
   ```bash
   dotnet build
   ```

4. **Executa a aplicação**
   ```bash
   dotnet run
   ```

## 🎮 Como Usar

### 1. Configuração Inicial
- Execute a aplicação
- Clique em **"Selecionar Pasta DARMs"** para escolher a pasta contendo os arquivos PDF

### 2. Processamento
- Clique em **"Processar DARMs"** para iniciar o processamento
- Acompanhe o progresso através da barra de progresso e logs em tempo real

### 3. Resultados
- Os arquivos SQL serão gerados na pasta `inserts/`
- Clique em **"Abrir Pasta"** para visualizar os resultados
- Use **"Exportar Relatório"** para gerar um relatório PDF detalhado

### 4. Ações Adicionais
- **"Reprocessar"**: Reprocessa todos os arquivos
- **"Exportar Relatório"**: Gera relatório PDF com estatísticas completas

## 📊 Estrutura do Projeto

```
pagador-de-darms/
├── 📁 bin/                    # Arquivos compilados
├── 📁 icons/                  # Ícones da interface
├── 📁 inserts/                # Scripts SQL gerados
├── 📁 obj/                    # Arquivos de compilação
├── 📁 relatorios/             # Relatórios PDF exportados
├── 📄 DarmProcessor.cs        # Lógica de processamento
├── 📄 Form1.cs               # Interface principal
├── 📄 Form1.Designer.cs      # Designer da interface
├── 📄 Program.cs             # Ponto de entrada
├── 📄 pagador-de-darms.csproj # Configuração do projeto
└── 📄 README.md              # Este arquivo
```

## 🔧 Funcionalidades Técnicas

### Extração de Dados
O sistema extrai automaticamente os seguintes campos dos PDFs:
- **Número da Guia**
- **Inscrição Municipal**
- **Código da Receita**
- **Valor Principal**
- **Valor Total**
- **Data de Vencimento**
- **Ano de Exercício**
- **Competência**

### Geração de SQL
- **Arquivos Individuais**: `INSERT_DARM_PAGO_[GUIA].sql`
- **Arquivo Consolidado**: `INSERT_TODOS_DARMs.sql`
- **Relatório Markdown**: `RELATORIO_PROCESSAMENTO.md`

### Relatório PDF
O relatório inclui:
- Informações gerais do processamento
- Dados de origem (arquivos, tamanhos, datas)
- Estatísticas de processamento
- Dados extraídos dos PDFs
- Informações técnicas
- Log completo de execução

## 📈 Estatísticas de Processamento

O sistema fornece estatísticas detalhadas:
- **Taxa de sucesso** do processamento
- **Velocidade média** por arquivo
- **Tamanho total** processado
- **Duração** do processamento
- **Arquivos com erro** (se houver)

## 🛠️ Desenvolvimento

### Estrutura de Classes

- **`DarmProcessor`**: Classe principal de processamento
- **`DadosDarm`**: Modelo de dados extraídos
- **`Form1`**: Interface gráfica principal

### Padrões Utilizados

- **Processamento Assíncrono**: Para não travar a interface
- **Progress Reporting**: Feedback em tempo real
- **Error Handling**: Tratamento robusto de erros
- **Logging**: Sistema de logs detalhado

## 🔒 Segurança

- Validação de tipos de arquivo (apenas PDFs)
- Tratamento seguro de caminhos de arquivo
- Verificação de integridade dos dados extraídos
- Logs de auditoria para rastreabilidade

## 📝 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 🤝 Contribuição

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Suporte

Para suporte ou dúvidas:
- Abra uma [Issue](https://github.com/rodrigosardinha/pagador-de-darms/issues) no GitHub
- Entre em contato através do repositório

## 🎯 Roadmap

- [ ] Suporte a outros formatos de documento
- [ ] Integração direta com banco de dados
- [ ] Interface web
- [ ] Processamento em nuvem
- [ ] API REST para integração

---

**Desenvolvido com ❤️ para facilitar a gestão de receitas municipais**

*Última atualização: Janeiro 2025* 