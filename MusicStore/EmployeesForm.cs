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
            //if (ValidateInput())
            //{
            //    try
            //    {
            //        conn.Open();
            //        NpgsqlCommand cmd = new NpgsqlCommand(
            //            "INSERT INTO shem.employees (first_name, last_name, patronymic, phone, id_employee_roles) " +
            //            "VALUES (@first, @last, @patr, @phone, @role)", conn);

            //        cmd.Parameters.AddWithValue("@first", txtFirstName.Text);
            //        cmd.Parameters.AddWithValue("@last", txtLastName.Text);
            //        cmd.Parameters.AddWithValue("@patr",
            //            string.IsNullOrEmpty(txtPatronymic.Text) ? (object)DBNull.Value : txtPatronymic.Text);
            //        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            //        cmd.Parameters.AddWithValue("@role", (int)cmbRole.SelectedValue);

            //        cmd.ExecuteNonQuery();
            //        conn.Close();

            //        MessageBox.Show("Сотрудник добавлен");
            //        this.DialogResult = DialogResult.OK;
            //        this.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Ошибка: " + ex.Message);
            //    }
            //}
        }
    }
}
