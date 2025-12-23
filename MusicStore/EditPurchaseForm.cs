using Npgsql;
using NpgsqlTypes;
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
    public partial class EditPurchaseForm : Form
    {
        private NpgsqlConnection conn;
        private int purchaseId;
        public EditPurchaseForm(int id)
        {
            InitializeComponent();
            purchaseId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadCustomers();
            LoadEmployees();
            LoadPaymentMethods();
            LoadPurchaseData();
        }

        private void LoadCustomers()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_customers, first_name || ' ' || last_name as full_name " +
                    "FROM shem.customers " +
                    "WHERE is_deleted = false " +
                    "ORDER BY last_name, first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "customers");
                cmbCustomer.DataSource = ds.Tables["customers"];
                cmbCustomer.DisplayMember = "full_name";
                cmbCustomer.ValueMember = "id_customers";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки покупателей: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadEmployees()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_employees, first_name || ' ' || last_name as full_name " +
                    "FROM shem.employees " +
                    "ORDER BY last_name, first_name", conn);
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

        private void LoadPaymentMethods()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT name FROM shem.payment_methods ORDER BY name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "payment_methods");
                cmbPaymentMethod.DataSource = ds.Tables["payment_methods"];
                cmbPaymentMethod.DisplayMember = "name";
                cmbPaymentMethod.ValueMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки способов оплаты: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadPurchaseData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT p.id_customers, p.id_employees, pm.name as payment_method, " +
                    "p.purchase_date " +
                    "FROM shem.purchases p " +
                    "JOIN shem.payment_methods pm ON p.id_payment_methods = pm.id_payment_methods " +
                    "WHERE p.id_purchases = @id", conn);
                cmd.Parameters.AddWithValue("@id", purchaseId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int customerId = reader.GetInt32(reader.GetOrdinal("id_customers"));
                        for (int i = 0; i < cmbCustomer.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbCustomer.Items[i];
                            if (Convert.ToInt32(row["id_customers"]) == customerId)
                            {
                                cmbCustomer.SelectedIndex = i;
                                break;
                            }
                        }

                        int employeeId = reader.GetInt32(reader.GetOrdinal("id_employees"));
                        for (int i = 0; i < cmbEmployee.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbEmployee.Items[i];
                            if (Convert.ToInt32(row["id_employees"]) == employeeId)
                            {
                                cmbEmployee.SelectedIndex = i;
                                break;
                            }
                        }

                        string paymentMethod = reader["payment_method"].ToString();
                        for (int i = 0; i < cmbPaymentMethod.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbPaymentMethod.Items[i];
                            if (row["name"].ToString() == paymentMethod)
                            {
                                cmbPaymentMethod.SelectedIndex = i;
                                break;
                            }
                        }

                        dtpPurchaseDate.Value = Convert.ToDateTime(reader["purchase_date"]);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.update_purchase(@id, @customer, @employee, @payment, @date)", conn);

                        cmd.Parameters.AddWithValue("@id", purchaseId);
                        cmd.Parameters.AddWithValue("@customer", (int)cmbCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@employee", (int)cmbEmployee.SelectedValue);
                        cmd.Parameters.AddWithValue("@payment", cmbPaymentMethod.Text);
                        cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = dtpPurchaseDate.Value.Date;

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Покупка успешно обновлена",
                            "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        }

        private bool ValidateInput()
        {
            if (cmbCustomer.SelectedItem == null)
            {
                MessageBox.Show("Выберите покупателя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbEmployee.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Выберите способ оплаты", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpPurchaseDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата покупки не может быть в будущем", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
