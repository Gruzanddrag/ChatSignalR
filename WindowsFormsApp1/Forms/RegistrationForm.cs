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
    public partial class RegistrationForm : Form
    {
        LoginForm Main_Form;
        public RegistrationForm(LoginForm f)
        {
            InitializeComponent();
            //string message = "DOROVA";
            //byte[] data = Encoding.Unicode.GetBytes(message);
            //Program.stream.Write(data, 0, data.Length);
            Main_Form = f;
            Main_Form.Opacity = 0.9;
            Main_Form.Enabled = false;
            Program.chatHub.On<string>("cantRegistrateThisUser", (message) =>
                 this.Invoke((Action)(() =>
                     addError(message)
            )));
            Program.chatHub.On("successRegistration", () =>
                 this.Invoke((Action)(() => 
                    successRegist()
            )));
        }

        public void successRegist()
        {
            ChatsFrom newChatForm = new ChatsFrom(textBox1.Text);
            Main_Form.Visible = false;
            newChatForm.Show();
            this.Close();
        }

        public void addError(string errorMessage)
        {
            loading.Visible = false;
            button1.Enabled = true;
            ErrorLable.Visible = true;
            ErrorLable.Text = errorMessage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_Form.Opacity = 1;
            Main_Form.Enabled = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string errors = "";
            if (textBox1.Text == "")
            {
                errors += "Введите логин";
            }
            if (textBox1.Text == "")
            {
                if (errors != "")
                {
                    errors += "\n";
                }
                errors += "Введите логин";
            }
            if (errors != "")
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errors = "";
            }
            else
            {
                Program.chatHub.Invoke("userRegistration", textBox1.Text, textBox2.Text);
                loading.Visible = true;
                button1.Enabled = false;
            }
        }

        private void RegistrationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
