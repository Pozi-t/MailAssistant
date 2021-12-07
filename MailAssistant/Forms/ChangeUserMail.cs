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
    public partial class ChangeUserMail : Form
    {
        public ChangeUserMail()
        {
            InitializeComponent();
        }
        private void ChangeUserMail_Load(object sender, EventArgs e)
        {
            SetMailListBox();
        }
        private void SetMailListBox()
        {
            MailListBox.Items.Clear();
            MailForm MF = (MailForm)this.Owner;
            if (MF.userMails.Count > 0)
            {
                foreach (var mail in MF.userMails)
                {
                    MailListBox.Items.Add(mail.Login);
                }
            }
            else{
                MessageBox.Show("У вас нет зарегестрированых почт, \nДля начала добавьте их","Ошибка");
                this.Close();
            }
        }
        private void ChangeButton_Click(object sender, EventArgs e)
        {
            new UserMailForm(MailListBox.SelectedItem.ToString(),MailListBox.SelectedIndex).Show(this);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            MailForm MF = (MailForm)this.Owner;
            MF.userMails.RemoveAt(MailListBox.SelectedIndex);
            SetMailListBox();
        }
        public void UpdateUserMail(UserMail userMail, int index)
        {
            MailForm MF = (MailForm)this.Owner;
            MF.userMails[index] = userMail;
            SetMailListBox();
        }
    }
}
