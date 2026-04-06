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
    public partial class FormPrestamos : Form
    {
        DataTable dtUsuarios = new DataTable();
        DataTable dtLibros = new DataTable();

        public FormPrestamos()
        {
            InitializeComponent();
            this.dgvprestamos.DataError += Dgvprestamos_DataError;
            dgvprestamos.DefaultValuesNeeded += dgvprestamos_DefaultValuesNeeded;
        }

        private void Dgvprestamos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        void MostrarPrestamos()
        {
            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT 
            P.IdPrestamo, 
            P.IdLibro, 
            P.IdUsuario, 
            P.IdUsuarioAtendio,
            P.FechaPrestamo,
            P.FechaDevolucionEsperada,
            P.FechaDevolucionReal,
            P.Estado
          FROM Tbl_Prestamos P",
            Conexion.cn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvprestamos.DataSource = dt;
        }

        private void FormPrestamos_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarLibros();
            MostrarPrestamos();

            // Formato de fechas
            dgvprestamos.Columns["FechaPrestamo"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvprestamos.Columns["FechaDevolucionEsperada"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvprestamos.Columns["FechaDevolucionReal"].DefaultCellStyle.Format = "yyyy-MM-dd";

            // Ocultar IDs
            dgvprestamos.Columns["IdLibro"].Visible = false;
            dgvprestamos.Columns["IdUsuario"].Visible = false;
            dgvprestamos.Columns["IdUsuarioAtendio"].Visible = false;

            AgregarCombos();
        }

        private void dgvprestamos_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["FechaPrestamo"].Value = DateTime.Now;
            e.Row.Cells["FechaDevolucionEsperada"].Value = DateTime.Now.AddDays(7);
            e.Row.Cells["Estado"].Value = "PRESTADO";
        }

        void CargarUsuarios()
        {
            dtUsuarios.Clear();
            using (SqlConnection cn = Conexion.GetOpenConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT 
            U.IdUsuario,
            P.PrimerNombre + ' ' + P.PrimerApellido AS Nombre
          FROM Tbl_Usuarios U
          INNER JOIN Tbl_Perfiles P ON U.IdUsuario = P.IdUsuario",
                cn);

                da.Fill(dtUsuarios);
            }
        }


        void CargarLibros()
        {
            dtLibros.Clear();
            using (SqlConnection cn = Conexion.GetOpenConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT IdLibro, Titulo FROM Tbl_Libros", cn);
                da.Fill(dtLibros);
            }
        }

        void AgregarCombos()
        {
            // Eliminar combos si ya existen
            for (int i = dgvprestamos.Columns.Count - 1; i >= 0; i--)
            {
                var col = dgvprestamos.Columns[i];
                if (col.Name == "cmbUsuario" || col.Name == "cmbLibro")
                {
                    dgvprestamos.Columns.RemoveAt(i);
                }
            }

            // Combo Usuario
            DataGridViewComboBoxColumn cmbUsuario = new DataGridViewComboBoxColumn();
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.DataSource = dtUsuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "IdUsuario"; 
            cmbUsuario.DataPropertyName = "IdUsuario";
            cmbUsuario.HeaderText = "Usuario";
            dgvprestamos.Columns.Add(cmbUsuario);

            // Combo Libro
            DataGridViewComboBoxColumn cmbLibro = new DataGridViewComboBoxColumn();
            cmbLibro.Name = "cmbLibro";
            cmbLibro.DataSource = dtLibros;
            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "IdLibro";
            cmbLibro.DataPropertyName = "IdLibro";
            cmbLibro.HeaderText = "Libro";
            dgvprestamos.Columns.Add(cmbLibro);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            dgvprestamos.EndEdit();

            if (Formlogin.IdUsuarioLogueado == 0)
            {
                MessageBox.Show("Debe iniciar sesión nuevamente");
                return;
            }

            if (Formlogin.RolUsuario != "ADMIN")
            {
                MessageBox.Show("No tiene permisos");
                return;
            }

            foreach (DataGridViewRow row in dgvprestamos.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["IdPrestamo"].Value != DBNull.Value) continue;

                object valLibro = row.Cells["cmbLibro"].Value;
                object valUsuario = row.Cells["cmbUsuario"].Value;
                object valFechaDev = row.Cells["FechaDevolucionEsperada"].Value;

                if (valLibro == null || valUsuario == null)
                {
                    MessageBox.Show("Seleccione libro y usuario");
                    continue;
                }

                int idLibro = Convert.ToInt32(valLibro);
                int idUsuario = Convert.ToInt32(valUsuario);

                DateTime fechaDevolucion;
                if (valFechaDev == null || valFechaDev == DBNull.Value)
                    fechaDevolucion = DateTime.Now.AddDays(7);
                else
                    fechaDevolucion = Convert.ToDateTime(valFechaDev);

                int idUsuarioAtendio = Formlogin.IdUsuarioLogueado;

                if (!HayStock(idLibro))
                {
                    MessageBox.Show("No hay stock disponible");
                    continue;
                }

                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlTransaction tran = cn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Tbl_Prestamos
                    (IdLibro, IdUsuario, IdUsuarioAtendio, FechaPrestamo, FechaDevolucionEsperada, Estado)
                    VALUES (@libro, @usuario, @atendio, GETDATE(), @fechaDev, 'PRESTADO')",
                        cn, tran);

                        cmd.Parameters.AddWithValue("@libro", idLibro);
                        cmd.Parameters.AddWithValue("@usuario", idUsuario);
                        cmd.Parameters.AddWithValue("@atendio", idUsuarioAtendio);
                        cmd.Parameters.Add("@fechaDev", SqlDbType.Date).Value = fechaDevolucion;

                        cmd.ExecuteNonQuery();

                        SqlCommand cmdStock = new SqlCommand(
                            "UPDATE Tbl_Libros SET StockTotal = StockTotal - 1 WHERE IdLibro=@id",
                            cn, tran);

                        cmdStock.Parameters.AddWithValue("@id", idLibro);
                        cmdStock.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                        return;
                    }
                }
            }

            MessageBox.Show("Préstamos guardados correctamente");
            MostrarPrestamos();
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvprestamos.CurrentRow == null) return;

            string estadoActual = dgvprestamos.CurrentRow.Cells["Estado"].Value.ToString();

            if (estadoActual == "DEVUELTO" || estadoActual == "VENCIDO")
            {
                MessageBox.Show("Este préstamo ya fue devuelto");
                return;
            }

            int idPrestamo = Convert.ToInt32(dgvprestamos.CurrentRow.Cells["IdPrestamo"].Value);
            int idLibro = Convert.ToInt32(((DataRowView)dgvprestamos.CurrentRow.DataBoundItem)["IdLibro"]);

            object valFecha = dgvprestamos.CurrentRow.Cells["FechaDevolucionEsperada"].Value;

            DateTime fechaEsperada;
            if (valFecha == null || valFecha == DBNull.Value)
                fechaEsperada = DateTime.Now.AddDays(7);
            else
                fechaEsperada = Convert.ToDateTime(valFecha);

            DateTime hoy = DateTime.Today;
            string estado = "DEVUELTO";

            if (hoy > fechaEsperada)
            {
                estado = "VENCIDO";
                MessageBox.Show("El libro se devuelve con retraso");
            }

            using (SqlConnection cn = Conexion.GetOpenConnection())
            using (var tran = cn.BeginTransaction())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Tbl_Prestamos SET Estado=@estado, FechaDevolucionReal=GETDATE() WHERE IdPrestamo=@id",
                        cn, tran);

                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@id", idPrestamo);
                    cmd.ExecuteNonQuery();

                    SqlCommand cmdStock = new SqlCommand(
                        "UPDATE Tbl_Libros SET StockTotal = StockTotal + 1 WHERE IdLibro=@id",
                        cn, tran);

                    cmdStock.Parameters.AddWithValue("@id", idLibro);
                    cmdStock.ExecuteNonQuery();

                    tran.Commit();
                    MessageBox.Show("Libro devuelto correctamente");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            MostrarPrestamos();
        }

        bool HayStock(int idLibro)
        {
            int stock = 0;

            using (SqlConnection cn = Conexion.GetOpenConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT StockTotal FROM Tbl_Libros WHERE IdLibro=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idLibro);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                    stock = Convert.ToInt32(result);
            }

            return stock > 0;
        }
    }
}
