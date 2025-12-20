namespace MusicStore
{
    partial class ReservationsForm
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
            this.dataGridViewReservations = new System.Windows.Forms.DataGridView();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnCancelReservation = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.butUpd = new System.Windows.Forms.Button();
            this.btnExpireOld = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReservations)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewReservations
            // 
            this.dataGridViewReservations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewReservations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReservations.Location = new System.Drawing.Point(45, 72);
            this.dataGridViewReservations.Name = "dataGridViewReservations";
            this.dataGridViewReservations.RowHeadersWidth = 51;
            this.dataGridViewReservations.RowTemplate.Height = 24;
            this.dataGridViewReservations.Size = new System.Drawing.Size(1063, 300);
            this.dataGridViewReservations.TabIndex = 0;
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(186, 410);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(129, 47);
            this.btnComplete.TabIndex = 1;
            this.btnComplete.Text = "Завершить";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnCancelReservation
            // 
            this.btnCancelReservation.Location = new System.Drawing.Point(321, 410);
            this.btnCancelReservation.Name = "btnCancelReservation";
            this.btnCancelReservation.Size = new System.Drawing.Size(129, 47);
            this.btnCancelReservation.TabIndex = 2;
            this.btnCancelReservation.Text = "Отменить";
            this.btnCancelReservation.UseVisualStyleBackColor = true;
            this.btnCancelReservation.Click += new System.EventHandler(this.btnCancelReservation_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(46, 541);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 47);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Выйти";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // butUpd
            // 
            this.butUpd.Location = new System.Drawing.Point(456, 410);
            this.butUpd.Name = "butUpd";
            this.butUpd.Size = new System.Drawing.Size(129, 47);
            this.butUpd.TabIndex = 4;
            this.butUpd.Text = "Обновить таблицу";
            this.butUpd.UseVisualStyleBackColor = true;
            this.butUpd.Click += new System.EventHandler(this.butUpd_Click);
            // 
            // btnExpireOld
            // 
            this.btnExpireOld.Location = new System.Drawing.Point(45, 410);
            this.btnExpireOld.Name = "btnExpireOld";
            this.btnExpireOld.Size = new System.Drawing.Size(135, 47);
            this.btnExpireOld.TabIndex = 5;
            this.btnExpireOld.Text = "Просрочить";
            this.btnExpireOld.UseVisualStyleBackColor = true;
            this.btnExpireOld.Click += new System.EventHandler(this.btnExpireOld_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(42, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Бронирования";
            // 
            // ReservationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 626);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExpireOld);
            this.Controls.Add(this.butUpd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancelReservation);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.dataGridViewReservations);
            this.MaximizeBox = false;
            this.Name = "ReservationsForm";
            this.Text = "Бронирования";
            this.Load += new System.EventHandler(this.ReservationsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReservations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewReservations;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnCancelReservation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button butUpd;
        private System.Windows.Forms.Button btnExpireOld;
        private System.Windows.Forms.Label label1;
    }
}