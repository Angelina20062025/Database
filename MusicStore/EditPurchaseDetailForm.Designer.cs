namespace MusicStore
{
    partial class EditPurchaseDetailForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numNewQuantity = new System.Windows.Forms.NumericUpDown();
            this.txtCurrentQuantity = new System.Windows.Forms.TextBox();
            this.txtAvailableStock = new System.Windows.Forms.TextBox();
            this.txtRecordTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numNewQuantity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(207, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(134, 46);
            this.btnCancel.TabIndex = 91;
            this.btnCancel.Text = "Выйти";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(67, 276);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(134, 46);
            this.btnEdit.TabIndex = 89;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(63, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 20);
            this.label3.TabIndex = 115;
            this.label3.Text = "Новое количество";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(63, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 114;
            this.label2.Text = "Пластинка";
            // 
            // numNewQuantity
            // 
            this.numNewQuantity.Location = new System.Drawing.Point(67, 207);
            this.numNewQuantity.Name = "numNewQuantity";
            this.numNewQuantity.Size = new System.Drawing.Size(120, 22);
            this.numNewQuantity.TabIndex = 113;
            // 
            // txtCurrentQuantity
            // 
            this.txtCurrentQuantity.Location = new System.Drawing.Point(332, 91);
            this.txtCurrentQuantity.Name = "txtCurrentQuantity";
            this.txtCurrentQuantity.Size = new System.Drawing.Size(209, 22);
            this.txtCurrentQuantity.TabIndex = 116;
            // 
            // txtAvailableStock
            // 
            this.txtAvailableStock.Location = new System.Drawing.Point(607, 91);
            this.txtAvailableStock.Name = "txtAvailableStock";
            this.txtAvailableStock.Size = new System.Drawing.Size(209, 22);
            this.txtAvailableStock.TabIndex = 117;
            // 
            // txtRecordTitle
            // 
            this.txtRecordTitle.Location = new System.Drawing.Point(67, 91);
            this.txtRecordTitle.Name = "txtRecordTitle";
            this.txtRecordTitle.Size = new System.Drawing.Size(209, 22);
            this.txtRecordTitle.TabIndex = 118;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(328, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 20);
            this.label1.TabIndex = 119;
            this.label1.Text = "Текущее количество";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(603, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 20);
            this.label4.TabIndex = 120;
            this.label4.Text = "В наличии";
            // 
            // EditPurchaseDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 366);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecordTitle);
            this.Controls.Add(this.txtAvailableStock);
            this.Controls.Add(this.txtCurrentQuantity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numNewQuantity);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEdit);
            this.MaximizeBox = false;
            this.Name = "EditPurchaseDetailForm";
            this.Text = "Изменить данные о детали";
            this.Load += new System.EventHandler(this.EditPurchaseDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numNewQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numNewQuantity;
        private System.Windows.Forms.TextBox txtCurrentQuantity;
        private System.Windows.Forms.TextBox txtAvailableStock;
        private System.Windows.Forms.TextBox txtRecordTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}