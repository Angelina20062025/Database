namespace MusicStore
{
    partial class ReserveRecordForm
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
            this.dtpReservationDate = new System.Windows.Forms.DateTimePicker();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.cmbRecord = new System.Windows.Forms.ComboBox();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.btnReserve = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpReservationDate
            // 
            this.dtpReservationDate.Location = new System.Drawing.Point(395, 249);
            this.dtpReservationDate.Name = "dtpReservationDate";
            this.dtpReservationDate.Size = new System.Drawing.Size(200, 22);
            this.dtpReservationDate.TabIndex = 0;
            this.dtpReservationDate.ValueChanged += new System.EventHandler(this.dtpReservationDate_ValueChanged);
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Location = new System.Drawing.Point(637, 249);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(200, 22);
            this.dtpExpiryDate.TabIndex = 1;
            // 
            // cmbRecord
            // 
            this.cmbRecord.FormattingEnabled = true;
            this.cmbRecord.Location = new System.Drawing.Point(37, 71);
            this.cmbRecord.Name = "cmbRecord";
            this.cmbRecord.Size = new System.Drawing.Size(221, 24);
            this.cmbRecord.TabIndex = 2;
            this.cmbRecord.SelectedIndexChanged += new System.EventHandler(this.cmbRecord_SelectedIndexChanged);
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(289, 71);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(221, 24);
            this.cmbCustomer.TabIndex = 3;
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrice.Location = new System.Drawing.Point(34, 200);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(0, 20);
            this.lblPrice.TabIndex = 5;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAvailable.Location = new System.Drawing.Point(168, 232);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(0, 20);
            this.lblAvailable.TabIndex = 6;
            // 
            // numQuantity
            // 
            this.numQuantity.Location = new System.Drawing.Point(543, 71);
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(157, 22);
            this.numQuantity.TabIndex = 8;
            this.numQuantity.ValueChanged += new System.EventHandler(this.numQuantity_ValueChanged);
            // 
            // btnReserve
            // 
            this.btnReserve.Location = new System.Drawing.Point(960, 232);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(124, 49);
            this.btnReserve.TabIndex = 10;
            this.btnReserve.Text = "Забронировать";
            this.btnReserve.UseVisualStyleBackColor = true;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(37, 344);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 49);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Выйти";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(738, 70);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(346, 79);
            this.txtNotes.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Название диска";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(285, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Покупатель";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(539, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Количество";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(734, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(251, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Заметка (необязательно)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(35, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Цена (шт.)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(168, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(198, 20);
            this.label8.TabIndex = 20;
            this.label8.Text = "Осталось в наличии";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(391, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(216, 20);
            this.label7.TabIndex = 21;
            this.label7.Text = "Начало бронирования";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(633, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(203, 20);
            this.label9.TabIndex = 22;
            this.label9.Text = "Конец бронирования";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(33, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(398, 20);
            this.label4.TabIndex = 23;
            this.label4.Text = "Бронирование доступно в течение семи дней";
            // 
            // ReserveRecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1141, 438);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNotes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.cmbRecord);
            this.Controls.Add(this.dtpExpiryDate);
            this.Controls.Add(this.dtpReservationDate);
            this.MaximizeBox = false;
            this.Name = "ReserveRecordForm";
            this.Text = "Забронировать";
            this.Load += new System.EventHandler(this.ReserveRecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpReservationDate;
        private System.Windows.Forms.DateTimePicker dtpExpiryDate;
        private System.Windows.Forms.ComboBox cmbRecord;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.NumericUpDown numQuantity;
        private System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
    }
}