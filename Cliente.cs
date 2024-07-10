using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MISCELANEA
{
    public partial class Cliente : Form
    {
        private string conexion = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";

        public Cliente()
        {
            InitializeComponent();
        }

        private void Cliente_Load(object sender, EventArgs e)
        {
            string consulta = "select * from Usuario2";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtid.Text = row.Cells["IdUsuario"].Value.ToString();
                Txtdocumentos.Text = row.Cells["Documento"].Value.ToString();
                txtnombrecompleto.Text = row.Cells["NombreCompleto"].Value.ToString();
                txtcorreo.Text = row.Cells["Correo"].Value.ToString();

                comboBox1.Text = row.Cells["Estado"].Value.ToString();


                if (row.Cells["FotoPerfil"].Value != DBNull.Value)
                {
                    byte[] fotoPerfil = (byte[])row.Cells["FotoPerfil"].Value;
                    using (MemoryStream ms = new MemoryStream(fotoPerfil))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            string documento = Txtdocumentos.Text;
            string nombreCompleto = txtnombrecompleto.Text;
            string correo = txtcorreo.Text;
            string idUsuario = txtid.Text;
            string estado = comboBox1.Text;


            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(nombreCompleto) || string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(idUsuario) || string.IsNullOrEmpty(estado))
            {
                MessageBox.Show("Por favor, seleccione un usuario para editar.");
                return;
            }
            string query = "UPDATE Usuario2 SET NombreCompleto = @NombreCompleto, Correo = @Correo, Estado = @Estado WHERE IdUsuario = @IdUsuario";


            using (SqlConnection connection = new SqlConnection(conexion))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@IdUsuario", SqlDbType.VarChar).Value = idUsuario;
                    command.Parameters.Add("@NombreCompleto", SqlDbType.VarChar).Value = nombreCompleto;
                    command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = correo;
                    command.Parameters.Add("@Estado", SqlDbType.VarChar).Value = estado;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Usuario actualizado exitosamente.");
                        txtid.Text = "";
                        Txtdocumentos.Text = "";
                        txtnombrecompleto.Text = "";
                        comboBox1.Text = "";
                        txtcorreo.Text = "";
                        pictureBox1.Image = null;





                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el usuario.");
                    }
                }


            }





        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Estás seguro de que quieres cambiar la foto de perfil?", "Confirmar Cambio de Foto", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Imágenes|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] fotoPerfil = File.ReadAllBytes(filePath);

                    string query = "UPDATE Usuario2 SET FotoPerfil = @FotoPerfil WHERE Idusuario = @IdUsuario";

                    using (SqlConnection connection = new SqlConnection(conexion))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@IdUsuario", SqlDbType.VarChar).Value = txtid.Text;
                            command.Parameters.Add("@FotoPerfil", SqlDbType.VarBinary).Value = fotoPerfil;

                            connection.Open();
                            int result = command.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Foto de perfil actualizada exitosamente.");
                            }
                            else
                            {
                                MessageBox.Show("Error al actualizar la foto de perfil.");
                            }
                        }
                    }

                }
            }
        }

      
        private void BTNMOSTRAR_Click(object sender, EventArgs e)
        {
            string consulta = "select * from Usuario2";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtid.Text))
            {
                MessageBox.Show("Por favor, seleccione un usuario antes de intentar eliminar.");
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string idUsuario = txtid.Text;
                string query = "DELETE FROM Usuario2 WHERE IdUsuario = @IdUsuario";

                using (SqlConnection connection = new SqlConnection(conexion))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = Convert.ToInt32(idUsuario);

                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Usuario eliminado exitosamente.");
                            txtid.Text = "";
                            Txtdocumentos.Text = "";
                            txtnombrecompleto.Text = "";
                            comboBox1.Text = "";
                            txtcorreo.Text = "";
                            pictureBox1.Image = null;




                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar el usuario.");
                        }
                    }
                }
            }
        }
    }
}