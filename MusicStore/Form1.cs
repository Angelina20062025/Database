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
        private string currentUserRole = "guest"; // гость, продавец, админ
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
            btnManageUsers.Visible = (currentUserRole == "admin");
            btnAddRecord.Visible = (currentUserRole == "admin" || currentUserRole == "seller");
            btnEditRecord.Visible = (currentUserRole == "admin" || currentUserRole == "seller");
            btnDeleteRecord.Visible = (currentUserRole == "admin");
            btnSell.Visible = (currentUserRole == "seller");
            btnReserve.Visible = (currentUserRole == "seller");
            btnViewReservations.Visible = (currentUserRole == "seller" || currentUserRole == "admin");

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
                "SELECT r.catalog_number as \"Название_каталога\", " +
                "r.title as \"Название_диска\", " +
                "r.release_date as \"Дата_выпуска\", " +
                "r.retail_price as \"Цена_(руб.)\", " +
                "r.remaining_quantity as \"В_наличии_(шт.)\", " +
                "r.description as \"Описание\" " +
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
                    "SELECT r.catalog_number as \"Название_каталога\", " +
                    "r.title as \"Название_диска\", r.release_date as \"Дата_выпуска_\", " +
                    "r.wholesale_price as \"Оптовая_цена_(руб.)\", r.retail_price as \"Розничная_цена_(руб.)\", " +
                    "r.remaining_quantity as \"Количество_в_наличии_(шт.)\", " +
                    "r.description as \"Описание_\" " +
                    "FROM shem.record r " +
                    "ORDER BY r.title", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "records");

                foreach (DataColumn column in ds.Tables["records"].Columns)
                {
                    column.ColumnName = column.ColumnName.Replace("_", " ");
                }

                dataGridView1.DataSource = ds.Tables["records"];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            //AddRecordForm addForm = new AddRecordForm();
            //addForm.ShowDialog();
            //LoadDetailedCatalog();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.CurrentRow != null)
            //{
            //    int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_record"].Value);
            //    EditRecordForm editForm = new EditRecordForm(recordId);
            //    editForm.ShowDialog();
            //    LoadDetailedCatalog();
            //}
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.CurrentRow != null)
            //{
            //    int recordId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_record"].Value);

            //    DialogResult result = MessageBox.Show(
            //        "Вы уверены, что хотите удалить эту запись?",
            //        "Подтверждение удаления",
            //        MessageBoxButtons.YesNo);

            //    if (result == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            conn.Open();
            //            NpgsqlCommand cmd = new NpgsqlCommand(
            //                "DELETE FROM shem.record WHERE id_record = @id", conn);
            //            cmd.Parameters.AddWithValue("id", recordId);
            //            cmd.ExecuteNonQuery();
            //            conn.Close();

            //            MessageBox.Show("Запись удалена успешно");
            //            LoadDetailedCatalog();
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Ошибка удаления: " + ex.Message);
            //        }
            //    }
            //}
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int employeeId = GetEmployeeId(currentUserId);
                SellRecordForm sellForm = new SellRecordForm(employeeId);
                sellForm.ShowDialog();
                LoadDetailedCatalog();
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
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            //ManageUsersForm usersForm = new ManageUsersForm();
            //usersForm.ShowDialog();
        }

        private void btnViewReservations_Click(object sender, EventArgs e)
        {
            //ReservationsForm reservationsForm = new ReservationsForm();
            //reservationsForm.ShowDialog();
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
                    ds.Tables["search_results"].Columns["cd_id"].ColumnName = "ID";
                    ds.Tables["search_results"].Columns["title"].ColumnName = "Название диска";
                    ds.Tables["search_results"].Columns["catalog_number"].ColumnName = "Название каталога";
                    ds.Tables["search_results"].Columns["ensemble_name"].ColumnName = "Ансамбль";
                    ds.Tables["search_results"].Columns["release_date"].ColumnName = "Дата выпуска";
                    ds.Tables["search_results"].Columns["retail_price"].ColumnName = "Цена (руб.)";
                    ds.Tables["search_results"].Columns["remaining_quantity"].ColumnName = "В наличиим (шт.)";
                }

                dataGridView1.DataSource = ds.Tables["search_results"];
                dataGridView1.Columns["ID"].Visible = false;
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
                    ds.Tables["leaders"].Columns["cd_title"].ColumnName = "Название диска";
                    ds.Tables["leaders"].Columns["catalog_number"].ColumnName = "Название каталога";
                    ds.Tables["leaders"].Columns["current_year"].ColumnName = "Продано в этом году (шт.)";
                    ds.Tables["leaders"].Columns["total_revenue"].ColumnName = "Общая выручка (руб.)";
                    ds.Tables["leaders"].Columns["last_year_sales"].ColumnName = "Продано за прошлый год (шт.)";
                    ds.Tables["leaders"].Columns["remaining_quantity"].ColumnName = "В наличии (шт.)";
                }

                dataGridView1.DataSource = ds.Tables["leaders"];
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
            LoadInitialData();
        }
    }
}
