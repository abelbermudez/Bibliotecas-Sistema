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
            // Evitar excepción en tiempo de ejecución si hay valores en la celda
            // que no existen en la lista del ComboBox (DataGridViewComboBoxCell)
            this.dgvprestamos.DataError += Dgvprestamos_DataError;
        }

        private void Dgvprestamos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Ignorar errores de datos en la cuadrícula (por ejemplo, valor de celda no presente en ComboBox)
            e.ThrowException = false;
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
            // Load lookup data first, then load prestamos and add combo columns
            CargarUsuarios();
            CargarLibros();
            MostrarPrestamos();
            AgregarCombos();
        }
        void CargarUsuarios()
        {
            try
            {
                dtUsuarios.Clear();
                using (SqlConnection cn = Conexion.GetOpenConnection())
                {
                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT IdPerfil, PrimerNombre + ' ' + PrimerApellido AS Nombre FROM Tbl_Perfiles",
                        cn);

                    da.Fill(dtUsuarios);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }
        void CargarLibros()
        {
            try
            {
                dtLibros.Clear();
                using (SqlConnection cn = Conexion.GetOpenConnection())
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT IdLibro, Titulo FROM Tbl_Libros", cn);
                    da.Fill(dtLibros);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void AgregarCombos()
        {
            // Remove any existing combo columns we added previously
            for (int i = dgvprestamos.Columns.Count - 1; i >= 0; i--)
            {
                var col = dgvprestamos.Columns[i];
                if (string.Equals(col.Name, "cmbUsuario", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.Name, "cmbLibro", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.DataPropertyName, "IdUsuario", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.DataPropertyName, "IdLibro", StringComparison.OrdinalIgnoreCase))
                {
                    dgvprestamos.Columns.RemoveAt(i);
                }
            }

            // Usuario combo
            DataGridViewComboBoxColumn cmbUsuario = new DataGridViewComboBoxColumn();
            cmbUsuario.Name = "cmbUsuario";
            cmbUsuario.DataSource = dtUsuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "IdPerfil";
            cmbUsuario.ValueType = typeof(int);
            cmbUsuario.DataPropertyName = "IdUsuario";
            cmbUsuario.HeaderText = "Usuario";
            cmbUsuario.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            try { dgvprestamos.Columns.Add(cmbUsuario); } catch { /* ignore add errors */ }

            // Libro combo
            DataGridViewComboBoxColumn cmbLibro = new DataGridViewComboBoxColumn();
            cmbLibro.Name = "cmbLibro";
            cmbLibro.DataSource = dtLibros;
            cmbLibro.DisplayMember = "Titulo";
            cmbLibro.ValueMember = "IdLibro";
            cmbLibro.ValueType = typeof(int);
            cmbLibro.DataPropertyName = "IdLibro";
            cmbLibro.HeaderText = "Libro";
            cmbLibro.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            try { dgvprestamos.Columns.Add(cmbLibro); } catch { /* ignore add errors */ }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            dgvprestamos.EndEdit(); // 🔥 IMPORTANTE

            foreach (DataGridViewRow row in dgvprestamos.Rows)
            {
                if (row.IsNewRow) continue;

                // 🔴 VALIDAR LIBRO
                object valLibro = row.Cells["cmbLibro"].Value;
                if (valLibro == null || valLibro.ToString() == "")
                {
                    MessageBox.Show("Seleccione un libro válido");
                    continue;
                }

                // 🔴 VALIDAR USUARIO (PERFIL)
                object valUsuario = row.Cells["cmbUsuario"].Value;
                if (valUsuario == null || valUsuario.ToString() == "")
                {
                    MessageBox.Show("Seleccione un usuario válido");
                    continue;
                }

                int idLibro;
                int idUsuario;

                // 🔴 VALIDAR NÚMEROS
                if (!int.TryParse(valLibro.ToString(), out idLibro))
                {
                    MessageBox.Show("IdLibro inválido");
                    continue;
                }

                if (!int.TryParse(valUsuario.ToString(), out idUsuario))
                {
                    MessageBox.Show("Usuario inválido");
                    continue;
                }

                // 🔥 VALIDAR STOCK
                if (!HayStock(idLibro))
                {
                    MessageBox.Show("No hay stock disponible");
                    continue;
                }

                // 🔥 GUARDAR CON TRANSACCIÓN
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        // INSERTAR PRÉSTAMO
                        using (SqlCommand cmd = new SqlCommand(
                            "INSERT INTO Tbl_Prestamos (IdLibro, IdUsuario, Estado) VALUES (@libro, @usuario, 'PRESTADO')",
                            cn, tran))
                        {
                            cmd.Parameters.AddWithValue("@libro", idLibro);
                            cmd.Parameters.AddWithValue("@usuario", idUsuario); // 👉 ESTE ES IdPerfil
                            cmd.ExecuteNonQuery();
                        }

                        // ACTUALIZAR STOCK
                        using (SqlCommand cmdStock = new SqlCommand(
                            "UPDATE Tbl_Libros SET StockTotal = StockTotal - 1 WHERE IdLibro=@id",
                            cn, tran))
                        {
                            cmdStock.Parameters.AddWithValue("@id", idLibro);
                            cmdStock.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error al guardar: " + ex.Message);
                        return;
                    }
                }
            }

            MessageBox.Show("Préstamos guardados correctamente");
            MostrarPrestamos();

        }
        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (dgvprestamos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un préstamo");
                return;
            }

            // 🔴 VALIDAR DATOS
            object valPrestamo = dgvprestamos.CurrentRow.Cells["IdPrestamo"].Value;
            object valLibro = dgvprestamos.CurrentRow.Cells["IdLibro"].Value;

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

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
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
                            "UPDATE Tbl_Libros SET StockTotal = StockTotal + 1 WHERE IdLibro=@id",
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
                        MessageBox.Show("Error en la devolución: " + ex.Message);
                        return;
                    }
                }

                MostrarPrestamos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error general: " + ex.Message);
            }
        }
        bool HayStock(int idLibro)
        {
            int stock = 0;

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT StockTotal FROM Tbl_Libros WHERE IdLibro=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", idLibro);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        stock = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar stock: " + ex.Message);
                return false;
            }

            return stock > 0;
        }
    }
}
