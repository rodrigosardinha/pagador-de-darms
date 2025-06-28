namespace pagador_de_darms;

partial class Form1
{
    /// <summary>
    /// Variável de designer necessária.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Limpar os recursos que estão sendo usados.
    /// </summary>
    /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Código gerado pelo Windows Form Designer

    /// <summary>
    /// Método necessário para suporte ao Designer - não modifique
    /// o conteúdo deste método com o editor de código.
    /// </summary>
    private void InitializeComponent()
    {
        this.labelTitulo = new System.Windows.Forms.Label();
        this.labelPastaDarms = new System.Windows.Forms.Label();
        this.labelStatus = new System.Windows.Forms.Label();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.btnSelecionarPastaDarms = new System.Windows.Forms.Button();
        this.btnProcessar = new System.Windows.Forms.Button();
        this.btnAbrirPasta = new System.Windows.Forms.Button();
        this.btnReprocessar = new System.Windows.Forms.Button();
        this.btnExportarRelatorio = new System.Windows.Forms.Button();
        this.groupBoxConfiguracao = new System.Windows.Forms.GroupBox();
        this.groupBoxAcoes = new System.Windows.Forms.GroupBox();
        this.groupBoxStatus = new System.Windows.Forms.GroupBox();
        this.groupBoxRelatorio = new System.Windows.Forms.GroupBox();
        this.textBoxRelatorio = new System.Windows.Forms.TextBox();
        this.groupBoxConfiguracao.SuspendLayout();
        this.groupBoxAcoes.SuspendLayout();
        this.groupBoxStatus.SuspendLayout();
        this.groupBoxRelatorio.SuspendLayout();
        this.SuspendLayout();
        // 
        // labelTitulo
        // 
        this.labelTitulo.AutoSize = true;
        this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.labelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
        this.labelTitulo.Location = new System.Drawing.Point(20, 20);
        this.labelTitulo.Name = "labelTitulo";
        this.labelTitulo.Size = new System.Drawing.Size(320, 30);
        this.labelTitulo.TabIndex = 0;
        this.labelTitulo.Text = "Pagador de DARMs";
        // 
        // groupBoxConfiguracao
        // 
        this.groupBoxConfiguracao.Controls.Add(this.btnSelecionarPastaDarms);
        this.groupBoxConfiguracao.Controls.Add(this.labelPastaDarms);
        this.groupBoxConfiguracao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.groupBoxConfiguracao.Location = new System.Drawing.Point(20, 70);
        this.groupBoxConfiguracao.Name = "groupBoxConfiguracao";
        this.groupBoxConfiguracao.Size = new System.Drawing.Size(300, 100);
        this.groupBoxConfiguracao.TabIndex = 1;
        this.groupBoxConfiguracao.TabStop = false;
        this.groupBoxConfiguracao.Text = "Configuração";
        // 
        // labelPastaDarms
        // 
        this.labelPastaDarms.AutoSize = true;
        this.labelPastaDarms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.labelPastaDarms.Location = new System.Drawing.Point(15, 30);
        this.labelPastaDarms.Name = "labelPastaDarms";
        this.labelPastaDarms.Size = new System.Drawing.Size(150, 15);
        this.labelPastaDarms.TabIndex = 0;
        this.labelPastaDarms.Text = "Pasta DARMs: Não selecionada";
        // 
        // btnSelecionarPastaDarms
        // 
        this.btnSelecionarPastaDarms.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
        this.btnSelecionarPastaDarms.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSelecionarPastaDarms.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnSelecionarPastaDarms.ForeColor = System.Drawing.Color.White;
        this.btnSelecionarPastaDarms.Location = new System.Drawing.Point(15, 55);
        this.btnSelecionarPastaDarms.Name = "btnSelecionarPastaDarms";
        this.btnSelecionarPastaDarms.Size = new System.Drawing.Size(180, 35);
        this.btnSelecionarPastaDarms.TabIndex = 1;
        this.btnSelecionarPastaDarms.Text = "Selecionar Pasta DARMs";
        this.btnSelecionarPastaDarms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnSelecionarPastaDarms.UseVisualStyleBackColor = false;
        this.btnSelecionarPastaDarms.Click += new System.EventHandler(this.btnSelecionarPastaDarms_Click);
        // 
        // groupBoxStatus
        // 
        this.groupBoxStatus.Controls.Add(this.labelStatus);
        this.groupBoxStatus.Controls.Add(this.progressBar);
        this.groupBoxStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.groupBoxStatus.Location = new System.Drawing.Point(340, 70);
        this.groupBoxStatus.Name = "groupBoxStatus";
        this.groupBoxStatus.Size = new System.Drawing.Size(300, 100);
        this.groupBoxStatus.TabIndex = 2;
        this.groupBoxStatus.TabStop = false;
        this.groupBoxStatus.Text = "Status do Processamento";
        // 
        // labelStatus
        // 
        this.labelStatus.AutoSize = true;
        this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.labelStatus.Location = new System.Drawing.Point(15, 30);
        this.labelStatus.Name = "labelStatus";
        this.labelStatus.Size = new System.Drawing.Size(140, 19);
        this.labelStatus.TabIndex = 0;
        this.labelStatus.Text = "Aguardando início...";
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(15, 55);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(270, 30);
        this.progressBar.TabIndex = 1;
        // 
        // groupBoxAcoes
        // 
        this.groupBoxAcoes.Controls.Add(this.btnProcessar);
        this.groupBoxAcoes.Controls.Add(this.btnAbrirPasta);
        this.groupBoxAcoes.Controls.Add(this.btnReprocessar);
        this.groupBoxAcoes.Controls.Add(this.btnExportarRelatorio);
        this.groupBoxAcoes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.groupBoxAcoes.Location = new System.Drawing.Point(20, 180);
        this.groupBoxAcoes.Name = "groupBoxAcoes";
        this.groupBoxAcoes.Size = new System.Drawing.Size(620, 80);
        this.groupBoxAcoes.TabIndex = 3;
        this.groupBoxAcoes.TabStop = false;
        this.groupBoxAcoes.Text = "Ações";
        // 
        // btnProcessar
        // 
        this.btnProcessar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
        this.btnProcessar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnProcessar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnProcessar.ForeColor = System.Drawing.Color.White;
        this.btnProcessar.Location = new System.Drawing.Point(15, 30);
        this.btnProcessar.Name = "btnProcessar";
        this.btnProcessar.Size = new System.Drawing.Size(145, 35);
        this.btnProcessar.TabIndex = 0;
        this.btnProcessar.Text = "Processar DARMs";
        this.btnProcessar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnProcessar.UseVisualStyleBackColor = false;
        this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
        // 
        // btnAbrirPasta
        // 
        this.btnAbrirPasta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
        this.btnAbrirPasta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnAbrirPasta.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnAbrirPasta.ForeColor = System.Drawing.Color.Black;
        this.btnAbrirPasta.Location = new System.Drawing.Point(170, 30);
        this.btnAbrirPasta.Name = "btnAbrirPasta";
        this.btnAbrirPasta.Size = new System.Drawing.Size(145, 35);
        this.btnAbrirPasta.TabIndex = 1;
        this.btnAbrirPasta.Text = "Abrir Pasta Resultados";
        this.btnAbrirPasta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnAbrirPasta.UseVisualStyleBackColor = false;
        this.btnAbrirPasta.Click += new System.EventHandler(this.btnAbrirPasta_Click);
        // 
        // btnReprocessar
        // 
        this.btnReprocessar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
        this.btnReprocessar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnReprocessar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnReprocessar.ForeColor = System.Drawing.Color.White;
        this.btnReprocessar.Location = new System.Drawing.Point(325, 30);
        this.btnReprocessar.Name = "btnReprocessar";
        this.btnReprocessar.Size = new System.Drawing.Size(145, 35);
        this.btnReprocessar.TabIndex = 2;
        this.btnReprocessar.Text = "Reprocessar Todos";
        this.btnReprocessar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnReprocessar.UseVisualStyleBackColor = false;
        this.btnReprocessar.Click += new System.EventHandler(this.btnReprocessar_Click);
        // 
        // btnExportarRelatorio
        // 
        this.btnExportarRelatorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
        this.btnExportarRelatorio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnExportarRelatorio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.btnExportarRelatorio.ForeColor = System.Drawing.Color.White;
        this.btnExportarRelatorio.Location = new System.Drawing.Point(480, 30);
        this.btnExportarRelatorio.Name = "btnExportarRelatorio";
        this.btnExportarRelatorio.Size = new System.Drawing.Size(145, 35);
        this.btnExportarRelatorio.TabIndex = 3;
        this.btnExportarRelatorio.Text = "Exportar Relatório PDF";
        this.btnExportarRelatorio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnExportarRelatorio.UseVisualStyleBackColor = false;
        this.btnExportarRelatorio.Click += new System.EventHandler(this.btnExportarRelatorio_Click);
        // 
        // groupBoxRelatorio
        // 
        this.groupBoxRelatorio.Controls.Add(this.textBoxRelatorio);
        this.groupBoxRelatorio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.groupBoxRelatorio.Location = new System.Drawing.Point(20, 270);
        this.groupBoxRelatorio.Name = "groupBoxRelatorio";
        this.groupBoxRelatorio.Size = new System.Drawing.Size(620, 200);
        this.groupBoxRelatorio.TabIndex = 4;
        this.groupBoxRelatorio.TabStop = false;
        this.groupBoxRelatorio.Text = "Relatório de Execução";
        // 
        // textBoxRelatorio
        // 
        this.textBoxRelatorio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
        this.textBoxRelatorio.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.textBoxRelatorio.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.textBoxRelatorio.Location = new System.Drawing.Point(15, 25);
        this.textBoxRelatorio.Multiline = true;
        this.textBoxRelatorio.Name = "textBoxRelatorio";
        this.textBoxRelatorio.ReadOnly = true;
        this.textBoxRelatorio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.textBoxRelatorio.Size = new System.Drawing.Size(590, 160);
        this.textBoxRelatorio.TabIndex = 0;
        this.textBoxRelatorio.Text = "Aguardando processamento...";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(660, 490);
        this.Controls.Add(this.groupBoxRelatorio);
        this.Controls.Add(this.groupBoxAcoes);
        this.Controls.Add(this.groupBoxStatus);
        this.Controls.Add(this.groupBoxConfiguracao);
        this.Controls.Add(this.labelTitulo);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Pagador de DARMs";
        this.groupBoxConfiguracao.ResumeLayout(false);
        this.groupBoxConfiguracao.PerformLayout();
        this.groupBoxStatus.ResumeLayout(false);
        this.groupBoxStatus.PerformLayout();
        this.groupBoxAcoes.ResumeLayout(false);
        this.groupBoxRelatorio.ResumeLayout(false);
        this.groupBoxRelatorio.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label labelTitulo;
    private System.Windows.Forms.Label labelPastaDarms;
    private System.Windows.Forms.Label labelStatus;
    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Button btnSelecionarPastaDarms;
    private System.Windows.Forms.Button btnProcessar;
    private System.Windows.Forms.Button btnAbrirPasta;
    private System.Windows.Forms.Button btnReprocessar;
    private System.Windows.Forms.Button btnExportarRelatorio;
    private System.Windows.Forms.GroupBox groupBoxConfiguracao;
    private System.Windows.Forms.GroupBox groupBoxAcoes;
    private System.Windows.Forms.GroupBox groupBoxStatus;
    private System.Windows.Forms.GroupBox groupBoxRelatorio;
    private System.Windows.Forms.TextBox textBoxRelatorio;
}
