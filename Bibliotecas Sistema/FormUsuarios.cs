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
    public partial class FormUsuarios : Form
    {
        // Mantener la tabla en memoria para detectar cambios (añadidos/modificados)
        private DataTable usuariosTable;

        public FormUsuarios()
        {
            InitializeComponent();

        }

        

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            MostrarUsuarios();
        }
        void MostrarUsuarios()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Usuarios", Conexion.cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                usuariosTable = dt;
                dgvusuarios.DataSource = usuariosTable;

                // Opcional: Ocultar el ID para que el usuario no lo edite por error
                if (dgvusuarios.Columns.Contains("Id_Usuario"))
                    dgvusuarios.Columns["Id_Usuario"].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Usar los cambios del DataTable para insertar sólo las filas añadidas
                if (usuariosTable == null)
                {
                    MessageBox.Show("No hay datos para guardar.");
                    return;
                }

                var added = usuariosTable.GetChanges(DataRowState.Added);
                if (added == null || added.Rows.Count == 0)
                {
                    MessageBox.Show("No hay nuevos registros para guardar.");
                    return;
                }

                Conexion.cn.Open();
                using (var tran = Conexion.cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in added.Rows)
                        {
                            string query = "INSERT INTO Tbl_Usuarios (Dni, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido) " +
                                           "VALUES (@dni, @pnom, @snom, @pape, @sape)";

                            using (var cmd = new SqlCommand(query, Conexion.cn, tran))
                            {
                                cmd.Parameters.AddWithValue("@dni", row.Table.Columns.Contains("Dni") ? row["Dni"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@pnom", row.Table.Columns.Contains("PrimerNombre") ? row["PrimerNombre"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@snom", row.Table.Columns.Contains("SegundoNombre") ? row["SegundoNombre"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@pape", row.Table.Columns.Contains("PrimerApellido") ? row["PrimerApellido"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@sape", row.Table.Columns.Contains("SegundoApellido") ? row["SegundoApellido"] ?? "" : "");

                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                        MessageBox.Show("Nuevos registros guardados correctamente.");
                        // Aceptar cambios y recargar
                        usuariosTable.AcceptChanges();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
            finally
            {
                if (Conexion.cn.State == System.Data.ConnectionState.Open)
                    Conexion.cn.Close();
                MostrarUsuarios();
            }
        }

        private void dgvusuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void bnteliminar_Click(object sender, EventArgs e)
        {
            if (dgvusuarios.CurrentRow == null || dgvusuarios.CurrentRow.IsNewRow) return;

            DialogResult result = MessageBox.Show("¿Seguro que desea eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            try
            {
                // CAMBIO AQUÍ: Usamos .Columns.Contains en lugar de .Cells.Contains
                if (!dgvusuarios.Columns.Contains("Id_Usuario"))
                {
                    MessageBox.Show("La tabla no contiene la columna Id_Usuario.");
                    return;
                }

                // Asegurarse de usar .Value para obtener el dato
                var valorId = dgvusuarios.CurrentRow.Cells["Id_Usuario"].Value;

                if (valorId == null || valorId == DBNull.Value)
                {
                    MessageBox.Show("El usuario seleccionado no tiene un ID válido.");
                    return;
                }

                int id = Convert.ToInt32(valorId);

                Conexion.cn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Usuarios WHERE Id_Usuario=@id", Conexion.cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Usuario eliminado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
            finally
            {
                if (Conexion.cn.State == System.Data.ConnectionState.Open)
                    Conexion.cn.Close();

                MostrarUsuarios();
            }
        }

        private void bnteditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Actualizar sólo las filas modificadas en el DataTable
                if (usuariosTable == null)
                {
                    MessageBox.Show("No hay datos para actualizar.");
                    return;
                }

                var modified = usuariosTable.GetChanges(DataRowState.Modified);
                if (modified == null || modified.Rows.Count == 0)
                {
                    MessageBox.Show("No hay cambios para actualizar.");
                    return;
                }

                Conexion.cn.Open();
                using (var tran = Conexion.cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in modified.Rows)
                        {
                            string query = "UPDATE Tbl_Usuarios SET Dni=@dni, PrimerNombre=@pnom, SegundoNombre=@snom, " +
                                           "PrimerApellido=@pape, SegundoApellido=@sape WHERE Id_Usuario=@id";

                            using (var cmd = new SqlCommand(query, Conexion.cn, tran))
                            {
                                cmd.Parameters.AddWithValue("@id", row["Id_Usuario"]);
                                cmd.Parameters.AddWithValue("@dni", row.Table.Columns.Contains("Dni") ? row["Dni"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@pnom", row.Table.Columns.Contains("PrimerNombre") ? row["PrimerNombre"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@snom", row.Table.Columns.Contains("SegundoNombre") ? row["SegundoNombre"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@pape", row.Table.Columns.Contains("PrimerApellido") ? row["PrimerApellido"] ?? "" : "");
                                cmd.Parameters.AddWithValue("@sape", row.Table.Columns.Contains("SegundoApellido") ? row["SegundoApellido"] ?? "" : "");

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        usuariosTable.AcceptChanges();
                        MessageBox.Show("Datos actualizados correctamente.");
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
            finally
            {
                Conexion.cn.Close();
                MostrarUsuarios();
            }
        }

        private void dgvusuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
