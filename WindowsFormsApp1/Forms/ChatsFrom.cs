using System;
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
            Program.chatHub.Invoke("userJoinChat", userLogin);
        }
        public void deleteFromListBox(ListBox listbox, string message)
        {
            listbox.Items.Remove(message);
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
            if(textBox1.Text != null)
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
