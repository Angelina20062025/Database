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
    public partial class PerformancesForm : Form
    {
        private NpgsqlConnection conn;
        private List<int> selectedRecordIds = new List<int>();
        public PerformancesForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadPerformances();
            LoadCompositions();
            LoadEnsembles();
            LoadRecords();
        }

        private void LoadCompositions()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT title FROM shem.compositions " +
                    "WHERE is_deleted = false " +
                    "ORDER BY title", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "compositions");
                cmbComposition.DataSource = ds.Tables["compositions"];
                cmbComposition.DisplayMember = "title";
                cmbComposition.ValueMember = "title";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки композиций: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadEnsembles()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT name FROM shem.ensembles " +
                    "WHERE is_deleted = false " +
                    "ORDER BY name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "ensembles");
                cmbEnsemble.DataSource = ds.Tables["ensembles"];
                cmbEnsemble.DisplayMember = "name";
                cmbEnsemble.ValueMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ансамблей: " + ex.Message);
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
                checkedListBoxRecords.Items.Clear();
                foreach (DataRow row in ds.Tables["records"].Rows)
                {
                    checkedListBoxRecords.Items.Add(
                        new RecordItem
                        {
                            Id = Convert.ToInt32(row["id_record"]),
                            Display = row["display"].ToString()
                        },
                        false);
                }
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

        private class RecordItem
        {
            public int Id { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display;
        }

        private void LoadPerformances()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT p.id_performances, " +
                    "c.title as composition_title, " +
                    "e.name as ensemble_name, " +
                    "p.performance_date, " +
                    "p.recording_location, " +
                    "COUNT(rp.id_record) as records_count " +
                    "FROM shem.performances p " +
                    "JOIN shem.compositions c ON p.id_compositions = c.id_compositions " +
                    "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                    "LEFT JOIN shem.record_performances rp ON p.id_performances = rp.id_performances " +
                    "WHERE p.is_deleted = false " +
                    "GROUP BY p.id_performances, c.title, e.name, p.performance_date, p.recording_location " +
                    "ORDER BY c.title", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "performances");
                dataGridViewPerformances.DataSource = ds.Tables["performances"];

                if (dataGridViewPerformances.Columns.Count > 0)
                {
                    dataGridViewPerformances.Columns["composition_title"].HeaderText = "Композиция";
                    dataGridViewPerformances.Columns["ensemble_name"].HeaderText = "Ансамбль";
                    dataGridViewPerformances.Columns["performance_date"].HeaderText = "Дата исполнения";
                    dataGridViewPerformances.Columns["recording_location"].HeaderText = "Место записи";
                    dataGridViewPerformances.Columns["records_count"].HeaderText = "Пластинок";
                }

                dataGridViewPerformances.Columns["id_performances"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки исполнений: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void PerformancesForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();
                    NpgsqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        List<int> recordIds = new List<int>();
                        foreach (RecordItem item in checkedListBoxRecords.CheckedItems)
                        {
                            recordIds.Add(item.Id);
                        }

                        int[] recordIdsArray = recordIds.ToArray();

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.insert_performance(@comp, @ens, @date, @loc, @record_ids)", conn);

                        cmd.Parameters.AddWithValue("@comp", cmbComposition.Text);
                        cmd.Parameters.AddWithValue("@ens", cmbEnsemble.Text);
                        cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = dtpDate.Value.Date;
                        cmd.Parameters.AddWithValue("@loc", txtLocation.Text.Trim());
                        cmd.Parameters.AddWithValue("@record_ids", recordIdsArray);

                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        conn.Close();

                        MessageBox.Show("Исполнение успешно добавлено",
                            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadPerformances();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw new Exception("Ошибка при добавлении исполнения: " + ex.Message);
                    }
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
            if (cmbComposition.SelectedItem == null)
            {
                MessageBox.Show("Выберите композицию", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbEnsemble.SelectedItem == null)
            {
                MessageBox.Show("Выберите ансамбль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLocation.Text.Trim()))
            {
                MessageBox.Show("Введите место записи", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата исполнения не может быть в будущем", "Ошибка",
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewPerformances.CurrentRow != null)
            {
                int performanceId = Convert.ToInt32(dataGridViewPerformances.CurrentRow.Cells["id_performances"].Value);

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить исполнение?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_performance(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", performanceId);

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
                        LoadPerformances();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message);
                        conn.Close();
                    }
                }
            }
        }

        private void buttUpd_Click(object sender, EventArgs e)
        {
            LoadPerformances();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchPerformances(searchTerm);
            }
        }

        private void SearchPerformances(string searchTerm)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT p.id_performances, " +
                    "c.title as composition_title, " +
                    "e.name as ensemble_name, " +
                    "p.performance_date, " +
                    "p.recording_location, " +
                    "COUNT(rp.id_record) as records_count " +
                    "FROM shem.performances p " +
                    "JOIN shem.compositions c ON p.id_compositions = c.id_compositions " +
                    "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                    "LEFT JOIN shem.record_performances rp ON p.id_performances = rp.id_performances " +
                    "WHERE p.is_deleted = false AND " +
                    "c.title ILIKE @search " +
                    "GROUP BY p.id_performances, c.title, e.name, p.performance_date, p.recording_location " +
                    "ORDER BY p.performance_date DESC", conn);

                cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "performances");
                dataGridViewPerformances.DataSource = ds.Tables["performances"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewPerformances.CurrentRow != null)
            {
                int performanceId = Convert.ToInt32(dataGridViewPerformances.CurrentRow.Cells["id_performances"].Value);
                EditPerformanceForm editForm = new EditPerformanceForm(performanceId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadPerformances();
            }
        }
    }
}
