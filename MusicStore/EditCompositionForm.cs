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
    public partial class EditCompositionForm : Form
    {
        private NpgsqlConnection conn;
        private int compositionId;
        public EditCompositionForm(int id)
        {
            InitializeComponent();
            compositionId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numYear.Maximum = 3000;
            numDuration.Maximum = 1000;
            LoadGenres();
            LoadCompositionData();
        }

        private void LoadGenres()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT name FROM shem.genres ORDER BY name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "genres");
                cmbGenre.DataSource = ds.Tables["genres"];
                cmbGenre.DisplayMember = "name";
                cmbGenre.ValueMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки жанров: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void LoadCompositionData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT c.title, g.name as genre_name, c.duration_seconds, " +
                    "c.year_created " +
                    "FROM shem.compositions c " +
                    "JOIN shem.genres g ON c.id_genres = g.id_genres " +
                    "WHERE c.id_compositions = @id", conn);
                cmd.Parameters.AddWithValue("@id", compositionId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtTitle.Text = reader["title"].ToString();

                        string currentGenre = reader["genre_name"].ToString();
                        for (int i = 0; i < cmbGenre.Items.Count; i++)
                        {
                            DataRowView row = (DataRowView)cmbGenre.Items[i];
                            if (row["name"].ToString() == currentGenre)
                            {
                                cmbGenre.SelectedIndex = i;
                                break;
                            }
                        }

                        numDuration.Value = Convert.ToDecimal(reader["duration_seconds"]);
                        numYear.Value = Convert.ToDecimal(reader["year_created"]);
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
                try
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "CALL shem.update_composition(@id, @title, @genre, @duration, @year)", conn);

                    cmd.Parameters.AddWithValue("@id", compositionId);
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", cmbGenre.Text);
                    cmd.Parameters.AddWithValue("@duration", (int)numDuration.Value);
                    cmd.Parameters.AddWithValue("@year", (int)numYear.Value);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Композиция обновлена успешно",
                        "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления: " + ex.Message,
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                MessageBox.Show("Введите название композиции", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numDuration.Value <= 0)
            {
                MessageBox.Show("Длительность должна быть положительным числом", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int currentYear = DateTime.Now.Year;
            if (numYear.Value < 1300 || numYear.Value > currentYear)
            {
                MessageBox.Show("Некорректный год создания", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numYear.Focus();
                return false;
            }

            return true;
        }
    }
}
