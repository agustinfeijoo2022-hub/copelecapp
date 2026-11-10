using System;
using System.Windows.Forms;

namespace CopelecApp
{
    static class Program
    {
        // Variables globales de sesión
        public static string RolActual = "Admin"; 
        public static string NombreActual = ""; 
        public static string CorreoActual = ""; 

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            ApplicationContext context = new ApplicationContext();
            Form1 login = new Form1();
            
            login.FormClosed += (s, e) => context.ExitThread();
            
            context.MainForm = login;
            login.Show();
            Application.Run(context);
        }    
    }
}