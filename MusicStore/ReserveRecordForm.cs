using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace MusicStore
{
    public partial class ReserveRecordForm : Form
    {
        private string connectionString;
        private int employeeId;
        public ReserveRecordForm(int empId)
        {
            InitializeComponent();
            employeeId = empId;
            numQuantity.ReadOnly = true;
            connectionString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            LoadComboBoxes();
            dtpReservationDate.Value = DateTime.Today;
            dtpExpiryDate.Value = DateTime.Today.AddDays(7);
        }

        private void LoadComboBoxes()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    //загрузка пластинок
                    DataSet dsRecords = new DataSet();
                    NpgsqlCommand cmdRecords = new NpgsqlCommand(
                        "SELECT id_record, title, retail_price, remaining_quantity " +
                        "FROM shem.record WHERE remaining_quantity > 0 ORDER BY title", conn);
                    NpgsqlDataAdapter daRecords = new NpgsqlDataAdapter(cmdRecords);
                    daRecords.Fill(dsRecords, "records");

                    cmbRecord.DataSource = dsRecords.Tables["records"];
                    cmbRecord.DisplayMember = "title";
                    cmbRecord.ValueMember = "id_record";

                    //загрузка клиентов
                    DataSet dsCustomers = new DataSet();
                    NpgsqlCommand cmdCustomers = new NpgsqlCommand(
                        "SELECT id_customers, first_name || ' ' || last_name as full_name, phone " +
                        "FROM shem.customers ORDER BY last_name", conn);
                    NpgsqlDataAdapter daCustomers = new NpgsqlDataAdapter(cmdCustomers);
                    daCustomers.Fill(dsCustomers, "customers");

                    cmbCustomer.DataSource = dsCustomers.Tables["customers"];
                    cmbCustomer.DisplayMember = "full_name";
                    cmbCustomer.ValueMember = "id_customers";

                    //загрузка статусов бронирования
                    DataSet dsStatuses = new DataSet();
                    NpgsqlCommand cmdStatuses = new NpgsqlCommand(
                        "SELECT status_name FROM shem.reservation_statuses ORDER BY id_status", conn);
                    NpgsqlDataAdapter daStatuses = new NpgsqlDataAdapter(cmdStatuses);
                    daStatuses.Fill(dsStatuses, "statuses");

                    cmbStatus.DataSource = dsStatuses.Tables["statuses"];
                    cmbStatus.DisplayMember = "status_name";
                    cmbStatus.ValueMember = "status_name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void ReserveRecordForm_Load(object sender, EventArgs e)
        {

        }

        private void cmbRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRecord.SelectedValue != null)
            {
                try
                {
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        DataRowView row = (DataRowView)cmbRecord.SelectedItem;
                        int recordId = Convert.ToInt32(row["id_record"]);

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT retail_price, remaining_quantity, title FROM shem.record WHERE id_record = @id", conn);
                        cmd.Parameters.AddWithValue("id", recordId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal price = reader.GetDecimal(reader.GetOrdinal("retail_price"));
                                int available = reader.GetInt32(reader.GetOrdinal("remaining_quantity"));
                                string title = reader.GetString(reader.GetOrdinal("title"));

                                lblPrice.Text = price.ToString("0.00") + " руб.";
                                lblAvailable.Text = available.ToString() + " шт.";
                                numQuantity.Maximum = available;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpReservationDate_ValueChanged(object sender, EventArgs e)
        {
            //дата истечения устанавливается автоматически
            if (dtpReservationDate.Value > dtpExpiryDate.Value)
            {
                dtpExpiryDate.Value = dtpReservationDate.Value.AddDays(7);
            }
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    //функция create_reservation из бд
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.create_reservation(" +
                        "@customer_id, @employee_id, @record_id, @quantity, @expiry_days, @notes)", conn);

                    cmd.Parameters.AddWithValue("customer_id", (int)cmbCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("employee_id", employeeId);
                    cmd.Parameters.AddWithValue("record_id", (int)cmbRecord.SelectedValue);
                    cmd.Parameters.AddWithValue("quantity", (int)numQuantity.Value);
                    cmd.Parameters.AddWithValue("expiry_days", (int)(dtpExpiryDate.Value - dtpReservationDate.Value).TotalDays);
                    cmd.Parameters.AddWithValue("notes", txtNotes.Text);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int reservationId = reader.GetInt32(reader.GetOrdinal("reservation_id"));
                            string customerName = reader.GetString(reader.GetOrdinal("customer_name"));
                            string recordTitle = reader.GetString(reader.GetOrdinal("record_title"));
                            int quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                            DateTime expiryDate = reader.GetDateTime(reader.GetOrdinal("expiry_date"));

                            MessageBox.Show($"Бронь успешно создана.\n\n" +
                                          $"Номер брони: {reservationId}\n" +
                                          $"Клиент: {customerName}\n" +
                                          $"Пластинка: {recordTitle}\n" +
                                          $"Количество: {quantity} шт.\n" +
                                          $"Действительна до: {expiryDate:dd.MM.yyyy}",
                                          "Бронь создана",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private bool ValidateInput()
        {
            if (cmbRecord.SelectedValue == null)
            {
                MessageBox.Show("Выберите пластинку для бронирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRecord.Focus();
                return false;
            }

            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCustomer.Focus();
                return false;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Введите количество для бронирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numQuantity.Focus();
                return false;
            }

            if (dtpExpiryDate.Value <= dtpReservationDate.Value)
            {
                MessageBox.Show("Дата истечения должна быть позже даты бронирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpiryDate.Focus();
                return false;
            }

            TimeSpan duration = dtpExpiryDate.Value - dtpReservationDate.Value;
            if (duration.Days > 7)
            {
                MessageBox.Show("Максимальный срок бронирования - 7 дней", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpExpiryDate.Focus();
                return false;
            }

            try
            {
                int available = int.Parse(lblAvailable.Text.Replace(" шт.", ""));
                if ((int)numQuantity.Value > available)
                {
                    MessageBox.Show($"Недостаточно пластинок в наличии. Доступно: {available} шт.",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch { }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
