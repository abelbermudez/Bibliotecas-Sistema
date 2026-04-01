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
        //Variables para extraer los ID
        int idUsuario = 0;
        int idPerfil = 0;

        public FormUsuarios()
        {
            InitializeComponent();

        }

        void CargarRoles()
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT IdRol, NombreRol FROM Tbl_Roles", Conexion.cn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmbRoles.DataSource = dt;
                cmbRoles.DisplayMember = "NombreRol";
                cmbRoles.ValueMember = "IdRol";
                cmbRoles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message);
            }
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            MostrarUsuarios();
            CargarRoles();
        }
        void MostrarUsuarios()
        {
            try
            {
                string query = @"
                SELECT 
                    U.IdUsuario,
                    U.Usuario,
                    R.NombreRol,
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
                    P.Activo
                FROM Tbl_Usuarios U
                INNER JOIN Tbl_Roles R ON U.IdRol = R.IdRol
                LEFT JOIN Tbl_Perfiles P ON P.IdUsuario = U.IdUsuario";

                SqlDataAdapter da = new SqlDataAdapter(query, Conexion.cn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvusuarios.DataSource = dt;

                dgvusuarios.Columns["IdUsuario"].Visible = false;
                dgvusuarios.Columns["IdPerfil"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion.cn.Open();
                SqlTransaction tran = Conexion.cn.BeginTransaction();

                // INSERT USUARIO
                string queryUser = @"
                    INSERT INTO Tbl_Usuarios (Usuario, Clave, IdRol, Activo)
                    VALUES (@usuario, '1234',
                    (SELECT IdRol FROM Tbl_Roles WHERE NombreRol=@rol), 1);
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmdUser = new SqlCommand(queryUser, Conexion.cn, tran);
                cmdUser.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                cmdUser.Parameters.AddWithValue("@rol", cmbRoles.Text);

                int nuevoIdUsuario = Convert.ToInt32(cmdUser.ExecuteScalar());

                // INSERT PERFIL
                string queryPerfil = @"
                    INSERT INTO Tbl_Perfiles
                    (Dni, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido,
                     NumeroDocumento, Correo, Telefono, Direccion, FechaNacimiento, Activo, IdUsuario)
                    VALUES
                    (@dni, @pnom, @snom, @pape, @sape,
                     @numdoc, @correo, @tel, @dir, @fecha, @activo, @idUsuario)";

                SqlCommand cmdPerfil = new SqlCommand(queryPerfil, Conexion.cn, tran);

                cmdPerfil.Parameters.AddWithValue("@dni", txtDNI.Text);
                cmdPerfil.Parameters.AddWithValue("@pnom", txtPrimerNombre.Text);
                cmdPerfil.Parameters.AddWithValue("@snom", txtSegundoNombre.Text);
                cmdPerfil.Parameters.AddWithValue("@pape", txtPrimerApellido.Text);
                cmdPerfil.Parameters.AddWithValue("@sape", txtSegundoApellido.Text);
                cmdPerfil.Parameters.AddWithValue("@numdoc", txtNumeroDocumento.Text);
                cmdPerfil.Parameters.AddWithValue("@correo", txtCorreo.Text);
                cmdPerfil.Parameters.AddWithValue("@tel", txtTelefono.Text);
                cmdPerfil.Parameters.AddWithValue("@dir", txtDirección.Text);
                cmdPerfil.Parameters.AddWithValue("@fecha", dateTimePicker1.Value);
                cmdPerfil.Parameters.AddWithValue("@activo", checkBox1.Checked);
                cmdPerfil.Parameters.AddWithValue("@idUsuario", nuevoIdUsuario);

                cmdPerfil.ExecuteNonQuery();

                tran.Commit();
                MessageBox.Show("Usuario Guardado correctamente", "Usuario Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();

                MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Conexion.cn.Close();
            }
        }

        private void dgvusuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvusuarios.CurrentRow == null) return;

            idUsuario = Convert.ToInt32(dgvusuarios.CurrentRow.Cells["IdUsuario"].Value);
            idPerfil = dgvusuarios.CurrentRow.Cells["IdPerfil"].Value == DBNull.Value ? 0 :
                       Convert.ToInt32(dgvusuarios.CurrentRow.Cells["IdPerfil"].Value);

            txtUsuario.Text = dgvusuarios.CurrentRow.Cells["Usuario"].Value.ToString();
            cmbRoles.Text = dgvusuarios.CurrentRow.Cells["NombreRol"].Value.ToString();

            txtDNI.Text = dgvusuarios.CurrentRow.Cells["Dni"].Value?.ToString();
            txtPrimerNombre.Text = dgvusuarios.CurrentRow.Cells["PrimerNombre"].Value?.ToString();
            txtSegundoNombre.Text = dgvusuarios.CurrentRow.Cells["SegundoNombre"].Value?.ToString();
            txtPrimerApellido.Text = dgvusuarios.CurrentRow.Cells["PrimerApellido"].Value?.ToString();
            txtSegundoApellido.Text = dgvusuarios.CurrentRow.Cells["SegundoApellido"].Value?.ToString();
            txtNumeroDocumento.Text = dgvusuarios.CurrentRow.Cells["NumeroDocumento"].Value?.ToString();
            txtCorreo.Text = dgvusuarios.CurrentRow.Cells["Correo"].Value?.ToString();
            txtTelefono.Text = dgvusuarios.CurrentRow.Cells["Telefono"].Value?.ToString();
            txtDirección.Text = dgvusuarios.CurrentRow.Cells["Direccion"].Value?.ToString();

            if (dgvusuarios.CurrentRow.Cells["FechaNacimiento"].Value != DBNull.Value)
                dateTimePicker1.Value = Convert.ToDateTime(dgvusuarios.CurrentRow.Cells["FechaNacimiento"].Value);

            checkBox1.Checked = Convert.ToBoolean(dgvusuarios.CurrentRow.Cells["Activo"].Value);
        }

        private void bnteliminar_Click(object sender, EventArgs e)
        {
            if (idUsuario == 0)
            {
                MessageBox.Show("Seleccione un usuario");
                return;
            }

            DialogResult result = MessageBox.Show("¿Desea desactivar este usuario?", "Confirmar", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            try
            {
                Conexion.cn.Open();
                SqlTransaction tran = Conexion.cn.BeginTransaction();

                // Desactivar perfil
                string queryPerfil = "UPDATE Tbl_Perfiles SET Activo = 0 WHERE IdUsuario = @idUsuario";
                SqlCommand cmdPerfil = new SqlCommand(queryPerfil, Conexion.cn, tran);
                cmdPerfil.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdPerfil.ExecuteNonQuery();

                // Desactivar usuario (login)
                string queryUsuario = "UPDATE Tbl_Usuarios SET Activo = 0 WHERE IdUsuario = @idUsuario";
                SqlCommand cmdUsuario = new SqlCommand(queryUsuario, Conexion.cn, tran);
                cmdUsuario.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmdUsuario.ExecuteNonQuery();

                tran.Commit();
                MessageBox.Show("Usuario desactivado correctamente");

                MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Conexion.cn.Close();
            }
        }

        private void bnteditar_Click(object sender, EventArgs e)
        {
            if (idUsuario == 0 || idPerfil == 0)
            {
                MessageBox.Show("Seleccione un registro");
                return;
            }

            try
            {
                Conexion.cn.Open();
                SqlTransaction tran = Conexion.cn.BeginTransaction();

                // UPDATE USUARIO
                string queryUser = @"
                    UPDATE Tbl_Usuarios 
                    SET Usuario=@usuario,
                        IdRol=(SELECT IdRol FROM Tbl_Roles WHERE NombreRol=@rol)
                    WHERE IdUsuario=@id";

                SqlCommand cmdUser = new SqlCommand(queryUser, Conexion.cn, tran);
                cmdUser.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                cmdUser.Parameters.AddWithValue("@rol", cmbRoles.Text);
                cmdUser.Parameters.AddWithValue("@id", idUsuario);
                cmdUser.ExecuteNonQuery();

                // UPDATE PERFIL
                string queryPerfil = @"
                    UPDATE Tbl_Perfiles SET
                    Dni=@dni,
                    PrimerNombre=@pnom,
                    SegundoNombre=@snom,
                    PrimerApellido=@pape,
                    SegundoApellido=@sape,
                    NumeroDocumento=@numdoc,
                    Correo=@correo,
                    Telefono=@tel,
                    Direccion=@dir,
                    FechaNacimiento=@fecha,
                    Activo=@activo
                    WHERE IdPerfil=@id";

                SqlCommand cmdPerfil = new SqlCommand(queryPerfil, Conexion.cn, tran);

                cmdPerfil.Parameters.AddWithValue("@dni", txtDNI.Text);
                cmdPerfil.Parameters.AddWithValue("@pnom", txtPrimerNombre.Text);
                cmdPerfil.Parameters.AddWithValue("@snom", txtSegundoNombre.Text);
                cmdPerfil.Parameters.AddWithValue("@pape", txtPrimerApellido.Text);
                cmdPerfil.Parameters.AddWithValue("@sape", txtSegundoApellido.Text);
                cmdPerfil.Parameters.AddWithValue("@numdoc", txtNumeroDocumento.Text);
                cmdPerfil.Parameters.AddWithValue("@correo", txtCorreo.Text);
                cmdPerfil.Parameters.AddWithValue("@tel", txtTelefono.Text);
                cmdPerfil.Parameters.AddWithValue("@dir", txtDirección.Text);
                cmdPerfil.Parameters.AddWithValue("@fecha", dateTimePicker1.Value);
                cmdPerfil.Parameters.AddWithValue("@activo", checkBox1.Checked);
                cmdPerfil.Parameters.AddWithValue("@id", idPerfil);

                cmdPerfil.ExecuteNonQuery();

                tran.Commit();
                MessageBox.Show("Actualizado correctamente", "Actualización correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();

                MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Conexion.cn.Close();
            }
        }

        void LimpiarCampos ()
        {
            txtUsuario.Clear();
            txtPrimerNombre.Clear();
            txtSegundoNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtDNI.Clear();
            txtNumeroDocumento.Clear();
            txtCorreo.Clear();
            txtDirección.Clear();
            txtTelefono.Clear();
        }

        private void dgvusuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}
