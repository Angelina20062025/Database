namespace MusicStore
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnAddRecord = new System.Windows.Forms.Button();
            this.btnEditRecord = new System.Windows.Forms.Button();
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.btnViewReservations = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShowSalesLeaders = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.buttonUpd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(37, 59);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(893, 252);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(37, 337);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(146, 37);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(428, 428);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(75, 23);
            this.btnManageUsers.TabIndex = 2;
            this.btnManageUsers.Text = "button2";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnAddRecord
            // 
            this.btnAddRecord.Location = new System.Drawing.Point(549, 452);
            this.btnAddRecord.Name = "btnAddRecord";
            this.btnAddRecord.Size = new System.Drawing.Size(75, 23);
            this.btnAddRecord.TabIndex = 3;
            this.btnAddRecord.Text = "button3";
            this.btnAddRecord.UseVisualStyleBackColor = true;
            this.btnAddRecord.Click += new System.EventHandler(this.btnAddRecord_Click);
            // 
            // btnEditRecord
            // 
            this.btnEditRecord.Location = new System.Drawing.Point(510, 495);
            this.btnEditRecord.Name = "btnEditRecord";
            this.btnEditRecord.Size = new System.Drawing.Size(75, 23);
            this.btnEditRecord.TabIndex = 4;
            this.btnEditRecord.Text = "button4";
            this.btnEditRecord.UseVisualStyleBackColor = true;
            this.btnEditRecord.Click += new System.EventHandler(this.btnEditRecord_Click);
            // 
            // btnDeleteRecord
            // 
            this.btnDeleteRecord.Location = new System.Drawing.Point(682, 495);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteRecord.TabIndex = 5;
            this.btnDeleteRecord.Text = "button5";
            this.btnDeleteRecord.UseVisualStyleBackColor = true;
            this.btnDeleteRecord.Click += new System.EventHandler(this.btnDeleteRecord_Click);
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(205, 391);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(147, 49);
            this.btnSell.TabIndex = 6;
            this.btnSell.Text = "Продать диск";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnReserve
            // 
            this.btnReserve.Location = new System.Drawing.Point(205, 446);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(147, 47);
            this.btnReserve.TabIndex = 7;
            this.btnReserve.Text = "Забронировать";
            this.btnReserve.UseVisualStyleBackColor = true;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // btnViewReservations
            // 
            this.btnViewReservations.Location = new System.Drawing.Point(205, 499);
            this.btnViewReservations.Name = "btnViewReservations";
            this.btnViewReservations.Size = new System.Drawing.Size(147, 47);
            this.btnViewReservations.TabIndex = 8;
            this.btnViewReservations.Text = "Показать бронирования";
            this.btnViewReservations.UseVisualStyleBackColor = true;
            this.btnViewReservations.Click += new System.EventHandler(this.btnViewReservations_Click);
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Location = new System.Drawing.Point(34, 24);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(44, 16);
            this.lblUserInfo.TabIndex = 9;
            this.lblUserInfo.Text = "label1";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(524, 343);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(132, 37);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Найти...";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShowSalesLeaders
            // 
            this.btnShowSalesLeaders.Location = new System.Drawing.Point(205, 337);
            this.btnShowSalesLeaders.Name = "btnShowSalesLeaders";
            this.btnShowSalesLeaders.Size = new System.Drawing.Size(147, 48);
            this.btnShowSalesLeaders.TabIndex = 11;
            this.btnShowSalesLeaders.Text = "Показать лидеров продаж";
            this.btnShowSalesLeaders.UseVisualStyleBackColor = true;
            this.btnShowSalesLeaders.Click += new System.EventHandler(this.btnShowSalesLeaders_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(682, 350);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(248, 23);
            this.txtSearch.TabIndex = 12;
            // 
            // buttonUpd
            // 
            this.buttonUpd.Location = new System.Drawing.Point(371, 343);
            this.buttonUpd.Name = "buttonUpd";
            this.buttonUpd.Size = new System.Drawing.Size(132, 37);
            this.buttonUpd.TabIndex = 13;
            this.buttonUpd.Text = "Обновить";
            this.buttonUpd.UseVisualStyleBackColor = true;
            this.buttonUpd.Click += new System.EventHandler(this.buttonUpd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 647);
            this.Controls.Add(this.buttonUpd);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnShowSalesLeaders);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.btnViewReservations);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnDeleteRecord);
            this.Controls.Add(this.btnEditRecord);
            this.Controls.Add(this.btnAddRecord);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Ассортимент компакт-дисков";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnAddRecord;
        private System.Windows.Forms.Button btnEditRecord;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.Button btnViewReservations;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowSalesLeaders;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button buttonUpd;
    }
}

