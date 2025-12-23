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
    public partial class PurchasesForm : Form
    {
        private NpgsqlConnection conn;
        public PurchasesForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadPurchases();
            LoadCustomers();
            LoadEmployees();
            LoadPaymentMethods();
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
                    "ORDER BY first_name", conn);
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
                    "SELECT e.id_employees, e.first_name || ' ' || e.last_name as full_name " +
            "FROM shem.employees e " +
            "JOIN shem.employee_roles er ON e.id_employee_roles = er.id_employee_roles " +
            "WHERE er.name = 'Продавец' " +
            "ORDER BY e.last_name, e.first_name", conn);
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

        private void LoadPurchases()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT p.id_purchases, " +
                    "p.purchase_date, " +
                    "c.first_name || ' ' || c.last_name as customer_name, " +
                    "e.first_name || ' ' || e.last_name as employee_name, " +
                    "pm.name as payment_method " +
                    "FROM shem.purchases p " +
                    "JOIN shem.customers c ON p.id_customers = c.id_customers " +
                    "JOIN shem.employees e ON p.id_employees = e.id_employees " +
                    "JOIN shem.payment_methods pm ON p.id_payment_methods = pm.id_payment_methods " +
                    "WHERE p.is_deleted = false " +
                    "ORDER BY p.purchase_date DESC, p.id_purchases DESC", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "purchases");
                dataGridView1.DataSource = ds.Tables["purchases"];

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["purchase_date"].HeaderText = "Дата";
                    dataGridView1.Columns["customer_name"].HeaderText = "Покупатель";
                    dataGridView1.Columns["employee_name"].HeaderText = "Сотрудник";
                    dataGridView1.Columns["payment_method"].HeaderText = "Способ оплаты";
                }
                dataGridView1.Columns["id_purchases"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки покупок: " + ex.Message);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "CALL shem.insert_purchase(@customer, @employee, @payment, @date)", conn);

                    cmd.Parameters.AddWithValue("@customer", (int)cmbCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@employee", (int)cmbEmployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@payment", cmbPaymentMethod.Text);
                    cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = dtpPurchaseDate.Value.Date;

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Покупка успешно создана",
                        "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadPurchases();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    conn.Close();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int purchaseId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_purchases"].Value);
                EditPurchaseForm editForm = new EditPurchaseForm(purchaseId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadPurchases();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int purchaseId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_purchases"].Value);

                DialogResult result = MessageBox.Show("Вы уверены, что хотите архивировать покупку?",
                    "Подтверждение архивации",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_purchase(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", purchaseId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool success = reader.GetBoolean(0);
                                string message = reader.GetString(1);
                                int detailsCount = reader.GetInt32(2);
                                decimal totalAmount = reader.GetDecimal(3);

                                if (success)
                                {
                                    MessageBox.Show(message, "Архивация успешна",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(message, "Невозможно архивировать",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        conn.Close();
                        LoadPurchases();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message);
                    }
                }
            }
        }

        private void butDetails_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int purchaseId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_purchases"].Value);
                PurchaseDetailsForm detailsForm = new PurchaseDetailsForm(purchaseId);
                detailsForm.ShowDialog();
            }
        }

        private void PurchasesForm_Load(object sender, EventArgs e)
        {

        }
    }
}
