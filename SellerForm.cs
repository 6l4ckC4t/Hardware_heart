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
    public partial class SellerForm : Form
    {
        string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";

        public SellerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();

        }

        private void LoadClients()
        {
            try
            {
                // Соединение с базой данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Открываем соединение
                    connection.Open();

                    // Запрос к таблице клиентов
                    string query = "SELECT fio as \"ФИО\" FROM clients";

                    // Создаем объект NpgsqlCommand
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);

                    // Выполняем запрос и получаем данные в виде DataTable
                    DataTable clientsTable = new DataTable();
                    clientsTable.Load(command.ExecuteReader());

                    // Заполняем combobox полученными ФИО
                    comboBox1.DataSource = clientsTable;
                    comboBox1.DisplayMember = "ФИО"; // Указываем столбец для отображения в combobox
                    comboBox1.ValueMember = "ФИО"; // Указываем столбец для хранения значения в combobox
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void LoadProducts(string searchTerm = "")
        {
            try
            {
                // Создание соединения с базой данных
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    // Открытие соединения
                    connection.Open();

                    // Создание команды для выполнения запроса
                    string query = "SELECT name_product as \"Название товара\", price as \"Цена\", characs as \"Характеристика товара\" FROM products";

                    // Если есть термин для поиска, добавляем его к запросу
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += $" WHERE name_product LIKE '%{searchTerm}%'";
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

        private void LoadCart()
        {
            // Соединение с базой данных
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                // Открываем соединение
                connection.Open();

                // Запрос к таблице "корзина"
                string query = "SELECT * FROM basket";

                // Создаем объект NpgsqlCommand
                NpgsqlCommand command = new NpgsqlCommand(query, connection);

                // Выполняем запрос и получаем данные в виде DataTable
                DataTable cartTable = new DataTable();
                cartTable.Load(command.ExecuteReader());

                // Заполняем DataGridView2 полученными данными
                dataGridView2.DataSource = cartTable;
            }
        }

        private void SellerForm_Load(object sender, EventArgs e)
        {
            // Заполняем DataGridView1 таблицей "товары"
            LoadProducts();

            // Заполняем DataGridView2 таблицей "корзина"
            LoadCart();

            // Заполняем combobox ФИО клиентов
            LoadClients();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void AddToCart(int id_product, int count)
        {
           
        }
    }
}
