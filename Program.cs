using System;
using System.Windows.Forms;

namespace pagador_de_darms;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        // Configurar DPI awareness para melhor compatibilidade
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        // Inicializar e executar a aplicação
        Application.Run(new Form1());
    }    
}