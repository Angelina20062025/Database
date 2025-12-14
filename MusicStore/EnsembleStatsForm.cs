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
    public partial class EnsembleStatsForm : Form
    {
        private string connectionString;
        public EnsembleStatsForm()
        {
            InitializeComponent();
            connectionString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            LoadEnsembles();
        }

        private void LoadEnsembles()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    DataSet ds = new DataSet();
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT name FROM shem.ensembles ORDER BY name", conn);
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(ds, "ensembles");

                    cmbEnsemble.DataSource = ds.Tables["ensembles"];
                    cmbEnsemble.DisplayMember = "name";
                    cmbEnsemble.ValueMember = "name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ансамблей: " + ex.Message);
            }
        }

        private void LoadEnsembleStats(string ensembleName)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Используем функцию get_ensemble_compositions_count
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT * FROM shem.get_ensemble_compositions_count(@ensemble)", conn);
                    cmd.Parameters.AddWithValue("ensemble", ensembleName);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lbCompositionsCount.Text = reader["compositions_count"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки статистики: " + ex.Message);
            }
        }

        private void LoadEnsembleCDs(string ensembleName)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Используем функцию get_ensemble_cds
                    DataSet ds = new DataSet();
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT * FROM shem.get_ensemble_cds(@ensemble)", conn);
                    cmd.Parameters.AddWithValue("ensemble", ensembleName);

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(ds, "cds");

                    // Проверяем, не вернулось ли сообщение об ошибке
                    if (ds.Tables["cds"].Rows.Count == 1)
                    {
                        string firstRow = ds.Tables["cds"].Rows[0]["cd_title"].ToString();

                        if (firstRow.Contains("не найден") || firstRow.Contains("нет дисков"))
                        {
                            lblCDCount.Text = "0 дисков";
                            dataGridViewCD.DataSource = null;
                            return;
                        }
                    }

                    // Переименовываем колонки
                    if (ds.Tables["cds"].Columns.Count > 0)
                    {
                        ds.Tables["cds"].Columns["cd_title"].ColumnName = "Название диска";
                        ds.Tables["cds"].Columns["catalog_number"].ColumnName = "Каталожный номер";
                        ds.Tables["cds"].Columns["release_date"].ColumnName = "Дата выпуска";
                    }

                    dataGridViewCD.DataSource = ds.Tables["cds"];
                    lblCDCount.Text = ds.Tables["cds"].Rows.Count + "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки дисков: " + ex.Message);
            }
        }

        private void LoadEnsembleDetails(string ensembleName)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Дополнительная информация об ансамбле
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT e.name, et.name as type, e.founded_date, e.description, " +
                        "COUNT(DISTINCT em.id_musicians) as musicians_count " +
                        "FROM shem.ensembles e " +
                        "JOIN shem.ensemble_types et ON e.id_ensemble_types = et.id_ensemble_types " +
                        "LEFT JOIN shem.ensemble_members em ON e.id_ensembles = em.id_ensembles " +
                        "WHERE e.name = @ensemble " +
                        "GROUP BY e.id_ensembles, e.name, et.name, e.founded_date, e.description", conn);
                    cmd.Parameters.AddWithValue("ensemble", ensembleName);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblEnsembleType.Text = reader["type"].ToString();
                            lblFoundedDate.Text = Convert.ToDateTime(reader["founded_date"]).ToString("dd.MM.yyyy");
                            lblMusiciansCount.Text = reader["musicians_count"].ToString();
                            txtEnsembleDescription.Text = reader["description"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки деталей: " + ex.Message);
            }
        }

        private void SearchCDsByTitle(string searchTerm)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    DataSet ds = new DataSet();
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT DISTINCT r.title as Название, " +
                        "r.catalog_number as КаталожныйНомер, " +
                        "r.release_date as Датавыпуска, " +
                        "e.name as Ансамбль, " +
                        "r.retail_price as Цена " +
                        "FROM shem.record r " +
                        "JOIN shem.record_performances rp ON r.id_record = rp.id_record " +
                        "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                        "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                        "WHERE e.name = @ensemble AND r.title ILIKE @search " +
                        "ORDER BY r.title", conn);

                    cmd.Parameters.AddWithValue("ensemble", cmbEnsemble.Text);
                    cmd.Parameters.AddWithValue("search", "%" + searchTerm + "%");

                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(ds, "search_results");

                    dataGridViewCD.DataSource = ds.Tables["search_results"];
                    lblCDCount.Text = ds.Tables["search_results"].Rows.Count + " найденных дисков";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

        private void EnsembleStatsForm_Load(object sender, EventArgs e)
        {

        }

        private void cmbEnsemble_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEnsemble.SelectedValue != null)
            {
                string ensembleName = cmbEnsemble.Text;
                LoadEnsembleStats(ensembleName);
                LoadEnsembleCDs(ensembleName);
                LoadEnsembleDetails(ensembleName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchCDs_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchCD.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchCDsByTitle(searchTerm);
            }
        }
    }
}
