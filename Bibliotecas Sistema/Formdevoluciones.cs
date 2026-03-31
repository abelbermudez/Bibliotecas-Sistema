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
    public partial class Formdevoluciones : Form
    {
        public Formdevoluciones()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void MostrarPendientes()
        {

            SqlDataAdapter da = new SqlDataAdapter(
        "SELECT P.IdPrestamo, U.PrimerNombre AS Usuario, L.Titulo AS Libro, P.IdLibro " +
        "FROM Tbl_Prestamos P " +
        "INNER JOIN Tbl_Usuarios U ON P.IdUsuario = U.Id_Usuario " +
        "INNER JOIN Tbl_Libros L ON P.IdLibro = L.IdLibro " +
        "WHERE P.Estado = 'PRESTADO'",
        Conexion.cn);

            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvdevo.DataSource = dt;
        }

        private void dgvdevolver_Click(object sender, EventArgs e)
        {

        }

        private void Formdevoluciones_Load(object sender, EventArgs e)
        {
            MostrarPendientes();
        }
    }
}
