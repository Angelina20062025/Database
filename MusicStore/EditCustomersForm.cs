using Npgsql;
using NpgsqlTypes;
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
    public partial class EditCustomersForm : Form
    {
        private NpgsqlConnection conn;
        private int custId;
        public EditCustomersForm(int id)
        {
            InitializeComponent();
            custId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT * FROM shem.customers WHERE id_customers = @id", conn);
                cmd.Parameters.AddWithValue("id", custId);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["first_name"].ToString();
                    txtLastName.Text = reader["last_name"].ToString();
                    txtPatronymic.Text = reader["patronymic"].ToString();
                    txtPhone.Text = reader["phone"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void EditCustomersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnUpd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                "Обновить данные?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.update_customer(@p_id_customers, @p_first_name, @p_last_name, @p_patronymic, @p_phone, @p_email)",
                            conn);

                        cmd.Parameters.AddWithValue("@p_id_customers", custId);
                        cmd.Parameters.AddWithValue("@p_first_name", txtName.Text);
                        cmd.Parameters.AddWithValue("@p_last_name", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@p_patronymic", txtPatronymic.Text);
                        cmd.Parameters.AddWithValue("@p_phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@p_email", txtEmail.Text);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Данные успешно обновлены", "Сообщение", MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка обновления: " + ex.Message, "Ошибка", MessageBoxButtons.OK,
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
