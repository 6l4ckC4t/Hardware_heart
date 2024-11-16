using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Hardware_heart
{
    public partial class Form2 : Form
    {
        public Form2()
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
                string name = textBox1.Text;
                decimal price = decimal.Parse(textBox2.Text);
                string characteristics = textBox3.Text;

                // Получение выбранного названия категории из ComboBox
                string selectedCategoryName = comboBox1.SelectedItem.ToString();

                // Проверка, не пустое ли название категории
                if (string.IsNullOrEmpty(selectedCategoryName))
                {
                    MessageBox.Show("Выберите категорию товара.");
                    return;
                }

                // Открытие подключения к PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Запрос для получения ID категории по названию
                    string query = "SELECT id_category FROM category WHERE name_category = @name_category";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        // Добавление параметра
                        command.Parameters.AddWithValue("@name_category", selectedCategoryName);

                        // Выполнение запроса и получение результата
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Получение ID категории
                                int categoryId = reader.GetInt32(0);

                                // Закрытие DataReader, чтобы освободить ресурсы
                                reader.Close();

                                // Создание команды INSERT
                                string insertQuery = "INSERT INTO Products (name_product, price, characs, id_category) VALUES (@name, @price, @characteristics, @category_id)";
                                using (NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection))
                                {
                                    // Добавление параметров
                                    insertCommand.Parameters.AddWithValue("@name", name);
                                    insertCommand.Parameters.AddWithValue("@price", price);
                                    insertCommand.Parameters.AddWithValue("@characteristics", characteristics);
                                    insertCommand.Parameters.AddWithValue("@category_id", categoryId);

                                    // Выполнение команды INSERT
                                    insertCommand.ExecuteNonQuery();

                                    // Очистка TextBox
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    textBox3.Text = "";

                                    // Вывод сообщения об успешном добавлении
                                    MessageBox.Show("Товар успешно добавлен!");
                                }
                            }
                            else
                            {
                                // Обработка ошибки: категория не найдена
                                MessageBox.Show("Категория не найдена. Проверьте правильность ввода.");
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный формат цены. Введите числовое значение.");
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show("Ошибка при добавлении товара: " + ex.Message);
            }

            
        }

        

        private void Form2_Load(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";

            // Открытие подключения к PostgreSQL
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Запрос для получения всех категорий
                string query = "SELECT name_category FROM category";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader.GetString(0)); // Access the first (and only) column
                            }
                        }
                        else
                        {
                            MessageBox.Show("No categories found in the database.");
                        }
                    }
                }
            }

            // Установка значения по умолчанию (например, первую категорию)
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }
    }
}
