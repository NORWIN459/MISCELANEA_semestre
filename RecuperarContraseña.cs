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

namespace MISCELANEA
{
    public partial class RecuperarContraseña : Form
    {
        string connectionString = "server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";
        public RecuperarContraseña()
        {
            InitializeComponent();
        }

        private void bAceptar_Click(object sender, EventArgs e)
        {
            string idUsuario = txtIDUsuario.Text;
            string nuevaContrasena = txtNuevaContrasena.Text;
            string repetirContrasena = txtRepetirContrasena.Text;

            if (nuevaContrasena != repetirContrasena)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, inténtelo de nuevo.");
                return;
            }

            if (ActualizarContrasena(idUsuario, nuevaContrasena))
            {
                MessageBox.Show("Contraseña actualizada con éxito.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Hubo un error al actualizar la contraseña. Por favor, inténtelo de nuevo.");
            }
        }

        private bool ActualizarContrasena(string idUsuario, string nuevaContrasena)
        {
            string query = "UPDATE Usuario2 SET Clave = @Clave WHERE IdUsuario = @IdUsuario";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Clave", nuevaContrasena);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void RecuperarContraseña_Load(object sender, EventArgs e)
        {

        }
    }
    }

