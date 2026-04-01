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
            string query = @"
            SELECT 
                P.IdPerfil,
                P.Dni,
                P.PrimerNombre,
                P.SegundoNombre,
                P.PrimerApellido,
                P.SegundoApellido,
                P.NumeroDocumento,
                P.Correo,
                P.Telefono,
                P.Direccion,
                P.FechaNacimiento,
                P.Activo,
                U.IdUsuario,
                U.Usuario
            FROM Tbl_Perfiles P
            INNER JOIN Tbl_Usuarios U ON P.IdUsuario = U.IdUsuario
            INNER JOIN Tbl_Roles R ON U.IdRol = R.IdRol
            WHERE R.NombreRol = 'USER' AND P.Activo = 1";

            SqlDataAdapter da = new SqlDataAdapter(query, Conexion.cn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lectoresTable = dt;
            dgvlectores.DataSource = lectoresTable;

            dgvlectores.Columns["IdPerfil"].Visible = false;
            dgvlectores.Columns["IdUsuario"].Visible = false;
        }

        private void Formlectores_Load(object sender, EventArgs e)
        {
            MostrarLectores();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvlectores.EndEdit();
                var added = lectoresTable.GetChanges(DataRowState.Added);

                if (added == null)
                {
                    MessageBox.Show("No hay nuevos registros.");
                    return;
                }

                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlTransaction tran = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in added.Rows)
                        {
                            // 1. Crear usuario con rol USER
                            string queryUser = @"
                    INSERT INTO Tbl_Usuarios (Usuario, Clave, IdRol, Activo)
                    VALUES (@usuario, '1234',
                    (SELECT IdRol FROM Tbl_Roles WHERE NombreRol='USER'), 1);
                    SELECT SCOPE_IDENTITY();";

                            SqlCommand cmdUser = new SqlCommand(queryUser, cn, tran);
                            cmdUser.Parameters.AddWithValue("@usuario", row["Dni"].ToString());

                            int idUsuario = Convert.ToInt32(cmdUser.ExecuteScalar());

                            // 2. Crear perfil
                            string queryPerfil = @"
                    INSERT INTO Tbl_Perfiles
                    (Dni, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido,
                     NumeroDocumento, Correo, Telefono, Direccion, FechaNacimiento, Activo, IdUsuario)
                    VALUES
                    (@dni, @nom, @nom2, @ape, @ape2,
                     @ndoc, @correo, @tel, @dir, @fecha, 1, @idUsuario)";

                            SqlCommand cmdPerfil = new SqlCommand(queryPerfil, cn, tran);

                            cmdPerfil.Parameters.AddWithValue("@dni", row["Dni"]);
                            cmdPerfil.Parameters.AddWithValue("@nom", row["PrimerNombre"]);
                            cmdPerfil.Parameters.AddWithValue("@nom2", row["SegundoNombre"]);
                            cmdPerfil.Parameters.AddWithValue("@ape", row["PrimerApellido"]);
                            cmdPerfil.Parameters.AddWithValue("@ape2", row["SegundoApellido"]);
                            cmdPerfil.Parameters.AddWithValue("@ndoc", row["NumeroDocumento"]);
                            cmdPerfil.Parameters.AddWithValue("@correo", row["Correo"]);
                            cmdPerfil.Parameters.AddWithValue("@tel", row["Telefono"]);
                            cmdPerfil.Parameters.AddWithValue("@dir", row["Direccion"]);
                            cmdPerfil.Parameters.AddWithValue("@fecha", row["FechaNacimiento"]);
                            cmdPerfil.Parameters.AddWithValue("@idUsuario", idUsuario);

                            cmdPerfil.ExecuteNonQuery();
                        }

                        tran.Commit();
                        MessageBox.Show("Lectores guardados correctamente");
                        lectoresTable.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            MostrarLectores();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvlectores.EndEdit();
                var modified = lectoresTable.GetChanges(DataRowState.Modified);

                if (modified == null)
                {
                    MessageBox.Show("No hay cambios.");
                    return;
                }

                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlTransaction tran = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow row in modified.Rows)
                        {
                            string query = @"
                    UPDATE Tbl_Perfiles SET
                    Dni=@dni,
                    PrimerNombre=@nom,
                    SegundoNombre=@nom2,
                    PrimerApellido=@ape,
                    SegundoApellido=@ape2,
                    NumeroDocumento=@ndoc,
                    Correo=@correo,
                    Telefono=@tel,
                    Direccion=@dir,
                    FechaNacimiento=@fecha
                    WHERE IdPerfil=@id";

                            SqlCommand cmd = new SqlCommand(query, cn, tran);

                            cmd.Parameters.AddWithValue("@id", row["IdPerfil"]);
                            cmd.Parameters.AddWithValue("@dni", row["Dni"]);
                            cmd.Parameters.AddWithValue("@nom", row["PrimerNombre"]);
                            cmd.Parameters.AddWithValue("@nom2", row["SegundoNombre"]);
                            cmd.Parameters.AddWithValue("@ape", row["PrimerApellido"]);
                            cmd.Parameters.AddWithValue("@ape2", row["SegundoApellido"]);
                            cmd.Parameters.AddWithValue("@ndoc", row["NumeroDocumento"]);
                            cmd.Parameters.AddWithValue("@correo", row["Correo"]);
                            cmd.Parameters.AddWithValue("@tel", row["Telefono"]);
                            cmd.Parameters.AddWithValue("@dir", row["Direccion"]);
                            cmd.Parameters.AddWithValue("@fecha", row["FechaNacimiento"]);

                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        MessageBox.Show("Lectores actualizados");
                        lectoresTable.AcceptChanges();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            MostrarLectores();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvlectores.CurrentRow == null) return;

            int idPerfil = Convert.ToInt32(dgvlectores.CurrentRow.Cells["IdPerfil"].Value);
            int idUsuario = Convert.ToInt32(dgvlectores.CurrentRow.Cells["IdUsuario"].Value);

            var result = MessageBox.Show("¿Desactivar lector?", "Confirmar", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlTransaction tran = cn.BeginTransaction())
                {
                    SqlCommand cmd1 = new SqlCommand("UPDATE Tbl_Perfiles SET Activo = 0 WHERE IdPerfil=@id", cn, tran);
                    cmd1.Parameters.AddWithValue("@id", idPerfil);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("UPDATE Tbl_Usuarios SET Activo = 0 WHERE IdUsuario=@id", cn, tran);
                    cmd2.Parameters.AddWithValue("@id", idUsuario);
                    cmd2.ExecuteNonQuery();

                    tran.Commit();
                }

                MessageBox.Show("Lector desactivado");
                MostrarLectores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
    
}
