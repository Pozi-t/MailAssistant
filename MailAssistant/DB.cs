using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace MailAssistant
{
    public class DB
    {

        /*Создание строки подключения
        Data Source -имя сервера, по стандарту(local)\SQLEXPRESS
        Initial Catalog - имя БД
        Integrated Security = -параметры безопасности */

        const string connStr = @"server=localhost;
                               user = root;
                               password =;
                               database = mailassistant;";
        private string query;
        private readonly MySqlConnection conn;
        public DB()
        {

            /*Здесь указано имя БД (для создания БД указывать не нужно)
              для проверки того, создана ли данная БД
            Создаем SqlConnection и передаем ему строку подключения */

            conn = new MySqlConnection(connStr);
            try
            {
                //попытка подключения
                conn.Open();
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе данных","Ошибка");
            }
            finally
            {
                conn.Close();
            }

        }
        public void AddUsers(string login, string pass)
        {
            try
            {
                conn.Open();
                query = $"INSERT INTO mailaccounts (login, pass) VALUES(\"{UserMail.Encrypt(login)}\",\"{UserMail.Encrypt(pass)}\");";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();

            }
            catch (SqlException se)
            {
                MessageBox.Show($"Ошибка подключения:{se.Message}", "Ошибка");
            }
            finally
            {
                //закрываем соединение
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public void LoadUsers(ref List<UserMail> userMails)
        {
            try
            {
                conn.Open();
                userMails.Clear();
                query = $"SELECT * FROM mailaccounts;";
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, conn);
                DataSet dataSet = new DataSet("mailaccounts");
                dataAdapter.Fill(dataSet);
                DataTable dataTable;
                dataTable = dataSet.Tables[0];

                foreach (DataRow row in dataTable.Rows)
                {
                    userMails.Add(new UserMail(row[0].ToString(), row[1].ToString(), row[2].ToString(),0));
                }
            }
            catch (SqlException se)
            {
                MessageBox.Show($"Ошибка подключения:{se.Message}", "Ошибка");
            }
            finally
            {
                //закрываем соединение
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public void UpdateUsers(UserMail userMail)
        {
            try
            {
                conn.Open();
                query = $"UPDATE mailaccounts SET login = '{UserMail.Encrypt(userMail.Login)}', pass = '{UserMail.Encrypt(userMail.Pass)}' WHERE id = {userMail.Id}; ";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();

            }
            catch (SqlException se)
            {
                MessageBox.Show($"Ошибка подключения:{se.Message}", "Ошибка");
            }
            finally
            {
                //закрываем соединение
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public void DeleteUsers(UserMail userMail)
        {
            try
            {
                conn.Open();
                query = $"DELETE FROM mailaccounts WHERE id = {userMail.Id};";

                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();

            }
            catch (SqlException se)
            {
                MessageBox.Show($"Ошибка подключения:{se.Message}", "Ошибка");
            }
            finally
            {
                //закрываем соединение
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}
