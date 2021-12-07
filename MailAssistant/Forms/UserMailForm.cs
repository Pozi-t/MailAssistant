using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailAssistant.Forms
{
    public partial class UserMailForm : Form
    {
        private bool add;
        public UserMailForm()
        {
            InitializeComponent();
            add = true;
        }

        private void AddUserMailButton_Click(object sender, EventArgs e)
        {
            try
            {
            var mailRepository = new MailRepository(
                                "imap.gmail.com",
                                993,
                                true,
                                LoginTextBox.Text.ToString(),
                                PassTextBox.Text.ToString()
                            );
            this.Close();
            }
             catch
             {
                 MessageBox.Show("Почта не найдена или\nНет разрешения на стороние приложения","Ошибка в почте");                
             }
            //if (add) F1.manager.AddComputer(textBox1.Text, radioButton1.Checked);
            /*else
            {
                foreach (var item in F1.manager.Computers)
                {
                    if (item.Name == computer.Name)
                    {
                        item.Name = textBox1.Text;
                        item.Ready = radioButton1.Checked;
                    }
                }
            }*/
        }
    }
}
