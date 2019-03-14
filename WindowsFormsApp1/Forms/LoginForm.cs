using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client;

namespace ChatForm.Forms
{
    public partial class LoginForm : Form
    {
        Point lastClick;

        public LoginForm(bool isConnected)
        {
            InitializeComponent(); 
            if (isConnected == false)
            {
                ErrorToConnect.Visible = true;
                ErrorToConnect.Text = "Не удалось подключиться к серверу";
                ErrorToConnect.ForeColor = Color.Red;
            }
            else
            {
                ErrorToConnect.Visible = false;
            }
            Program.chatHub.On<string>("cantAuthThisUser", (message) => 
                this.Invoke((Action)(() => addError(message)
            )));
            Program.chatHub.On("successAuth", () =>
                this.Invoke((Action)(() =>
                {
                    ChatsFrom newChatForm = new ChatsFrom(this);
                    this.Visible = false;
                    newChatForm.Show();
                }
            )));
        }

        public void addError(string errorMessage)
        {
            ErrorLable.Visible = true;
            ErrorLable.Text = errorMessage;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            RegistrationForm regForm = new RegistrationForm(this);
            regForm.Show();
        }

        private void Log_Reg_MouseDown(object sender, MouseEventArgs e)
        {
            lastClick = e.Location;
        }

        private void Log_Reg_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastClick.X;
                this.Top += e.Y - lastClick.Y;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.chatHub.Invoke("userAuth", textBox1.Text, textBox2.Text);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Логин")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Логин";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Пароль")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }
        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Пароль";
                textBox2.ForeColor = Color.Gray;
            }
        }

    }
}
