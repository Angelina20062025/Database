using System;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class LoginForm : Form
    {
        private NpgsqlConnection conn;
        public string UserRole { get; private set; } = "guest";
        public int UserId { get; private set; } = -1;
        public LoginForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn.Open();

                //проверка пользователя с использованием crypt
                string checkUserQuery = @"
                    SELECT u.id_users, u.login, 
                           (u.password_hash = crypt(@password, u.password_hash)) as password_match,
                           er.name as role_name,
                           e.id_employees
                    FROM shem.users u
                    JOIN shem.employees e ON u.id_employees = e.id_employees
                    JOIN shem.employee_roles er ON e.id_employee_roles = er.id_employee_roles
                    WHERE u.login = @login";

                NpgsqlCommand cmd = new NpgsqlCommand(checkUserQuery, conn);
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    bool passwordMatch = reader.GetBoolean(reader.GetOrdinal("password_match"));

                    if (passwordMatch)
                    {
                        UserId = reader.GetInt32(reader.GetOrdinal("id_users"));
                        string role = reader.GetString(reader.GetOrdinal("role_name"));

                        //уровень доступа
                        if (role.ToLower() == "администратор")
                            UserRole = "admin";
                        else if (role.ToLower() == "продавец")
                            UserRole = "seller";
                        else
                            UserRole = "guest";

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
