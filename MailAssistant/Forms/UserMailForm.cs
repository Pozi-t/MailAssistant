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
        private int index;
        private UserMail userMail;
        public UserMailForm()
        {
            InitializeComponent();
            add = true;
        }
        public UserMailForm(UserMail UserMail, int index)
        {
            InitializeComponent();
            add = false;
            LoginTextBox.Text = UserMail.Login;
            this.index = index;
            AddUserMailButton.Text = "Сохранить";
            userMail = UserMail;
        }
        private void UserMailForm_Load(object sender, EventArgs e)
        {
            PassTextBox.PasswordChar = '*';
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
                if (add)
                {
                    MailForm MF = (MailForm)this.Owner;
                    //MF.userMails.Add(new UserMail(LoginTextBox.Text, PassTextBox.Text));
                    MF.db.AddUsers(LoginTextBox.Text,PassTextBox.Text);
                    MF.LoadMailAccount();
                }
                else
                {
                    ChangeUserMail Change = (ChangeUserMail)this.Owner;
                    Change.UpdateUserMail(new UserMail(userMail.Id, LoginTextBox.Text, PassTextBox.Text),index);
                }
                this.Close();
            }
             catch
             {
                 MessageBox.Show("Почта не найдена или\nНет разрешения на стороние приложения","Ошибка в почте");                
             }
            
        }

    }
}
