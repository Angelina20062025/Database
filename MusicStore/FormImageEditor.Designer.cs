namespace MusicStore
{
    partial class FormImageEditor
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.butUpd = new System.Windows.Forms.Button();
            this.butDel = new System.Windows.Forms.Button();
            this.butClose = new System.Windows.Forms.Button();
            this.picCover = new System.Windows.Forms.PictureBox();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(60, 673);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(131, 53);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Добавить изображение";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // butUpd
            // 
            this.butUpd.Location = new System.Drawing.Point(328, 673);
            this.butUpd.Name = "butUpd";
            this.butUpd.Size = new System.Drawing.Size(128, 53);
            this.butUpd.TabIndex = 1;
            this.butUpd.Text = "Сохранить изменения";
            this.butUpd.UseVisualStyleBackColor = true;
            this.butUpd.Click += new System.EventHandler(this.butUpd_Click);
            // 
            // butDel
            // 
            this.butDel.Location = new System.Drawing.Point(197, 673);
            this.butDel.Name = "butDel";
            this.butDel.Size = new System.Drawing.Size(125, 53);
            this.butDel.TabIndex = 2;
            this.butDel.Text = "Удалить изображение";
            this.butDel.UseVisualStyleBackColor = true;
            this.butDel.Click += new System.EventHandler(this.butDel_Click);
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(60, 745);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(131, 53);
            this.butClose.TabIndex = 3;
            this.butClose.Text = "Выйти";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // picCover
            // 
            this.picCover.Location = new System.Drawing.Point(62, 56);
            this.picCover.Name = "picCover";
            this.picCover.Size = new System.Drawing.Size(468, 466);
            this.picCover.TabIndex = 5;
            this.picCover.TabStop = false;
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.AutoSize = true;
            this.lblImageInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblImageInfo.Location = new System.Drawing.Point(58, 19);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(59, 20);
            this.lblImageInfo.TabIndex = 4;
            this.lblImageInfo.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(58, 551);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 100);
            this.label1.TabIndex = 7;
            this.label1.Text = "Требования к изображению:\r\n\r\nФорматы: JPG, JPEG, PNG\r\nРазмер: минимум 100x100, ма" +
    "ксимум 400x400\r\nВес: не более 10 МБ";
            // 
            // FormImageEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 835);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picCover);
            this.Controls.Add(this.lblImageInfo);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butDel);
            this.Controls.Add(this.butUpd);
            this.Controls.Add(this.btnLoad);
            this.MaximizeBox = false;
            this.Name = "FormImageEditor";
            this.Text = "Добавить/изменить изображение";
            this.Load += new System.EventHandler(this.FormImageEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button butUpd;
        private System.Windows.Forms.Button butDel;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.PictureBox picCover;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.Label label1;
    }
}