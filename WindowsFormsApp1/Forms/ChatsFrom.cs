﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using WindowsFormsApp1;

namespace ChatForm.Forms
{
    public partial class ChatsFrom : Form
    {
        string curUserLogin;
        public ChatsFrom(string userLogin)
        {
            InitializeComponent();
            curUserLogin = userLogin;
            listBox1.DrawItem += new DrawItemEventHandler(SetForeColor);
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox3.Items.Add(userLogin);
            Program.chatHub.On<string>("addMessage", (message) =>
                 this.Invoke((Action)(() =>
                    {
                        addMessage(message);
                    }
            )));
            Program.chatHub.On<List<MessageEx>>("addLastMessages", (messages) =>
                 this.Invoke((Action)(() =>
                 {
                     string curString;
                     foreach (var m in messages)
                     {
                         curString = String.Format("{0}: {1}", m.userLogin, m.textMessage);
                         addMessage(curString);
                     }
                 }
            )));
            Program.chatHub.On<string>("throwExceptiontoChat", (ex) =>
                 this.Invoke((Action)(() =>
                 {
                     ex = String.Format("Ошибка! {0}", ex);
                     addMessage(ex);
                 }
            )));
            Program.chatHub.On<string>("addUser", (newUserLogin) =>
                 this.Invoke((Action)(() =>
                 {
                     addUser(newUserLogin);
                 }
            )));
            Program.chatHub.On<List<string>>("addAllUser", (userInChat) =>
                 this.Invoke((Action)(() =>
                 {
                     foreach (var u in userInChat)
                     {
                         if(u != userLogin)
                         {
                             addUser(u);
                         }
                     }
                 }
            )));
            Program.chatHub.On<string>("deleteUser", (delUserLogin) =>
                 this.Invoke((Action)(() =>
                 {
                     deleteFromListBox(delUserLogin);
                 }
            )));
            Program.chatHub.Invoke("userJoinChat", userLogin);
        }
        public void deleteFromListBox(string delUserLogin)
        {
            listBox3.Items.Remove(delUserLogin);
        }
        public void SetForeColor(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.DrawBackground();
                Brush myBrush = Brushes.White;

                string sayi = ((ListBox)sender).Items[e.Index].ToString();
                if (sayi.IndexOf(':') == -1 && sayi.IndexOf("вошел в чат") != -1 || sayi.IndexOf("покинул чат") != -1)
                {
                    myBrush = Brushes.Orchid;
                }
                else if(sayi.IndexOf(':') == -1 && sayi.IndexOf("Ошибка!") != -1)
                {
                    myBrush = Brushes.Red;
                }
                else
                {
                    myBrush = Brushes.Black;
                }

                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(),
                e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
            catch
            {

            }
        }
        

        private void ChatFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.chatHub.Invoke("userDissconnectChat", curUserLogin);
            Program.MainForm.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != null && textBox1.Text != "")
            {
                string message = textBox1.Text;
                textBox1.Text = null;
                try
                {
                    Program.chatHub.Invoke("sendMessage", curUserLogin, message);
                }
                catch
                {
                    listBox1.Items.Add("Ошбика поключения к серверу");
                }

            }
        }

        public void addMessage(string message)
        {
            listBox1.Items.Add(message);
        }

        public void addUser(string userLogin)
        {
            listBox3.Items.Add(userLogin);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Black;
            button2.Enabled = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBox1.Text == null)
            {
                button2.ForeColor = Color.DimGray;
                button2.Enabled = false;
            }
        }
    }
}
