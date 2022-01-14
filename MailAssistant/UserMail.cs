using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace MailAssistant
{
    public class UserMail
    {
        private string id;
        private string login;
        private string pass;
        public string Login
        {
            get
            {
                return login;
            }
            set => login = value;
        }
        public string Pass
        {
            get
            {
                return pass;
            }
            set => pass = value;
        }
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        UserMail() { }
        public UserMail(string id, string login, string pass, int state)
        {
            Id = id;
            if (state == 0)
            {
                Login = Decrypt(login);
                Pass = Decrypt(pass);
            }
            else if (state == 1)
            {
                Login = Encrypt(login);
                Pass = Encrypt(pass);
            }
            else
            {
                Login = login;
                Pass = pass;
            }
        }
        public UserMail(UserMail userMail)
        {
            Id = userMail.Id;
            Login = userMail.Login;
            Pass = userMail.Pass;
        }

        private static readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string password = "password";
        //генерация повторяющегося пароля
        private static string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p.Substring(0, n);
        }

        private static string Vigenere(string text, bool encrypting = true)
        {
            var gamma = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(gamma[i]);
                if (letterIndex < 0)
                {
                    //если буква не найдена, добавляем её в исходном виде
                    retValue += text[i].ToString();
                }
                else
                {
                    retValue += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }
            return retValue;
        }

        //шифрование текста
        public static string Encrypt(string plainMessage) { return Vigenere(plainMessage); }

        //дешифрование текста
        public static string Decrypt(string encryptedMessage) { return Vigenere(encryptedMessage, false); }
    }
}
