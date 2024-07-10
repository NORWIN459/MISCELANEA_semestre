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
    public partial class AgregarProductos : Form
    {
        public AgregarProductos()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación de campos numéricos
                int codigo, inventario, existencias;
                if (!int.TryParse(txtCodigo.Text, out codigo) ||
                    !int.TryParse(txtInventario.Text, out inventario) ||
                    !int.TryParse(txtExistencias.Text, out existencias))
                {
                    MessageBox.Show("Los campos de código, inventario y existencias deben ser números enteros válidos.");
                    return;
                }

                // Conexión a la base de datos
                string connectionString = "server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE = DonBaraton ;INTEGRATED SECURITY=TRUE";
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Calculando IVA e IEPS
                    decimal precio, costo;
                    if (!decimal.TryParse(txtPrecio.Text, out precio) ||
                        !decimal.TryParse(txtCosto.Text, out costo))
                    {
                        MessageBox.Show("Los campos de precio y costo deben ser números decimales válidos.");
                        return;
                    }

                    decimal iva = precio * 0.16M; // Asumiendo 16% de IVA
                    decimal ieps = precio * 0.08M; // Asumiendo 8% de IEPS

                    // Insertando los datos en la tabla Productos
                    string query = "INSERT INTO Productos (Codigo, NumeroParte, Nombre, Departamento, Proveedor, Unidad, Imagen, Costo, Precio, IVA, IEPS, InventarioBajo, Existencias, Descripcion) " +
                                   "VALUES (@Codigo, @NumeroParte, @Nombre, @Departamento, @Proveedor, @Unidad, @Imagen, @Costo, @Precio, @IVA, @IEPS, @InventarioBajo, @Existencias, @Descripcion)";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigo);
                        cmd.Parameters.AddWithValue("@NumeroParte", txtParte.Text);
                        cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@Departamento", txtDepartamento.Text);
                        cmd.Parameters.AddWithValue("@Proveedor", txtProveedor.Text);
                        cmd.Parameters.AddWithValue("@Unidad", txtUnidad.Text);
                        cmd.Parameters.AddWithValue("@Imagen", txtImagen.Text); // Asumiendo que el campo Imagen contiene la ruta o URL de la imagen
                        cmd.Parameters.AddWithValue("@Costo", costo);
                        cmd.Parameters.AddWithValue("@Precio", precio);
                        cmd.Parameters.AddWithValue("@IVA", iva);
                        cmd.Parameters.AddWithValue("@IEPS", ieps);
                        cmd.Parameters.AddWithValue("@InventarioBajo", inventario);
                        cmd.Parameters.AddWithValue("@Existencias", existencias);
                        cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Producto guardado exitosamente.");
                // Limpiar los TextBox después de guardar
                txtCodigo.Clear();
                txtParte.Clear();
                txtNombre.Clear();
                txtDepartamento.Clear();
                txtProveedor.Clear();
                txtUnidad.Clear();
                txtImagen.Clear();
                txtCosto.Clear();
                txtPrecio.Clear();
                txtInventario.Clear();
                txtExistencias.Clear();
                txtDescripcion.Clear();
                pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message);
            }
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialogoArchivo = new OpenFileDialog();
                dialogoArchivo.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif";
                if (dialogoArchivo.ShowDialog() == DialogResult.OK)
                {
                    // Mostrar la imagen seleccionada en el control PictureBox
                    pictureBox1.Image = new Bitmap(dialogoArchivo.FileName);

                    // Convertir la imagen seleccionada a bytes y asignarla al campo Imagen
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                        // Asignar los bytes de la imagen a txtImagen.Text o a la entidad correspondiente
                        txtImagen.Text = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message);
            }
        }

        private void AgregarProductos_Load(object sender, EventArgs e)
        {

        }
    }
    }
    

