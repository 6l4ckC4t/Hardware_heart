using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Hardware_heart
{
    public partial class Form1 : Form
    {

        // Параметры подключения (замените на свои)
        string connectionString = "Host=localhost;Username=postgres;Password=virtual;Database=Hardware_heart";


        public Form1()
        {

        NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            InitializeComponent();

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '\0')
            {
                textBox2.PasswordChar = '●';
            }
            else
            {
                textBox2.PasswordChar = '\0';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка пользователя в БД
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id_user, role FROM users WHERE login = @username AND passwd = @password";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = (int)reader["id_user"];
                                string role = (string)reader["role"];

                               
                                // Переход на соответствующую форму
                                if (role == "Администратор")
                                {
                                    AdminForm adminForm = new AdminForm();
                                    adminForm.Show();
                                    this.Hide();
                                }

                                else if (role == "Руководитель")
                                {
                                    ManagerForm managerForm = new ManagerForm();
                                    managerForm.Show();
                                    this.Hide();
                                }

                                else if (role == "Продавец")
                                {
                                    SellerForm sellerForm = new SellerForm();
                                    sellerForm.Show();
                                    this.Hide();
                                }
                                                               
                            }

                            else
                            {
                                MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       
    }

}

