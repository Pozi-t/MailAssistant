﻿using System;
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
    public partial class HTMLMessageForm : Form
    {
        public HTMLMessageForm()
        {
            InitializeComponent();
            MessageWebBrowser.Navigate("C:\\Users\\Tikhon\\Desktop\\Универ\\semestr3\\СПЗ\\curs\\final\\MailAssistant\\MailAssistant\\bin\\Debug\\index.html");
        }
        public HTMLMessageForm(string fullPath)
        {
            InitializeComponent();
            MessageWebBrowser.Navigate(fullPath);
        }
    }
}
