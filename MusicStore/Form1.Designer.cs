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
            this.btnAddRecord = new System.Windows.Forms.Button();
            this.btnEditRecord = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.btnViewReservations = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnShowSalesLeaders = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.buttonUpd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.butSrchAns = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnArcRecord = new System.Windows.Forms.Button();
            this.btnEnsembles = new System.Windows.Forms.Button();
            this.butCust = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSell = new System.Windows.Forms.Label();
            this.lblAdm = new System.Windows.Forms.Label();
            this.lblTov = new System.Windows.Forms.Label();
            this.buttAddCust = new System.Windows.Forms.Button();
            this.buttMus = new System.Windows.Forms.Button();
            this.buttComp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(45, 88);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1049, 364);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLogin.Location = new System.Drawing.Point(45, 753);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(148, 49);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnAddRecord
            // 
            this.btnAddRecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddRecord.Location = new System.Drawing.Point(1149, 121);
            this.btnAddRecord.Name = "btnAddRecord";
            this.btnAddRecord.Size = new System.Drawing.Size(147, 49);
            this.btnAddRecord.TabIndex = 3;
            this.btnAddRecord.Text = "Добавить пластинку";
            this.btnAddRecord.UseVisualStyleBackColor = true;
            this.btnAddRecord.Click += new System.EventHandler(this.btnAddRecord_Click);
            // 
            // btnEditRecord
            // 
            this.btnEditRecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEditRecord.Location = new System.Drawing.Point(1151, 176);
            this.btnEditRecord.Name = "btnEditRecord";
            this.btnEditRecord.Size = new System.Drawing.Size(147, 47);
            this.btnEditRecord.TabIndex = 4;
            this.btnEditRecord.Text = "Редактировать пластинку";
            this.btnEditRecord.UseVisualStyleBackColor = true;
            this.btnEditRecord.Click += new System.EventHandler(this.btnEditRecord_Click);
            // 
            // btnSell
            // 
            this.btnSell.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSell.Location = new System.Drawing.Point(531, 753);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(147, 49);
            this.btnSell.TabIndex = 6;
            this.btnSell.Text = "Продать";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnReserve
            // 
            this.btnReserve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReserve.Location = new System.Drawing.Point(684, 753);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(147, 49);
            this.btnReserve.TabIndex = 7;
            this.btnReserve.Text = "Забронировать";
            this.btnReserve.UseVisualStyleBackColor = true;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // btnViewReservations
            // 
            this.btnViewReservations.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnViewReservations.Location = new System.Drawing.Point(837, 753);
            this.btnViewReservations.Name = "btnViewReservations";
            this.btnViewReservations.Size = new System.Drawing.Size(147, 49);
            this.btnViewReservations.TabIndex = 8;
            this.btnViewReservations.Text = "Управлять бронированиями";
            this.btnViewReservations.UseVisualStyleBackColor = true;
            this.btnViewReservations.Click += new System.EventHandler(this.btnViewReservations_Click);
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblUserInfo.Location = new System.Drawing.Point(40, 711);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(70, 25);
            this.lblUserInfo.TabIndex = 9;
            this.lblUserInfo.Text = "label1";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.Location = new System.Drawing.Point(43, 528);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(147, 47);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Найти по названию...";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnShowSalesLeaders
            // 
            this.btnShowSalesLeaders.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnShowSalesLeaders.Location = new System.Drawing.Point(490, 554);
            this.btnShowSalesLeaders.Name = "btnShowSalesLeaders";
            this.btnShowSalesLeaders.Size = new System.Drawing.Size(147, 48);
            this.btnShowSalesLeaders.TabIndex = 11;
            this.btnShowSalesLeaders.Text = "Показать лидеров продаж";
            this.btnShowSalesLeaders.UseVisualStyleBackColor = true;
            this.btnShowSalesLeaders.Click += new System.EventHandler(this.btnShowSalesLeaders_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSearch.Location = new System.Drawing.Point(206, 540);
            this.txtSearch.Multiline = true;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(248, 23);
            this.txtSearch.TabIndex = 12;
            // 
            // buttonUpd
            // 
            this.buttonUpd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonUpd.Location = new System.Drawing.Point(947, 554);
            this.buttonUpd.Name = "buttonUpd";
            this.buttonUpd.Size = new System.Drawing.Size(147, 47);
            this.buttonUpd.TabIndex = 13;
            this.buttonUpd.Text = "Обновить ассортимент";
            this.buttonUpd.UseVisualStyleBackColor = true;
            this.buttonUpd.Click += new System.EventHandler(this.buttonUpd_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Location = new System.Drawing.Point(643, 554);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 48);
            this.button1.TabIndex = 14;
            this.button1.Text = "Подробнее о товаре";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.Location = new System.Drawing.Point(795, 554);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 48);
            this.button2.TabIndex = 15;
            this.button2.Text = "Подробнее о группах";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // butSrchAns
            // 
            this.butSrchAns.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.butSrchAns.Location = new System.Drawing.Point(43, 581);
            this.butSrchAns.Name = "butSrchAns";
            this.butSrchAns.Size = new System.Drawing.Size(147, 48);
            this.butSrchAns.TabIndex = 16;
            this.butSrchAns.Text = "Найти по группе...";
            this.butSrchAns.UseVisualStyleBackColor = true;
            this.butSrchAns.Click += new System.EventHandler(this.butSrchAns_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.Location = new System.Drawing.Point(206, 594);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(248, 23);
            this.textBox1.TabIndex = 17;
            // 
            // btnArcRecord
            // 
            this.btnArcRecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnArcRecord.Location = new System.Drawing.Point(1149, 229);
            this.btnArcRecord.Name = "btnArcRecord";
            this.btnArcRecord.Size = new System.Drawing.Size(147, 47);
            this.btnArcRecord.TabIndex = 18;
            this.btnArcRecord.Text = "Удалить пластинку";
            this.btnArcRecord.UseVisualStyleBackColor = true;
            this.btnArcRecord.Click += new System.EventHandler(this.btnArcRecord_Click);
            // 
            // btnEnsembles
            // 
            this.btnEnsembles.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEnsembles.Location = new System.Drawing.Point(1302, 121);
            this.btnEnsembles.Name = "btnEnsembles";
            this.btnEnsembles.Size = new System.Drawing.Size(146, 46);
            this.btnEnsembles.TabIndex = 19;
            this.btnEnsembles.Text = "Управлять ансамблями";
            this.btnEnsembles.UseVisualStyleBackColor = true;
            this.btnEnsembles.Click += new System.EventHandler(this.btnEnsembles_Click);
            // 
            // butCust
            // 
            this.butCust.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.butCust.Location = new System.Drawing.Point(1302, 176);
            this.butCust.Name = "butCust";
            this.butCust.Size = new System.Drawing.Size(146, 47);
            this.butCust.TabIndex = 20;
            this.butCust.Text = "Управлять покупателями";
            this.butCust.UseVisualStyleBackColor = true;
            this.butCust.Click += new System.EventHandler(this.butCust_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(38, 478);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 25);
            this.label1.TabIndex = 21;
            this.label1.Text = "Просмотр";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(40, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 25);
            this.label2.TabIndex = 22;
            this.label2.Text = "Ассортимент";
            // 
            // lblSell
            // 
            this.lblSell.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSell.AutoSize = true;
            this.lblSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSell.Location = new System.Drawing.Point(373, 711);
            this.lblSell.Name = "lblSell";
            this.lblSell.Size = new System.Drawing.Size(254, 25);
            this.lblSell.TabIndex = 23;
            this.lblSell.Text = "Продажа/Бронирование";
            // 
            // lblAdm
            // 
            this.lblAdm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAdm.AutoSize = true;
            this.lblAdm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAdm.Location = new System.Drawing.Point(1146, 41);
            this.lblAdm.Name = "lblAdm";
            this.lblAdm.Size = new System.Drawing.Size(223, 25);
            this.lblAdm.TabIndex = 24;
            this.lblAdm.Text = "Администрирование";
            // 
            // lblTov
            // 
            this.lblTov.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTov.AutoSize = true;
            this.lblTov.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTov.Location = new System.Drawing.Point(1147, 85);
            this.lblTov.Name = "lblTov";
            this.lblTov.Size = new System.Drawing.Size(71, 20);
            this.lblTov.TabIndex = 25;
            this.lblTov.Text = "Товары";
            // 
            // buttAddCust
            // 
            this.buttAddCust.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttAddCust.Location = new System.Drawing.Point(378, 753);
            this.buttAddCust.Name = "buttAddCust";
            this.buttAddCust.Size = new System.Drawing.Size(147, 49);
            this.buttAddCust.TabIndex = 26;
            this.buttAddCust.Text = "Добавить покупателя";
            this.buttAddCust.UseVisualStyleBackColor = true;
            this.buttAddCust.Click += new System.EventHandler(this.buttAddCust_Click);
            // 
            // buttMus
            // 
            this.buttMus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttMus.Location = new System.Drawing.Point(1303, 230);
            this.buttMus.Name = "buttMus";
            this.buttMus.Size = new System.Drawing.Size(145, 46);
            this.buttMus.TabIndex = 27;
            this.buttMus.Text = "Управлять музыкантами";
            this.buttMus.UseVisualStyleBackColor = true;
            this.buttMus.Click += new System.EventHandler(this.buttMus_Click);
            // 
            // buttComp
            // 
            this.buttComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttComp.Location = new System.Drawing.Point(1151, 282);
            this.buttComp.Name = "buttComp";
            this.buttComp.Size = new System.Drawing.Size(147, 46);
            this.buttComp.TabIndex = 28;
            this.buttComp.Text = "Управлять композициями";
            this.buttComp.UseVisualStyleBackColor = true;
            this.buttComp.Click += new System.EventHandler(this.buttComp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1549, 1055);
            this.Controls.Add(this.buttComp);
            this.Controls.Add(this.buttMus);
            this.Controls.Add(this.buttAddCust);
            this.Controls.Add(this.lblTov);
            this.Controls.Add(this.lblAdm);
            this.Controls.Add(this.lblSell);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCust);
            this.Controls.Add(this.btnEnsembles);
            this.Controls.Add(this.btnArcRecord);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.butSrchAns);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonUpd);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnShowSalesLeaders);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.btnViewReservations);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.btnEditRecord);
            this.Controls.Add(this.btnAddRecord);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "Ассортимент компакт-дисков и виниловых пластинок";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnAddRecord;
        private System.Windows.Forms.Button btnEditRecord;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.Button btnViewReservations;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnShowSalesLeaders;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button buttonUpd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button butSrchAns;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnArcRecord;
        private System.Windows.Forms.Button btnEnsembles;
        private System.Windows.Forms.Button butCust;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSell;
        private System.Windows.Forms.Label lblAdm;
        private System.Windows.Forms.Label lblTov;
        private System.Windows.Forms.Button buttAddCust;
        private System.Windows.Forms.Button buttMus;
        private System.Windows.Forms.Button buttComp;
    }
}

