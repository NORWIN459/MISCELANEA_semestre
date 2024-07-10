using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace MISCELANEA
{
    class conexionbd
    {
        string cadena = "data source = server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE = DonBaraton ;INTEGRATED SECURITY = TRUE";

        public SqlConnection Conectarbd = new SqlConnection();

        //Constructor
        public conexionbd()
        {
            Conectarbd.ConnectionString = cadena;
        }

        //Metodo para abrir la conexion
        public void abrir()
        {
            try
            {
                Conectarbd.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al abrir BD " + ex.Message);
            }
        }

        //Metodo para cerrar la conexion
        public void cerrar()
        {
            Conectarbd.Close();
        }
    }
}
