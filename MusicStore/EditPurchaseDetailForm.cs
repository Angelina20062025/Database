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
    public partial class EditPurchaseDetailForm : Form
    {
        private NpgsqlConnection conn;
        private int purchaseDetailId;
        private int currentQuantity;
        private int recordId;
        private string recordTitle;
        public EditPurchaseDetailForm(int id)
        {
            InitializeComponent();
            purchaseDetailId = id;
            string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";
            conn = new NpgsqlConnection(connString);
            numNewQuantity.Maximum = 500;
            LoadPurchaseDetailData();
        }

        private void LoadPurchaseDetailData()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(
                    "SELECT pd.quantity, r.id_record, r.title, r.remaining_quantity " +
                    "FROM shem.purchase_details pd " +
                    "JOIN shem.record r ON pd.id_record = r.id_record " +
                    "WHERE pd.id_purchase_details = @id", conn);
                cmd.Parameters.AddWithValue("@id", purchaseDetailId);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentQuantity = Convert.ToInt32(reader["quantity"]);
                        recordId = Convert.ToInt32(reader["id_record"]);
                        recordTitle = reader["title"].ToString();
                        int availableStock = Convert.ToInt32(reader["remaining_quantity"]);

                        txtRecordTitle.Text = recordTitle;
                        txtCurrentQuantity.Text = currentQuantity.ToString();
                        numNewQuantity.Value = currentQuantity;
                        txtAvailableStock.Text = availableStock.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Деталь покупки не найдена", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
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

        private void EditPurchaseDetailForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int newQuantity = (int)numNewQuantity.Value;

            if (ValidateInput(newQuantity))
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        conn.Open();

                        NpgsqlCommand cmd = new NpgsqlCommand("CALL shem.update_purchase_detail(@id, @quantity)", conn);

                        cmd.Parameters.AddWithValue("@id", purchaseDetailId);
                        cmd.Parameters.AddWithValue("@quantity", newQuantity);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Количество обновлено",
                            "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка обновления: " + ex.Message, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                    }
                }
            }
        }

        private bool ValidateInput(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                MessageBox.Show("Количество должно быть положительным числом", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (newQuantity > currentQuantity)
            {
                int additionalQuantity = newQuantity - currentQuantity;
                int availableStock = Convert.ToInt32(txtAvailableStock.Text);

                if (availableStock < additionalQuantity)
                {
                    MessageBox.Show(
                        $"Недостаточно товара на складе.\n" +
                        $"Требуется дополнительно: {additionalQuantity} шт.\n" +
                        $"Доступно: {availableStock} шт.",
                        "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
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
