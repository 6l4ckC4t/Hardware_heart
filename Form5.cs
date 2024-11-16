using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hardware_heart
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";

            try
            {
                // Получение данных из TextBox
                string surname = textBox1.Text;
                string mail = textBox2.Text;
                string phone = textBox3.Text;




                // Открытие подключения к PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();


                    // Создание команды INSERT
                    string insertQuery = "INSERT INTO staff (fio, mail, phone) VALUES (@surname, @mail, @phone)";
                    using (NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Добавление параметров
                        insertCommand.Parameters.AddWithValue("@surname", surname);
                        insertCommand.Parameters.AddWithValue("@mail", mail);
                        insertCommand.Parameters.AddWithValue("@phone", phone);

                        // Выполнение команды INSERT
                        insertCommand.ExecuteNonQuery();

                        // Очистка TextBox
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                       

                        // Вывод сообщения об успешном добавлении
                        MessageBox.Show("Сотрудник успешно добавлен!");
                    }

                }
            }

            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show("Ошибка при добавлении сотрудника: " + ex.Message);
            }
        }
    }
}
