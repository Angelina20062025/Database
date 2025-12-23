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

namespace MusicStore
{
    public partial class UsersForm : Form
    {
        private NpgsqlConnection conn;
        public UsersForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadUsers();
            LoadEmployees();
        }

        private void LoadUsers()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
    "SELECT u.id_users, u.login as Логин, " +
    "e.first_name || ' ' || e.last_name as Сотрудник, " +
    "er.name as Роль " +
    "FROM shem.users u " +
    "JOIN shem.employees e ON u.id_employees = e.id_employees " +
    "JOIN shem.employee_roles er ON e.id_employee_roles = er.id_employee_roles " +
    "WHERE u.is_deleted = false " +
    "ORDER BY er.name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "users");

                dataGridView1.DataSource = ds.Tables["users"];
                dataGridView1.Columns["id_users"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message);
                conn.Close();
            }
        }

        private void LoadEmployees()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_employees, first_name || ' ' || last_name as full_name " +
                    "FROM shem.employees " +
                    "WHERE is_deleted = false " +
                    "ORDER BY last_name, first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "employees");

                cmbEmployee.DataSource = ds.Tables["employees"];
                cmbEmployee.DisplayMember = "full_name";
                cmbEmployee.ValueMember = "id_employees";
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки сотрудников: " + ex.Message);
                conn.Close();
            }
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.insert_user(@login, @password, @emp_id)", conn);

                    cmd.Parameters.AddWithValue("@login", txtLogin.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@emp_id", (int)cmbEmployee.SelectedValue);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Пользователь успешно добавлен", "Успешно",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    conn.Close();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления: " + ex.Message, "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPassword.Text.Length < 7)
            {
                MessageBox.Show("Пароль должен содержать минимум 7 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbEmployee.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int userId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_users"].Value);

                EditUserForm editForm = new EditUserForm(userId);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int userId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_users"].Value);
                string login = dataGridView1.CurrentRow.Cells["Логин"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить пользователя {login}?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.archive_user(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Пользователь успешно архивирован", "Успешно",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }
    }
}
