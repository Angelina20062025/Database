using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class EmployeesForm : Form
    {
        private NpgsqlConnection conn;
        public EmployeesForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadRoles();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT e.id_employees, e.first_name as Имя, e.last_name as Фамилия, e.patronymic as Отчество, " +
                    "e.phone as Телефон, er.name as Роль " +
                    "FROM shem.employees e " +
                    "JOIN shem.employee_roles er ON e.id_employee_roles = er.id_employee_roles " +
                    "WHERE e.is_deleted = false " +
                    "ORDER BY e.last_name, e.first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "employees");
                dataGridView1.DataSource = ds.Tables["employees"];
                dataGridView1.Columns["id_employees"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void LoadRoles()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_employee_roles, name FROM shem.employee_roles ORDER BY name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "roles");
                cmbRole.DataSource = ds.Tables["roles"];
                cmbRole.DisplayMember = "name";
                cmbRole.ValueMember = "id_employee_roles";
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }


        private void EmployeesForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.insert_employee(@first, @last, @patr, @phone, @role)", conn);

                    cmd.Parameters.AddWithValue("@first", txtName.Text);
                    cmd.Parameters.AddWithValue("@last", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@patr", txtPatronymic.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@role", (int)cmbRole.SelectedValue);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Сотрудник добавлен", "Сообщение", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    LoadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    conn.Close();
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Введите имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("Введите фамилию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBox.Show("Введите телефон", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль", "Ошибка",
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
                int empId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_employees"].Value);
                EditEmployeesForm editForm = new EditEmployeesForm(empId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadEmployees();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int empId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_employees"].Value);
                string empName = dataGridView1.CurrentRow.Cells["Имя"].Value.ToString();
                string empLastName = dataGridView1.CurrentRow.Cells["Фамилия"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить сотрудника {empName} {empLastName}?",
                    "Подтверждение", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.archive_employee(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", empId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Сотрудник успешно архивирован",
                            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        conn.Close();
                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при архивации: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }
    }
}
