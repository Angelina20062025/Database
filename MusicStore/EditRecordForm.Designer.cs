namespace MusicStore
{
    partial class EditRecordForm
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
            this.dateReleaseDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.numRetailPrice = new System.Windows.Forms.NumericUpDown();
            this.numWholesalePrice = new System.Windows.Forms.NumericUpDown();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtCatalogNumber = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetailPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWholesalePrice)).BeginInit();
            this.SuspendLayout();
            // 
            // dateReleaseDate
            // 
            this.dateReleaseDate.Location = new System.Drawing.Point(522, 81);
            this.dateReleaseDate.Name = "dateReleaseDate";
            this.dateReleaseDate.Size = new System.Drawing.Size(200, 22);
            this.dateReleaseDate.TabIndex = 36;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(275, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Название диска";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(518, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 20);
            this.label8.TabIndex = 34;
            this.label8.Text = "Дата выпуска";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(39, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 20);
            this.label7.TabIndex = 33;
            this.label7.Text = "Оптовая цена";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(275, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "Розничная цена";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(518, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Количество дисков";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(39, 226);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 30;
            this.label4.Text = "Описание";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(38, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(190, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "Название каталога";
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(522, 170);
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(142, 22);
            this.numQuantity.TabIndex = 28;
            // 
            // numRetailPrice
            // 
            this.numRetailPrice.DecimalPlaces = 2;
            this.numRetailPrice.Location = new System.Drawing.Point(279, 170);
            this.numRetailPrice.Name = "numRetailPrice";
            this.numRetailPrice.Size = new System.Drawing.Size(142, 22);
            this.numRetailPrice.TabIndex = 27;
            this.numRetailPrice.ThousandsSeparator = true;
            // 
            // numWholesalePrice
            // 
            this.numWholesalePrice.DecimalPlaces = 2;
            this.numWholesalePrice.Location = new System.Drawing.Point(42, 170);
            this.numWholesalePrice.Name = "numWholesalePrice";
            this.numWholesalePrice.Size = new System.Drawing.Size(142, 22);
            this.numWholesalePrice.TabIndex = 26;
            this.numWholesalePrice.ThousandsSeparator = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(43, 262);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(679, 109);
            this.txtDescription.TabIndex = 25;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(279, 81);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(201, 22);
            this.txtTitle.TabIndex = 24;
            // 
            // txtCatalogNumber
            // 
            this.txtCatalogNumber.Location = new System.Drawing.Point(41, 81);
            this.txtCatalogNumber.Name = "txtCatalogNumber";
            this.txtCatalogNumber.Size = new System.Drawing.Size(201, 22);
            this.txtCatalogNumber.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(164, 426);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 50);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Выйти";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(43, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 50);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Изменить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // EditRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 527);
            this.Controls.Add(this.dateReleaseDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.numRetailPrice);
            this.Controls.Add(this.numWholesalePrice);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.txtCatalogNumber);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.Name = "EditRecordForm";
            this.Text = "Редактировать данные о диске";
            this.Load += new System.EventHandler(this.EditRecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRetailPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWholesalePrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateReleaseDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.NumericUpDown numRetailPrice;
        private System.Windows.Forms.NumericUpDown numWholesalePrice;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtCatalogNumber;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
    }
}