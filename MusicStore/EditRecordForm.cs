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
    public partial class EditRecordForm : Form
    {
        private NpgsqlConnection conn;
        private int recordId;
        public EditRecordForm(int id)
        {
            InitializeComponent();
            recordId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numRetailPrice.Maximum = 9999;
            numWholesalePrice.Maximum = 9999;
            LoadRecordData();
        }

        private void LoadRecordData()
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT * FROM shem.record WHERE id_record = @id", conn);
                cmd.Parameters.AddWithValue("id", recordId);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtCatalogNumber.Text = reader["catalog_number"].ToString();
                    txtTitle.Text = reader["title"].ToString();
                    dateReleaseDate.Value = Convert.ToDateTime(reader["release_date"]);
                    numWholesalePrice.Value = Convert.ToDecimal(reader["wholesale_price"]);
                    numRetailPrice.Value = Convert.ToDecimal(reader["retail_price"]);
                    numQuantity.Value = Convert.ToDecimal(reader["remaining_quantity"]);
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

        private void EditRecordForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
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

                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.update_cd_info(@p_id_record, @p_catalog_number, @p_title, @p_release_date, @p_wholesale_price, @p_retail_price, @p_remaining_quantity, @p_description)", conn);

                        cmd.Parameters.AddWithValue("@p_id_record", recordId);
                        cmd.Parameters.AddWithValue("@p_catalog_number", txtCatalogNumber.Text);
                        cmd.Parameters.AddWithValue("@p_title", txtTitle.Text);
                        cmd.Parameters.Add("@p_release_date", NpgsqlDbType.Date).Value = dateReleaseDate.Value.Date;
                        cmd.Parameters.AddWithValue("@p_wholesale_price", numWholesalePrice.Value);
                        cmd.Parameters.AddWithValue("@p_retail_price", numRetailPrice.Value);
                        cmd.Parameters.AddWithValue("@p_remaining_quantity", (int)numQuantity.Value);
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
            if (string.IsNullOrEmpty(txtCatalogNumber.Text))
            {
                MessageBox.Show("Введите название каталога", "Ошибка", MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("Введите название пластинки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numRetailPrice.Value < numWholesalePrice.Value)
            {
                MessageBox.Show("Розничная цена не может быть меньше оптовой", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dateReleaseDate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата не может быть дальше сегодняшней", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateReleaseDate.Focus();
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
