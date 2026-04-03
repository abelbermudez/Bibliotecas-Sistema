namespace Bibliotecas_Sistema
{
    partial class FormPrestamos
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
            this.dgvprestamos = new System.Windows.Forms.DataGridView();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnDevolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvprestamos)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvprestamos
            // 
            this.dgvprestamos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvprestamos.Location = new System.Drawing.Point(10, 11);
            this.dgvprestamos.Name = "dgvprestamos";
            this.dgvprestamos.RowHeadersWidth = 51;
            this.dgvprestamos.Size = new System.Drawing.Size(1100, 150);
            this.dgvprestamos.TabIndex = 0;
            // 
            // btnGuardar
            // 
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.Location = new System.Drawing.Point(185, 215);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnDevolver
            // 
            this.btnDevolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevolver.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevolver.Location = new System.Drawing.Point(325, 215);
            this.btnDevolver.Name = "btnDevolver";
            this.btnDevolver.Size = new System.Drawing.Size(99, 23);
            this.btnDevolver.TabIndex = 2;
            this.btnDevolver.Text = "DEVOLVER";
            this.btnDevolver.UseVisualStyleBackColor = true;
            this.btnDevolver.Click += new System.EventHandler(this.btnDevolver_Click);
            // 
            // FormPrestamos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 450);
            this.Controls.Add(this.btnDevolver);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.dgvprestamos);
            this.Name = "FormPrestamos";
            this.Text = "FormPrestamos";
            this.Load += new System.EventHandler(this.FormPrestamos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvprestamos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvprestamos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnDevolver;
    }
}