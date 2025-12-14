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
            LoadRecordDetails();
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
                "SELECT DISTINCT p.performance_date, p.recording_location, " +
                "e.name as ensemble_name " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                "WHERE rp.id_record = @id " +
                "ORDER BY p.performance_date", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "performances");

            dataGridViewPerformances.DataSource = ds.Tables["performances"];

            if (ds.Tables["performances"].Columns.Count > 0)
            {
                ds.Tables["performances"].Columns["performance_date"].ColumnName = "Дата записи";
                ds.Tables["performances"].Columns["recording_location"].ColumnName = "Место записи";
                ds.Tables["performances"].Columns["ensemble_name"].ColumnName = "Ансамбль";
            }
        }

        private void LoadEnsembles(NpgsqlConnection conn)
        {
            DataSet ds = new DataSet();
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT DISTINCT e.name as Название, " +
                "et.name as Тип, " +
                "e.founded_date as Основан, " +
                "e.description as Описание " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                "JOIN shem.ensemble_types et ON e.id_ensemble_types = et.id_ensemble_types " +
                "WHERE rp.id_record = @id " +
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
                "c.duration_seconds as Длительность, " +
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
            NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT e.name as Ансамбль, " +
                "m.first_name || ' ' || m.last_name as Музыкант, " +
                "STRING_AGG(mr.name, ', ') as Роли " +
                "FROM shem.record_performances rp " +
                "JOIN shem.performances p ON rp.id_performances = p.id_performances " +
                "JOIN shem.ensemble_members em ON p.id_ensembles = em.id_ensembles " +
                "JOIN shem.musicians m ON em.id_musicians = m.id_musicians " +
                "JOIN shem.ensembles e ON p.id_ensembles = e.id_ensembles " +
                "JOIN shem.different_roles_musician drm ON m.id_musicians = drm.id_musicians " +
                "JOIN shem.musician_roles mr ON drm.id_musician_roles = mr.id_musician_roles " +
                "WHERE rp.id_record = @id " +
                "GROUP BY e.name, m.first_name, m.last_name, m.id_musicians " +
                "ORDER BY e.name, m.last_name, m.first_name", conn);
            cmd.Parameters.AddWithValue("id", recordId);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            da.Fill(ds, "members");
            dataGridViewMembers.DataSource = ds.Tables["members"];
        }

        private void RecordDetailsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
