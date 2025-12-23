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
    public partial class CompositionsForm : Form
    {
        private NpgsqlConnection conn;
        public CompositionsForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numYear.Maximum = 3000;
            numDuration.Maximum = 1000;
            LoadCompositions();
            LoadGenres();
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

        private void LoadCompositions()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT c.id_compositions, c.title, g.name as genre, " +
                    "CONCAT(FLOOR(c.duration_seconds / 60), ' мин. ', c.duration_seconds % 60, ' сек.'  ) as Длительность, c.year_created " +
                    "FROM shem.compositions c " +
                    "JOIN shem.genres g ON c.id_genres = g.id_genres " +
                    "WHERE c.is_deleted = false " +
                    "ORDER BY c.title", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "compositions");
                dataGridView1.DataSource = ds.Tables["compositions"];

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["title"].HeaderText = "Название";
                    dataGridView1.Columns["genre"].HeaderText = "Жанр";
                    dataGridView1.Columns["year_created"].HeaderText = "Год создания";
                }

                dataGridView1.Columns["id_compositions"].Visible = false;
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

        private void CompositionsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "CALL shem.insert_composition(@title, @genre, @duration, @year)", conn);

                    cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", cmbGenre.Text);
                    cmd.Parameters.AddWithValue("@duration", (int)numDuration.Value);
                    cmd.Parameters.AddWithValue("@year", (int)numYear.Value);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Композиция добавлена успешно",
                        "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadCompositions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка добавления: " + ex.Message,
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int compositionId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_compositions"].Value);
                EditCompositionForm editForm = new EditCompositionForm(compositionId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadCompositions();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                SearchCompositions(searchTerm);
            }
        }

        private void SearchCompositions(string searchTerm)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT c.id_compositions, c.title, g.name as genre, " +
                    "CONCAT(FLOOR(c.duration_seconds / 60), ' мин. ', c.duration_seconds % 60, ' сек.'  ) as Длительность, c.year_created " +
                    "FROM shem.compositions c " +
                    "JOIN shem.genres g ON c.id_genres = g.id_genres " +
                    "WHERE c.is_deleted = false AND " +
                    "(c.title ILIKE @search) " +
                    "ORDER BY c.title", conn);

                cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "compositions");
                dataGridView1.DataSource = ds.Tables["compositions"];
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

        private void buttUpd_Click(object sender, EventArgs e)
        {
            LoadCompositions();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int compositionId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id_compositions"].Value);

                DialogResult result = MessageBox.Show(
                    $"Вы уверены, что хотите удалить композицию?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_composition(@id)", conn);
                        cmd.Parameters.AddWithValue("@id", compositionId);

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
                                    conn.Close();
                                    return;
                                }
                            }
                        }
                        conn.Close();
                        LoadCompositions();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message);
                        conn.Close();
                    }
                }
            }
        }
    }
}
