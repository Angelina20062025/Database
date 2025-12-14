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
using System.Xml.Linq;

namespace MusicStore
{
    public partial class AddEnsembleForm : Form
    {
        private NpgsqlConnection conn;
        public AddEnsembleForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            LoadEnsembleTypes();
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

        private void AddEnsembleForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                "Добавить ансамбль?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();
                        NpgsqlCommand cmd = new NpgsqlCommand(
                            "CALL shem.insert_new_ensemble(@name, @type, @date, @desc)", conn);

                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@type", cmbType.Text);
                        cmd.Parameters.Add("@date", NpgsqlDbType.Date).Value = dtpFoundedDate.Value.Date;
                        cmd.Parameters.AddWithValue("@desc", txtDescription.Text);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Ансамбль добавлен", "Сообщение", MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
