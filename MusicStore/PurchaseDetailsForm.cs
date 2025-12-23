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
    public partial class PurchaseDetailsForm : Form
    {
        private NpgsqlConnection conn;
        private int purchaseId;
        public PurchaseDetailsForm(int id)
        {
            InitializeComponent();
            purchaseId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numQuantity.Maximum = 500;
            LoadPurchaseInfo();
            LoadPurchaseDetails();
            LoadRecords();
        }

        private void LoadPurchaseInfo()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT p.id_purchases, p.purchase_date, " +
                    "c.first_name || ' ' || c.last_name as customer_name, " +
                    "e.first_name || ' ' || e.last_name as employee_name, " +
                    "pm.name as payment_method " +
                    "FROM shem.purchases p " +
                    "JOIN shem.customers c ON p.id_customers = c.id_customers " +
                    "JOIN shem.employees e ON p.id_employees = e.id_employees " +
                    "JOIN shem.payment_methods pm ON p.id_payment_methods = pm.id_payment_methods " +
                    "WHERE p.id_purchases = @id", conn);
                cmd.Parameters.AddWithValue("@id", purchaseId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        this.Text = $"Детали покупки №{reader["id_purchases"]}";
                        lblDate.Text = $"Дата: {reader["purchase_date"]:dd.MM.yyyy}";
                        lblCustomer.Text = $"Покупатель: {reader["customer_name"]}";
                        lblEmployee.Text = $"Сотрудник: {reader["employee_name"]}";
                        lblPaymentMethod.Text = $"Оплата: {reader["payment_method"]}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки информации о покупке: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadRecords()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_record, CONCAT(title, ' (', catalog_number, ')') as display, " +
                    "retail_price, remaining_quantity " +
                    "FROM shem.record " +
                    "WHERE is_deleted = false AND remaining_quantity > 0 " +
                    "ORDER BY title", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "records");
                cmbRecord.DataSource = ds.Tables["records"];
                cmbRecord.DisplayMember = "display";
                cmbRecord.ValueMember = "id_record";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки пластинок: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadPurchaseDetails()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT pd.id_purchase_details, " +
                    "r.title, " +
                    "r.catalog_number, " +
                    "pd.quantity, " +
                    "pd.unit_price, " +
                    "pd.quantity * pd.unit_price as subtotal " +
                    "FROM shem.purchase_details pd " +
                    "JOIN shem.record r ON pd.id_record = r.id_record " +
                    "WHERE pd.id_purchases = @id " +
                    "ORDER BY r.title", conn);
                cmd.Parameters.AddWithValue("@id", purchaseId);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "details");
                dataGridViewDetails.DataSource = ds.Tables["details"];

                if (dataGridViewDetails.Columns.Count > 0)
                {
                    dataGridViewDetails.Columns["title"].HeaderText = "Пластинка";
                    dataGridViewDetails.Columns["catalog_number"].HeaderText = "Каталожный номер";
                    dataGridViewDetails.Columns["quantity"].HeaderText = "Количество";
                    dataGridViewDetails.Columns["unit_price"].HeaderText = "Цена";
                    dataGridViewDetails.Columns["subtotal"].HeaderText = "Сумма";

                    dataGridViewDetails.Columns["unit_price"].DefaultCellStyle.Format = "0.00 руб.";
                    dataGridViewDetails.Columns["subtotal"].DefaultCellStyle.Format = "0.00 руб.";
                }
                
                dataGridViewDetails.Columns["id_purchase_details"].Visible = false;
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки деталей покупки: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void CalculateTotals()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT COALESCE(SUM(pd.quantity), 0) as total_quantity, " +
                    "COALESCE(SUM(pd.quantity * pd.unit_price), 0) as total_amount " +
                    "FROM shem.purchase_details pd " +
                    "WHERE pd.id_purchases = @id", conn);
                cmd.Parameters.AddWithValue("@id", purchaseId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblTotalQuantity.Text = $"Всего товаров: {reader["total_quantity"]} шт.";
                        lblTotalAmount.Text = $"Общая сумма: {reader["total_amount"]:0.00} руб.";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подсчета итогов: " + ex.Message);
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

        private void cmbRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRecord.SelectedValue != null)
            {
                try
                {

                    DataRowView row = (DataRowView)cmbRecord.SelectedItem;
                    int recordId = Convert.ToInt32(row["id_record"]);

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT retail_price, remaining_quantity, title " +
                        "FROM shem.record " +
                        "WHERE id_record = @id", conn);
                    cmd.Parameters.AddWithValue("@id", recordId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblRecordPrice.Text = $"Цена: {reader["retail_price"]} руб.";
                            lblRecordStock.Text = $"В наличии: {reader["remaining_quantity"]} шт.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbRecord.SelectedValue == null)
            {
                MessageBox.Show("Выберите пластинку", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = (int)numQuantity.Value;
            if (quantity <= 0)
            {
                MessageBox.Show("Введите количество", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.insert_purchase_detail(@purchase, @record, @quantity)", conn);

                cmd.Parameters.AddWithValue("@purchase", purchaseId);
                cmd.Parameters.AddWithValue("@record", (int)cmbRecord.SelectedValue);
                cmd.Parameters.AddWithValue("@quantity", quantity);

                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Товар добавлен в покупку",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadPurchaseDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetails.CurrentRow != null)
            {
                int detailId = Convert.ToInt32(dataGridViewDetails.CurrentRow.Cells["id_purchase_details"].Value);
                string recordTitle = dataGridViewDetails.CurrentRow.Cells["title"].Value.ToString();
                int quantity = Convert.ToInt32(dataGridViewDetails.CurrentRow.Cells["quantity"].Value);

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить из покупки:\n" +
                    $"{recordTitle}\n" +
                    $"Количество: {quantity} шт.\n" +
                    "Товар будет возвращен на склад.",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.delete_purchase_detail(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", detailId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool success = reader.GetBoolean(0);
                                string message = reader.GetString(1);

                                if (success)
                                {
                                    MessageBox.Show(message, "Удалено",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show(message, "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        conn.Close();
                        LoadPurchaseDetails();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewDetails.CurrentRow != null)
            {
                int detailId = Convert.ToInt32(dataGridViewDetails.CurrentRow.Cells["id_purchase_details"].Value);
                EditPurchaseDetailForm editForm = new EditPurchaseDetailForm(detailId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadPurchaseInfo();
                LoadPurchaseDetails();
            }
        }
    }
}
