using System;
using System.Windows.Forms;
using ArbolEmpresaMudanzas.Vistas;

namespace ArbolEmpresaMudanzas
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}