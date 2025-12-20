namespace MusicStore
{
    partial class SellRecordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbRecord = new System.Windows.Forms.ComboBox();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.btnRemoveFromCart = new System.Windows.Forms.Button();
            this.btnClearCart = new System.Windows.Forms.Button();
            this.listViewCart = new System.Windows.Forms.ListView();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbRecord
            // 
            this.cmbRecord.FormattingEnabled = true;
            this.cmbRecord.Location = new System.Drawing.Point(35, 83);
            this.cmbRecord.Name = "cmbRecord";
            this.cmbRecord.Size = new System.Drawing.Size(191, 24);
            this.cmbRecord.TabIndex = 0;
            this.cmbRecord.SelectedIndexChanged += new System.EventHandler(this.cmbRecord_SelectedIndexChanged);
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(505, 84);
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(120, 22);
            this.numQuantity.TabIndex = 1;
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(356, 428);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(105, 50);
            this.btnSell.TabIndex = 2;
            this.btnSell.Text = "Продать";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(31, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 50);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Выйти";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(271, 83);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(191, 24);
            this.cmbCustomer.TabIndex = 4;
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Location = new System.Drawing.Point(670, 82);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(191, 24);
            this.cmbPaymentMethod.TabIndex = 5;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrice.Location = new System.Drawing.Point(35, 354);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(0, 20);
            this.lblPrice.TabIndex = 6;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAvailable.Location = new System.Drawing.Point(168, 354);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(0, 20);
            this.lblAvailable.TabIndex = 7;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotal.Location = new System.Drawing.Point(392, 354);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 20);
            this.lblTotal.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(32, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Название диска";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(267, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Покупатель";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(501, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Количество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(666, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Способ оплаты";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(35, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Цена (шт.)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(168, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(198, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Осталось в наличии";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(392, 323);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Итоговая сумма";
            // 
            // btnAddToCart
            // 
            this.btnAddToCart.Location = new System.Drawing.Point(33, 428);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(111, 50);
            this.btnAddToCart.TabIndex = 16;
            this.btnAddToCart.Text = "Добавить в корзину";
            this.btnAddToCart.UseVisualStyleBackColor = true;
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);
            // 
            // btnRemoveFromCart
            // 
            this.btnRemoveFromCart.Location = new System.Drawing.Point(150, 428);
            this.btnRemoveFromCart.Name = "btnRemoveFromCart";
            this.btnRemoveFromCart.Size = new System.Drawing.Size(101, 50);
            this.btnRemoveFromCart.TabIndex = 17;
            this.btnRemoveFromCart.Text = "Удалить из корзины";
            this.btnRemoveFromCart.UseVisualStyleBackColor = true;
            this.btnRemoveFromCart.Click += new System.EventHandler(this.btnRemoveFromCart_Click);
            // 
            // btnClearCart
            // 
            this.btnClearCart.Location = new System.Drawing.Point(257, 428);
            this.btnClearCart.Name = "btnClearCart";
            this.btnClearCart.Size = new System.Drawing.Size(93, 50);
            this.btnClearCart.TabIndex = 18;
            this.btnClearCart.Text = "Очистить корзину";
            this.btnClearCart.UseVisualStyleBackColor = true;
            this.btnClearCart.Click += new System.EventHandler(this.btnClearCart_Click);
            // 
            // listViewCart
            // 
            this.listViewCart.HideSelection = false;
            this.listViewCart.Location = new System.Drawing.Point(36, 192);
            this.listViewCart.Name = "listViewCart";
            this.listViewCart.Size = new System.Drawing.Size(825, 91);
            this.listViewCart.TabIndex = 19;
            this.listViewCart.UseCompatibleStateImageBehavior = false;
            this.listViewCart.SelectedIndexChanged += new System.EventHandler(this.listViewCart_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(34, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 20);
            this.label8.TabIndex = 20;
            this.label8.Text = "Корзина";
            // 
            // SellRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 683);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.listViewCart);
            this.Controls.Add(this.btnClearCart);
            this.Controls.Add(this.btnRemoveFromCart);
            this.Controls.Add(this.btnAddToCart);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.cmbPaymentMethod);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.cmbRecord);
            this.MaximizeBox = false;
            this.Name = "SellRecordForm";
            this.Text = "Продать";
            this.Load += new System.EventHandler(this.SellRecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRecord;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnAddToCart;
        private System.Windows.Forms.Button btnRemoveFromCart;
        private System.Windows.Forms.Button btnClearCart;
        private System.Windows.Forms.ListView listViewCart;
        private System.Windows.Forms.Label label8;
    }
}