namespace InAndOut
{
    partial class Report
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
            this.date_from = new System.Windows.Forms.DateTimePicker();
            this.date_to = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.reportType_cmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // date_from
            // 
            this.date_from.Location = new System.Drawing.Point(72, 22);
            this.date_from.Name = "date_from";
            this.date_from.Size = new System.Drawing.Size(200, 20);
            this.date_from.TabIndex = 0;
            // 
            // date_to
            // 
            this.date_to.Location = new System.Drawing.Point(72, 60);
            this.date_to.Name = "date_to";
            this.date_to.Size = new System.Drawing.Size(200, 20);
            this.date_to.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Desde :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hasta :";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(253, 37);
            this.button1.TabIndex = 4;
            this.button1.Text = "Generar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // reportType_cmb
            // 
            this.reportType_cmb.FormattingEnabled = true;
            this.reportType_cmb.Items.AddRange(new object[] {
            "Usuario Actual Total",
            "Usuario Actual Detalle",
            "General Total",
            "General Detalle"});
            this.reportType_cmb.Location = new System.Drawing.Point(72, 97);
            this.reportType_cmb.Name = "reportType_cmb";
            this.reportType_cmb.Size = new System.Drawing.Size(200, 21);
            this.reportType_cmb.TabIndex = 5;
            this.reportType_cmb.Text = "Elija una opcion...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tipo :";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 187);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.reportType_cmb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.date_to);
            this.Controls.Add(this.date_from);
            this.MaximumSize = new System.Drawing.Size(300, 225);
            this.MinimumSize = new System.Drawing.Size(300, 225);
            this.Name = "Report";
            this.Text = "Generar Reporte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker date_from;
        private System.Windows.Forms.DateTimePicker date_to;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox reportType_cmb;
        private System.Windows.Forms.Label label3;

    }
}