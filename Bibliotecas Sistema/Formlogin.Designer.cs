namespace Bibliotecas_Sistema
{
    partial class Formlogin
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtusuario = new System.Windows.Forms.TextBox();
            this.txtcontraseña = new System.Windows.Forms.TextBox();
            this.bntingresar = new System.Windows.Forms.Button();
            this.usuario = new System.Windows.Forms.Label();
            this.contraseña = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtusuario
            // 
            this.txtusuario.Location = new System.Drawing.Point(248, 59);
            this.txtusuario.Name = "txtusuario";
            this.txtusuario.Size = new System.Drawing.Size(141, 22);
            this.txtusuario.TabIndex = 0;
            // 
            // txtcontraseña
            // 
            this.txtcontraseña.Location = new System.Drawing.Point(248, 109);
            this.txtcontraseña.Name = "txtcontraseña";
            this.txtcontraseña.Size = new System.Drawing.Size(141, 22);
            this.txtcontraseña.TabIndex = 1;
            this.txtcontraseña.UseSystemPasswordChar = true;
            // 
            // bntingresar
            // 
            this.bntingresar.BackColor = System.Drawing.Color.Blue;
            this.bntingresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntingresar.ForeColor = System.Drawing.SystemColors.Control;
            this.bntingresar.Location = new System.Drawing.Point(173, 163);
            this.bntingresar.Name = "bntingresar";
            this.bntingresar.Size = new System.Drawing.Size(127, 27);
            this.bntingresar.TabIndex = 2;
            this.bntingresar.Text = "INICIAR SESION";
            this.bntingresar.UseMnemonic = false;
            this.bntingresar.UseVisualStyleBackColor = false;
            this.bntingresar.Click += new System.EventHandler(this.bntingresar_Click);
            // 
            // usuario
            // 
            this.usuario.AutoSize = true;
            this.usuario.BackColor = System.Drawing.Color.Blue;
            this.usuario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.usuario.ForeColor = System.Drawing.SystemColors.Control;
            this.usuario.Location = new System.Drawing.Point(135, 65);
            this.usuario.Name = "usuario";
            this.usuario.Size = new System.Drawing.Size(71, 16);
            this.usuario.TabIndex = 3;
            this.usuario.Text = "USUARIO:";
            // 
            // contraseña
            // 
            this.contraseña.AutoSize = true;
            this.contraseña.BackColor = System.Drawing.Color.Blue;
            this.contraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.contraseña.ForeColor = System.Drawing.SystemColors.Control;
            this.contraseña.Location = new System.Drawing.Point(135, 115);
            this.contraseña.Name = "contraseña";
            this.contraseña.Size = new System.Drawing.Size(104, 16);
            this.contraseña.TabIndex = 4;
            this.contraseña.Text = "CONTRASEÑA:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = global::Bibliotecas_Sistema.Properties.Resources.security_lock_flat_icon_by_Vexels;
            this.pictureBox2.Location = new System.Drawing.Point(87, 109);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(42, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::Bibliotecas_Sistema.Properties.Resources.user_solid;
            this.pictureBox1.Location = new System.Drawing.Point(87, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Formlogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 312);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.contraseña);
            this.Controls.Add(this.usuario);
            this.Controls.Add(this.bntingresar);
            this.Controls.Add(this.txtcontraseña);
            this.Controls.Add(this.txtusuario);
            this.Name = "Formlogin";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Formlogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtusuario;
        private System.Windows.Forms.TextBox txtcontraseña;
        private System.Windows.Forms.Button bntingresar;
        private System.Windows.Forms.Label usuario;
        private System.Windows.Forms.Label contraseña;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

