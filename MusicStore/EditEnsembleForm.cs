using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicStore
{
    public partial class EditEnsembleForm : Form
    {
        private NpgsqlConnection conn;
        private int ensId;
        public EditEnsembleForm(int id)
        {
            InitializeComponent();
            ensId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadEnsembleTypes();
            LoadData();
        }

        private void LoadEnsembleTypes()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                "SELECT name FROM shem.ensemble_types ORDER BY name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "types");
                cmbType.DataSource = ds.Tables["types"];
                cmbType.DisplayMember = "name";
                cmbType.ValueMember = "name";
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT e.id_ensembles, e.name, e.id_ensemble_types, e.founded_date, e.description, et.name as type_name " +
                    "FROM shem.ensembles e " +
                    "JOIN shem.ensemble_types et ON e.id_ensemble_types = et.id_ensemble_types " +
                    "WHERE e.id_ensembles = @id", conn);
                cmd.Parameters.AddWithValue("@id", ensId);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["name"].ToString();
                    string currentType = reader["type_name"].ToString();
                    for (int i = 0; i < cmbType.Items.Count; i++)
                    {
                        DataRowView row = (DataRowView)cmbType.Items[i];
                        if (row["name"].ToString() == currentType)
                        {
                            cmbType.SelectedIndex = i;
                            break;
                        }
                    }
                    dtpFoundedDate.Value = Convert.ToDateTime(reader["founded_date"]);
                    txtDescription.Text = reader["description"].ToString();
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void EditEnsembleForm_Load(object sender, EventArgs e)
        {

        }

        private void btnupd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                "Обновить данные?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.update_ensemble_info(@p_id_ensembles, @p_name, @p_ensemble_type_name, @p_founded_date, @p_description)",
                            conn);
                        
                        cmd.Parameters.AddWithValue("@p_id_ensembles", ensId);
                        cmd.Parameters.AddWithValue("@p_name", txtName.Text);
                        cmd.Parameters.AddWithValue("@p_ensemble_type_name", ((DataRowView)cmbType.SelectedItem)["name"].ToString());
                        cmd.Parameters.Add("@p_founded_date", NpgsqlDbType.Date).Value = dtpFoundedDate.Value.Date;
                        cmd.Parameters.AddWithValue("@p_description", txtDescription.Text);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Данные успешно обновлены", "Сообщение", MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка обновления: " + ex.Message);
                        conn.Close();
                    }
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpFoundedDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата не может быть дальше сегодняшней", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFoundedDate.Focus();
                return false;
            }

            if (cmbType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип ансамбля", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbType.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
