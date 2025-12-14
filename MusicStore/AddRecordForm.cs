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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace MusicStore
{
    public partial class AddRecordForm : Form
    {
        private NpgsqlConnection conn;
        public AddRecordForm()
        {
            InitializeComponent();
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numRetailPrice.Maximum = 9999;
            numWholesalePrice.Maximum = 9999;
        }

        private void AddRecordForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult result = MessageBox.Show(
                "Добавить пластинку?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        string callCommand = "CALL shem.insert_new_cd(@p_catalog_number, @p_title, @p_release_date, @p_wholesale_price, @p_retail_price, @p_remaining_quantity, @p_description)";

                        NpgsqlCommand cmd = new NpgsqlCommand(callCommand, conn);

                        cmd.Parameters.AddWithValue("@p_catalog_number", txtCatalogNumber.Text);
                        cmd.Parameters.AddWithValue("@p_title", txtTitle.Text);
                        cmd.Parameters.Add("@p_release_date", NpgsqlDbType.Date).Value = dateReleaseDate.Value.Date;
                        cmd.Parameters.AddWithValue("@p_wholesale_price", numWholesalePrice.Value);
                        cmd.Parameters.AddWithValue("@p_retail_price", numRetailPrice.Value);
                        cmd.Parameters.AddWithValue("@p_remaining_quantity", (int)numQuantity.Value);
                        cmd.Parameters.AddWithValue("@p_description", txtDescription.Text);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Пластинка успешно добавлена", "Сообщение", MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка добавления", MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
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
