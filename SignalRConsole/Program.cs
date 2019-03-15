using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SignalRConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Program.Startup))]
namespace SignalRConsole
{
    class Program
    {
        static IDisposable SignalR;
        static public List<string> usersInChat = new List<string>();
        static void Main(string[] args)
        {
            string url = "http://127.0.0.1:8088";
            SignalR = WebApp.Start(url);
            Console.WriteLine("Starting SIGNALR server with: {0}", url);
            Console.ReadKey();
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }
    }
}
