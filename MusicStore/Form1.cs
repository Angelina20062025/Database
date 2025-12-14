using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection conn;
        private string currentUserRole = "guest"; //гость, продавец, админ
        private int currentUserId = -1;
        public Form1()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadInitialData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateByRole();
        }

        private void UpdateByRole()
        {
            //видимость элементов в зависимости от роли
            btnLogin.Text = currentUserRole == "guest" ? "Авторизоваться" : "Выйти";
            btnAddRecord.Visible = (currentUserRole == "admin");
            btnEditRecord.Visible = (currentUserRole == "admin");
            btnDeleteRecord.Visible = (currentUserRole == "admin");
            btnArcRecord.Visible = (currentUserRole == "admin");
            btnEnsembles.Visible = (currentUserRole == "admin");
            btnSell.Visible = (currentUserRole == "seller");
            btnReserve.Visible = (currentUserRole == "seller");
            btnViewReservations.Visible = (currentUserRole == "seller");

            if (currentUserRole == "admin")
            {
                lblUserInfo.Text = "Вы вошли как: администратор";
                lblUserInfo.Visible = true;
            }
            else if (currentUserRole == "seller")
            {
                lblUserInfo.Text = "Вы вошли как: сотрудник";
                lblUserInfo.Visible = true;
            }
            else
            {
                lblUserInfo.Text = "Вы вошли как: гость";
                lblUserInfo.Visible = true;
            }
        }

        private void LoadInitialData()
        {
            try
            {
                conn.Open();

                //публичный доступ к каталогу пластинок
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT r.id_record, r.catalog_number as \"Название_каталога\", " +
                "r.title as \"Название_диска\", " +
                "r.release_date as \"Дата_выпуска\", " +
                "r.retail_price as \"Цена_(руб.)\", " +
                "r.remaining_quantity as \"В_наличии_(шт.)\" " +
                "FROM shem.record r " +
                "WHERE r.remaining_quantity > 0 " +
                "ORDER BY r.title", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "records");

                //колонки для отображения
                foreach (DataColumn column in ds.Tables["records"].Columns)
                {
                    column.ColumnName = column.ColumnName.Replace("_", " ");
                }

                dataGridView1.DataSource = ds.Tables["records"];
                dataGridView1.Columns["id record"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (currentUserRole == "guest")
            {
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    currentUserRole = loginForm.UserRole;
                    currentUserId = loginForm.UserId;
                    UpdateByRole();

                    if (currentUserRole == "admin")
                    {
                        LoadDetailedCatalog();
                    }
                    else if (currentUserRole == "seller")
                    {
                        LoadInitialData();
                    }
                }
            }
            else
            {
                //выход из системы
                currentUserRole = "guest";
                currentUserId = -1;
                UpdateByRole();
                LoadInitialData();
            }
        }

        private void LoadDetailedCatalog()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();

                //каталог для администратора
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT r.id_record, r.catalog_number as \"Название_каталога\", " +
                    "r.title as \"Название_диска\", r.release_date as \"Дата_выпуска_\", " +
                    "r.wholesale_price as \"Оптовая_цена_(руб.)\", r.retail_price as \"Розничная_цена_(руб.)\", " +
                    "r.remaining_quantity as \"Количество_в_наличии_(шт.)\" " +
                    "FROM shem.record r " +
                    "ORDER BY r.is_deleted", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "records");

                foreach (DataColumn column in ds.Tables["records"].Columns)
                {
                    column.ColumnName = column.ColumnName.Replace("_", " ");
                }
                
                dataGridView1.DataSource = ds.Tables["records"];
                dataGridView1.Columns["id record"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            AddRecordForm addForm = new AddRecordForm();
            addForm.ShowDialog();
            LoadDetailedCatalog();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id record"].Value);
                EditRecordForm editForm = new EditRecordForm(recordId);
                editForm.ShowDialog();
                LoadDetailedCatalog();
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id record"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить эту пластинку, а также записи о её покупках и бронированиях?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.delete_record(@id)", conn);
                        cmd.Parameters.AddWithValue("id", recordId);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Запись удалена успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDetailedCatalog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка удаления: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int employeeId = GetEmployeeId(currentUserId);
                SellRecordForm sellForm = new SellRecordForm(employeeId);
                sellForm.ShowDialog();
                LoadInitialData();
            }
        }

        private int GetEmployeeId(int userId)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_employees FROM shem.users WHERE id_users = @user_id", conn);
                cmd.Parameters.AddWithValue("user_id", userId);

                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : -1;
            }
            catch
            {
                MessageBox.Show("Ошибка получения id сотрудника.");
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            int employeeId = GetEmployeeId(currentUserId);
            ReserveRecordForm reserveForm = new ReserveRecordForm(employeeId);
            reserveForm.ShowDialog();
            LoadInitialData();
        }

        private void btnViewReservations_Click(object sender, EventArgs e)
        {
            ReservationsForm reservationsForm = new ReservationsForm();
            reservationsForm.ShowDialog();
            LoadInitialData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchRecords(searchTerm);
            }
        }

        private void SearchRecords(string searchTerm)
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.search_cds(@search)", conn);
                cmd.Parameters.AddWithValue("search", searchTerm);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "search_results");

                if (ds.Tables["search_results"].Columns.Count > 0)
                {
                    ds.Tables["search_results"].Columns["cd_id"].ColumnName = "id record";
                    ds.Tables["search_results"].Columns["title"].ColumnName = "Название диска";
                    ds.Tables["search_results"].Columns["catalog_number"].ColumnName = "Название каталога";
                    ds.Tables["search_results"].Columns["ensemble_name"].ColumnName = "Ансамбль";
                    ds.Tables["search_results"].Columns["release_date"].ColumnName = "Дата выпуска";
                    ds.Tables["search_results"].Columns["retail_price"].ColumnName = "Цена (руб.)";
                    ds.Tables["search_results"].Columns["remaining_quantity"].ColumnName = "В наличиим (шт.)";
                }

                dataGridView1.DataSource = ds.Tables["search_results"];
                dataGridView1.Columns["id record"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void btnShowSalesLeaders_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.get_sales_leaders(5)", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "leaders");

                if (ds.Tables["leaders"].Columns.Count > 0)
                {
                    ds.Tables["leaders"].Columns["id_record"].ColumnName = "id record";
                    ds.Tables["leaders"].Columns["cd_title"].ColumnName = "Название диска";
                    ds.Tables["leaders"].Columns["catalog_number"].ColumnName = "Название каталога";
                    ds.Tables["leaders"].Columns["current_year"].ColumnName = "Продано в этом году (шт.)";
                    ds.Tables["leaders"].Columns["total_revenue"].ColumnName = "Общая выручка (руб.)";
                    ds.Tables["leaders"].Columns["last_year_sales"].ColumnName = "Продано за прошлый год (шт.)";
                    ds.Tables["leaders"].Columns["remaining_quantity"].ColumnName = "В наличии (шт.)";
                }

                dataGridView1.DataSource = ds.Tables["leaders"];
                dataGridView1.Columns["id record"].Visible = false;
                if (currentUserRole != "admin")
                {
                    dataGridView1.Columns["Общая выручка (руб.)"].Visible = false;
                    dataGridView1.Columns["Продано за прошлый год (шт.)"].Visible = false;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (currentUserRole == "admin")
            {
                LoadDetailedCatalog();
            }
            else
            {
                LoadInitialData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id record"].Value);
                RecordDetailsForm detailsForm = new RecordDetailsForm(recordId);
                detailsForm.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EnsembleStatsForm statsForm = new EnsembleStatsForm();
            statsForm.ShowDialog();
        }

        private void butSrchAns_Click(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchRecordsAns(searchTerm);
            }
        }

        private void SearchRecordsAns(string searchTerm)
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();

                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM shem.search_cds_by_ensemble(@search)", conn);
                cmd.Parameters.AddWithValue("search", searchTerm);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "search_results");

                if (ds.Tables["search_results"].Columns.Count > 0)
                {
                    ds.Tables["search_results"].Columns["cd_id"].ColumnName = "id record";
                    ds.Tables["search_results"].Columns["title"].ColumnName = "Название диска";
                    ds.Tables["search_results"].Columns["catalog_number"].ColumnName = "Название каталога";
                    ds.Tables["search_results"].Columns["ensemble_name"].ColumnName = "Ансамбль";
                    ds.Tables["search_results"].Columns["release_date"].ColumnName = "Дата выпуска";
                    ds.Tables["search_results"].Columns["retail_price"].ColumnName = "Цена (руб.)";
                    ds.Tables["search_results"].Columns["remaining_quantity"].ColumnName = "В наличиим (шт.)";
                }

                dataGridView1.DataSource = ds.Tables["search_results"];
                dataGridView1.Columns["id record"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void btnArcRecord_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id record"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите архивировать эту запись?",
                    "Подтверждение архивации",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_record(@id)", conn);
                        cmd.Parameters.AddWithValue("id", recordId);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool success = reader.GetBoolean(0);
                                string message = reader.GetString(1);
                                int activeReservations = reader.GetInt32(2);
                                int totalSales = reader.GetInt32(3);
                                int performancesCount = reader.GetInt32(4);

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
                        LoadDetailedCatalog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }

        private void btnEnsembles_Click(object sender, EventArgs e)
        {
            EnsemblesManagementForm form = new EnsemblesManagementForm();
            form.ShowDialog();
        }
    }
}
