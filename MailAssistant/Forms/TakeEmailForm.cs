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
    public partial class TakeEmailForm : Form
    {
        private bool State;
        public TakeEmailForm()
        {
            InitializeComponent();
        }
        public TakeEmailForm(bool state)
        {
            InitializeComponent();
            State = state;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MailForm MF = (MailForm)this.Owner;
            if (MailTextBox.Text.Length != 0)
            {
                if (State) MF.FindSender(MailTextBox.Text);
                else MF.FindRecipient(MailTextBox.Text);
                this.Close();
            }
            else MessageBox.Show("Введите значение","Предупреждение");
        }
    }
}
