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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReservations)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewReservations
            // 
            this.dataGridViewReservations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewReservations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReservations.Location = new System.Drawing.Point(48, 40);
            this.dataGridViewReservations.Name = "dataGridViewReservations";
            this.dataGridViewReservations.RowHeadersWidth = 51;
            this.dataGridViewReservations.RowTemplate.Height = 24;
            this.dataGridViewReservations.Size = new System.Drawing.Size(1063, 214);
            this.dataGridViewReservations.TabIndex = 0;
            // 
            // btnComplete
            // 
            this.btnComplete.Location = new System.Drawing.Point(189, 293);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(129, 47);
            this.btnComplete.TabIndex = 1;
            this.btnComplete.Text = "Завершить";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnCancelReservation
            // 
            this.btnCancelReservation.Location = new System.Drawing.Point(324, 293);
            this.btnCancelReservation.Name = "btnCancelReservation";
            this.btnCancelReservation.Size = new System.Drawing.Size(129, 47);
            this.btnCancelReservation.TabIndex = 2;
            this.btnCancelReservation.Text = "Отменить";
            this.btnCancelReservation.UseVisualStyleBackColor = true;
            this.btnCancelReservation.Click += new System.EventHandler(this.btnCancelReservation_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(594, 293);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(115, 47);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // butUpd
            // 
            this.butUpd.Location = new System.Drawing.Point(459, 293);
            this.butUpd.Name = "butUpd";
            this.butUpd.Size = new System.Drawing.Size(129, 47);
            this.butUpd.TabIndex = 4;
            this.butUpd.Text = "Обновить таблицу";
            this.butUpd.UseVisualStyleBackColor = true;
            this.butUpd.Click += new System.EventHandler(this.butUpd_Click);
            // 
            // btnExpireOld
            // 
            this.btnExpireOld.Location = new System.Drawing.Point(48, 284);
            this.btnExpireOld.Name = "btnExpireOld";
            this.btnExpireOld.Size = new System.Drawing.Size(135, 65);
            this.btnExpireOld.TabIndex = 5;
            this.btnExpireOld.Text = "Установить статус \"Просрочено\"";
            this.btnExpireOld.UseVisualStyleBackColor = true;
            this.btnExpireOld.Click += new System.EventHandler(this.btnExpireOld_Click);
            // 
            // ReservationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 536);
            this.Controls.Add(this.btnExpireOld);
            this.Controls.Add(this.butUpd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCancelReservation);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.dataGridViewReservations);
            this.Name = "ReservationsForm";
            this.Text = "Бронирования";
            this.Load += new System.EventHandler(this.ReservationsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReservations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewReservations;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnCancelReservation;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button butUpd;
        private System.Windows.Forms.Button btnExpireOld;
    }
}