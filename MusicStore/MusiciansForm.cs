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
    public partial class MusiciansForm : Form
    {
        private NpgsqlConnection conn;
        private List<int> selectedEnsembleIds = new List<int>();
        public MusiciansForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadMusicians();
            LoadEnsembles();
            LoadMusicianRoles();
        }

        private void LoadMusicians()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_musicians, first_name as Имя, last_name as Фамилия, patronymic as Отчество, " +
                    "birth_date as \"Дата_рождения\"" +
                    "FROM shem.musicians WHERE is_deleted = false ORDER BY first_name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "musicians");
                foreach (DataColumn column in ds.Tables["musicians"].Columns)
                {
                    column.ColumnName = column.ColumnName.Replace("_", " ");
                }
                dataGridView1.DataSource = ds.Tables["musicians"];
                dataGridView1.Columns["id musicians"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void LoadEnsembles()
        {
            try
            {
                conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_ensembles, name " +
                    "FROM shem.ensembles " +
                    "WHERE is_deleted = false " +
                    "ORDER BY name", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "ensembles");

                checkedListBoxEnsembles.Items.Clear();
                foreach (DataRow row in ds.Tables["ensembles"].Rows)
                {
                    checkedListBoxEnsembles.Items.Add(
                        new EnsembleItem
                        {
                            Id = Convert.ToInt32(row["id_ensembles"]),
                            Name = row["name"].ToString()
                        },
                        false);
                }
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

        private void LoadMusicianRoles()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT id_musician_roles, name " +
                    "FROM shem.musician_roles " +
                    "ORDER BY name", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "roles");

                checkedListBoxRoles.Items.Clear();
                foreach (DataRow row in ds.Tables["roles"].Rows)
                {
                    checkedListBoxRoles.Items.Add(
                        new RoleItem
                        {
                            Id = Convert.ToInt32(row["id_musician_roles"]),
                            Name = row["name"].ToString()
                        },
                        false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки ролей: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private class EnsembleItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        private class RoleItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        private void MusiciansForm_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
                        NpgsqlCommand cmd = new NpgsqlCommand("SELECT shem.insert_musician(@first, @last, @patr, @birth)", conn);

                        cmd.Parameters.AddWithValue("@first", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@last", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@patr", txtPatronymic.Text.Trim());
                        cmd.Parameters.Add("@birth", NpgsqlDbType.Date).Value = dtpBirthDate.Value.Date;

                        int musicianId = (int)cmd.ExecuteScalar();

                        foreach (RoleItem role in checkedListBoxRoles.CheckedItems)
                        {
                            NpgsqlCommand roleCmd = new NpgsqlCommand(
                                "INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles) " +
                                "VALUES (@musician_id, @role_id)", conn);

                            roleCmd.Parameters.AddWithValue("@musician_id", musicianId);
                            roleCmd.Parameters.AddWithValue("@role_id", role.Id);
                            roleCmd.ExecuteNonQuery();
                        }

                        foreach (EnsembleItem ensemble in checkedListBoxEnsembles.CheckedItems)
                        {
                            NpgsqlCommand ensembleCmd = new NpgsqlCommand(
                                "INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) " +
                                "VALUES (@ensemble_id, @musician_id)", conn);

                            ensembleCmd.Parameters.AddWithValue("@ensemble_id", ensemble.Id);
                            ensembleCmd.Parameters.AddWithValue("@musician_id", musicianId);
                            ensembleCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        conn.Close();
                        MessageBox.Show($"Музыкант добавлен успешно",
                            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadMusicians();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Ошибка при добавлении музыканта: " + ex.Message);
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
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Введите имя музыканта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                MessageBox.Show("Введите фамилию музыканта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpBirthDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата рождения не может дальше сегодняшней", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int musId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id musicians"].Value);
                EditMusicianForm editForm = new EditMusicianForm(musId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadMusicians();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int musId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id musicians"].Value);

                DialogResult result = MessageBox.Show(
                    "Вы уверены, что хотите удалить музыканта?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "SELECT * FROM shem.soft_delete_musician(@id)", conn);
                        cmd.Parameters.AddWithValue("id", musId);

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
                        LoadMusicians();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка архивации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }
    }
}
