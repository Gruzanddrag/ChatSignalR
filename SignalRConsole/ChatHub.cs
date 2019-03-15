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
            Console.WriteLine("Try to register user");
            var user = DB.Users.Where(u => u.Login == login).FirstOrDefault();
            if (user != null)
            {
                Console.WriteLine("Can't register user");
                Clients.Caller.cantRegistrateThisUser("Пользователь с таким логином уже зарегистрирован");
            }
            else
            {
                try
                {
                    DB.Users.Add(new User { Login = login, Password = pass });
                    DB.SaveChanges();
                    Console.WriteLine("Seccess registration " + login + " " + pass);
                    Clients.Caller.successRegistration();
                }
                catch
                {
                    Console.WriteLine("Can't register user");
                    Clients.Caller.cantRegistrateThisUser("Отсутсвтвует поключение к серверам баз данных");
                }
            }
        }
        /// <summary>
        ///     User Auth
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        public void userAuth(string login, string pass)
        {
            User user = null;
            try
            {
                user = DB.Users.Where(u => u.Login == login && u.Password == pass).FirstOrDefault();
                if (user != null)
                {
                    if (Program.usersInChat.Where(u => u == login).FirstOrDefault() != null)
                    {
                        Clients.Caller.cantAuthThisUser("Пользователь уже в чате");
                        Console.WriteLine("Can't auth");
                    }
                    else
                    {
                        Clients.Caller.successAuth();
                        Console.WriteLine("Seccess auth " + login);
                    }
                } else
                {
                    Clients.Caller.cantAuthThisUser("Не правильный логин или пароль");
                    Console.WriteLine("Can't auth");
                }
            }
            catch
            {
                Clients.Caller.cantAuthThisUser("Не удалось подключиться к серверам баз данных");
                Console.WriteLine("Can't auth");
            }
        }

        public void sendMessage(string userLogin, string message)
        {
            try
            {
                DB.Messages.Add(new Message { userLogin = userLogin, textMessage = message });
                DB.SaveChanges();
                if (Program.listLastMessage.Count() >= Program.numOfLastMessages)
                {
                    Program.listLastMessage.Add(new Message { userLogin = userLogin, textMessage = message });
                    for (int i = 1; i <= Program.numOfLastMessages; i++)
                    {
                        Program.listLastMessage[i - 1] = Program.listLastMessage[i];
                    }
                    Program.listLastMessage.RemoveAt(Program.numOfLastMessages);
                }
                else
                {
                    Program.listLastMessage.Add(new Message { userLogin = userLogin, textMessage = message });
                }
                message = String.Format("{0}: {1}", userLogin, message);
                Console.WriteLine(message);
                Clients.All.addMessage(message);
            }
            catch
            {
                Clients.Caller.throwExceptiontoChat("Разрыв соединения с сервером баз данных");
            }
        }
    
        public void userJoinChat(string userLogin)
        {
            Clients.Caller.addLastMessages(Program.listLastMessage);
            Clients.Caller.addAllUser(Program.usersInChat);
            Clients.Others.addUser(userLogin);
            Console.WriteLine(userLogin + " connected");
            Program.usersInChat.Add(userLogin);
            Clients.All.addMessage(userLogin + " вошел в чат");
        }
        public void userDissconnectChat(string userLogin)
        {
            Clients.Others.deleteUser(userLogin);
            Console.WriteLine(userLogin + " dissconnected");
            Program.usersInChat.Remove(userLogin);
            Clients.Others.addMessage(userLogin + " покинул чат");
        }
    }
}
