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

        private void Formlogin_Load(object sender, EventArgs e)
        {

        }

        private void bntingresar_Click(object sender, EventArgs e)
        {
            if (txtusuario.Text == "" || txtcontraseña.Text == "")
            {
                MessageBox.Show("Ingrese usuario y contraseña");
                return;
            }

            try
            {
                if (Conexion.cn.State == System.Data.ConnectionState.Closed)
                    Conexion.cn.Open();

                string query = "SELECT * FROM Tbl_Administradores WHERE Usuario=@u AND Clave=@c AND Activo=1";

                SqlCommand cmd = new SqlCommand(query, Conexion.cn);
                cmd.Parameters.AddWithValue("@u", txtusuario.Text);
                cmd.Parameters.AddWithValue("@c", txtcontraseña.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string rol = dr["Rol"].ToString();

                    dr.Close(); 

                    if (rol == "ADMIN")
                    {
                        FormAdmin fa = new FormAdmin();
                        fa.Show();
                    }
                    else
                    {
                        FormUsuario fu = new FormUsuario();
                        fu.Show();
                    }

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
    }
}
