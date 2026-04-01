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

        private DataTable categoriasTable;
        private BindingSource categoriasBinding;

        public Formcategoria()
        {
            InitializeComponent();
        }
        void MostrarCategorias()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tbl_Categorias", Conexion.cn);
            categoriasTable = new DataTable();
            da.Fill(categoriasTable);

            categoriasBinding = new BindingSource();
            categoriasBinding.DataSource = categoriasTable;

            dgvcategoria.DataSource = categoriasBinding;
            dgvcategoria.AutoGenerateColumns = true;
            dgvcategoria.AllowUserToAddRows = true;
            dgvcategoria.ReadOnly = false;
        }

        private void Formcategoria_Load(object sender, EventArgs e)
        {
            MostrarCategorias();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgvcategoria.EndEdit();
            dgvcategoria.CommitEdit(DataGridViewDataErrorContexts.Commit);
            categoriasBinding.EndEdit();

            var added = categoriasTable.GetChanges(DataRowState.Added);
            if (added == null || added.Rows.Count == 0)
            {
                MessageBox.Show("No hay nuevas filas para guardar.");
                return;
            }

            using (var conn = Conexion.GetOpenConnection())
            {
                foreach (DataRow row in added.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Tbl_Categorias (Nombre, Descripcion) VALUES (@nom, @desc)", conn))
                    {
                        cmd.Parameters.AddWithValue("@nom", row["Nombre"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@desc", row["Descripcion"] ?? DBNull.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            categoriasTable.AcceptChanges();
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
            MessageBox.Show("Categoría actualizada correctamente", "Actualización", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Conexion.cn.Close();
            MostrarCategorias();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvcategoria.CurrentRow == null) return;

            var cellValue = dgvcategoria.CurrentRow.Cells["IdCategoria"].Value;
            if (cellValue == null || cellValue == DBNull.Value)
            {
                MessageBox.Show("No hay IdCategoria válido en la fila seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(cellValue.ToString(), out int id))
            {
                MessageBox.Show("IdCategoria inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar la categoría?",
                                                  "Confirmación",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = Conexion.GetOpenConnection())
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Tbl_Categorias WHERE IdCategoria=@id", cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int affected = cmd.ExecuteNonQuery();

                    if (affected == 0)
                        MessageBox.Show("No se eliminó ninguna fila. Verifique el Id.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Categoría eliminada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MostrarCategorias();
            }
        }
    }
}
