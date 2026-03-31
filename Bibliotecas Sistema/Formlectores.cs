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
    public partial class Formlectores : Form
    {
        private DataTable lectoresTable;
        private BindingSource lectoresBinding;

        public Formlectores()
        {
            InitializeComponent();
        }

        void MostrarLectores()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Usuarios", Conexion.cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            lectoresTable = dt;
            dgvlectores.AutoGenerateColumns = true;
            dgvlectores.AllowUserToAddRows = true;
            dgvlectores.ReadOnly = false;

            lectoresBinding = new BindingSource();
            lectoresBinding.DataSource = lectoresTable;
            dgvlectores.DataSource = lectoresBinding;
        }

        private void Formlectores_Load(object sender, EventArgs e)
        {
            MostrarLectores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lectoresTable == null)
                {
                    MessageBox.Show("No hay datos cargados.");
                    return;
                }
                // Ensure the grid commits any current edits into the underlying DataTable
                dgvlectores.EndEdit();
                dgvlectores.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (lectoresBinding != null)
                    lectoresBinding.EndEdit();
                else if (this.BindingContext[lectoresTable] != null)
                    this.BindingContext[lectoresTable].EndCurrentEdit();

                var added = lectoresTable.GetChanges(DataRowState.Added);
                if (added == null || added.Rows.Count == 0)
                {
                    MessageBox.Show("No hay nuevas filas para guardar.");
                    return;
                }

                // Check required columns exist in the datatable
                string[] required = new[] { "Dni", "PrimerNombre", "SegundoNombre", "PrimerApellido", "SegundoApellido", "NumeroDocumento", "Correo", "Telefono", "Direccion", "FechaNacimiento" };
                foreach (var col in required)
                {
                    if (!lectoresTable.Columns.Contains(col))
                    {
                        MessageBox.Show($"La columna '{col}' no existe en la tabla de datos. Verifique los nombres de columnas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in added.Rows)
                        {
                            string query = @"INSERT INTO Tbl_Usuarios 
                                (Dni, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, NumeroDocumento, Correo, Telefono, Direccion, FechaNacimiento) 
                                VALUES (@dni, @nom, @nom2, @ape, @ape2, @nc, @co, @tel, @di, @fc)";

                            using (SqlCommand cmd = new SqlCommand(query, cn, tran))
                            {
                                cmd.Parameters.AddWithValue("@dni", row["Dni"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@nom", row["PrimerNombre"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@nom2", row["SegundoNombre"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ape", row["PrimerApellido"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ape2", row["SegundoApellido"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@nc", row["NumeroDocumento"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@co", row["Correo"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@tel", row["Telefono"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@di", row["Direccion"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@fc", row["FechaNacimiento"] ?? DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        lectoresTable.AcceptChanges();
                        MessageBox.Show("Guardado correctamente");
                    }
                    catch (Exception ex)
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
            finally { MostrarLectores(); }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Update existing records using the DataTable change tracking
            try
            {
                if (lectoresTable == null)
                {
                    MessageBox.Show("No hay datos cargados para actualizar.");
                    return;
                }

                // Ensure any current edit in the grid is committed to the DataTable
                dgvlectores.EndEdit();

                var modified = lectoresTable.GetChanges(DataRowState.Modified);
                if (modified == null || modified.Rows.Count == 0)
                {
                    MessageBox.Show("No hay cambios para actualizar.");
                    return;
                }

                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (var tran = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in modified.Rows)
                        {
                            string query = "UPDATE Tbl_Usuarios SET Dni=@dni, PrimerNombre=@pnom, SegundoNombre=@snom, PrimerApellido=@pape, SegundoApellido=@sape, NumeroDocumento=@nc, Correo=@co, Telefono=@tel, Direccion=@di, FechaNacimiento=@fc WHERE Id_Usuario=@id";
                            using (var cmd = new SqlCommand(query, cn, tran))
                            {
                                cmd.Parameters.AddWithValue("@id", row["Id_Usuario"]);
                                cmd.Parameters.AddWithValue("@dni", lectoresTable.Columns.Contains("Dni") ? (row["Dni"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@pnom", lectoresTable.Columns.Contains("PrimerNombre") ? (row["PrimerNombre"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@snom", lectoresTable.Columns.Contains("SegundoNombre") ? (row["SegundoNombre"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@pape", lectoresTable.Columns.Contains("PrimerApellido") ? (row["PrimerApellido"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@sape", lectoresTable.Columns.Contains("SegundoApellido") ? (row["SegundoApellido"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@nc", lectoresTable.Columns.Contains("NumeroDocumento") ? (row["NumeroDocumento"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@co", lectoresTable.Columns.Contains("Correo") ? (row["Correo"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@tel", lectoresTable.Columns.Contains("Telefono") ? (row["Telefono"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@di", lectoresTable.Columns.Contains("Direccion") ? (row["Direccion"] ?? "") : "");
                                cmd.Parameters.AddWithValue("@fc", lectoresTable.Columns.Contains("FechaNacimiento") ? (row["FechaNacimiento"] ?? "") : "");

                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        lectoresTable.AcceptChanges();
                        MessageBox.Show("Datos actualizados correctamente.");
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error al actualizar: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
            finally
            {
                MostrarLectores();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Obtener la fila seleccionada (preferir SelectedRows si existe)
            DataGridViewRow selectedRow = null;
            if (dgvlectores.SelectedRows != null && dgvlectores.SelectedRows.Count > 0)
                selectedRow = dgvlectores.SelectedRows[0];
            else
                selectedRow = dgvlectores.CurrentRow;

            if (selectedRow == null)
            {
                MessageBox.Show("No hay ninguna fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Buscar la columna que contiene el Id (Id_Usuario) por Name o DataPropertyName
            DataGridViewColumn idColumn = null;
            foreach (DataGridViewColumn col in dgvlectores.Columns)
            {
                if (string.Equals(col.Name, "Id_Usuario", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.DataPropertyName, "Id_Usuario", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.HeaderText, "Id_Usuario", StringComparison.OrdinalIgnoreCase))
                {
                    idColumn = col;
                    break;
                }
            }

            if (idColumn == null)
            {
                MessageBox.Show("La columna Id_Usuario no se encuentra en la tabla.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cellValue = selectedRow.Cells[idColumn.Index].Value;
            if (cellValue == null || cellValue == DBNull.Value)
            {
                MessageBox.Show("Id_Usuario no está definido en la fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(cellValue.ToString(), out int id))
            {
                MessageBox.Show("Id_Usuario inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("¿Seguro que desea eliminar este lector?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Usuarios WHERE Id_Usuario=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        MessageBox.Show("No se eliminó ninguna fila. Verifique el Id.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lector eliminado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MostrarLectores();
            }
        }
    }
    
}
