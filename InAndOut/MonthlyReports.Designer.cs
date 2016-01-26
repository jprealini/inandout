namespace InAndOut
{
    partial class MonthlyReports
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
            this.month_cmb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.generate_btn = new System.Windows.Forms.Button();
            this.format_cmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // month_cmb
            // 
            this.month_cmb.FormattingEnabled = true;
            this.month_cmb.Items.AddRange(new object[] {
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre"});
            this.month_cmb.Location = new System.Drawing.Point(89, 32);
            this.month_cmb.Name = "month_cmb";
            this.month_cmb.Size = new System.Drawing.Size(173, 21);
            this.month_cmb.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mes :";
            // 
            // generate_btn
            // 
            this.generate_btn.Location = new System.Drawing.Point(30, 111);
            this.generate_btn.Name = "generate_btn";
            this.generate_btn.Size = new System.Drawing.Size(232, 41);
            this.generate_btn.TabIndex = 2;
            this.generate_btn.Text = "Generar";
            this.generate_btn.UseVisualStyleBackColor = true;
            // 
            // format_cmb
            // 
            this.format_cmb.FormattingEnabled = true;
            this.format_cmb.Items.AddRange(new object[] {
            "Excel",
            "PDF",
            "csv"});
            this.format_cmb.Location = new System.Drawing.Point(89, 67);
            this.format_cmb.Name = "format_cmb";
            this.format_cmb.Size = new System.Drawing.Size(173, 21);
            this.format_cmb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Formato :";
            // 
            // MonthlyReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 183);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.format_cmb);
            this.Controls.Add(this.generate_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.month_cmb);
            this.Name = "MonthlyReports";
            this.Text = "Generar Reportes Mensuales";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox month_cmb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button generate_btn;
        private System.Windows.Forms.ComboBox format_cmb;
        private System.Windows.Forms.Label label2;
    }
}