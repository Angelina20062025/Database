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
    public partial class CustomersForm : Form
    {
        private NpgsqlConnection conn;
        public CustomersForm()
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

        private void CustomersForm_Load(object sender, EventArgs e)
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int custId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_customers"].Value);
                EditCustomersForm editForm = new EditCustomersForm(custId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadCustomers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int custId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_customers"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить покупателя?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_customer(@id)", conn);
                        cmd.Parameters.AddWithValue("id", custId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool success = reader.GetBoolean(0);
                                string message = reader.GetString(1);

                                if (success)
                                {
                                    MessageBox.Show(message, "Архивация успешна",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(message, "Невозможно архивировать",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    conn.Close();
                                    return;
                                }
                            }
                        }

                        conn.Close();
                        LoadCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }
    }
}
