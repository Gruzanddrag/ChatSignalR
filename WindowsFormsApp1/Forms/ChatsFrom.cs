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
        LoginForm Main_form;
        public void addMessage(ListBox listbox, string message)
        {
            listbox.Items.Add(message);
            if(message.IndexOf("покинул чат") != -1)
            {
                string deletedUser = "";
                int i = 0;
                while(message[i] != ':')
                {
                    deletedUser += message[i];
                    i++;
                }
                //deleteFromListBox(listBox3, deletedUser.ToString());
            }
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
                if (sayi.IndexOf("вошел в чат") != -1 || sayi.IndexOf("покинул чат") != -1)
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
        public ChatsFrom(LoginForm f)
        {
            InitializeComponent();
            Main_form = f;
            listBox1.DrawItem += new DrawItemEventHandler(SetForeColor);
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            byte[] data = Encoding.Unicode.GetBytes("вошел в чат");
            //Program.stream.Write(data, 0, data.Length);
            //Thread receiveThread = new Thread(new ParameterizedThreadStart(Program.ReceiveMessage));
            //receiveThread.Start(this);
            listBox3.Items.Add(f.textBox1.Text);
            
            //пользователь вошел в чат
        }

        private void ChatFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Program.Disconnect();
            Thread.Sleep(400);
            Application.Exit();
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
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    //Program.stream.Write(data, 0, data.Length);
                }
                catch
                {
                    listBox1.Items.Add("Ошбика поключения к серверу");
                }

            }
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
