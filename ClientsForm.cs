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
    public partial class ClientsForm : Form
    {

        string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";

        public ClientsForm()
        {
            InitializeComponent();
        }

       
        private void LoadGoodsData(string searchTerm = "")
        {
            try
            {
                // Создание соединения с базой данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Открытие соединения
                    connection.Open();

                    // Создание команды для выполнения запроса
                    string query = "SELECT fio as \"ФИО\", mail as \"Эл. почта\", phone as \"Номер телефона\" FROM clients";

                    // Если есть термин для поиска, добавляем его к запросу
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += $" WHERE phone LIKE '%{searchTerm}%'";
                    }

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // Выполнение запроса
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            // Создание DataTable для хранения результатов запроса
                            DataTable dataTable = new DataTable();
                            // Заполнение DataTable данными из DataReader
                            dataTable.Load(reader);

                            // Очистка DataGridView
                            dataGridView1.DataSource = null;

                            // Установка источника данных для DataGridView
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }

        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Получаем текст из поля ввода
            string searchTerm = textBox1.Text;

            // Вызываем загрузку данных с поиском
            LoadGoodsData(searchTerm);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления.");
                return;
            }

            
            string PhoneClient = dataGridView1.SelectedRows[0].Cells["ФИО"].Value.ToString();


            // Подтверждение удаления
            if (MessageBox.Show($"Вы уверены, что хотите удалить клиента \"{PhoneClient}\"?", "Удаление клиента", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    // Создание соединения с базой данных
                    using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                    {
                        // Открытие соединения
                        connection.Open();

                        // Создание команды для удаления товара
                        using (NpgsqlCommand command = new NpgsqlCommand("DELETE FROM clients WHERE phone = @phone", connection))
                        {
                            // Добавление параметра
                            command.Parameters.AddWithValue("@phone", PhoneClient);

                            // Выполнение команды удаления
                            command.ExecuteNonQuery();

                            // Обновление DataGridView
                            LoadGoodsData();

                            MessageBox.Show("Клиент успешно удален.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.Show();
            this.Hide();
        }
    }
}
