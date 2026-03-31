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
        }
        void MostrarPrestamos()
        {
            SqlDataAdapter da = new SqlDataAdapter(
        "SELECT P.IdPrestamo, P.IdLibro, P.IdUsuario, L.Titulo, P.Estado " +
        "FROM Tbl_Prestamos P " +
        "INNER JOIN Tbl_Libros L ON P.IdLibro = L.IdLibro",
        Conexion.cn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvprestamos.DataSource = dt;
        }

        private void FormPrestamos_Load(object sender, EventArgs e)
        {
            MostrarPrestamos();
            CargarUsuarios();
            CargarLibros();
            AgregarCombos();
        }
        void CargarUsuarios()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Usuario, PrimerNombre FROM Tbl_Usuarios", Conexion.cn);
            dtUsuarios.Clear();
            da.Fill(dtUsuarios);
        }
        void CargarLibros()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT IdLibro, Titulo FROM Tbl_Libros", Conexion.cn);
            dtLibros.Clear();
            da.Fill(dtLibros);
        }
        void AgregarCombos()
        {
            dgvprestamos.Columns.Remove("IdUsuario");
            dgvprestamos.Columns.Remove("IdLibro");

            // Usuario
            DataGridViewComboBoxColumn cmbUsuario = new DataGridViewComboBoxColumn();
            cmbUsuario.DataSource = dtUsuarios;
            cmbUsuario.DisplayMember = "PrimerNombre";
            cmbUsuario.ValueMember = "Id_Usuario";
            cmbUsuario.DataPropertyName = "IdUsuario";
            cmbUsuario.HeaderText = "Usuario";

            dgvprestamos.Columns.Add(cmbUsuario);

            // Libro
            DataGridViewComboBoxColumn cmbLibro = new DataGridViewComboBoxColumn();
            cmbLibro.DataSource = dtLibros;
            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "IdLibro";
            cmbLibro.DataPropertyName = "IdLibro";
            cmbLibro.HeaderText = "Libro";

            dgvprestamos.Columns.Add(cmbLibro);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Conexion.cn.Open();

            foreach (DataGridViewRow row in dgvprestamos.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["IdPrestamo"].Value == null)
                {
                    
                    if (row.Cells["IdLibro"].Value == null || row.Cells["IdLibro"].Value.ToString() == "")
                    {
                        MessageBox.Show("Seleccione un libro válido");
                        continue;
                    }

                    
                    if (row.Cells["IdUsuario"].Value == null || row.Cells["IdUsuario"].Value.ToString() == "")
                    {
                        MessageBox.Show("Seleccione un usuario válido");
                        continue;
                    }

                    int idLibro;
                    int idUsuario;

                    
                    if (!int.TryParse(row.Cells["IdLibro"].Value.ToString(), out idLibro))
                    {
                        MessageBox.Show("IdLibro inválido");
                        continue;
                    }

                    if (!int.TryParse(row.Cells["IdUsuario"].Value.ToString(), out idUsuario))
                    {
                        MessageBox.Show("IdUsuario inválido");
                        continue;
                    }

                    
                    if (!HayStock(idLibro))
                    {
                        MessageBox.Show("No hay stock disponible para este libro");
                        continue;
                    }

                    
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Tbl_Prestamos (IdLibro, IdUsuario, Estado) VALUES (@libro, @usuario, 'PRESTADO')",
                        Conexion.cn);

                    cmd.Parameters.AddWithValue("@libro", idLibro);
                    cmd.Parameters.AddWithValue("@usuario", idUsuario);

                    cmd.ExecuteNonQuery();

                    SqlCommand cmdStock = new SqlCommand(
                        "UPDATE Tbl_Libros SET StockTotal = StockTotal - 1 WHERE IdLibro=@id",
                        Conexion.cn);

                    cmdStock.Parameters.AddWithValue("@id", idLibro);
                    cmdStock.ExecuteNonQuery();
                }
            }
        }
        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvprestamos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un préstamo");
                return;
            }

            if (dgvprestamos.CurrentRow.Cells["IdPrestamo"].Value == null ||
                dgvprestamos.CurrentRow.Cells["IdLibro"].Value == null)
            {
                MessageBox.Show("Datos incompletos");
                return;
            }

            int idPrestamo = Convert.ToInt32(dgvprestamos.CurrentRow.Cells["IdPrestamo"].Value);
            int idLibro = Convert.ToInt32(dgvprestamos.CurrentRow.Cells["IdLibro"].Value);

            Conexion.cn.Open();

            // ACTUALIZAR PRÉSTAMO
            SqlCommand cmd = new SqlCommand(
                "UPDATE Tbl_Prestamos SET Estado='DEVUELTO', FechaDevolucionReal=GETDATE() WHERE IdPrestamo=@id",
                Conexion.cn);

            cmd.Parameters.AddWithValue("@id", idPrestamo);
            cmd.ExecuteNonQuery();

            // DEVOLVER STOCK
            SqlCommand cmdStock = new SqlCommand(
                "UPDATE Tbl_Libros SET StockTotal = StockTotal + 1 WHERE IdLibro=@id",
                Conexion.cn);

            cmdStock.Parameters.AddWithValue("@id", idLibro);
            cmdStock.ExecuteNonQuery();

            Conexion.cn.Close();
            MessageBox.Show("Libro devuelto");
            MostrarPrestamos();
        }
        bool HayStock(int idLibro)
        {
            int stock = 0;

            SqlCommand cmd = new SqlCommand("SELECT StockTotal FROM Tbl_Libros WHERE IdLibro=@id", Conexion.cn);
            cmd.Parameters.AddWithValue("@id", idLibro);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                stock = Convert.ToInt32(dr["StockTotal"]);
            }

            dr.Close();

            return stock > 0;
        }
    }
}
