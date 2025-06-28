using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace pagador_de_darms;

public partial class Form1 : Form
{
    private string pastaSaida = @"inserts";
    private string pastaRelatorios = @"relatorios";
    private string pastaDarms = "";
    private bool processando = false;
    private DarmProcessor processor;
    private DateTime inicioProcessamento;
    private int totalArquivos = 0;
    private int arquivosProcessados = 0;
    private int arquivosComErro = 0;
    
    // Dados detalhados para o relatório
    private List<string> arquivosProcessadosList = new List<string>();
    private List<string> arquivosComErroList = new List<string>();
    private List<string> guiasExtraidas = new List<string>();
    private Dictionary<string, object> dadosProcessamento = new Dictionary<string, object>();
    private long tamanhoTotalArquivos = 0;
    private List<string> arquivosGerados = new List<string>();
    private List<(string Arquivo, pagador_de_darms.DadosDarm Dados)> dadosExtraidosPorArquivo = new List<(string, pagador_de_darms.DadosDarm)>();

    public Form1()
    {
        InitializeComponent();
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = 0;
        labelStatus.Text = "Aguardando início...";
        
        processor = new DarmProcessor();
        
        // Criar pasta de relatórios se não existir
        if (!Directory.Exists(pastaRelatorios))
            Directory.CreateDirectory(pastaRelatorios);
    }

    private async Task IniciarProcessamentoAsync()
    {
        if (processando) return;
        
        if (string.IsNullOrEmpty(pastaDarms))
        {
            MessageBox.Show("Por favor, selecione uma pasta contendo os arquivos PDF dos DARMs primeiro.", 
                "Pasta não selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        processando = true;
        inicioProcessamento = DateTime.Now;
        arquivosProcessados = 0;
        arquivosComErro = 0;
        
        // Limpar dados do relatório anterior
        arquivosProcessadosList.Clear();
        arquivosComErroList.Clear();
        guiasExtraidas.Clear();
        dadosProcessamento.Clear();
        tamanhoTotalArquivos = 0;
        arquivosGerados.Clear();
        
        LimparRelatorio();
        AdicionarLog("=== INÍCIO DO PROCESSAMENTO ===");
        AdicionarLog($"Data/Hora de início: {inicioProcessamento:dd/MM/yyyy HH:mm:ss}");
        AdicionarLog($"Pasta DARMs: {pastaDarms}");
        AdicionarLog($"Pasta de saída: {pastaSaida}");
        AdicionarLog("");
        
        try
        {
            var arquivosPdf = Directory.GetFiles(pastaDarms, "*.pdf");
            totalArquivos = arquivosPdf.Length;
            
            // Coletar informações detalhadas dos arquivos
            foreach (var arquivo in arquivosPdf)
            {
                var fileInfo = new FileInfo(arquivo);
                tamanhoTotalArquivos += fileInfo.Length;
                dadosProcessamento[$"arquivo_{Path.GetFileName(arquivo)}"] = new
                {
                    Nome = Path.GetFileName(arquivo),
                    Tamanho = fileInfo.Length,
                    DataCriacao = fileInfo.CreationTime,
                    DataModificacao = fileInfo.LastWriteTime
                };
            }
            
            AdicionarLog($"Total de arquivos PDF encontrados: {totalArquivos}");
            AdicionarLog($"Tamanho total dos arquivos: {FormatBytes(tamanhoTotalArquivos)}");
            AdicionarLog("");
            
            if (totalArquivos == 0)
            {
                AdicionarLog("⚠️ Nenhum arquivo PDF encontrado na pasta selecionada.");
                return;
            }
            
            var progress = new Progress<(string status, int progress)>(update =>
            {
                labelStatus.Text = update.status;
                progressBar.Value = update.progress;
                AdicionarLog(update.status);
                Application.DoEvents();
            });

            dadosExtraidosPorArquivo.Clear();
            dadosExtraidosPorArquivo.AddRange(await processor.ProcessarDarmsComRetornoAsync(pastaDarms, progress));
            
            // Coletar arquivos gerados
            if (Directory.Exists(pastaSaida))
            {
                var arquivosSql = Directory.GetFiles(pastaSaida, "*.sql");
                arquivosGerados.AddRange(arquivosSql.Select(f => Path.GetFileName(f)));
            }
            
            var fimProcessamento = DateTime.Now;
            var duracao = fimProcessamento - inicioProcessamento;
            
            AdicionarLog("");
            AdicionarLog("=== RESUMO DO PROCESSAMENTO ===");
            AdicionarLog($"Data/Hora de fim: {fimProcessamento:dd/MM/yyyy HH:mm:ss}");
            AdicionarLog($"Duração total: {duracao.TotalMinutes:F1} minutos");
            AdicionarLog($"Arquivos processados com sucesso: {arquivosProcessados}");
            AdicionarLog($"Arquivos com erro: {arquivosComErro}");
            AdicionarLog($"Taxa de sucesso: {(totalArquivos > 0 ? (arquivosProcessados * 100.0 / totalArquivos):0):F1}%");
            AdicionarLog("=== PROCESSAMENTO CONCLUÍDO ===");
            
            labelStatus.Text = $"Concluído! {arquivosProcessados}/{totalArquivos} arquivos processados";
        }
        catch (Exception ex)
        {
            arquivosComErro++;
            var fimProcessamento = DateTime.Now;
            var duracao = fimProcessamento - inicioProcessamento;
            
            AdicionarLog("");
            AdicionarLog("=== ERRO NO PROCESSAMENTO ===");
            AdicionarLog($"Erro: {ex.Message}");
            AdicionarLog($"Data/Hora do erro: {fimProcessamento:dd/MM/yyyy HH:mm:ss}");
            AdicionarLog($"Duração até o erro: {duracao.TotalMinutes:F1} minutos");
            AdicionarLog("=== PROCESSAMENTO INTERROMPIDO ===");
            
            labelStatus.Text = $"Erro: {ex.Message}";
            MessageBox.Show($"Erro durante o processamento:\n{ex.Message}", "Erro", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            processando = false;
        }
    }

    private void LimparRelatorio()
    {
        textBoxRelatorio.Clear();
        textBoxRelatorio.Text = "=== RELATÓRIO DE EXECUÇÃO ===\r\n";
    }

    private void AdicionarLog(string mensagem)
    {
        if (textBoxRelatorio.InvokeRequired)
        {
            textBoxRelatorio.Invoke(new Action<string>(AdicionarLog), mensagem);
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        textBoxRelatorio.AppendText($"[{timestamp}] {mensagem}\r\n");
        textBoxRelatorio.SelectionStart = textBoxRelatorio.Text.Length;
        textBoxRelatorio.ScrollToCaret();
    }

    private void btnAbrirPasta_Click(object sender, EventArgs e)
    {
        // Caminho da raiz do projeto (3 níveis acima do executável)
        string pastaCompleta = Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..")), pastaSaida);
        if (!Directory.Exists(pastaCompleta))
            Directory.CreateDirectory(pastaCompleta);
        Process.Start("explorer.exe", pastaCompleta);
    }

    private async void btnReprocessar_Click(object sender, EventArgs e)
    {
        if (processando) return;
        var result = MessageBox.Show("Deseja reprocessar todos os arquivos?", "Reprocessar", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            await IniciarProcessamentoAsync();
        }
    }

    private void btnExportarRelatorio_Click(object sender, EventArgs e)
    {
        try
        {
            // Limpar pasta de relatórios antes de gerar novo
            if (Directory.Exists(pastaRelatorios))
            {
                var arquivos = Directory.GetFiles(pastaRelatorios, "*.pdf");
                foreach (var arquivo in arquivos)
                {
                    try
                    {
                        File.Delete(arquivo);
                    }
                    catch (Exception ex)
                    {
                        AdicionarLog($"Aviso: Não foi possível deletar {Path.GetFileName(arquivo)}: {ex.Message}");
                    }
                }
                AdicionarLog("Pasta de relatórios limpa");
            }
            
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var nomeArquivo = $"Relatorio_DARMs_{timestamp}.pdf";
            var caminhoCompleto = Path.Combine(pastaRelatorios, nomeArquivo);
            
            GerarRelatorioPDF(caminhoCompleto);
            
            MessageBox.Show($"Relatório exportado com sucesso!\n\nArquivo: {nomeArquivo}\nPasta: {pastaRelatorios}", 
                "Relatório Exportado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            // Abrir a pasta de relatórios
            Process.Start("explorer.exe", pastaRelatorios);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao exportar relatório:\n{ex.Message}", "Erro", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void GerarRelatorioPDF(string caminhoArquivo)
    {
        using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create))
        {
            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25f, 25f, 30f, 30f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // Título principal
            var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20f);
            var titulo = new Paragraph("RELATÓRIO DETALHADO DE PROCESSAMENTO DE DARMs", tituloFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.SpacingAfter = 20f;
            document.Add(titulo);

            // Data/Hora de geração
            var dataFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12f);
            var data = new Paragraph($"Relatório gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", dataFont);
            data.Alignment = Element.ALIGN_CENTER;
            data.SpacingAfter = 20f;
            document.Add(data);

            // Fontes para o relatório
            var infoFont = FontFactory.GetFont(FontFactory.HELVETICA, 9f);
            var infoBoldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9f);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f);
            var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8f);

            if (inicioProcessamento != DateTime.MinValue)
            {
                var fimProcessamento = DateTime.Now;
                var duracao = fimProcessamento - inicioProcessamento;

                // ===== SEÇÃO 1: INFORMAÇÕES GERAIS =====
                var secao1 = new Paragraph("1. INFORMAÇÕES GERAIS", headerFont);
                secao1.SpacingAfter = 10f;
                document.Add(secao1);

                document.Add(new Paragraph($"• Data/Hora de início: {inicioProcessamento:dd/MM/yyyy HH:mm:ss}", infoFont));
                document.Add(new Paragraph($"• Data/Hora de fim: {fimProcessamento:dd/MM/yyyy HH:mm:ss}", infoFont));
                document.Add(new Paragraph($"• Duração total: {duracao.TotalMinutes:F2} minutos ({duracao.TotalSeconds:F0} segundos)", infoFont));
                document.Add(new Paragraph($"• Pasta de origem: {pastaDarms}", infoFont));
                document.Add(new Paragraph($"• Pasta de destino: {pastaSaida}", infoFont));
                document.Add(new Paragraph("", infoFont));

                // ===== SEÇÃO 2: DADOS DE ORIGEM =====
                var secao2 = new Paragraph("2. DADOS DE ORIGEM", headerFont);
                secao2.SpacingAfter = 10f;
                document.Add(secao2);

                document.Add(new Paragraph($"• Total de arquivos PDF encontrados: {totalArquivos}", infoFont));
                document.Add(new Paragraph($"• Tamanho total dos arquivos: {FormatBytes(tamanhoTotalArquivos)}", infoFont));
                long tamanhoMedio = (totalArquivos > 0) ? (tamanhoTotalArquivos / totalArquivos) : 0;
                document.Add(new Paragraph($"• Tamanho médio por arquivo: {FormatBytes(tamanhoMedio)}", infoFont));
                document.Add(new Paragraph("", infoFont));

                // Lista detalhada dos arquivos de origem
                if (dadosProcessamento.Count > 0)
                {
                    document.Add(new Paragraph("Arquivos de origem:", infoBoldFont));
                    document.Add(new Paragraph("", infoFont));
                    
                    foreach (var item in dadosProcessamento.Values)
                    {
                        var dados = item as dynamic;
                        document.Add(new Paragraph($"  - {dados.Nome}", smallFont));
                        document.Add(new Paragraph($"    Tamanho: {FormatBytes(dados.Tamanho)} | Criado: {dados.DataCriacao:dd/MM/yyyy HH:mm} | Modificado: {dados.DataModificacao:dd/MM/yyyy HH:mm}", smallFont));
                    }
                    document.Add(new Paragraph("", infoFont));
                }

                // ===== SEÇÃO 3: ESTATÍSTICAS DE PROCESSAMENTO =====
                var secao3 = new Paragraph("3. ESTATÍSTICAS DE PROCESSAMENTO", headerFont);
                secao3.SpacingAfter = 10f;
                document.Add(secao3);

                document.Add(new Paragraph($"• Arquivos processados com sucesso: {arquivosProcessados}", infoFont));
                document.Add(new Paragraph($"• Arquivos com erro: {arquivosComErro}", infoFont));
                
                if (totalArquivos > 0)
                {
                    double taxaSucesso = (totalArquivos > 0) ? (arquivosProcessados * 100.0 / totalArquivos) : 0;
                    double taxaErro = (totalArquivos > 0) ? (arquivosComErro * 100.0 / totalArquivos) : 0;
                    double velocidadeMedia = (totalArquivos > 0) ? (duracao.TotalSeconds / totalArquivos) : 0;
                    string processamentoResumo = $"{FormatBytes(tamanhoTotalArquivos)} em {duracao.TotalMinutes:F2} minutos";
                    document.Add(new Paragraph($"• Taxa de sucesso: {taxaSucesso:F1}%", infoBoldFont));
                    document.Add(new Paragraph($"• Taxa de erro: {taxaErro:F1}%", infoFont));
                    document.Add(new Paragraph($"• Velocidade média: {velocidadeMedia:F2} segundos por arquivo", infoFont));
                    document.Add(new Paragraph($"• Processamento: {processamentoResumo}", infoFont));
                }

                document.Add(new Paragraph("", infoFont));

                // ===== SEÇÃO 4: DADOS GERADOS =====
                var secao4 = new Paragraph("4. DADOS GERADOS", headerFont);
                secao4.SpacingAfter = 10f;
                document.Add(secao4);

                document.Add(new Paragraph($"• Total de arquivos SQL gerados: {arquivosGerados.Count}", infoFont));
                
                if (arquivosGerados.Count > 0)
                {
                    document.Add(new Paragraph("", infoFont));
                    document.Add(new Paragraph("Arquivos gerados:", infoBoldFont));
                    document.Add(new Paragraph("", infoFont));
                    
                    foreach (var arquivo in arquivosGerados)
                    {
                        var caminhoCompleto = Path.Combine(pastaSaida, arquivo);
                        if (File.Exists(caminhoCompleto))
                        {
                            var fileInfo = new FileInfo(caminhoCompleto);
                            document.Add(new Paragraph($"  - {arquivo}", smallFont));
                            document.Add(new Paragraph($"    Tamanho: {FormatBytes(fileInfo.Length)} | Gerado: {fileInfo.CreationTime:dd/MM/yyyy HH:mm:ss}", smallFont));
                        }
                        else
                        {
                            document.Add(new Paragraph($"  - {arquivo} (arquivo não encontrado)", smallFont));
                        }
                    }
                    document.Add(new Paragraph("", infoFont));
                }

                // ===== SEÇÃO 5: TIPOS DE ARQUIVOS GERADOS =====
                var secao5 = new Paragraph("5. TIPOS DE ARQUIVOS GERADOS", headerFont);
                secao5.SpacingAfter = 10f;
                document.Add(secao5);

                var tiposArquivos = arquivosGerados.GroupBy(f => Path.GetFileNameWithoutExtension(f).Split('_')[0]).ToList();
                foreach (var tipo in tiposArquivos)
                {
                    document.Add(new Paragraph($"• {tipo.Key}: {tipo.Count()} arquivo(s)", infoFont));
                }
                document.Add(new Paragraph("", infoFont));

                // ===== SEÇÃO 6: INFORMAÇÕES TÉCNICAS =====
                var secao6 = new Paragraph("6. INFORMAÇÕES TÉCNICAS", headerFont);
                secao6.SpacingAfter = 10f;
                document.Add(secao6);

                document.Add(new Paragraph("• Tecnologia utilizada: C# .NET 8.0", infoFont));
                document.Add(new Paragraph("• Biblioteca de extração: iTextSharp 5.5.13.4", infoFont));
                document.Add(new Paragraph("• Interface: Windows Forms", infoFont));
                document.Add(new Paragraph("• Processamento: Assíncrono com barra de progresso", infoFont));
                document.Add(new Paragraph("• Codificação: UTF-8", infoFont));
                document.Add(new Paragraph("• Formato de saída: SQL (INSERT statements)", infoFont));
                document.Add(new Paragraph("", infoFont));

                // ===== SEÇÃO 7: DADOS EXTRAÍDOS DOS PDFs =====
                var secao7 = new Paragraph("7. DADOS EXTRAÍDOS DOS PDFs", headerFont);
                secao7.SpacingAfter = 10f;
                document.Add(secao7);

                if (dadosExtraidosPorArquivo.Count > 0)
                {
                    foreach (var (arquivo, dados) in dadosExtraidosPorArquivo)
                    {
                        document.Add(new Paragraph($"Arquivo: {arquivo}", infoBoldFont));
                        document.Add(new Paragraph($"  Guia: {dados.NumeroGuia}", smallFont));
                        document.Add(new Paragraph($"  Inscrição: {dados.Inscricao}", smallFont));
                        document.Add(new Paragraph($"  Receita: {dados.CodigoReceita}", smallFont));
                        document.Add(new Paragraph($"  Valor Principal: {dados.ValorPrincipal}", smallFont));
                        document.Add(new Paragraph($"  Valor Total: {dados.ValorTotal}", smallFont));
                        document.Add(new Paragraph($"  Data Vencimento: {dados.DataVencimento}", smallFont));
                        document.Add(new Paragraph($"  Exercício: {dados.Exercicio}", smallFont));
                        document.Add(new Paragraph($"  Competência: {dados.Competencia}", smallFont));
                        document.Add(new Paragraph("", smallFont));
                    }
                }
                else
                {
                    document.Add(new Paragraph("Nenhum dado extraído disponível.", infoFont));
                }

                // ===== SEÇÃO 8: VALIDAÇÕES E CONTROLES =====
                var secao8 = new Paragraph("8. VALIDAÇÕES E CONTROLES", headerFont);
                secao8.SpacingAfter = 10f;
                document.Add(secao8);

                document.Add(new Paragraph("✅ Verificação de existência da pasta de origem", infoFont));
                document.Add(new Paragraph("✅ Validação de arquivos PDF", infoFont));
                document.Add(new Paragraph("✅ Controle de processamento assíncrono", infoFont));
                document.Add(new Paragraph("✅ Tratamento de erros individual por arquivo", infoFont));
                document.Add(new Paragraph("✅ Criação automática da pasta de destino", infoFont));
                document.Add(new Paragraph("✅ Log detalhado de execução", infoFont));
                document.Add(new Paragraph("✅ Relatório de progresso em tempo real", infoFont));
                document.Add(new Paragraph("", infoFont));

                // ===== SEÇÃO 9: PRÓXIMOS PASSOS =====
                var secao9 = new Paragraph("9. PRÓXIMOS PASSOS", headerFont);
                secao9.SpacingAfter = 10f;
                document.Add(secao9);

                document.Add(new Paragraph("1. Verificar os arquivos SQL gerados na pasta 'inserts'", infoFont));
                document.Add(new Paragraph("2. Executar o arquivo INSERT_TODOS_DARMs.sql para inserir todos os registros", infoFont));
                document.Add(new Paragraph("3. Ou executar os arquivos individuais INSERT_DARM_PAGO_*.sql", infoFont));
                document.Add(new Paragraph("4. Verificar os logs de execução para detalhes", infoFont));
                document.Add(new Paragraph("", infoFont));
            }

            // ===== SEÇÃO 10: LOG DE EXECUÇÃO =====
            var secao10 = new Paragraph("10. LOG DE EXECUÇÃO", headerFont);
            secao10.SpacingAfter = 10f;
            document.Add(secao10);

            // Converter o texto do relatório para o PDF
            string[] linhas = textBoxRelatorio.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string linha in linhas)
            {
                if (!string.IsNullOrWhiteSpace(linha))
                {
                    document.Add(new Paragraph(linha, smallFont));
                }
            }

            // ===== RODAPÉ =====
            document.Add(new Paragraph("", infoFont));
            var rodape = new Paragraph("--- Relatório gerado automaticamente pelo Pagador de DARMs (C#) ---", infoFont);
            rodape.Alignment = Element.ALIGN_CENTER;
            document.Add(rodape);

            document.Close();
        }
    }

    private async void btnProcessar_Click(object sender, EventArgs e)
    {
        await IniciarProcessamentoAsync();
    }

    private void btnSelecionarPastaDarms_Click(object sender, EventArgs e)
    {
        using (var folderDialog = new FolderBrowserDialog())
        {
            folderDialog.Description = "Selecione a pasta contendo os arquivos PDF dos DARMs";
            folderDialog.ShowNewFolderButton = false;
            
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                pastaDarms = folderDialog.SelectedPath;
                labelPastaDarms.Text = $"Pasta DARMs: {Path.GetFileName(pastaDarms)}";
                
                // Verificar se há arquivos PDF na pasta
                var arquivosPdf = Directory.GetFiles(pastaDarms, "*.pdf");
                if (arquivosPdf.Length > 0)
                {
                    labelStatus.Text = $"Encontrados {arquivosPdf.Length} arquivos PDF";
                }
                else
                {
                    labelStatus.Text = "Nenhum arquivo PDF encontrado na pasta selecionada";
                }
            }
        }
    }

    private string FormatBytes(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }
}
