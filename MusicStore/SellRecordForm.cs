using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace MusicStore
{
    public partial class SellRecordForm : Form
    {
        private NpgsqlConnection conn;
        private int employeeId;

        //класс для хранения товаров в корзине
        public class CartItem
        {
            public int RecordId { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int Available { get; set; }
            public decimal Total => Price * Quantity;
        }

        private List<CartItem> cart = new List<CartItem>();
        string connString = "Host=localhost; Database=MusicStore; User Id=postgres; Password=123;";

        public SellRecordForm(int empId)
        {
            InitializeComponent();
            employeeId = empId;
            numQuantity.ReadOnly = true;
            conn = new NpgsqlConnection(connString);
            LoadComboBoxes();
            UpdateCartDisplay();
        }

        private void LoadComboBoxes()
        {
            try
            {
                conn.Open();

                //загрузка пластинок
                DataSet dsRecords = new DataSet();
                NpgsqlCommand cmdRecords = new NpgsqlCommand(
                    "SELECT id_record, title, retail_price, remaining_quantity " +
                    "FROM shem.record WHERE remaining_quantity > 0 ORDER BY title", conn);
                NpgsqlDataAdapter daRecords = new NpgsqlDataAdapter(cmdRecords);
                daRecords.Fill(dsRecords, "records");

                cmbRecord.DataSource = dsRecords.Tables["records"];
                cmbRecord.DisplayMember = "title";
                cmbRecord.ValueMember = "id_record";

                //загрузка клиентов
                DataSet dsCustomers = new DataSet();
                NpgsqlCommand cmdCustomers = new NpgsqlCommand(
                    "SELECT id_customers, first_name || ' ' || last_name as full_name " +
                    "FROM shem.customers ORDER BY last_name", conn);
                NpgsqlDataAdapter daCustomers = new NpgsqlDataAdapter(cmdCustomers);
                daCustomers.Fill(dsCustomers, "customers");

                cmbCustomer.DataSource = dsCustomers.Tables["customers"];
                cmbCustomer.DisplayMember = "full_name";
                cmbCustomer.ValueMember = "id_customers";

                //загрузка способов оплаты
                DataSet dsPayments = new DataSet();
                NpgsqlCommand cmdPayments = new NpgsqlCommand(
                    "SELECT id_payment_methods, name FROM shem.payment_methods ORDER BY name", conn);
                NpgsqlDataAdapter daPayments = new NpgsqlDataAdapter(cmdPayments);
                daPayments.Fill(dsPayments, "payments");

                cmbPaymentMethod.DataSource = dsPayments.Tables["payments"];
                cmbPaymentMethod.DisplayMember = "name";
                cmbPaymentMethod.ValueMember = "id_payment_methods";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void SellRecordForm_Load(object sender, EventArgs e)
        {

        }

        private void cmbRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRecord.SelectedValue != null)
            {
                try
                {
                    DataRowView row = (DataRowView)cmbRecord.SelectedItem;
                    int recordId = Convert.ToInt32(row["id_record"]);

                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT retail_price, remaining_quantity FROM shem.record WHERE id_record = @id", conn);
                    cmd.Parameters.AddWithValue("id", recordId);

                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblPrice.Text = reader["retail_price"].ToString() + " руб.";
                        lblAvailable.Text = reader["remaining_quantity"].ToString() + " шт.";
                        numQuantity.Maximum = Convert.ToDecimal(reader["remaining_quantity"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPaymentMethod.SelectedValue == null)
            {
                MessageBox.Show("Выберите способ оплаты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Совершить продажу?",
                "Подтверждение",
                MessageBoxButtons.YesNo);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    NpgsqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        int customerId = (int)cmbCustomer.SelectedValue;
                        int paymentMethodId = (int)cmbPaymentMethod.SelectedValue;
                        List<int> purchaseIds = new List<int>();

                        NpgsqlCommand cmdPurchase = new NpgsqlCommand(
                        "INSERT INTO shem.purchases (id_customers, id_employees, id_payment_methods, purchase_date) " +
                        "VALUES (@customer_id, @employee_id, @payment_id, CURRENT_DATE) " +
                        "RETURNING id_purchases", conn);

                        cmdPurchase.Parameters.AddWithValue("customer_id", customerId);
                        cmdPurchase.Parameters.AddWithValue("employee_id", employeeId);
                        cmdPurchase.Parameters.AddWithValue("payment_id", paymentMethodId);
                        cmdPurchase.Transaction = transaction;

                        int purchaseId = (int)cmdPurchase.ExecuteScalar();

                        foreach (CartItem item in cart)
                        {
                            NpgsqlCommand cmdCheck = new NpgsqlCommand(
                            "SELECT remaining_quantity FROM shem.record WHERE id_record = @record_id", conn);
                            cmdCheck.Parameters.AddWithValue("record_id", item.RecordId);
                            cmdCheck.Transaction = transaction;

                            int currentStock = (int)cmdCheck.ExecuteScalar();

                            if (currentStock < item.Quantity)
                            {
                                throw new Exception($"Недостаточно пластинок '{item.Title}' в наличии. Доступно: {currentStock}, запрошено: {item.Quantity}");
                            }

                            NpgsqlCommand cmdDetail = new NpgsqlCommand(
                            "INSERT INTO shem.purchase_details (id_purchases, id_record, quantity, unit_price) " +
                            "VALUES (@purchase_id, @record_id, @quantity, @price)", conn);

                            cmdDetail.Parameters.AddWithValue("purchase_id", purchaseId);
                            cmdDetail.Parameters.AddWithValue("record_id", item.RecordId);
                            cmdDetail.Parameters.AddWithValue("quantity", item.Quantity);
                            cmdDetail.Parameters.AddWithValue("price", item.Price);
                            cmdDetail.Transaction = transaction;

                            cmdDetail.ExecuteNonQuery();

                            NpgsqlCommand cmdUpdate = new NpgsqlCommand(
                            "UPDATE shem.record SET remaining_quantity = remaining_quantity - @quantity " +
                            "WHERE id_record = @record_id", conn);

                            cmdUpdate.Parameters.AddWithValue("quantity", item.Quantity);
                            cmdUpdate.Parameters.AddWithValue("record_id", item.RecordId);
                            cmdUpdate.Transaction = transaction;

                            cmdUpdate.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        string receipt = GenerateReceipt(purchaseId, customerId);

                        MessageBox.Show($"Покупка успешно оформлена.\nНомер покупки: {purchaseId}\n\nЧек:\n{receipt}",
                                    "Покупка завершена",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                        cart.Clear();
                        UpdateCartDisplay();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Ошибка при оформлении покупки: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка продажи: " + ex.Message);
                }
            }
        }

        private string GenerateReceipt(int purchaseId, int customerId)
        {
            try
            {
                string receipt = "      МАГАЗИН КОМПАКТ-ДИСКОВ\n";
                receipt += $"Номер покупки: {purchaseId}\n";
                receipt += $"Дата: {DateTime.Now:dd.MM.yyyy HH:mm}\n\n";

                NpgsqlCommand cmdCustomer = new NpgsqlCommand(
                    "SELECT first_name, last_name FROM shem.customers WHERE id_customers = @customer_id", conn);
                cmdCustomer.Parameters.AddWithValue("customer_id", customerId);

                NpgsqlDataReader reader = cmdCustomer.ExecuteReader();
                if (reader.Read())
                {
                    receipt += $"Клиент: {reader["last_name"]} {reader["first_name"]}\n";
                }
                reader.Close();

                //информация о сотруднике
                NpgsqlCommand cmdEmployee = new NpgsqlCommand(
                    "SELECT first_name, last_name FROM shem.employees WHERE id_employees = @employee_id", conn);
                cmdEmployee.Parameters.AddWithValue("employee_id", employeeId);

                reader = cmdEmployee.ExecuteReader();
                if (reader.Read())
                {
                    receipt += $"Продавец: {reader["last_name"]} {reader["first_name"]}\n";
                }
                reader.Close();

                receipt += "ТОВАРЫ:\n";

                decimal totalAmount = 0;

                //товары из покупки
                NpgsqlCommand cmdItems = new NpgsqlCommand(
                    "SELECT pd.quantity, pd.unit_price, r.title " +
                    "FROM shem.purchase_details pd " +
                    "JOIN shem.record r ON pd.id_record = r.id_record " +
                    "WHERE pd.id_purchases = @purchase_id", conn);
                cmdItems.Parameters.AddWithValue("purchase_id", purchaseId);

                reader = cmdItems.ExecuteReader();
                while (reader.Read())
                {
                    int quantity = reader.GetInt32(0);
                    decimal price = reader.GetDecimal(1);
                    string title = reader.GetString(2);
                    decimal itemTotal = quantity * price;

                    receipt += $"{title}\n";
                    receipt += $"  {quantity} x {price:0.00} = {itemTotal:0.00} руб.\n";

                    totalAmount += itemTotal;
                }
                reader.Close();

                //способ оплаты
                NpgsqlCommand cmdPayment = new NpgsqlCommand(
                    "SELECT pm.name FROM shem.purchases p " +
                    "JOIN shem.payment_methods pm ON p.id_payment_methods = pm.id_payment_methods " +
                    "WHERE p.id_purchases = @purchase_id", conn);
                cmdPayment.Parameters.AddWithValue("purchase_id", purchaseId);

                reader = cmdPayment.ExecuteReader();
                if (reader.Read())
                {
                    receipt += $"Способ оплаты: {reader["name"]}\n";
                }
                reader.Close();

                receipt += $"ИТОГО: {totalAmount:0.00} руб.\n";
                receipt += "Спасибо за покупку!\n";

                return receipt;
            }
            catch (Exception)
            {
                return "Не удалось сформировать детальный чек.";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (cart.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "В корзине есть товары. Вы уверены, что хотите отменить покупку?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbRecord.SelectedValue == null)
            {
                MessageBox.Show("Выберите пластинку", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int recordId = (int)cmbRecord.SelectedValue;
                string title = cmbRecord.Text;

                decimal price = 0;
                int available = 0;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(
                        "SELECT retail_price, remaining_quantity FROM shem.record WHERE id_record = @id", conn);
                    cmd.Parameters.AddWithValue("id", recordId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            price = reader.GetDecimal(reader.GetOrdinal("retail_price"));
                            available = reader.GetInt32(reader.GetOrdinal("remaining_quantity"));

                            //метки на форме
                            lblPrice.Text = price.ToString("0.00") + " руб.";
                            lblAvailable.Text = available.ToString() + " шт.";
                            numQuantity.Maximum = available;
                        }
                        else
                        {
                            MessageBox.Show("Пластинка не найдена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                int quantity = (int)numQuantity.Value;

                //проверка, не превышает ли количество доступное
                if (quantity > available)
                {
                    MessageBox.Show($"Недостаточно пластинок в наличии. Доступно: {available} шт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //есть ли уже такая пластинка в корзине
                CartItem existingItem = cart.Find(item => item.RecordId == recordId);
                if (existingItem != null)
                {
                    //увеличивается количество
                    if (existingItem.Quantity + quantity > available)
                    {
                        MessageBox.Show($"Недостаточно пластинок в наличии. Уже в корзине: {existingItem.Quantity} шт., доступно: {available} шт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    existingItem.Quantity += quantity;
                }
                else
                {
                    CartItem newItem = new CartItem
                    {
                        RecordId = recordId,
                        Title = title,
                        Price = price,
                        Quantity = quantity,
                        Available = available
                    };
                    cart.Add(newItem);
                }

                UpdateCartDisplay();

                numQuantity.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления в корзину: " + ex.Message);
            }
        }

        private void UpdateCartDisplay()
        {
            listViewCart.Items.Clear();

            decimal totalAmount = 0;
            foreach (CartItem item in cart)
            {
                ListViewItem listItem = new ListViewItem(item.Title);
                listItem.SubItems.Add(item.Price.ToString("0.00"));
                listItem.SubItems.Add(item.Quantity.ToString());
                listItem.SubItems.Add(item.Total.ToString("0.00"));
                listItem.Tag = item;
                listViewCart.Items.Add(listItem);

                totalAmount += item.Total;
            }

            //обновляем итоговую сумму
            lblTotal.Text = totalAmount.ToString("0.00") + " руб.";

            //активируем/деактивируем кнопки
            btnRemoveFromCart.Enabled = (listViewCart.SelectedItems.Count > 0);
            btnClearCart.Enabled = (cart.Count > 0);
            btnSell.Enabled = (cmbCustomer.SelectedValue != null);
            btnSell.Enabled = (cart.Count > 0);
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (listViewCart.SelectedItems.Count > 0)
            {
                CartItem item = (CartItem)listViewCart.SelectedItems[0].Tag;
                cart.Remove(item);
                UpdateCartDisplay();
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Очистить всю корзину?",
                "Подтверждение",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                cart.Clear();
                UpdateCartDisplay();
            }
        }

        private void listViewCart_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemoveFromCart.Enabled = (listViewCart.SelectedItems.Count > 0);
        }
    }
}
