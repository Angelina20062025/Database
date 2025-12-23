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
    public partial class EditPerformanceForm : Form
    {
        private NpgsqlConnection conn;
        private int performanceId;
        public EditPerformanceForm(int id)
        {
            InitializeComponent();
            performanceId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadCompositions();
            LoadEnsembles();
            LoadRecords();
            LoadPerformanceData();
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

                DataSet dsAll = new DataSet();
                NpgsqlCommand cmdAll = new NpgsqlCommand(
                    "SELECT id_record, CONCAT(title, ' (', catalog_number, ')') as display " +
                    "FROM shem.record " +
                    "WHERE is_deleted = false " +
                    "ORDER BY title", conn);
                NpgsqlDataAdapter daAll = new NpgsqlDataAdapter(cmdAll);
                daAll.Fill(dsAll, "all_records");

                DataSet dsCurrent = new DataSet();
                NpgsqlCommand cmdCurrent = new NpgsqlCommand(
                    "SELECT id_record FROM shem.record_performances " +
                    "WHERE id_performances = @performance_id", conn);
                cmdCurrent.Parameters.AddWithValue("@performance_id", performanceId);
                NpgsqlDataAdapter daCurrent = new NpgsqlDataAdapter(cmdCurrent);
                daCurrent.Fill(dsCurrent, "current_records");

                HashSet<int> currentRecordIds = new HashSet<int>();
                foreach (DataRow row in dsCurrent.Tables["current_records"].Rows)
                {
                    currentRecordIds.Add(Convert.ToInt32(row["id_record"]));
                }

                checkedListBoxRecords.Items.Clear();
                foreach (DataRow row in dsAll.Tables["all_records"].Rows)
                {
                    int recordId = Convert.ToInt32(row["id_record"]);
                    string display = row["display"].ToString();

                    checkedListBoxRecords.Items.Add(
                        new RecordItem
                        {
                            Id = recordId,
                            Display = display
                        },
                        currentRecordIds.Contains(recordId));
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

        private void LoadPerformanceData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT c.title as composition_title, e.name as ensemble_name, " +
                    "p.performance_date, p.recording_location " +
                    "FROM shem.performances p " +
                    "JOIN shem.compositions c ON p.id_compositions = c.id_compositions " +
                    "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                    "WHERE p.id_performances = @id", conn);
                cmd.Parameters.AddWithValue("@id", performanceId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string currentComposition = reader["composition_title"].ToString();
                        for (int i = 0; i < cmbComposition.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbComposition.Items[i];
                            if (row["title"].ToString() == currentComposition)
                            {
                                cmbComposition.SelectedIndex = i;
                                break;
                            }
                        }

                        string currentEnsemble = reader["ensemble_name"].ToString();
                        for (int i = 0; i < cmbEnsemble.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbEnsemble.Items[i];
                            if (row["name"].ToString() == currentEnsemble)
                            {
                                cmbEnsemble.SelectedIndex = i;
                                break;
                            }
                        }

                        dtpDate.Value = Convert.ToDateTime(reader["performance_date"]);
                        txtLocation.Text = reader["recording_location"].ToString();
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
                                "CALL shem.update_performance(@id, @comp, @ens, @date, @loc, @record_ids)", conn);

                            cmd.Parameters.AddWithValue("@id", performanceId);
                            cmd.Parameters.AddWithValue("@comp", cmbComposition.Text);
                            cmd.Parameters.AddWithValue("@ens", cmbEnsemble.Text);
                            cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = dtpDate.Value.Date;
                            cmd.Parameters.AddWithValue("@loc", txtLocation.Text.Trim());
                            cmd.Parameters.AddWithValue("@record_ids",
                                recordIdsArray.Length > 0 ? (object)recordIdsArray : DBNull.Value);

                            cmd.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Исполнение успешно обновлено",
                                "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Ошибка при обновлении: " + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
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
    }
}
