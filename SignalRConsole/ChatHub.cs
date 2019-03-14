using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRConsole.Model;

namespace SignalRConsole
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        UserContext DB = new UserContext();
        public override Task OnConnected()
        {
            Console.WriteLine("New connection to server");
            return base.OnConnected();
        }
        public void userRegistration(string login, string pass)
        {
            var user = DB.Users.Where(u => u.Login == login).FirstOrDefault();
            if(user != null)
            {
                Clients.Caller.cantRegistrateThisUser("Пользователь с таким логином уже зарегистрирован");
            }
            else
            {
                try
                {
                    DB.Users.Add(new User { Login = login, Password = pass });
                    DB.SaveChanges();
                }
                catch
                {
                   Clients.Caller.cantRegistrateThisUser("Отсутсвтвует поключение к серверам баз данных");
                }
            }
        }
    }
}
