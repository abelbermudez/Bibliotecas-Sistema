using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bibliotecas_Sistema
{
    public partial class Formlibros : Form
    {
        DataTable dtAutores = new DataTable();
        DataTable dtCategorias = new DataTable();
        public Formlibros()
        {
            InitializeComponent();
        }

        private void dgvlibros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Formlibros_Load(object sender, EventArgs e)
        {
            Mostrarlibros();
            CargarAutores();
            CargarCategorias();
            AgregarCombos();
        }
        void Mostrarlibros()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Libros", Conexion.cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvlibros.DataSource = dt;
        }
        void CargarAutores()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT IdAutor, PrimerNombre FROM Tbl_Autores", Conexion.cn);
            dtAutores.Clear();
            da.Fill(dtAutores);
        }

        void CargarCategorias()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT IdCategoria, Nombre FROM Tbl_Categorias", Conexion.cn);
            dtCategorias.Clear();
            da.Fill(dtCategorias);
        }
        void AgregarCombos()
        {
            dgvlibros.Columns.Remove("IdAutor");
            dgvlibros.Columns.Remove("IdCategoria");

            DataGridViewComboBoxColumn cmbAutor = new DataGridViewComboBoxColumn();
            cmbAutor.DataSource = dtAutores;
            cmbAutor.DisplayMember = "PrimerNombre";
            cmbAutor.ValueMember = "IdAutor";
            cmbAutor.DataPropertyName = "IdAutor";
            cmbAutor.HeaderText = "Autor";

            dgvlibros.Columns.Add(cmbAutor);

            DataGridViewComboBoxColumn cmbCategoria = new DataGridViewComboBoxColumn();
            cmbCategoria.DataSource = dtCategorias;
            cmbCategoria.DisplayMember = "Nombre";
            cmbCategoria.ValueMember = "IdCategoria";
            cmbCategoria.DataPropertyName = "IdCategoria";
            cmbCategoria.HeaderText = "Categoría";

            dgvlibros.Columns.Add(cmbCategoria);
        }
    }
}
