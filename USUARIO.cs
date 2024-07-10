using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MISCELANEA
{
    public partial class usuario : Form
    {
        public usuario()
        {
            InitializeComponent();
        }
       

        private void btnSection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdUsuario.Text) ||
               string.IsNullOrEmpty(txtDocumento.Text) ||
               string.IsNullOrEmpty(txtNombreCompleto.Text) ||
               string.IsNullOrEmpty(txtCorreo.Text) ||
               string.IsNullOrEmpty(txtClave.Text) ||
               string.IsNullOrEmpty(txtidrol.Text) ||
               comboBox1.SelectedIndex == -1 ||
               string.IsNullOrEmpty(txtFechaCreacion.Text) ||
               pictureBox1.Image == null)
            {
                MessageBox.Show("Por favor, complete todos los campos y seleccione una foto de perfil.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Creacion nuevoUsuario = new Creacion();

            nuevoUsuario.IdUsuario = txtIdUsuario.Text;
            nuevoUsuario.Documento = txtDocumento.Text;
            nuevoUsuario.NombreCompleto = txtNombreCompleto.Text;
            nuevoUsuario.Correo = txtCorreo.Text;
            nuevoUsuario.Clave = txtClave.Text;
            nuevoUsuario.IdRol = txtidrol.Text;
            nuevoUsuario.Estado = comboBox1.Text;
            nuevoUsuario.FechaCreacion = txtFechaCreacion.Text;

            if (pictureBox1.Image != null)
            {
                // Convertir la imagen seleccionada a bytes y asignarla al campo de la foto de perfil del usuario
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    nuevoUsuario.FotoPerfil = ms.ToArray();
                }
            }
            else
            {
                // Manejar el caso en el que el PictureBox esté vacío o sin ninguna foto
                // Aquí puedes mostrar un mensaje de error o tomar la acción adecuada según tus requerimientos
                MessageBox.Show("Por favor seleccione una foto de perfil.", "Foto de perfil requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
                // Salir del método sin insertar el usuario
            }

            // Llamar al método para insertar el nuevo usuario en la base de datos
            nuevoUsuario.InsertarUsuario();

            MessageBox.Show("El usuario se ha insertado correctamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Borrar los TextBox después de insertar el usuario
            txtIdUsuario.Text = "";
            txtDocumento.Text = "";
            txtNombreCompleto.Text = "";
            txtCorreo.Text = "";
            txtClave.Text = "";
            txtidrol.Text = "";
            comboBox1.SelectedIndex = -1;  // Limpiar la selección del ComboBox
            txtFechaCreacion.Text = "";

            // Limpiar el PictureBox
            pictureBox1.Image = null;


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Creacion nuevoUsuario = new Creacion();
            OpenFileDialog dialogoArchivo = new OpenFileDialog();
            dialogoArchivo.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif";
            if (dialogoArchivo.ShowDialog() == DialogResult.OK)
            {
                // Mostrar la imagen seleccionada en el control PictureBox
                pictureBox1.Image = new Bitmap(dialogoArchivo.FileName);

                // Convertir la imagen seleccionada a bytes y asignarla al campo de la foto de perfil del usuario
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    nuevoUsuario.FotoPerfil = ms.ToArray();
                }
            }
    }

        private void usuario_Load(object sender, EventArgs e)
        {

        }

        private void txtIdrol_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

