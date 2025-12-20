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
    public partial class CustomersFormSeller : Form
    {
        private NpgsqlConnection conn;
        public CustomersFormSeller()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_customers, first_name as Имя, last_name as Фамилия, " +
                    "patronymic as Отчество, phone as Телефон, email as Почта " +
                    "FROM shem.customers WHERE is_deleted = false ORDER BY first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "customers");
                dataGridView1.DataSource = ds.Tables["customers"];
                dataGridView1.Columns["id_customers"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void CustomersFormSeller_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                "Добавить покупателя?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.add_customer(@first, @last, @patr, @phone, @email)", conn);

                        cmd.Parameters.AddWithValue("@first", txtName.Text);
                        cmd.Parameters.AddWithValue("@last", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@patr", txtPatronymic.Text);
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Покупатель добавлен", "Сообщение", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        conn.Close();
                    }
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

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
