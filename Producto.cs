using iTextSharp.text.pdf.codec.wmf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISCELANEA
{
    public class Producto
    {
        // Propiedades de la clase Producto
        public string Codigo { get; set; }
        public string NumeroParte { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public string Proveedor { get; set; }
        public int Unidad { get; set; }
        public byte[] Imagen { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }
        public decimal IVA { get; set; }
        public decimal IEPS { get; set; }
        public decimal InventarioBajo { get; set; }
        public decimal Existencias { get; set; }
        public byte[] Descripcion { get; set; }

        public static class Connection
    {
        public static SqlConnection ObtenerConexion()
        {
            string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER ;DATABASE = DonBaraton;INTEGRATED SECURITY=TRUE";
            SqlConnection conexion = new SqlConnection(connectionString);
            conexion.Open();
            return conexion;
        }
    }

    public static int InsertarProducto(Producto pData)
    {
        int retorno = 0;
        using (SqlConnection conexion = Connection.ObtenerConexion())
        {
            string squery = "INSERT INTO Productos(Codigo, NumeroParte, Nombre, Departamento, Proveedor, Unidad, Imagen, Costo, Precio, IVA, IEPS, InventarioBajo, Existencias, Descripcion) " +
                            "VALUES (@Codigo, @NumeroParte, @Nombre, @Departamento, @Proveedor, @Unidad, @Imagen, @Costo, @Precio, @IVA, @IEPS, @InventarioBajo, @Existencias, @Descripcion)";

            SqlCommand cmd = new SqlCommand(squery, conexion);
            cmd.Parameters.AddWithValue("@Codigo", pData.Codigo);
            cmd.Parameters.AddWithValue("@NumeroParte", pData.NumeroParte);
            cmd.Parameters.AddWithValue("@Nombre", pData.Nombre);
            cmd.Parameters.AddWithValue("@Departamento", pData.Departamento);
            cmd.Parameters.AddWithValue("@Proveedor", pData.Proveedor);
            cmd.Parameters.AddWithValue("@Unidad", pData.Unidad); // Ajustado para Unidad como entero
            cmd.Parameters.AddWithValue("@Imagen", pData.Imagen); // Ajustado para Imagen como byte[]
            cmd.Parameters.AddWithValue("@Costo", pData.Costo);
            cmd.Parameters.AddWithValue("@Precio", pData.Precio);
            cmd.Parameters.AddWithValue("@IVA", pData.IVA);
            cmd.Parameters.AddWithValue("@IEPS", pData.IEPS);
            cmd.Parameters.AddWithValue("@InventarioBajo", pData.InventarioBajo);
            cmd.Parameters.AddWithValue("@Existencias", pData.Existencias);
            cmd.Parameters.AddWithValue("@Descripcion", pData.Descripcion);

            retorno = cmd.ExecuteNonQuery();
        }
        return retorno;
    }

    public static List<Producto> BuscarProductos(string pSearch = "")
    {
        List<Producto> listaProductos = new List<Producto>();
        using (SqlConnection conexion = Connection.ObtenerConexion())
        {
            string squery = "SELECT Codigo, NumeroParte, Nombre, Departamento, Proveedor, Unidad, Imagen, Costo, Precio, IVA, IEPS, InventarioBajo, Existencias, Descripcion " +
                            "FROM Productos WHERE Nombre LIKE @search";
            SqlCommand cmd = new SqlCommand(squery, conexion);
            cmd.Parameters.AddWithValue("@search", "%" + pSearch + "%");

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Producto producto = new Producto
                {
                    Codigo = reader.GetString(0),
                    NumeroParte = reader.GetString(1),
                    Nombre = reader.GetString(2),
                    Departamento = reader.GetString(3),
                    Proveedor = reader.GetString(4),
                    Unidad = reader.GetInt32(5), // Ajustado para Unidad como entero
                    Imagen = reader["Imagen"] as byte[], // Ajustado para Imagen como byte[]
                    Costo = reader.GetDecimal(7),
                    Precio = reader.GetDecimal(8),
                    IVA = reader.GetDecimal(9),
                    IEPS = reader.GetDecimal(10),
                    InventarioBajo = reader.GetDecimal(11),
                    Existencias = reader.GetDecimal(12),
                    Descripcion = reader["Descripcion"] as byte[] // Ajustado para Descripcion como byte[]
                };

                listaProductos.Add(producto);
            }
        }
        return listaProductos;
    }

    public static int EliminarProducto(string codigoProducto)
    {
        int retorno = 0;
        using (SqlConnection conexion = Connection.ObtenerConexion())
        {
            string squery = "DELETE FROM Productos WHERE Codigo = @codigo";
            SqlCommand cmd = new SqlCommand(squery, conexion);
            cmd.Parameters.AddWithValue("@codigo", codigoProducto);

            retorno = cmd.ExecuteNonQuery();
        }
        return retorno;
    }
    }
}
