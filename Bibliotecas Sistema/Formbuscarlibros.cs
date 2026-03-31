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
    public partial class Formbuscarlibros : Form
    {
        public Formbuscarlibros()
        {
            InitializeComponent();
            // Asegurar que el TextBox esté visible sobre otros controles
            txtbuscar?.BringToFront();

            // Cargar todos los libros inicialmente (opcional)
            try { BuscarLibros(""); } catch { }
        }
        void BuscarLibros(string texto)
        {
            try
            {
                // Asegurar que el grid genere columnas según el resultado
                dgvBuscar.AutoGenerateColumns = true;

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Tbl_Libros WHERE Titulo LIKE @txt",
                    Conexion.cn);

                da.SelectCommand.Parameters.AddWithValue("@txt", "%" + texto + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvBuscar.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    // Opcional: informar que no hay resultados
                    // MessageBox.Show("No se encontraron libros para la búsqueda.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar libros: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Usar el texto del textbox de búsqueda, no el texto del botón
            BuscarLibros((txtbuscar.Text ?? string.Empty).Trim());
        }
    }
}
