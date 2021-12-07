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
    public partial class MailForm : Form
    {
        // Хранилище почт пользователя
        List<UserMail> userMails;
        public MailForm()
        {
            InitializeComponent();
            userMails = new List<UserMail>();
            //Созание первой вкладки меню
            ToolStripMenuItem MailItem = new ToolStripMenuItem("Почты");

            MailItem.DropDownItems.Add("Добавить");
            MailItem.DropDownItems.Add("Изменить");
            MailItem.DropDownItems.Add("Удалить");
            MailItem.DropDownItems[0].Click += AddUserMailItem_Click;
            MailItem.DropDownItems[1].Click += aboutItem_Click;
            MailItem.DropDownItems[2].Click += aboutItem_Click;

            //Добавление полученного элемента
            menuStrip.Items.Add(MailItem);

        }
        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

        void AddUserMailItem_Click(object sender, EventArgs e)
        {
            new UserMailForm().Show(this);
        }
    }
}
