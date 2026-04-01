using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Bibliotecas_Sistema
{
    public partial class FormAdmin : Form
    {
        Form _LoginForm = new Form();
        public FormAdmin(Form LoginForm)
        {
            InitializeComponent();
            _LoginForm = LoginForm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            FormUsuarios f = new FormUsuarios();
            f.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Formlibros f = new Formlibros();
            f.Show();
        }

        private void pbusuario_Click(object sender, EventArgs e)
        {
            FormUsuarios f = new FormUsuarios();
            f.Show();


        }

        private void label3_Click(object sender, EventArgs e)
        {
            Formlibros f = new Formlibros();
            f.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormPrestamos f = new FormPrestamos();
            f.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            FormPrestamos f = new FormPrestamos();
            f.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Formdevoluciones f = new Formdevoluciones();
            f.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Formdevoluciones f = new Formdevoluciones();
            f.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Formlectores f = new Formlectores();
            f.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Formlectores f = new Formlectores();
            f.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Formcategoria f = new Formcategoria();
            f.Show();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Formacercade f = new Formacercade();
            f.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Formacercade f = new Formacercade();
            f.Show();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            this.Close();
            _LoginForm.Show();
        }
    }

}