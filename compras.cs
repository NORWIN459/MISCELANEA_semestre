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
    public partial class compras : Form
    {
        private decimal totalAcumulado = 0;
        public compras()
        {
            InitializeComponent();
            label4.Text = "Total: $0.00";
            ConfigurarDataGridView();
            
           

        }

        private string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int datalistado = dataGridView1.SelectedCells[0].RowIndex;
                txtproductos.Text = dataGridView1.Rows[datalistado].Cells[0].Value.ToString();
                txtprecio.Text = dataGridView1.Rows[datalistado].Cells[1].Value.ToString();
                txtdescripcion.Text = dataGridView1.Rows[datalistado].Cells[2].Value.ToString();





            }
        }

        private void compras_Load(object sender, EventArgs e)
        {
            string consulta = "select  Nombre, Precio, Descripcion FROM Productos ";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, connectionString);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);



            dataGridView1.DataSource = dt;
        }
        private void ConfigurarDataGridView()
        {
            dataGridView2.Columns.Clear(); // Limpiar columnas existentes

            dataGridView2.Columns.Add("Nombre", "Nombre");
            dataGridView2.Columns.Add("Cantidad", "Cantidad");
            dataGridView2.Columns.Add("Precio", "Precio");




        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            string cliente = txtnombre.Text;
            string local = txtlocal.Text;
            string direccion = txtdireccion.Text;
            string total = label4.Text;

            if (string.IsNullOrEmpty(cliente) || string.IsNullOrEmpty(local) || string.IsNullOrEmpty(direccion))
            {
                MessageBox.Show("Por favor, ingrese todos los datos del cliente, local y dirección.");
                return;
            }

            List<string> lines = new List<string>();

            // Añadir los datos del cliente
            lines.Add($"Cliente: {cliente}");
            lines.Add($"Dirección: {direccion}");
            lines.Add($"Local: {local}");
            lines.Add(new string('-', 50)); // Línea separadora
            lines.Add($"{"Producto",-20} | {"Cantidad",10} | {"Precio",10} | {"Total",10}");
            lines.Add(new string('-', 50)); // Línea separadora


            decimal totalGeneral = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    string nombreProducto = row.Cells["Nombre"].Value?.ToString();
                    int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                    decimal precio = Convert.ToDecimal(row.Cells["Precio"].Value);
                    decimal totalProducto = cantidad * precio;

                    lines.Add($"{nombreProducto,-20} | {cantidad,10} | {precio,10:C} | {totalProducto,10:C}");
                    totalGeneral += totalProducto;
                }
            }


            lines.Add(new string('-', 50)); // Línea separadora
            lines.Add($"{"Total General:",-20} {totalGeneral,30:C}");


            // Utilizar SaveFileDialog para seleccionar la ubicación y el nombre del archivo
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Guardar Factura";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    System.IO.File.WriteAllLines(filePath, lines);
                    MessageBox.Show("La información ha sido guardada correctamente.");
                     

                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = txtproductos.Text;
            int cantidad;
            decimal precio;

            if (string.IsNullOrEmpty(nombre) || !int.TryParse(txtcantidad.Text, out cantidad) || !decimal.TryParse(txtprecio.Text, out precio))
            {
                MessageBox.Show("Por favor, ingrese valores válidos para el producto, cantidad y precio.");
                return;
            }

            dataGridView2.Rows.Add(nombre, cantidad, precio);

            // Actualizar el total acumulado
            totalAcumulado += cantidad * precio;
            label4.Text = $"Total: {totalAcumulado:C}";

            // Limpiar los campos de entrada
            txtnombre.Clear();
            txtcantidad.Clear();
            txtprecio.Clear();
            txtnombre.Focus();
        }






        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {





        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtener los valores de la fila seleccionada
                string nombre = dataGridView2.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                int cantidad = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["Cantidad"].Value);
                decimal precio = Convert.ToDecimal(dataGridView2.Rows[e.RowIndex].Cells["Precio"].Value);

                // Llenar los TextBox con los valores obtenidos
                txtproductocarrito.Text = nombre;
                txtcantidadcarrito.Text = cantidad.ToString();
                txtpreciocarrito.Text = precio.ToString();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string nombre = txtproductos.Text;
            int cantidad;
            decimal precio;

            if (string.IsNullOrEmpty(nombre) || !int.TryParse(txtcantidad.Text, out cantidad) || !decimal.TryParse(txtprecio.Text, out precio))
            {
                MessageBox.Show("Por favor, ingrese valores válidos para el producto, cantidad y precio.");
                return;
            }

            dataGridView2.Rows.Add(nombre, cantidad, precio);

            // Actualizar el total acumulado
            totalAcumulado += cantidad * precio;
            label4.Text = $"Total: {totalAcumulado:C}";

            // Limpiar los campos de entrada
            txtproductos.Clear();
            txtcantidad.Clear();
            txtprecio.Clear();
            txtdescripcion.Clear();
            txtproductos.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtproductocarrito.Text) || string.IsNullOrEmpty(txtcantidadcarrito.Text) || string.IsNullOrEmpty(txtpreciocarrito.Text))
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }
            // Buscar la fila que coincide con los valores de los TextBox
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                string nombre = row.Cells[0].Value.ToString(); // Asumiendo que el nombre está en la primera columna
                int cantidad = Convert.ToInt32(row.Cells[1].Value); // Asumiendo que la cantidad está en la segunda columna
                decimal precio = Convert.ToDecimal(row.Cells[2].Value); // Asumiendo que el precio está en la tercera columna

                if (nombre == txtproductocarrito.Text &&
                    cantidad == Convert.ToInt32(txtcantidadcarrito.Text) &&
                    precio == Convert.ToDecimal(txtpreciocarrito.Text))
                {
                    // Restar el total de la fila del total acumulado
                    decimal totalFila = cantidad * precio;
                    totalAcumulado -= totalFila;
                    label4.Text = $"Total: {totalAcumulado:C}";

                    // Eliminar la fila del DataGridView
                    dataGridView2.Rows.Remove(row);

                    // Limpiar los TextBox
                    txtproductocarrito.Text="";
                    txtcantidadcarrito.Text = "";
                    txtpreciocarrito.Text="";

                    MessageBox.Show($"El producto {nombre} ha sido eliminado.");
                    break;
                }
            }


        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
    }   

