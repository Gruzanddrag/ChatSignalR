using ChatForm.Forms;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatForm
{
    static class Program
    {
        static public IHubProxy chatHub;
        static public LoginForm MainForm;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isConnected = false;
            try
            {
                var connection = new HubConnection("http://127.0.0.1:8088/");
                chatHub = connection.CreateHubProxy("ChatHub");
                connection.Start().Wait();
                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ой что-то не так\n" + ex, "Fuck", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new LoginForm(isConnected);
            Application.Run(MainForm);
        }
    }
}
