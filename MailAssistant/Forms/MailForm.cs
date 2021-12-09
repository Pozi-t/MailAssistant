﻿using System;
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

namespace MailAssistant.Forms
{
    public partial class MailForm : Form
    {
        // Хранилище почт пользователя
        public List<UserMail> userMails;
        public List<ActiveUp.Net.Mail.Message> messages;
        private Mutex MutexObj;
        public MailForm()
        {
            InitializeComponent();
            /*if (File.Exists("userMails.xml")) DB.Load(out userMails);
            else userMails = new List<UserMail>();*/
            MutexObj = new Mutex();
            userMails = new List<UserMail>();
            messages = new List<ActiveUp.Net.Mail.Message>();
            //Созание первой вкладки меню
            ToolStripMenuItem MailItem = new ToolStripMenuItem("Почты");

            MailItem.DropDownItems.Add("Добавить");
            MailItem.DropDownItems.Add("Изменить");
            MailItem.DropDownItems[0].Click += AddUserMailItem_Click;
            MailItem.DropDownItems[1].Click += ChangeUserMails_Click;

            //Добавление полученного элемента
            menuStrip.Items.Add(MailItem);


            MailItem = new ToolStripMenuItem("Письма");

            MailItem.DropDownItems.Add("Обновить"); 
            MailItem.DropDownItems[0].Click += UpdateMails_Click;

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
                action = () => MessagedataGrid.Rows.Add(email.Date, email.From, email.BodyText.Text);
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
        private async void GetUSerMessage(object obj)
        {
            UserMail userMail = (UserMail)obj;
            var mailRepository = new MailRepository(
                                    "imap.gmail.com",
                                    993,
                                    true,
                                    userMail.Login,
                                    userMail.Pass
                                );
            var emailList = mailRepository.GetAllMails("inbox");
            MutexObj.WaitOne();
            messages.AddRange(emailList);
            /*foreach (ActiveUp.Net.Mail.Message email in emailList)
            {
                MessagedataGrid.Rows.Add(email.Date, email.From, email.BodyText.Text);
            }*/
            /*await Task.Run(() => */SetInformUSerMessage();
            MutexObj.ReleaseMutex();
        }
        private void UpdateMails_Click(object sender, EventArgs e)
        {
            messages.Clear();
            if (userMails.Count > 0)
            {
                Thread get;
                foreach (var item in userMails)
                {
                    get = new Thread(new ParameterizedThreadStart(GetUSerMessage));
                    get.Start(item);
                }
            }
            else MessageBox.Show("Программа не может нормально функционировать.\nНет ни одной зарегистрированой почты","Ошибка");
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

                /*using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(messages[index].BodyHtml.Text);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                }*/
                new HTMLMessageForm(Path.GetFullPath(path)).Show(this);

            }
            catch { }
        }
    }
}
