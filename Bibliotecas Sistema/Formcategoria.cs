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
    public partial class Formcategoria : Form
    {
        public Formcategoria()
        {
            InitializeComponent();
        }
        void MostrarCategorias()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Categorias", Conexion.cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvcategoria.DataSource = dt;
        }

        private void Formcategoria_Load(object sender, EventArgs e)
        {
            MostrarCategorias();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conexion.cn.Open();

            foreach (DataGridViewRow row in dgvcategoria.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["IdCategoria"].Value == null)
                {
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Tbl_Categorias (Nombre, Descripcion) VALUES (@nom, @desc)",
                        Conexion.cn);

                    cmd.Parameters.AddWithValue("@nom", row.Cells["Nombre"].Value ?? "");
                    cmd.Parameters.AddWithValue("@desc", row.Cells["Descripcion"].Value ?? "");

                    cmd.ExecuteNonQuery();
                }
            }

            Conexion.cn.Close();
            MostrarCategorias();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Conexion.cn.Open();

            foreach (DataGridViewRow row in dgvcategoria.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["IdCategoria"].Value != null)
                {
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Tbl_Categorias SET Nombre=@nom, Descripcion=@desc WHERE IdCategoria=@id",
                        Conexion.cn);

                    cmd.Parameters.AddWithValue("@nom", row.Cells["Nombre"].Value ?? "");
                    cmd.Parameters.AddWithValue("@desc", row.Cells["Descripcion"].Value ?? "");
                    cmd.Parameters.AddWithValue("@id", row.Cells["IdCategoria"].Value);

                    cmd.ExecuteNonQuery();
                }
            }

            Conexion.cn.Close();
            MostrarCategorias();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvcategoria.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvcategoria.CurrentRow.Cells["IdCategoria"].Value);

            Conexion.cn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Categorias WHERE IdCategoria=@id", Conexion.cn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            Conexion.cn.Close();
            MostrarCategorias();
        }
    }
}
