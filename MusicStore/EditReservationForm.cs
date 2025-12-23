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
    public partial class EditReservationForm : Form
    {
        private NpgsqlConnection conn;
        private int reservationId;
        private int currentQuantity;
        private int currentRecordId;
        private int currentCustomerId;
        private int currentEmployeeId;
        public EditReservationForm(int id)
        {
            InitializeComponent();
            reservationId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadCustomers();
            LoadEmployees();
            LoadRecords();
            LoadStatuses();
            LoadReservationData();
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
                    "SELECT id_record, CONCAT(title, ' (', catalog_number, ')') as display " +
                    "FROM shem.record " +
                    "WHERE is_deleted = false " +
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

        private void LoadReservationData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT r.id_customers, r.id_employees, r.id_record, " +
                    "r.quantity, r.expiry_date, r.status, r.notes " +
                    "FROM shem.reservations r " +
                    "WHERE r.id_reservations = @id", conn);
                cmd.Parameters.AddWithValue("@id", reservationId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentCustomerId = reader.GetInt32(reader.GetOrdinal("id_customers"));
                        currentEmployeeId = reader.GetInt32(reader.GetOrdinal("id_employees"));
                        currentRecordId = reader.GetInt32(reader.GetOrdinal("id_record"));
                        currentQuantity = reader.GetInt32(reader.GetOrdinal("quantity"));

                        for (int i = 0; i < cmbCustomer.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbCustomer.Items[i];
                            if (Convert.ToInt32(row["id_customers"]) == currentCustomerId)
                            {
                                cmbCustomer.SelectedIndex = i;
                                break;
                            }
                        }

                        for (int i = 0; i < cmbEmployee.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbEmployee.Items[i];
                            if (Convert.ToInt32(row["id_employees"]) == currentEmployeeId)
                            {
                                cmbEmployee.SelectedIndex = i;
                                break;
                            }
                        }

                        for (int i = 0; i < cmbRecord.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbRecord.Items[i];
                            if (Convert.ToInt32(row["id_record"]) == currentRecordId)
                            {
                                cmbRecord.SelectedIndex = i;
                                break;
                            }
                        }

                        string currentStatus = reader["status"].ToString();
                        for (int i = 0; i < cmbStatus.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbStatus.Items[i];
                            if (row["status_name"].ToString() == currentStatus)
                            {
                                cmbStatus.SelectedIndex = i;
                                break;
                            }
                        }

                        numQuantity.Value = currentQuantity;
                        dtpExpiryDate.Value = Convert.ToDateTime(reader["expiry_date"]);
                        txtNotes.Text = reader["notes"].ToString();
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

        private void EditReservationForm_Load(object sender, EventArgs e)
        {

        }

        private void btnUpd_Click(object sender, EventArgs e)
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

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.update_reservation(@id, @customer, @employee, @record, @quantity, @expiry, @notes, @status)", conn);

                        cmd.Parameters.AddWithValue("@id", reservationId);
                        cmd.Parameters.AddWithValue("@customer", (int)cmbCustomer.SelectedValue);
                        cmd.Parameters.AddWithValue("@employee", (int)cmbEmployee.SelectedValue);
                        cmd.Parameters.AddWithValue("@record", (int)cmbRecord.SelectedValue);
                        cmd.Parameters.AddWithValue("@quantity", (int)numQuantity.Value);
                        cmd.Parameters.Add("@expiry", NpgsqlDbType.Date).Value = dtpExpiryDate.Value.Date;
                        cmd.Parameters.AddWithValue("@notes", txtNotes.Text);
                        cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Бронирование обновлено",
                            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Количество должно быть положительным числом", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpExpiryDate.Value <= DateTime.Today)
            {
                MessageBox.Show("Дата истечения должна быть в будущем", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpExpiryDate.Value > DateTime.Today.AddDays(7))
            {
                MessageBox.Show("Срок бронирования не может превышать 7 дней", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус", "Ошибка",
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
    }
}
