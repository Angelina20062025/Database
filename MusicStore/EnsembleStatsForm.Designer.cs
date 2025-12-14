namespace MusicStore
{
    partial class EnsembleStatsForm
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
            this.cmbEnsemble = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSearchCDs = new System.Windows.Forms.Button();
            this.lbCompositionsCount = new System.Windows.Forms.Label();
            this.lblCDCount = new System.Windows.Forms.Label();
            this.dataGridViewCD = new System.Windows.Forms.DataGridView();
            this.lblMusiciansCount = new System.Windows.Forms.Label();
            this.lblFoundedDate = new System.Windows.Forms.Label();
            this.lblEnsembleType = new System.Windows.Forms.Label();
            this.txtEnsembleDescription = new System.Windows.Forms.TextBox();
            this.txtSearchCD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCD)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbEnsemble
            // 
            this.cmbEnsemble.FormattingEnabled = true;
            this.cmbEnsemble.Location = new System.Drawing.Point(57, 63);
            this.cmbEnsemble.Name = "cmbEnsemble";
            this.cmbEnsemble.Size = new System.Drawing.Size(249, 24);
            this.cmbEnsemble.TabIndex = 0;
            this.cmbEnsemble.SelectedIndexChanged += new System.EventHandler(this.cmbEnsemble_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(908, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Выйти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSearchCDs
            // 
            this.btnSearchCDs.Location = new System.Drawing.Point(783, 275);
            this.btnSearchCDs.Name = "btnSearchCDs";
            this.btnSearchCDs.Size = new System.Drawing.Size(119, 43);
            this.btnSearchCDs.TabIndex = 3;
            this.btnSearchCDs.Text = "Найти диск";
            this.btnSearchCDs.UseVisualStyleBackColor = true;
            this.btnSearchCDs.Click += new System.EventHandler(this.btnSearchCDs_Click);
            // 
            // lbCompositionsCount
            // 
            this.lbCompositionsCount.AutoSize = true;
            this.lbCompositionsCount.Location = new System.Drawing.Point(57, 335);
            this.lbCompositionsCount.Name = "lbCompositionsCount";
            this.lbCompositionsCount.Size = new System.Drawing.Size(0, 16);
            this.lbCompositionsCount.TabIndex = 4;
            // 
            // lblCDCount
            // 
            this.lblCDCount.AutoSize = true;
            this.lblCDCount.Location = new System.Drawing.Point(57, 271);
            this.lblCDCount.Name = "lblCDCount";
            this.lblCDCount.Size = new System.Drawing.Size(0, 16);
            this.lblCDCount.TabIndex = 5;
            // 
            // dataGridViewCD
            // 
            this.dataGridViewCD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCD.Location = new System.Drawing.Point(520, 63);
            this.dataGridViewCD.Name = "dataGridViewCD";
            this.dataGridViewCD.RowHeadersWidth = 51;
            this.dataGridViewCD.RowTemplate.Height = 24;
            this.dataGridViewCD.Size = new System.Drawing.Size(650, 194);
            this.dataGridViewCD.TabIndex = 6;
            // 
            // lblMusiciansCount
            // 
            this.lblMusiciansCount.AutoSize = true;
            this.lblMusiciansCount.Location = new System.Drawing.Point(198, 407);
            this.lblMusiciansCount.Name = "lblMusiciansCount";
            this.lblMusiciansCount.Size = new System.Drawing.Size(0, 16);
            this.lblMusiciansCount.TabIndex = 10;
            // 
            // lblFoundedDate
            // 
            this.lblFoundedDate.AutoSize = true;
            this.lblFoundedDate.Location = new System.Drawing.Point(56, 407);
            this.lblFoundedDate.Name = "lblFoundedDate";
            this.lblFoundedDate.Size = new System.Drawing.Size(0, 16);
            this.lblFoundedDate.TabIndex = 11;
            // 
            // lblEnsembleType
            // 
            this.lblEnsembleType.AutoSize = true;
            this.lblEnsembleType.Location = new System.Drawing.Point(387, 407);
            this.lblEnsembleType.Name = "lblEnsembleType";
            this.lblEnsembleType.Size = new System.Drawing.Size(0, 16);
            this.lblEnsembleType.TabIndex = 12;
            // 
            // txtEnsembleDescription
            // 
            this.txtEnsembleDescription.Location = new System.Drawing.Point(57, 135);
            this.txtEnsembleDescription.Multiline = true;
            this.txtEnsembleDescription.Name = "txtEnsembleDescription";
            this.txtEnsembleDescription.Size = new System.Drawing.Size(249, 85);
            this.txtEnsembleDescription.TabIndex = 13;
            // 
            // txtSearchCD
            // 
            this.txtSearchCD.Location = new System.Drawing.Point(520, 285);
            this.txtSearchCD.Name = "txtSearchCD";
            this.txtSearchCD.Size = new System.Drawing.Size(242, 22);
            this.txtSearchCD.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Группа";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(520, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "Все диски группы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "Описание группы";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(327, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Количество музыкальных произведений группы:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(187, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "Количество дисков группы:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(387, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 16);
            this.label6.TabIndex = 20;
            this.label6.Text = "Тип:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 375);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "Дата основания:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(198, 375);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(171, 16);
            this.label8.TabIndex = 22;
            this.label8.Text = "Количество музыкантов:";
            // 
            // EnsembleStatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1359, 624);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSearchCD);
            this.Controls.Add(this.txtEnsembleDescription);
            this.Controls.Add(this.lblEnsembleType);
            this.Controls.Add(this.lblFoundedDate);
            this.Controls.Add(this.lblMusiciansCount);
            this.Controls.Add(this.dataGridViewCD);
            this.Controls.Add(this.lblCDCount);
            this.Controls.Add(this.lbCompositionsCount);
            this.Controls.Add(this.btnSearchCDs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbEnsemble);
            this.Name = "EnsembleStatsForm";
            this.Text = "Подробнее о группах";
            this.Load += new System.EventHandler(this.EnsembleStatsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbEnsemble;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSearchCDs;
        private System.Windows.Forms.Label lbCompositionsCount;
        private System.Windows.Forms.Label lblCDCount;
        private System.Windows.Forms.DataGridView dataGridViewCD;
        private System.Windows.Forms.Label lblMusiciansCount;
        private System.Windows.Forms.Label lblFoundedDate;
        private System.Windows.Forms.Label lblEnsembleType;
        private System.Windows.Forms.TextBox txtEnsembleDescription;
        private System.Windows.Forms.TextBox txtSearchCD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}