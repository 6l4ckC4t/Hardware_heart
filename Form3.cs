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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hardware_heart
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";

            try
            {
                // Получение данных из TextBox
                string name = textBox1.Text;


                // Открытие подключения к PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Запрос для получения ID категории по названию


                    // Создание команды INSERT
                    string insertQuery = "INSERT INTO category (name_category) VALUES (@name)";
                    using (NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Добавление параметров
                        insertCommand.Parameters.AddWithValue("@name", name);


                        // Выполнение команды INSERT
                        insertCommand.ExecuteNonQuery();

                        // Очистка TextBox
                        textBox1.Text = "";


                        // Вывод сообщения об успешном добавлении
                        MessageBox.Show("Категория успешно добавлена!");




                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show("Ошибка при добавлении категории: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
