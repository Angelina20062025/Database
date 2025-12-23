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
    public partial class EditUserForm : Form
    {
        private NpgsqlConnection conn;
        private int userId;
        public EditUserForm(int id)
        {
            InitializeComponent();
            userId = id;

            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);

            LoadEmployees();
            LoadUserData();
        }

        private void LoadEmployees()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_employees, first_name || ' ' || last_name as full_name " +
                    "FROM shem.employees WHERE is_deleted = false ORDER BY first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "employees");

                cmbEmployee.DataSource = ds.Tables["employees"];
                cmbEmployee.DisplayMember = "full_name";
                cmbEmployee.ValueMember = "id_employees";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки сотрудников: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadUserData()
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT u.login, u.id_employees, e.first_name || ' ' || e.last_name as emp_name " +
                    "FROM shem.users u " +
                    "JOIN shem.employees e ON u.id_employees = e.id_employees " +
                    "WHERE u.id_users = @id AND u.is_deleted = false", conn);
                cmd.Parameters.AddWithValue("@id", userId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtLogin.Text = reader["login"].ToString();

                        int empId = Convert.ToInt32(reader["id_employees"]);
                        for (int i = 0; i < cmbEmployee.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbEmployee.Items[i];
                            if (Convert.ToInt32(row["id_employees"]) == empId)
                            {
                                cmbEmployee.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();

                    object passwordParam;
                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        passwordParam = txtPassword.Text;
                    }
                    else
                    {
                        passwordParam = DBNull.Value;
                    }

                    NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.update_user(@id, @login, @password, @emp_id)", conn);

                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@login", txtLogin.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", passwordParam);
                    cmd.Parameters.AddWithValue("@emp_id", (int)cmbEmployee.SelectedValue);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Данные пользователя обновлены", "Сообщение",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления: " + ex.Message, "Ошибка",
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
    }
}
