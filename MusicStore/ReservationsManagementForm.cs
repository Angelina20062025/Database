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
    public partial class ReservationsManagementForm : Form
    {
        private NpgsqlConnection conn;
        public ReservationsManagementForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadReservations();
            LoadCustomers();
            LoadEmployees();
            LoadRecords();
            LoadStatuses();
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

        private void LoadStatuses()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT status_name FROM shem.reservation_statuses ORDER BY status_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "statuses");
                cmbStatus.DataSource = ds.Tables["statuses"];
                cmbStatus.DisplayMember = "status_name";
                cmbStatus.ValueMember = "status_name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки статусов: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadReservations()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT r.id_reservations, " +
                    "r.reservation_date, " +
                    "c.first_name || ' ' || c.last_name as customer_name, " +
                    "e.first_name || ' ' || e.last_name as employee_name, " +
                    "rec.title as record_title, " +
                    "r.quantity, " +
                    "r.expiry_date, " +
                    "r.status, " +
                    "r.notes " +
                    "FROM shem.reservations r " +
                    "JOIN shem.customers c ON r.id_customers = c.id_customers " +
                    "JOIN shem.employees e ON r.id_employees = e.id_employees " +
                    "JOIN shem.record rec ON r.id_record = rec.id_record " +
                    "WHERE r.is_deleted = false " +
                    "ORDER BY r.status, r.reservation_date DESC, r.id_reservations DESC", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "reservations");
                dataGridViewReservations.DataSource = ds.Tables["reservations"];

                if (dataGridViewReservations.Columns.Count > 0)
                {
                    dataGridViewReservations.Columns["reservation_date"].HeaderText = "Дата брони";
                    dataGridViewReservations.Columns["customer_name"].HeaderText = "Покупатель";
                    dataGridViewReservations.Columns["employee_name"].HeaderText = "Сотрудник";
                    dataGridViewReservations.Columns["record_title"].HeaderText = "Пластинка";
                    dataGridViewReservations.Columns["quantity"].HeaderText = "Количество";
                    dataGridViewReservations.Columns["expiry_date"].HeaderText = "Действует до";
                    dataGridViewReservations.Columns["status"].HeaderText = "Статус";
                    dataGridViewReservations.Columns["notes"].HeaderText = "Заметки";

                    dataGridViewReservations.Columns["id_reservations"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки бронирований: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void ReservationsManagementForm_Load(object sender, EventArgs e)
        {

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
            if (ValidateInput())
            {
                try
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.insert_reservation(@customer, @employee, @record, @quantity, @expiry, @notes)", conn);

                    cmd.Parameters.AddWithValue("@customer", (int)cmbCustomer.SelectedValue);
                    cmd.Parameters.AddWithValue("@employee", (int)cmbEmployee.SelectedValue);
                    cmd.Parameters.AddWithValue("@record", (int)cmbRecord.SelectedValue);
                    cmd.Parameters.AddWithValue("@quantity", (int)numQuantity.Value);
                    cmd.Parameters.AddWithValue("@expiry", (int)numExpiryDays.Value);
                    cmd.Parameters.AddWithValue("@notes", txtNotes.Text);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Бронирование успешно создано",
                        "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadReservations();
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

            if (cmbRecord.SelectedItem == null)
            {
                MessageBox.Show("Выберите пластинку", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Введите количество", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numExpiryDays.Value < 1 || numExpiryDays.Value > 7)
            {
                MessageBox.Show("Срок бронирования должен быть от 1 до 7 дней", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewReservations.CurrentRow != null)
            {
                int reservationId = Convert.ToInt32(dataGridViewReservations.CurrentRow.Cells["id_reservations"].Value);
                EditReservationForm editForm = new EditReservationForm(reservationId);
                if (editForm.ShowDialog() == DialogResult.OK)
                    LoadReservations();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewReservations.CurrentRow != null)
            {
                int reservationId = Convert.ToInt32(dataGridViewReservations.CurrentRow.Cells["id_reservations"].Value);
                string status = dataGridViewReservations.CurrentRow.Cells["status"].Value.ToString();
                string customer = dataGridViewReservations.CurrentRow.Cells["customer_name"].Value.ToString();
                string record = dataGridViewReservations.CurrentRow.Cells["record_title"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Архивировать бронирование?\n" +
                    $"Покупатель: {customer}\n" +
                    $"Пластинка: {record}\n" +
                    $"Статус: {status}",
                    "Подтверждение архивации",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.soft_delete_reservation(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", reservationId);

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
                                }
                            }
                        }
                        conn.Close();
                        LoadReservations();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message);
                        conn.Close();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
