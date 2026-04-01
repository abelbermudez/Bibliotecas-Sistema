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
    public partial class Formlogin : Form
    {
        public Formlogin()
        {
            InitializeComponent();
        }

        // Variables globales
        public static int IdUsuarioLogueado;
        public static string RolUsuario;

        private void bntingresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtusuario.Text) || string.IsNullOrEmpty(txtcontraseña.Text))
            {
                MessageBox.Show("Ingrese usuario y contraseña");
                return;
            }

            try
            {
                if (Conexion.cn.State == System.Data.ConnectionState.Closed)
                    Conexion.cn.Open();

                string query = @"SELECT U.IdUsuario, R.NombreRol
                             FROM Tbl_Usuarios U
                             INNER JOIN Tbl_Roles R ON U.IdRol = R.IdRol
                             WHERE U.Usuario=@u AND U.Clave=@c AND U.Activo=1";

                SqlCommand cmd = new SqlCommand(query, Conexion.cn);
                cmd.Parameters.AddWithValue("@u", txtusuario.Text);
                cmd.Parameters.AddWithValue("@c", txtcontraseña.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    IdUsuarioLogueado = Convert.ToInt32(dr["IdUsuario"]);
                    RolUsuario = dr["NombreRol"].ToString();

                    dr.Close();

                    if (RolUsuario == "ADMIN")
                    {
                        FormAdmin fa = new FormAdmin(this);
                        fa.Show();
                    }
                    else
                    {
                        FormUsuario fu = new FormUsuario(this);
                        fu.Show();
                    }

                    limpiarCampos(txtusuario, txtcontraseña);
                    this.Hide();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Datos incorrectos");
                }

                Conexion.cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void limpiarCampos(TextBox txtusuario, TextBox txtcontraseña)
        {
            txtusuario.Clear();
            txtcontraseña.Clear();
        }
    }
}
