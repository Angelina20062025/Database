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
    public partial class EditMusicianForm : Form
    {
        private NpgsqlConnection conn;
        private int musicianId;
        public EditMusicianForm(int id)
        {
            InitializeComponent();
            musicianId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadMusicianData();
            LoadEnsembles();
            LoadRoles();
        }

        private void LoadMusicianData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT first_name, last_name, patronymic, birth_date " +
                    "FROM shem.musicians WHERE id_musicians = @id", conn);
                cmd.Parameters.AddWithValue("@id", musicianId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtName.Text = reader["first_name"].ToString();
                        txtLastName.Text = reader["last_name"].ToString();
                        txtPatronymic.Text = reader["patronymic"].ToString();
                        dtpBirthDate.Value = Convert.ToDateTime(reader["birth_date"]);
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

        private void LoadEnsembles()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet dsAll = new DataSet();
                NpgsqlCommand cmdAll = new NpgsqlCommand(
                    "SELECT id_ensembles, name FROM shem.ensembles " +
                    "WHERE is_deleted = false ORDER BY name", conn);
                NpgsqlDataAdapter daAll = new NpgsqlDataAdapter(cmdAll);
                daAll.Fill(dsAll, "all_ensembles");

                DataSet dsCurrent = new DataSet();
                NpgsqlCommand cmdCurrent = new NpgsqlCommand(
                    "SELECT id_ensembles FROM shem.ensemble_members " +
                    "WHERE id_musicians = @musician_id", conn);
                cmdCurrent.Parameters.AddWithValue("@musician_id", musicianId);
                NpgsqlDataAdapter daCurrent = new NpgsqlDataAdapter(cmdCurrent);
                daCurrent.Fill(dsCurrent, "current_ensembles");

                HashSet<int> currentEnsembleIds = new HashSet<int>();
                foreach (DataRow row in dsCurrent.Tables["current_ensembles"].Rows)
                {
                    currentEnsembleIds.Add(Convert.ToInt32(row["id_ensembles"]));
                }

                checkedListBoxEnsembles.Items.Clear();
                foreach (DataRow row in dsAll.Tables["all_ensembles"].Rows)
                {
                    int ensembleId = Convert.ToInt32(row["id_ensembles"]);
                    string ensembleName = row["name"].ToString();

                    checkedListBoxEnsembles.Items.Add(
                        new EnsembleItem
                        {
                            Id = ensembleId,
                            Name = ensembleName
                        },
                        currentEnsembleIds.Contains(ensembleId));
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

        private void LoadRoles()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DataSet dsAll = new DataSet();
                NpgsqlCommand cmdAll = new NpgsqlCommand(
                    "SELECT id_musician_roles, name FROM shem.musician_roles " +
                    "ORDER BY name", conn);
                NpgsqlDataAdapter daAll = new NpgsqlDataAdapter(cmdAll);
                daAll.Fill(dsAll, "all_roles");

                DataSet dsCurrent = new DataSet();
                NpgsqlCommand cmdCurrent = new NpgsqlCommand(
                    "SELECT id_musician_roles FROM shem.different_roles_musician " +
                    "WHERE id_musicians = @musician_id", conn);
                cmdCurrent.Parameters.AddWithValue("@musician_id", musicianId);
                NpgsqlDataAdapter daCurrent = new NpgsqlDataAdapter(cmdCurrent);
                daCurrent.Fill(dsCurrent, "current_roles");

                HashSet<int> currentRoleIds = new HashSet<int>();
                foreach (DataRow row in dsCurrent.Tables["current_roles"].Rows)
                {
                    currentRoleIds.Add(Convert.ToInt32(row["id_musician_roles"]));
                }

                checkedListBoxRoles.Items.Clear();
                foreach (DataRow row in dsAll.Tables["all_roles"].Rows)
                {
                    int roleId = Convert.ToInt32(row["id_musician_roles"]);
                    string roleName = row["name"].ToString();

                    checkedListBoxRoles.Items.Add(
                        new RoleItem
                        {
                            Id = roleId,
                            Name = roleName
                        },
                        currentRoleIds.Contains(roleId));
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
            if (ValidateInput())
            {
                try
                {
                    conn.Open();
                    NpgsqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.update_musician(@id, @first, @last, @patr, @birth)", conn);

                        cmd.Parameters.AddWithValue("@id", musicianId);
                        cmd.Parameters.AddWithValue("@first", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@last", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@patr", txtPatronymic.Text.Trim());
                        cmd.Parameters.Add("@birth", NpgsqlDbType.Date).Value = dtpBirthDate.Value.Date;

                        cmd.ExecuteNonQuery();

                        ManageRoles(conn, musicianId);

                        ManageEnsembles(conn, musicianId);

                        transaction.Commit();

                        MessageBox.Show("Данные музыканта обновлены",
                            "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void ManageRoles(NpgsqlConnection conn, int musicianId)
        {
            HashSet<int> currentRoleIds = new HashSet<int>();
            NpgsqlCommand cmdCurrent = new NpgsqlCommand(
                "SELECT id_musician_roles FROM shem.different_roles_musician " +
                "WHERE id_musicians = @musician_id", conn);
            cmdCurrent.Parameters.AddWithValue("@musician_id", musicianId);

            using (NpgsqlDataReader reader = cmdCurrent.ExecuteReader())
            {
                while (reader.Read())
                {
                    currentRoleIds.Add(reader.GetInt32(0));
                }
            }

            HashSet<int> selectedRoleIds = new HashSet<int>();
            foreach (RoleItem role in checkedListBoxRoles.CheckedItems)
            {
                selectedRoleIds.Add(role.Id);

                if (!currentRoleIds.Contains(role.Id))
                {
                    NpgsqlCommand cmdAdd = new NpgsqlCommand(
                        "INSERT INTO shem.different_roles_musician (id_musicians, id_musician_roles) " +
                        "VALUES (@musician_id, @role_id)", conn);
                    cmdAdd.Parameters.AddWithValue("@musician_id", musicianId);
                    cmdAdd.Parameters.AddWithValue("@role_id", role.Id);
                    cmdAdd.ExecuteNonQuery();
                }
            }

            foreach (int roleId in currentRoleIds)
            {
                if (!selectedRoleIds.Contains(roleId))
                {
                    NpgsqlCommand cmdDelete = new NpgsqlCommand(
                        "DELETE FROM shem.different_roles_musician " +
                        "WHERE id_musicians = @musician_id AND id_musician_roles = @role_id", conn);
                    cmdDelete.Parameters.AddWithValue("@musician_id", musicianId);
                    cmdDelete.Parameters.AddWithValue("@role_id", roleId);
                    cmdDelete.ExecuteNonQuery();
                }
            }
        }

        private void ManageEnsembles(NpgsqlConnection conn, int musicianId)
        {
            HashSet<int> currentEnsembleIds = new HashSet<int>();
            NpgsqlCommand cmdCurrent = new NpgsqlCommand(
                "SELECT id_ensembles FROM shem.ensemble_members " +
                "WHERE id_musicians = @musician_id", conn);
            cmdCurrent.Parameters.AddWithValue("@musician_id", musicianId);

            using (NpgsqlDataReader reader = cmdCurrent.ExecuteReader())
            {
                while (reader.Read())
                {
                    currentEnsembleIds.Add(reader.GetInt32(0));
                }
            }

            HashSet<int> selectedEnsembleIds = new HashSet<int>();
            foreach (EnsembleItem ensemble in checkedListBoxEnsembles.CheckedItems)
            {
                selectedEnsembleIds.Add(ensemble.Id);

                if (!currentEnsembleIds.Contains(ensemble.Id))
                {
                    NpgsqlCommand cmdAdd = new NpgsqlCommand(
                        "INSERT INTO shem.ensemble_members (id_ensembles, id_musicians) " +
                        "VALUES (@ensemble_id, @musician_id)", conn);
                    cmdAdd.Parameters.AddWithValue("@ensemble_id", ensemble.Id);
                    cmdAdd.Parameters.AddWithValue("@musician_id", musicianId);
                    cmdAdd.ExecuteNonQuery();
                }
            }

            foreach (int ensembleId in currentEnsembleIds)
            {
                if (!selectedEnsembleIds.Contains(ensembleId))
                {
                    NpgsqlCommand cmdDelete = new NpgsqlCommand(
                        "DELETE FROM shem.ensemble_members " +
                        "WHERE id_musicians = @musician_id AND id_ensembles = @ensemble_id", conn);
                    cmdDelete.Parameters.AddWithValue("@musician_id", musicianId);
                    cmdDelete.Parameters.AddWithValue("@ensemble_id", ensembleId);
                    cmdDelete.ExecuteNonQuery();
                }
            }
        }
    }
}
