using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bibliotecas_Sistema
{
    public partial class FormUsuario : Form
    {
        Form _loginForm = new Form();
        public FormUsuario(Form loginForm)
        {
            InitializeComponent();
            _loginForm = loginForm;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Formlibros f = new Formlibros();
            f.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Formbuscarlibros f = new Formbuscarlibros();
            f.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Formmisprestamos f = new Formmisprestamos();
            f.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Formacercade f = new Formacercade();
            f.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            this.Close();
            _loginForm.Show();
        }
    }
}
