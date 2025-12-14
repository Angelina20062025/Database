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
    public partial class EnsemblesManagementForm : Form
    {
        private NpgsqlConnection conn;
        public EnsemblesManagementForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadEnsembles();
        }

        private void LoadEnsembles()
        {
            try
            {
                conn.Open();
                DataSet ds = new DataSet();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT e.id_ensembles, e.name as \"Название_ансамбля\", et.name as \"Тип\", " +
                    "e.founded_date as \"Дата_основания\", e.description as \"Описание\"" +
                    "FROM shem.ensembles e " +
                    "JOIN shem.ensemble_types et ON e.id_ensemble_types = et.id_ensemble_types " +
                    "ORDER BY e.name", conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(ds, "ensembles");
                foreach (DataColumn column in ds.Tables["ensembles"].Columns)
                {
                    column.ColumnName = column.ColumnName.Replace("_", " ");
                }
                dataGridView1.DataSource = ds.Tables["ensembles"];
                dataGridView1.Columns["id ensembles"].Visible = false;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки: " + ex.Message);
            }
        }

        private void EnsemblesManagementForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEnsembleForm addForm = new AddEnsembleForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            LoadEnsembles();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int ensembleId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id ensembles"].Value);
                EditEnsembleForm editForm = new EditEnsembleForm(ensembleId);
                if (editForm.ShowDialog() == DialogResult.OK)
                LoadEnsembles();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int ensembleId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id ensembles"].Value);

                DialogResult result = MessageBox.Show(
                    "Удалить ансамбль?",
                    "Подтверждение", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "DELETE FROM shem.ensembles WHERE id_ensembles = @id", conn);
                        cmd.Parameters.AddWithValue("id", ensembleId);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Ансамбль удален", "Сообщение", MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        LoadEnsembles();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                        conn.Close();
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
