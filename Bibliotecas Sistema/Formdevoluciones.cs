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
            dgvdevo.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        void MostrarPendientes()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT P.IdPrestamo, P.IdUsuario, P.IdLibro, " +
                    "PF.PrimerNombre + ' ' + PF.PrimerApellido AS Usuario, " +
                    "L.Titulo " +
                    "FROM Tbl_Prestamos P " +
                    "INNER JOIN Tbl_Perfiles PF ON P.IdUsuario = PF.IdPerfil " +
                    "INNER JOIN Tbl_Libros L ON P.IdLibro = L.IdLibro " +
                    "WHERE P.Estado = 'PRESTADO'",
                    Conexion.cn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvdevo.DataSource = dt;

                // 🔥 OCULTAR IDS
                dgvdevo.Columns["IdUsuario"].Visible = false;
                dgvdevo.Columns["IdLibro"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            dgvdevo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        
        
        

        private void dgvdevolver_Click(object sender, EventArgs e)
        {
            if (dgvdevo.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un préstamo");
                return;
            }

            // 🔴 VALIDAR COLUMNAS
            if (!dgvdevo.Columns.Contains("IdPrestamo") || !dgvdevo.Columns.Contains("IdLibro"))
            {
                MessageBox.Show("Faltan columnas necesarias");
                return;
            }

            object valPrestamo = dgvdevo.CurrentRow.Cells["IdPrestamo"].Value;
            object valLibro = dgvdevo.CurrentRow.Cells["IdLibro"].Value;

            if (valPrestamo == null || valLibro == null)
            {
                MessageBox.Show("Datos incompletos");
                return;
            }

            int idPrestamo;
            int idLibro;

            if (!int.TryParse(valPrestamo.ToString(), out idPrestamo))
            {
                MessageBox.Show("IdPrestamo inválido");
                return;
            }

            if (!int.TryParse(valLibro.ToString(), out idLibro))
            {
                MessageBox.Show("IdLibro inválido");
                return;
            }

            var confirm = MessageBox.Show("¿Desea devolver este libro?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        // 🔥 VALIDAR QUE NO ESTÉ DEVUELTO
                        using (SqlCommand check = new SqlCommand(
                            "SELECT Estado FROM Tbl_Prestamos WHERE IdPrestamo=@id",
                            cn, tran))
                        {
                            check.Parameters.AddWithValue("@id", idPrestamo);
                            var estado = check.ExecuteScalar();

                            if (estado != null && estado.ToString() == "DEVUELTO")
                            {
                                MessageBox.Show("Este préstamo ya fue devuelto");
                                return;
                            }
                        }

                        // 🔥 ACTUALIZAR PRÉSTAMO
                        using (SqlCommand cmd = new SqlCommand(
                            "UPDATE Tbl_Prestamos SET Estado='DEVUELTO', FechaDevolucionReal=GETDATE() WHERE IdPrestamo=@id",
                            cn, tran))
                        {
                            cmd.Parameters.AddWithValue("@id", idPrestamo);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔥 DEVOLVER STOCK
                        using (SqlCommand cmdStock = new SqlCommand(
                            "UPDATE Tbl_Libros SET StockTotal = ISNULL(StockTotal,0) + 1 WHERE IdLibro=@id",
                            cn, tran))
                        {
                            cmdStock.Parameters.AddWithValue("@id", idLibro);
                            cmdStock.ExecuteNonQuery();
                        }

                        tran.Commit();
                        MessageBox.Show("Libro devuelto correctamente");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error al devolver: " + ex.Message);
                        return;
                    }
                }

                MostrarPendientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }

        private void Formdevoluciones_Load(object sender, EventArgs e)
        {
            MostrarPendientes();// 3
            
        }
    }
}
