using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class RecordDetailsForm : Form
    {
        private string connectionString;
        private int recordId;
        public RecordDetailsForm(int id)
        {
            InitializeComponent();
            connectionString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            recordId = id;
            this.WindowState = FormWindowState.Maximized;
            txtDescription.ReadOnly = true;
            txtDescription.BackColor = Color.White;
            txtDescription.Enter += (s, e) => { txtDescription.Select(0, 0); };
            dataGridViewEnsembles.ReadOnly = true;
            dataGridViewMembers.ReadOnly = true;
            dataGridViewCompositions.ReadOnly = true;
            dataGridViewPerformances.ReadOnly = true;
            LoadRecordDetails();
            //dataGridViewMembers.Columns[dataGridViewMembers.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void LoadRecordDetails()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    LoadMainInfo(conn);

                    LoadCoverImage(conn);

                    LoadPerformances(conn);

                    LoadEnsembles(conn);

                    LoadCompositions(conn);

                    LoadEnsembleMembers(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void LoadCoverImage(NpgsqlConnection conn)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT cover_image FROM shem.record WHERE id_record = @id AND cover_image IS NOT NULL", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                byte[] imageData = (byte[])result;
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    picCover.Image = Image.FromStream(ms);
                }
            }
            else
            {
                picCover.Image = SystemIcons.Warning.ToBitmap();
                picCover.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }

        private void LoadMainInfo(NpgsqlConnection conn)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT description " +
                "FROM shem.record WHERE id_record = @id", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    txtDescription.Text = reader["description"].ToString();
                }
            }
        }

        private void LoadPerformances(NpgsqlConnection conn)
        {
            DataSet ds = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT c.title as composition_name, p.performance_date, p.recording_location, " +
                "e.name as ensemble_name " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                "JOIN shem.compositions c ON p.id_compositions = c.id_compositions " +
                "WHERE rp.id_record = @id " +
                "ORDER BY c.title", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "performances");

            dataGridViewPerformances.DataSource = ds.Tables["performances"];

            if (ds.Tables["performances"].Columns.Count > 0)
            {
                ds.Tables["performances"].Columns["performance_date"].ColumnName = "Дата записи";
                ds.Tables["performances"].Columns["recording_location"].ColumnName = "Место записи";
                ds.Tables["performances"].Columns["ensemble_name"].ColumnName = "Ансамбль";
                ds.Tables["performances"].Columns["composition_name"].ColumnName = "Композиция";
            }
        }

        private void LoadEnsembles(NpgsqlConnection conn)
        {
            DataSet ds = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT DISTINCT e.name as Название, " +
                "et.name as Тип, " +
                "e.founded_date as Основан " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                "JOIN shem.ensemble_types et ON e.id_ensemble_types = et.id_ensemble_types " +
                "WHERE rp.id_record = @id AND e.is_deleted = false " +
                "ORDER BY e.name", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "ensembles");
            dataGridViewEnsembles.DataSource = ds.Tables["ensembles"];
        }

        private void LoadCompositions(NpgsqlConnection conn)
        {
            DataSet ds = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT c.title as Название, " +
                "g.name as Жанр, " +
                "CONCAT(FLOOR(c.duration_seconds / 60), ' мин. ', c.duration_seconds % 60, ' сек.'  ) as Длительность, " +
                "c.year_created as Год " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.compositions c ON p.id_compositions = c.id_compositions " +
                "JOIN shem.genres g ON c.id_genres = g.id_genres " +
                "WHERE rp.id_record = @id " +
                "ORDER BY c.title", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "compositions");
            dataGridViewCompositions.DataSource = ds.Tables["compositions"];
        }

        private void LoadEnsembleMembers(NpgsqlConnection conn)
        {
            DataSet ds = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand(@"
    SELECT DISTINCT 
        e.name as Ансамбль,
        m.first_name || ' ' || m.last_name as Музыкант,
        (
            SELECT STRING_AGG(DISTINCT mr2.name, ', ' ORDER BY mr2.name)
            FROM shem.different_roles_musician drm2
            JOIN shem.musician_roles mr2 ON drm2.id_musician_roles = mr2.id_musician_roles
            WHERE drm2.id_musicians = m.id_musicians
        ) as Роли
    FROM shem.record_performances rp 
    JOIN shem.performances p ON rp.id_performances = p.id_performances 
    JOIN shem.ensemble_members em ON p.id_ensembles = em.id_ensembles 
    JOIN shem.musicians m ON em.id_musicians = m.id_musicians 
    JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles 
    WHERE rp.id_record = @id AND m.is_deleted = false
    ORDER BY e.name", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "members");
            dataGridViewMembers.DataSource = ds.Tables["members"];
        }

        private void RecordDetailsForm_Load(object sender, EventArgs e)
        {

        }

        private void dataGridViewPerformances_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
