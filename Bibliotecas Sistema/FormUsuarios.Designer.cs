namespace Bibliotecas_Sistema
{
    partial class FormUsuarios
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
            this.dgvusuarios = new System.Windows.Forms.DataGridView();
            this.bnteliminar = new System.Windows.Forms.Button();
            this.bnteditar = new System.Windows.Forms.Button();
            this.btnguardar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvusuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvusuarios
            // 
            this.dgvusuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvusuarios.Location = new System.Drawing.Point(12, 12);
            this.dgvusuarios.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvusuarios.Name = "dgvusuarios";
            this.dgvusuarios.RowHeadersWidth = 51;
            this.dgvusuarios.RowTemplate.Height = 24;
            this.dgvusuarios.Size = new System.Drawing.Size(1373, 150);
            this.dgvusuarios.TabIndex = 7;
            // 
            // bnteliminar
            // 
            this.bnteliminar.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnteliminar.Location = new System.Drawing.Point(675, 199);
            this.bnteliminar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bnteliminar.Name = "bnteliminar";
            this.bnteliminar.Size = new System.Drawing.Size(100, 44);
            this.bnteliminar.TabIndex = 6;
            this.bnteliminar.Text = "ELIMINAR";
            this.bnteliminar.UseVisualStyleBackColor = true;
            this.bnteliminar.Click += new System.EventHandler(this.bnteliminar_Click);
            // 
            // bnteditar
            // 
            this.bnteditar.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnteditar.Location = new System.Drawing.Point(531, 199);
            this.bnteditar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.bnteditar.Name = "bnteditar";
            this.bnteditar.Size = new System.Drawing.Size(95, 44);
            this.bnteditar.TabIndex = 5;
            this.bnteditar.Text = "EDITAR";
            this.bnteditar.UseVisualStyleBackColor = true;
            this.bnteditar.Click += new System.EventHandler(this.bnteditar_Click);
            // 
            // btnguardar
            // 
            this.btnguardar.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnguardar.Location = new System.Drawing.Point(395, 199);
            this.btnguardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnguardar.Name = "btnguardar";
            this.btnguardar.Size = new System.Drawing.Size(101, 44);
            this.btnguardar.TabIndex = 4;
            this.btnguardar.Text = "Guardar";
            this.btnguardar.UseVisualStyleBackColor = true;
            this.btnguardar.Click += new System.EventHandler(this.btnguardar_Click);
            // 
            // FormUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 450);
            this.Controls.Add(this.dgvusuarios);
            this.Controls.Add(this.bnteliminar);
            this.Controls.Add(this.bnteditar);
            this.Controls.Add(this.btnguardar);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormUsuarios";
            this.Text = "FormUsuarios";
            this.Load += new System.EventHandler(this.FormUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvusuarios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvusuarios;
        private System.Windows.Forms.Button bnteliminar;
        private System.Windows.Forms.Button bnteditar;
        private System.Windows.Forms.Button btnguardar;
    }
}