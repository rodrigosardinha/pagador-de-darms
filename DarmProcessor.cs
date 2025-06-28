using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Path = System.IO.Path;
using PdfReader = iTextSharp.text.pdf.PdfReader;

namespace pagador_de_darms
{
    public class DarmProcessor
    {
        public string BaseDir { get; private set; }
        public string DarmsDir { get; private set; }
        public string OutputDir { get; private set; }
        public List<string> GuiasProcessadas { get; private set; } = new List<string>();
        public List<string> AllSQLInserts { get; private set; } = new List<string>();

        public DarmProcessor()
        {
            // Caminho da raiz do projeto (3 níveis acima do executável)
            BaseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            OutputDir = Path.Combine(BaseDir, "inserts");
            
            // Criar diretório de saída se não existir
            if (!Directory.Exists(OutputDir))
                Directory.CreateDirectory(OutputDir);
        }

        public async Task<List<(string Arquivo, DadosDarm Dados)>> ProcessarDarmsComRetornoAsync(string pastaDarms, IProgress<(string status, int progress)> progress)
        {
            var arquivos = Directory.GetFiles(pastaDarms, "*.pdf");
            var resultado = new List<(string, DadosDarm)>();
            if (arquivos.Length == 0)
            {
                progress.Report(("Nenhum arquivo PDF encontrado na pasta selecionada", 0));
                return resultado;
            }

            progress.Report(($"Encontrados {arquivos.Length} arquivos PDF para processar", 10));

            for (int i = 0; i < arquivos.Length; i++)
            {
                var arquivo = arquivos[i];
                var progresso = 10 + (int)((i + 1) * 70.0 / arquivos.Length);
                progress.Report(($"Processando {Path.GetFileName(arquivo)}...", progresso));
                try
                {
                    var texto = await ExtrairTextoPdfAsync(arquivo);
                    var dados = ExtrairDadosDarm(texto);
                    if (dados != null)
                    {
                        var sql = GerarSQLInsert(dados);
                        var nomeArquivo = $"INSERT_DARM_PAGO_{dados.NumeroGuia}.sql";
                        var caminhoArquivo = Path.Combine(OutputDir, nomeArquivo);
                        await File.WriteAllTextAsync(caminhoArquivo, sql, Encoding.UTF8);
                        GuiasProcessadas.Add(dados.NumeroGuia);
                        AllSQLInserts.Add(nomeArquivo);
                        resultado.Add((Path.GetFileName(arquivo), dados));
                    }
                }
                catch (Exception ex)
                {
                    progress.Report(($"Erro ao processar {Path.GetFileName(arquivo)}: {ex.Message}", progresso));
                }
            }

            progress.Report(("Gerando arquivo consolidado...", 85));
            await GerarArquivoConsolidadoAsync();
            progress.Report(("Gerando relatório final...", 95));
            await GerarRelatorioAsync();
            progress.Report(("Processamento concluído!", 100));
            return resultado;
        }

        private async Task<string> ExtrairTextoPdfAsync(string arquivoPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var reader = new PdfReader(arquivoPath))
                    {
                        var texto = new StringBuilder();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            var strategy = new SimpleTextExtractionStrategy();
                            var conteudo = PdfTextExtractor.GetTextFromPage(reader, i, strategy);
                            texto.AppendLine(conteudo);
                        }
                        return texto.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao extrair texto do PDF {arquivoPath}: {ex.Message}");
                    return "";
                }
            });
        }

        private DadosDarm? ExtrairDadosDarm(string texto)
        {
            var dados = new DadosDarm();

            // Receita (ex: 262-3)
            var receitaMatch = Regex.Match(texto, @"01\. RECEITA\s*([\d\-]+)", RegexOptions.IgnoreCase);
            if (receitaMatch.Success)
                dados.CodigoReceita = receitaMatch.Groups[1].Value.Replace("-", "");

            // Inscrição Municipal
            var inscricaoMatch = Regex.Match(texto, @"02\. INSCRIÇÃO MUNICIPAL\s*(\d+)", RegexOptions.IgnoreCase);
            if (inscricaoMatch.Success)
                dados.Inscricao = inscricaoMatch.Groups[1].Value;

            // Data de Vencimento
            var vencimentoMatch = Regex.Match(texto, @"03\. DATA VENCIMENTO\s*(\d{2}/\d{2}/\d{4})", RegexOptions.IgnoreCase);
            if (vencimentoMatch.Success)
                dados.DataVencimento = vencimentoMatch.Groups[1].Value;

            // Ano de Referência
            var exercicioMatch = Regex.Match(texto, @"04\. ANO DE REFERÊNCIA\s*(\d{4})", RegexOptions.IgnoreCase);
            if (exercicioMatch.Success)
                dados.Exercicio = exercicioMatch.Groups[1].Value;

            // Guia (remover zeros à esquerda)
            var guiaMatch = Regex.Match(texto, @"05\. GUIA N[ØO]\s*0*(\d+)", RegexOptions.IgnoreCase);
            if (guiaMatch.Success)
                dados.NumeroGuia = guiaMatch.Groups[1].Value;

            // Valor do Tributo
            var valorPrincipalMatch = Regex.Match(texto, @"06\. VALOR DO TRIBUTO\s*R\$\s*([\d\.,]+)", RegexOptions.IgnoreCase);
            if (valorPrincipalMatch.Success)
                dados.ValorPrincipal = valorPrincipalMatch.Groups[1].Value;

            // Valor Total
            var valorTotalMatch = Regex.Match(texto, @"09\. VALOR TOTAL\s*R\$\s*([\d\.,]+)", RegexOptions.IgnoreCase);
            if (valorTotalMatch.Success)
                dados.ValorTotal = valorTotalMatch.Groups[1].Value;

            // Competência (não existe no exemplo, mas pode ser o ano de referência)
            dados.Competencia = dados.Exercicio;

            // Se não encontrou valor principal, usar valor total
            if (string.IsNullOrEmpty(dados.ValorPrincipal) && !string.IsNullOrEmpty(dados.ValorTotal))
                dados.ValorPrincipal = dados.ValorTotal;

            // Se não conseguiu extrair dados essenciais, retorna null
            if (string.IsNullOrEmpty(dados.NumeroGuia) || string.IsNullOrEmpty(dados.Inscricao) || string.IsNullOrEmpty(dados.ValorPrincipal))
                return null;

            return dados;
        }

        private string GerarSQLInsert(DadosDarm dados)
        {
            // Converter data de vencimento do formato DD/MM/YYYY para YYYY-MM-DD
            string dataVencimento = null;
            if (!string.IsNullOrEmpty(dados.DataVencimento))
            {
                var partes = dados.DataVencimento.Split('/');
                if (partes.Length == 3)
                {
                    dataVencimento = $"{partes[2]}-{partes[1]}-{partes[0]} 00:00:00";
                }
            }

            // Converter competência do formato MM/YYYY para YYYY
            int competencia = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(dados.Competencia))
            {
                var partes = dados.Competencia.Split('/');
                if (partes.Length == 2 && int.TryParse(partes[1], out int ano))
                {
                    competencia = ano;
                }
            }

            // Processar valores monetários
            var valorPrincipal = ParseMonetaryValue(dados.ValorPrincipal);
            var valorTotal = ParseMonetaryValue(dados.ValorTotal ?? dados.ValorPrincipal);

            // Limitar código de barras a 48 dígitos e remover caracteres não numéricos
            string codigoBarras = null;
            if (!string.IsNullOrEmpty(dados.CodigoBarras))
            {
                codigoBarras = Regex.Replace(dados.CodigoBarras, @"\D", "");
                if (codigoBarras.Length > 48) codigoBarras = codigoBarras.Substring(0, 48);
            }

            // Usar código de receita do PDF ou valor padrão
            var codigoReceita = !string.IsNullOrEmpty(dados.CodigoReceita) ? dados.CodigoReceita : "2585";

            // Gerar expressão SQL para SQ_DOC dinâmico
            var numeroGuia = RemoveLeadingZeros(dados.NumeroGuia ?? "0");
            var sqDocExpression = $"((({numeroGuia} % 1000) * 1000) + (UNIX_TIMESTAMP() % 1000)) % 1000000";

            // Gerar SQL no formato correto
            var sql = $@"use silfae;

INSERT INTO FarrDarmsPagos (
    id, AA_EXERCICIO, CD_BANCO, NR_BDA, NR_COMPLEMENTO, NR_LOTE_NSA, TP_LOTE_D,
    SQ_DOC, CD_RECEITA, CD_USU_ALT, CD_USU_INCL, DT_ALT, DT_INCL, DT_VENCTO,
    DT_PAGTO, NR_INSCRICAO, NR_GUIA, NR_COMPETENCIA, NR_CODIGO_BARRAS,
    NR_LOTE_IPTU, ST_DOC_D, TP_IMPOSTO, VL_PAGO, VL_RECEITA, VL_PRINCIPAL,
    VL_MORA, VL_MULTA, VL_MULTAF_TCDL, VL_MULTAP_TSD, VL_INSU_TIP, VL_JUROS,
    processado, criticaProcessamento
) VALUES (
    NULL, {dados.Exercicio ?? "2025"}, 70, 37, 0, 730, 1,
    {sqDocExpression}, {codigoReceita}, NULL, 'FARR', NULL,
    NOW(), {(dataVencimento != null ? $"'{dataVencimento}'" : "NULL")}, NOW(),
    '{dados.Inscricao}', {RemoveLeadingZeros(dados.NumeroGuia ?? "NULL")}, {competencia}, {(codigoBarras != null ? $"'{codigoBarras}'" : "NULL")},
    NULL, '13', NULL, {valorTotal}, {valorTotal}, {valorPrincipal},
    0.00, 0.00, NULL, NULL, NULL, 0.00,
    0, NULL
);";

            return sql;
        }

        private string RemoveLeadingZeros(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "NULL") return value;
            return value.TrimStart('0') == "" ? "0" : value.TrimStart('0');
        }

        private string ParseMonetaryValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "0.00";

            // Remover R$, espaços e pontos de milhares
            var cleanValue = Regex.Replace(value, @"[R$\s]", "");

            // Se tem vírgula, tratar como separador decimal brasileiro
            if (cleanValue.Contains(","))
            {
                // Se tem ponto antes da vírgula, é formato brasileiro (ex: 9.014,06)
                if (cleanValue.Contains("."))
                {
                    // Remover pontos de milhares e converter vírgula para ponto
                    cleanValue = cleanValue.Replace(".", "").Replace(",", ".");
                }
                else
                {
                    // Só vírgula, converter para ponto
                    cleanValue = cleanValue.Replace(",", ".");
                }
            }

            try
            {
                var numericValue = float.Parse(cleanValue);
                return $"{numericValue:F2}";
            }
            catch
            {
                return "0.00";
            }
        }

        private async Task GerarRelatorioAsync()
        {
            var relatorio = $@"# Relatório de Processamento de DARMs

## Resumo
- **Data/Hora:** {DateTime.Now:dd/MM/yyyy HH:mm:ss}
- **Total de arquivos processados:** {GuiasProcessadas.Count}
- **Arquivos SQL individuais gerados:** {AllSQLInserts.Count}
- **Arquivo consolidado:** INSERT_TODOS_DARMs.sql
- **Pasta de saída:** {OutputDir}

## Guias Processadas
{string.Join("\n", GuiasProcessadas.Select(g => $"- Guia {g}"))}

## Arquivos Gerados
### Arquivos Individuais:
{string.Join("\n", AllSQLInserts.Select(f => $"- {f}"))}

### Arquivo Consolidado:
- INSERT_TODOS_DARMs.sql (contém todos os {GuiasProcessadas.Count} INSERTs)
";

            var caminhoRelatorio = Path.Combine(OutputDir, "RELATORIO_PROCESSAMENTO.md");
            await File.WriteAllTextAsync(caminhoRelatorio, relatorio, Encoding.UTF8);
        }

        private async Task GerarArquivoConsolidadoAsync()
        {
            if (!AllSQLInserts.Any())
            {
                Console.WriteLine("Nenhum INSERT para gerar no arquivo consolidado.");
                return;
            }

            var consolidado = new StringBuilder();
            consolidado.AppendLine("use silfae;");
            consolidado.AppendLine();
            consolidado.AppendLine("INSERT INTO FarrDarmsPagos (");
            consolidado.AppendLine("    id, AA_EXERCICIO, CD_BANCO, NR_BDA, NR_COMPLEMENTO, NR_LOTE_NSA, TP_LOTE_D,");
            consolidado.AppendLine("    SQ_DOC, CD_RECEITA, CD_USU_ALT, CD_USU_INCL, DT_ALT, DT_INCL, DT_VENCTO,");
            consolidado.AppendLine("    DT_PAGTO, NR_INSCRICAO, NR_GUIA, NR_COMPETENCIA, NR_CODIGO_BARRAS,");
            consolidado.AppendLine("    NR_LOTE_IPTU, ST_DOC_D, TP_IMPOSTO, VL_PAGO, VL_RECEITA, VL_PRINCIPAL,");
            consolidado.AppendLine("    VL_MORA, VL_MULTA, VL_MULTAF_TCDL, VL_MULTAP_TSD, VL_INSU_TIP, VL_JUROS,");
            consolidado.AppendLine("    processado, criticaProcessamento");
            consolidado.AppendLine(") VALUES");

            var valuesList = new List<string>();

            foreach (var arquivo in AllSQLInserts)
            {
                var caminhoArquivo = Path.Combine(OutputDir, arquivo);
                if (File.Exists(caminhoArquivo))
                {
                    var conteudo = await File.ReadAllTextAsync(caminhoArquivo, Encoding.UTF8);
                    
                    // Extrair apenas a parte VALUES do INSERT
                    var valuesMatch = Regex.Match(conteudo, @"VALUES\s*\(\s*(.+?)\s*\);", RegexOptions.Singleline);
                    if (valuesMatch.Success)
                    {
                        var valuesPart = valuesMatch.Groups[1].Value.Trim();
                        valuesList.Add($"({valuesPart})");
                    }
                }
            }

            // Adicionar todos os VALUES separados por vírgula
            consolidado.AppendLine(string.Join(",\n", valuesList));
            consolidado.AppendLine(";");

            var caminhoConsolidado = Path.Combine(OutputDir, "INSERT_TODOS_DARMs.sql");
            await File.WriteAllTextAsync(caminhoConsolidado, consolidado.ToString(), Encoding.UTF8);
        }
    }

    public class DadosDarm
    {
        public string Inscricao { get; set; } = "";
        public string NumeroGuia { get; set; } = "";
        public string Valor { get; set; } = "0";
        public string DataVencimento { get; set; } = "";
        public string CodigoBarras { get; set; } = "";
        public string CodigoReceita { get; set; } = "";
        public string ValorPrincipal { get; set; } = "";
        public string ValorTotal { get; set; } = "";
        public string Exercicio { get; set; } = "";
        public string Competencia { get; set; } = "";
    }
} 