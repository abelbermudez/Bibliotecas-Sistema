using System;
using System.Windows.Forms;

namespace Bibliotecas_Sistema
{
    public class FormUsuarioEditor : Form
    {
        private Button btnOk;
        private Button btnCancel;

        // Mantener propiedades para compatibilidad con FormUsuarios
        

        public FormUsuarioEditor()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Usuario";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new System.Drawing.Size(240, 100);

            btnOk = new Button { Text = "OK", Left = 40, Width = 70, Top = 30, DialogResult = DialogResult.OK };
            btnOk.Click += BtnOk_Click;
            btnCancel = new Button { Text = "Cancelar", Left = 130, Width = 70, Top = 30, DialogResult = DialogResult.Cancel };

            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            // No hay validación de campos porque no existen controles en el diálogo.
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormUsuarioEditor
            // 
            this.ClientSize = new System.Drawing.Size(977, 253);
            this.Name = "FormUsuarioEditor";
            this.ResumeLayout(false);

        }
    }
}
