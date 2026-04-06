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
    public partial class Formmisprestamos : Form
    {
        public Formmisprestamos()
        {
            InitializeComponent();
        }
        void MostrarMisPrestamos()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT P.IdPrestamo, L.Titulo, P.Estado, P.FechaPrestamo " +
                "FROM Tbl_Prestamos P " +
                "INNER JOIN Tbl_Libros L ON P.IdLibro = L.IdLibro " +
                "WHERE P.IdUsuario = @IdUsuario",
                Conexion.cn);

            da.SelectCommand.Parameters.AddWithValue("@IdUsuario", Formlogin.IdUsuarioLogueado);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvPrestamos.DataSource = dt;
        }

        private void Formmisprestamos_Load(object sender, EventArgs e)
        {
            MostrarMisPrestamos();
        }
    }
}
