using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace MailAssistant
{
    class DB
    {
        public static void Save(List<UserMail> userMails)
        {
            File.Delete("userMails.xml");
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<UserMail>));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("userMails.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, userMails);
            }
        }
        public static void Load(out List<UserMail> userMails)
        {
            userMails = new List<UserMail>();
            // передаем в конструктор тип класса
            XmlSerializer formatter = new XmlSerializer(typeof(List<UserMail>));

            using (FileStream fs = new FileStream("persons.xml", FileMode.OpenOrCreate))
            {
                userMails = (List<UserMail>)formatter.Deserialize(fs);

            }
        }
    }
}
