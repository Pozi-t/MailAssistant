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
        private string login;
        private string pass;
        public string Login
        {
            get
            {
                SecureString temp = DecryptString(login);
                string readable = ToInsecureString(temp);
                return login;
                //return readable;
            }
            //set => login = EncryptString(ToSecureString(value));
            set => login = value;
        }
        public string Pass
        {
            get
            {
                SecureString temp = DecryptString(pass);
                string readable = ToInsecureString(temp);
                //return readable;
                return pass;
            }
            //set => pass = EncryptString(ToSecureString(value));
            set => pass = value;
        }
        UserMail() { }
        public UserMail(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }
        public UserMail(UserMail userMail)
        {
            Login = userMail.Login;
            Pass = userMail.Pass;
        }
        static byte[] entropy = Encoding.Unicode.GetBytes("SaLtY bOy 6970 ePiC");

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
