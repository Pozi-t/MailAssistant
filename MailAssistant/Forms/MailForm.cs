using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ActiveUp.Net.Mail;

namespace MailAssistant.Forms
{
    public partial class MailForm : Form
    {
        // Хранилище почт пользователя
        public List<UserMail> userMails;
        public List<ActiveUp.Net.Mail.Message> messages;
        private readonly Mutex MutexObj;
        public readonly DB db;
        private string whatLetters;
        public MailForm()
        {
            InitializeComponent();
            MutexObj = new Mutex();
            userMails = new List<UserMail>();
            messages = new List<ActiveUp.Net.Mail.Message>();
            db = new DB();

            // Получаем зарегестрированные акаунты гугл
            LoadMailAccount();
            // Получение непрочитанных писем
            whatLetters = "Новые";
            if(userMails.Count != 0) CreateTreadsLoadMessage();
            //Созание первой вкладки меню
            ToolStripMenuItem MailItem = new ToolStripMenuItem("Почты");

            MailItem.DropDownItems.Add("Добавить");
            MailItem.DropDownItems.Add("Изменить");
            MailItem.DropDownItems[0].Click += AddUserMailItem_Click;
            MailItem.DropDownItems[1].Click += ChangeUserMails_Click;

            //Добавление полученного элемента
            menuStrip.Items.Add(MailItem);

            MailItem = new ToolStripMenuItem("Письма");

            MailItem.DropDownItems.Add("Все");
            MailItem.DropDownItems.Add("Новые");
            MailItem.DropDownItems[0].Click += UpdateMails_Click;
            MailItem.DropDownItems[1].Click += NewMails_Click;

            menuStrip.Items.Add(MailItem);


            MailItem = new ToolStripMenuItem("Фильтрация");

            MailItem.DropDownItems.Add("По отправителю");
            MailItem.DropDownItems.Add("По получателю");
            MailItem.DropDownItems.Add("Сегодняшние");
            MailItem.DropDownItems.Add("Всe");

            MailItem.DropDownItems[0].Click += ShowWithoutMails_Click;
            MailItem.DropDownItems[1].Click += ShowFromMails_Click;
            MailItem.DropDownItems[2].Click += ShowTodayMails_Click;
            MailItem.DropDownItems[3].Click += ShowAllMails_Click;

            menuStrip.Items.Add(MailItem);
        }
        private void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

        private void AddUserMailItem_Click(object sender, EventArgs e)
        {
            new UserMailForm().Show(this);
        }
        private void ChangeUserMails_Click(object sender, EventArgs e)
        {
            new ChangeUserMail().Show(this);
        }
        private void SetInformUSerMessage()
        {
            Action clear = () => MessagedataGrid.Rows.Clear();
            Action action;
            Invoke(clear);
            foreach (ActiveUp.Net.Mail.Message email in messages)
            {
                action = () => MessagedataGrid.Rows.Add(email.From.Email,email.To[0].Email, email.Date.ToString(),email.BodyText.Text);
                // Свойство InvokeRequired указывает, нeжно ли обращаться к контролу с помощью Invoke
                if (MessagedataGrid.InvokeRequired)
                {
                    Invoke(action);
                }
                else
                {
                    action();
                }
            }
        }
        private void ShowTodayMails_Click(object sender, EventArgs e) => SetUSerMessage(messages.Where(m => m.Date.Day == DateTime.Today.Day && m.Date.Year == DateTime.Today.Year && m.Date.Month == DateTime.Today.Month).ToList());
        private void ShowAllMails_Click(object sender, EventArgs e) => SetUSerMessage(messages);
        private void ShowWithoutMails_Click(object sender, EventArgs e)
        {
            new TakeEmailForm(true).Show(this);
        }
        private void ShowFromMails_Click(object sender, EventArgs e)
        {
            new TakeEmailForm(false).Show(this);
        }
        public void FindSender(string email)
        {
            SetUSerMessage(messages.Where(m => m.From.Email.Contains(email)).ToList());
        }
        public void FindRecipient(string email)
        {
            SetUSerMessage(messages.Where(m => m.To[0].Email.Contains(email)).ToList());
        }
        private void SetUSerMessage(List<ActiveUp.Net.Mail.Message> findMessages)
        {
            if (findMessages.Count > 0)
            {
                MessagedataGrid.Rows.Clear();
                foreach (ActiveUp.Net.Mail.Message email in findMessages)
                {
                    MessagedataGrid.Rows.Add(email.From.Email, email.To[0].Email, email.Date.ToString(), email.BodyText.Text);
                }
            }
            else MessageBox.Show("Сообщений удовлетворяющих ваш запрос, нет","Увидовление");
        }
        private void GetUSerMessage(object obj)
        {
            UserMail userMail = (UserMail)obj;
            var mailRepository = new MailRepository(
                                    "imap.gmail.com",
                                    993,
                                    true,
                                    userMail.Login,
                                    userMail.Pass
                                );
            IEnumerable<ActiveUp.Net.Mail.Message> emailList;
            if(whatLetters == "Все") emailList = mailRepository.GetAllMails("inbox");
            else emailList = mailRepository.GetUnreadMails("inbox");
            
            MutexObj.WaitOne();
            messages.AddRange(emailList);
            
            SetInformUSerMessage();
            MutexObj.ReleaseMutex();
        }
        private void CreateTreadsLoadMessage()
        {
            Thread get;
            foreach (var item in userMails)
            {
                get = new Thread(new ParameterizedThreadStart(GetUSerMessage));
                get.Start(item);
            }
        }
        private void UpdateMails_Click(object sender, EventArgs e)
        {
            messages.Clear();
            if (userMails.Count > 0)
            {
                whatLetters = "Все";
                CreateTreadsLoadMessage();
            }
            else MessageBox.Show("Программа не может нормально функционировать.\nНет ни одной зарегистрированой почты","Ошибка");
        }
        private void NewMails_Click(object sender, EventArgs e)
        {
            messages.Clear();
            if (userMails.Count > 0)
            {
                whatLetters = "Новые";
                CreateTreadsLoadMessage();
            }
            else MessageBox.Show("Программа не может нормально функционировать.\nНет ни одной зарегистрированой почты", "Ошибка");
        }
        private void MessagedataGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = MessagedataGrid.SelectedRows[0].Index;
                string path = "index.html";

                if (File.Exists(path)) File.Delete(path);
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(messages[index].BodyHtml.Text);
                }

                new HTMLMessageForm(Path.GetFullPath(path)).Show(this);

            }
            catch { }
        }
        public void LoadMailAccount() => db.LoadUsers(ref userMails);
    }
}
