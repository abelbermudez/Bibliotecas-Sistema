namespace Bibliotecas_Sistema
{
    partial class Formdevoluciones
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
            this.dgvdevo = new System.Windows.Forms.DataGridView();
            this.dgvdevolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdevo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvdevo
            // 
            this.dgvdevo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvdevo.Location = new System.Drawing.Point(12, 12);
            this.dgvdevo.Name = "dgvdevo";
            this.dgvdevo.Size = new System.Drawing.Size(776, 186);
            this.dgvdevo.TabIndex = 0;
            this.dgvdevo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dgvdevolver
            // 
            this.dgvdevolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvdevolver.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdevolver.Location = new System.Drawing.Point(310, 229);
            this.dgvdevolver.Name = "dgvdevolver";
            this.dgvdevolver.Size = new System.Drawing.Size(107, 23);
            this.dgvdevolver.TabIndex = 1;
            this.dgvdevolver.Text = "DEVOLVER";
            this.dgvdevolver.UseVisualStyleBackColor = true;
            this.dgvdevolver.Click += new System.EventHandler(this.dgvdevolver_Click);
            // 
            // Formdevoluciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvdevolver);
            this.Controls.Add(this.dgvdevo);
            this.Name = "Formdevoluciones";
            this.Text = "Formdevoluciones";
            this.Load += new System.EventHandler(this.Formdevoluciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvdevo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvdevo;
        private System.Windows.Forms.Button dgvdevolver;
    }
}