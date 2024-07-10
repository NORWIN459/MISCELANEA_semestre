using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISCELANEA
{
    internal class Creacion
    {
        public string IdUsuario { get; set; }
        public string Documento { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string IdRol { get; set; }
        public string Estado { get; set; }
        public string FechaCreacion { get; set; }
        public byte[] FotoPerfil { get; set; }  // Campo para almacenar la foto de perfil del usuario

        public void InsertarUsuario()
        {
            string connectionString = "server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE = DonBaraton;INTEGRATED SECURITY=TRUE";  // Reemplaza con tu cadena de conexión a la base de datos

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Usuario2 (IdUsuario, Documento, NombreCompleto, Correo, Clave, IdRol, Estado, FechaCreacion, FotoPerfil) " +
                               "VALUES (@IdUsuario, @Documento, @NombreCompleto, @Correo, @Clave, @IdRol, @Estado, @FechaCreacion, @FotoPerfil)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@IdUsuario", SqlDbType.VarChar).Value = IdUsuario;
                    command.Parameters.Add("@Documento", SqlDbType.VarChar).Value = Documento;
                    command.Parameters.Add("@NombreCompleto", SqlDbType.VarChar).Value = NombreCompleto;
                    command.Parameters.Add("@Correo", SqlDbType.VarChar).Value = Correo;
                    command.Parameters.Add("@Clave", SqlDbType.VarChar).Value = Clave;
                    command.Parameters.Add("@IdRol", SqlDbType.VarChar).Value = IdRol;
                    command.Parameters.Add("@Estado", SqlDbType.VarChar).Value = Estado;
                    command.Parameters.Add("@FechaCreacion", SqlDbType.VarChar).Value = FechaCreacion;
                    command.Parameters.Add("@FotoPerfil", SqlDbType.VarBinary).Value = FotoPerfil;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
public class Login
{
    public string IdUsuario { get; private set; }
    public string Documento { get; private set; }
    public string NombreCompleto { get; private set; }
    public string Correo { get; private set; }
    public string Clave { get; private set; }
    public string IdRol { get; private set; }
    public string Estado { get; private set; }
    public string FechaCreacion { get; private set; }
    public byte[] FotoPerfil { get; private set; }
    public bool IniciarSesion(string nombreUsuario, string clave)
    {
        string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";

        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT IdUsuario, Documento, NombreCompleto, Correo, Clave, IdRol, Estado, FechaCreacion, FotoPerfil " +
                           "FROM Usuario2 WHERE NombreCompleto = @NombreCompleto AND Clave = @Clave";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@NombreCompleto", SqlDbType.VarChar).Value = nombreUsuario;
                command.Parameters.Add("@Clave", SqlDbType.VarChar).Value = clave;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IdUsuario = reader["IdUsuario"].ToString();
                        Documento = reader["Documento"].ToString();
                        NombreCompleto = reader["NombreCompleto"].ToString();
                        Correo = reader["Correo"].ToString();
                        Clave = reader["Clave"].ToString();
                        IdRol = reader["IdRol"].ToString();
                        Estado = reader["Estado"].ToString();
                        FechaCreacion = reader["FechaCreacion"].ToString();
                        FotoPerfil = reader["FotoPerfil"] as byte[];
                        return true;
                    }
                }
            }
        }
        return false;
    }
}