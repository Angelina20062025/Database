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
    public partial class ReservationsForm : Form
    {
        private NpgsqlConnection conn;
        public ReservationsForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadReservations();
        }

        private void LoadReservations()
        {
            try
            {
                DataSet ds = new DataSet();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.reservations_view ORDER BY status", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "reservations");

                if (ds.Tables["reservations"].Columns.Count > 0)
                {
                    ds.Tables["reservations"].Columns["id_reservations"].ColumnName = "ID";
                    ds.Tables["reservations"].Columns["customer_name"].ColumnName = "Имя покупателя";
                    ds.Tables["reservations"].Columns["employee_name"].ColumnName = "Имя сотрудника";
                    ds.Tables["reservations"].Columns["record_title"].ColumnName = "Название диска";
                    ds.Tables["reservations"].Columns["quantity"].ColumnName = "Количество";
                    ds.Tables["reservations"].Columns["reservation_date"].ColumnName = "Дата начала бронирования";
                    ds.Tables["reservations"].Columns["expiry_date"].ColumnName = "Крайний срок";
                    ds.Tables["reservations"].Columns["status"].ColumnName = "Статус";
                    ds.Tables["reservations"].Columns["notes"].ColumnName = "Заметки";
                    ds.Tables["reservations"].Columns["urgency"].ColumnName = "Срочность";
                }

                dataGridViewReservations.DataSource = ds.Tables["reservations"];
                dataGridViewReservations.Columns["ID"].Visible = false;

                //подсветка срочных бронирований
                foreach (DataGridViewRow row in dataGridViewReservations.Rows)
                {
                    if (row.Cells["Срочность"].Value?.ToString() == "СРОК ИСТЕК")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                    else if (row.Cells["Срочность"].Value?.ToString() == "ИСТЕКАЕТ СЕГОДНЯ")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки бронирований: " + ex.Message);
            }
        }

        private void ReservationsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (dataGridViewReservations.CurrentRow != null)
            {
                int reservationId = Convert.ToInt32(dataGridViewReservations.CurrentRow.Cells["ID"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите завершить бронирование?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand("SELECT shem.complete_reservation(@id)", conn);
                        cmd.Parameters.AddWithValue("id", reservationId);
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Бронирование завершено", "Сообщение", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                        LoadReservations();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }

        private void btnCancelReservation_Click(object sender, EventArgs e)
        {
            if (dataGridViewReservations.CurrentRow != null)
            {
                int reservationId = Convert.ToInt32(dataGridViewReservations.CurrentRow.Cells["ID"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите отменить бронирование?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand("SELECT shem.cancel_reservation(@id)", conn);
                        cmd.Parameters.AddWithValue("id", reservationId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Бронирование отменено", "Сообщение", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                        LoadReservations();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butUpd_Click(object sender, EventArgs e)
        {
            LoadReservations();
        }

        private void btnExpireOld_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show(
                    "Вы уверены, что хотите установить статус \"Просрочено\" для истёкших бронирований и вернуть диски в наличие?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

            if (result1 == DialogResult.Yes)
            {
                try
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT shem.expire_old_reservations()", conn);
                    object result = cmd.ExecuteScalar();

                    int expiredCount = 0;
                    if (result != null && result != DBNull.Value)
                    {
                        expiredCount = Convert.ToInt32(result);
                    }

                    if (expiredCount > 0)
                    {
                        MessageBox.Show($"Просрочено броней: {expiredCount}\n" +
                                      "Диски возвращены на склад",
                                      "Просроченные брони",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Нет просроченных активных бронирований",
                                      "Сообщение",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }

                    LoadReservations();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при обработке просроченных броней: " + ex.Message);
                    conn.Close();
                }
            }
        }
    }
}
