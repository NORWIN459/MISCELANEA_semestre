using System.Drawing;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data.SqlTypes;




namespace MISCELANEA
{
    public partial class proveedor : Form
    {

        private string myEmail = "norwinfranciscozepeda@gmail.com";
        private string MyPassword = "izzu xumu fbhg ucbi";
        private string MyAlias = "Don Baraton";
        private string[] myAdjuntos;
        private MailMessage mCorreo;

        public proveedor()
        {
            InitializeComponent();
            
        }

      

        SqlConnection conexion = new SqlConnection("server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE = DonBaraton ;INTEGRATED SECURITY=TRUE"); //recuerda cambiar el origen de la base de datos.
    

    private void btnFiles_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
             
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void btnFiles_Click_1(object sender, EventArgs e)
        {
             

            adjuntar_archivos();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            conexion.Open();
            string busqueda = "select * from proveedores where Idproveedor =@Idproveedor";
            SqlCommand comando = new SqlCommand(busqueda,conexion);
            comando.Parameters.AddWithValue("@Idproveedor",txtBuscar.Text);
            SqlDataReader reader = comando.ExecuteReader();

            if(reader.HasRows )
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataListado.DataSource = dt;
            }
            else
            {
                MessageBox.Show("no se ha econtrado ningun Id en la base de datos");
                    dataListado.DataSource=null;
                    
                    
            }

            reader.Close();
            conexion.Close();
        }

        private void adjuntar_archivos()
        {
            var names = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "Adjuntar archivos al correo";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                myAdjuntos = ofd.FileNames;

                if (myAdjuntos.Length > 0)
                {
                    for (int i = 0; i < myAdjuntos.Length; i++)
                    {
                        names += myAdjuntos[i] + "\n";
                    }
                    label8.Text = names;
                }
                else
                {
                    // caso que no se haya seleccionado nada
                    MessageBox.Show("no se añadio nada.");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            // Recupera el valor del TextBox txtBuscar
            string valorABuscar = txtBuscar.Text;

            // Crea la consulta SQL para eliminar el dato de la base de datos
          

            // Establece la conexión con la base de datos y ejecuta la consulta
         

            // Limpia el TextBox después de eliminar el dato
            txtBuscar.Text = string.Empty;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                   // revisa si el id existe
                    string checkIdQuery = "SELECT COUNT(*) FROM Proveedores WHERE IDProveedor = @IdProveedor";
                    SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, conexion);
                    checkIdCommand.Parameters.AddWithValue("@IdProveedor", TXTPROVEEDOR.Text);
                    int count = (int)checkIdCommand.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("El ID no existe en la tabla. No se puede eliminar el registro.");
                        return;
                    }

                    // elimina si el id si existe 
                    string eliminar = "DELETE FROM Proveedores WHERE IDProveedor = @IdProveedor";
                    SqlCommand comando = new SqlCommand(eliminar, conexion);
                    comando.Parameters.AddWithValue("@IdProveedor", TXTPROVEEDOR.Text);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("El registro se ha eliminado correctamente.");

                    // codigo de limpieza
                    TxTDOCUMENTO.Text = "";
                    TXTPROVEEDOR.Text = "";
                    txtrazonsocial.Text = "";
                    TXTCORREO.Text = "";
                    TXTESTADO.Text = "";
                    txtTelefono.Text = "";
                    TXTFECHA.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al eliminar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            
            if (string.IsNullOrEmpty(TXTPROVEEDOR.Text))
            {
                MessageBox.Show( "El campo Proveedor no puede estar vacío."  ,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            
            else 
            {
                try
                {
                    string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";
                    using (SqlConnection conexion = new SqlConnection(connectionString))
                    {
                        conexion.Open();

                        // Revisa si existe ya el id 
                        string checkIdQuery = "SELECT COUNT(*) FROM Proveedores WHERE IDProveedor = @IdProveedor";
                        SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, conexion);
                        checkIdCommand.Parameters.AddWithValue("@IdProveedor", TXTPROVEEDOR.Text);
                        int count = (int)checkIdCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("El ID ya existe en la tabla. No se puede insertar el registro.");
                            return;
                        }
                        DateTime fechaCreacion;
                        if (!DateTime.TryParse(TXTFECHA.Text, out fechaCreacion))
                        {
                            MessageBox.Show("La fecha ingresada no es válida.");
                            return;
                        }

                        // REGISTRA SI EL ID ES NUEVO
                        string añadir = "INSERT INTO Proveedores (Documento, RazonSocial, Correo, Estado, Telefono, FechaCreacion) " +
                       "VALUES (@Documento, @RazonSocial, @Correo, @Estado, @Telefono, @FechaCreacion)";
                        SqlCommand comando = new SqlCommand(añadir, conexion);
                        comando.Parameters.AddWithValue("@Documento", TxTDOCUMENTO.Text);
                        comando.Parameters.AddWithValue("@RazonSocial", txtrazonsocial.Text);
                        comando.Parameters.AddWithValue("@Correo", TXTCORREO.Text);
                        comando.Parameters.AddWithValue("@Estado", TXTESTADO.Text);
                        comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                        comando.Parameters.AddWithValue("@FechaCreacion", fechaCreacion);

                        comando.ExecuteNonQuery();

                        MessageBox.Show("Los datos se han registrado correctamente.");

                        // Clear the textboxes after successful execution
                        TxTDOCUMENTO.Text = "";
                        TXTPROVEEDOR.Text = "";
                        txtrazonsocial.Text = "";
                        TXTCORREO.Text = "";
                        TXTESTADO.Text = "";
                        txtTelefono.Text = "";
                        TXTFECHA.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al registrar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }





        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataListado.Rows.Count == 0)
                {
                    MessageBox.Show("Error: no hay información para guardar");
                    return;
                }

                // Show SaveFileDialog to let the user choose the save location
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    Title = "Guardar archivo PDF",
                    FileName = "Don_Baraton.pdf"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string filepath = saveFileDialog.FileName;
                Document doc = new Document(PageSize.A4);
                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
                doc.Open();

                // Adding Title and Logo
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                float[] widths = new float[] { 1f, 3f };
                headerTable.SetWidths(widths);

                // Adding image
                string imagePath = @"C:\Users\norwi\Downloads\DONBARATON.jpg"; // Replace with your image path
                if (File.Exists(imagePath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                    logo.ScaleToFit(100f, 100f);
                    PdfPCell imageCell = new PdfPCell(logo)
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    };
                    headerTable.AddCell(imageCell);
                }
                else
                {
                    MessageBox.Show("Error: La imagen del logo no se encontró.");
                    return;
                }

                // Adding title
                PdfPCell titleCell = new PdfPCell(new Phrase("MISCELANEA DON BARATON\nBELLO HORIZONTE 1C AL SUR\nTEL :2222-2222 /RUC-0011\nPROVEEDOR\n\n",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)))
                {
                    Border = iTextSharp.text.Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                headerTable.AddCell(titleCell);

                doc.Add(headerTable);

                PdfPTable table = new PdfPTable(dataListado.Columns.Count)
                {
                    WidthPercentage = 100
                };

                // Set column widths based on DataGridView column widths
                float[] columnWidths = new float[dataListado.Columns.Count];
                for (int i = 0; i < dataListado.Columns.Count; i++)
                {
                    columnWidths[i] = dataListado.Columns[i].Width;
                }
                table.SetWidths(columnWidths);

                // Add Header
                foreach (DataGridViewColumn column in dataListado.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                // Add Rows
                foreach (DataGridViewRow row in dataListado.Rows)
                {
                    if (row.IsNewRow) continue;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        PdfPCell dataCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? string.Empty))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(dataCell);
                    }
                }

                doc.Add(table);

                // Adding Signature
                string signaturePath = @"C:\Users\norwi\Downloads\DONBARATON.jpg"; // Replace with your signature path
                if (File.Exists(signaturePath))
                {
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(signaturePath);
                    signature.ScaleToFit(150f, 50f);
                    signature.SetAbsolutePosition(doc.LeftMargin, doc.BottomMargin);
                    doc.Add(signature);
                }
                else
                {
                    MessageBox.Show("Error: La imagen de la firma no se encontró.");
                    return;
                }

                doc.Close();

                MessageBox.Show("Los datos se han guardado en un archivo PDF", "Guardar PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void crear_correo()
        {
            errorProvider1.Clear(); // Limpiar los mensajes de error anteriores

            if (string.IsNullOrEmpty(txtEmailFrom.Text))
            {
                errorProvider1.SetError(txtEmailFrom, "Seleccione una dirección de correo");
                return; // Detener el envío si hay errores
            }

            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                errorProvider1.SetError(txtSubject, "Ingrese un asunto");
                return; // Detener el envío si hay errores
            }

            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                errorProvider1.SetError(txtMessage, "Ingrese un mensaje");
                return; // Detener el envío si hay errores
            }
            //funcion para habilitar el envio de correo
            mCorreo = new MailMessage();
            mCorreo.From = new MailAddress(myEmail, MyAlias, System.Text.Encoding.UTF8);
            mCorreo.To.Add(txtEmailFrom.Text.Trim());
            mCorreo.Subject = txtSubject.Text.Trim();
            mCorreo.Body = txtMessage.Text.Trim();
            mCorreo.Priority = MailPriority.Normal;

            // envio de archivos

            for (int i = 0; i < myAdjuntos.Length; i++)
            {
                mCorreo.Attachments.Add(new Attachment(myAdjuntos[i]));
            }
        }

        private void enviar ()
        {
            errorProvider1.Clear(); // Limpiar los mensajes de error anteriores

            if (string.IsNullOrEmpty(txtEmailFrom.Text))
            {
                errorProvider1.SetError(txtEmailFrom, "Seleccione una dirección de correo");
                return; // Detener el envío si hay errores
            }

            if (string.IsNullOrEmpty(txtSubject.Text))
            {
                errorProvider1.SetError(txtSubject, "Ingrese un asunto");
                return; // Detener el envío si hay errores
            }

            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                errorProvider1.SetError(txtMessage, "Ingrese un mensaje");
                return; // Detener el envío si hay errores
            }
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(myEmail, MyPassword);
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; };
                smtp.EnableSsl = true;
                smtp.Send(mCorreo);
                MessageBox.Show("Enviado");
                label8.Text = "";
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }




        }


        
            private void tabPage1_Load(object sender, EventArgs e)
        {
            string consulta = "select * from Proveedores";
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta,conexion);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            dataListado.DataSource = dt;
            dataListado.Columns["IdUsuario"].Visible = false;
        }

        private void TXTPROVEEDOR_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {
            crear_correo();
            enviar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE";
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();

                    // Check if the ID exists in the table
                    string checkIdQuery = "SELECT COUNT(*) FROM Proveedores WHERE IDProveedor = @IdProveedor";
                    SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, conexion);
                    checkIdCommand.Parameters.AddWithValue("@IdProveedor", TXTPROVEEDOR.Text);
                    int count = (int)checkIdCommand.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("El ID no existe en la tabla. No se puede editar el registro.");
                        return;
                    }

                    // Update the record if the ID exists
                    string editar = "UPDATE Proveedores SET Documento = @Documento, RazonSocial = @RazonSocial, Correo = @Correo, Estado = @Estado, Telefono = @Telefono, FechaCreacion = @FechaCreacion WHERE IDProveedor = @IdProveedor";
                    SqlCommand comando = new SqlCommand(editar, conexion);
                    comando.Parameters.AddWithValue("@Documento", TxTDOCUMENTO.Text);
                    comando.Parameters.AddWithValue("@RazonSocial", txtrazonsocial.Text);
                    comando.Parameters.AddWithValue("@Correo", TXTCORREO.Text);
                    comando.Parameters.AddWithValue("@Estado", TXTESTADO.Text);
                    comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                    comando.Parameters.AddWithValue("@FechaCreacion", TXTFECHA.Text);
                    comando.Parameters.AddWithValue("@IdProveedor", TXTPROVEEDOR.Text);
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Los datos se han editado correctamente.");

                    // Clear the textboxes after successful execution
                    TxTDOCUMENTO.Text = "";
                    TXTPROVEEDOR.Text = "";
                    txtrazonsocial.Text = "";
                    TXTCORREO.Text = "";
                    TXTESTADO.Text = "";
                    txtTelefono.Text = "";
                    TXTFECHA.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error al editar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataListado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataListado.SelectedCells.Count > 0) 
            
            {
            int selectrow= dataListado.SelectedCells[0].RowIndex;
                TXTPROVEEDOR.Text = dataListado.Rows[selectrow].Cells[0].Value.ToString();
                TxTDOCUMENTO.Text = dataListado.Rows[selectrow].Cells[1].Value.ToString();
                txtrazonsocial.Text = dataListado.Rows[selectrow].Cells[2].Value.ToString();
                TXTCORREO.Text = dataListado.Rows[selectrow].Cells[3].Value.ToString();
                txtTelefono.Text = dataListado.Rows[selectrow].Cells[4].Value.ToString();
                TXTESTADO.Text = dataListado.Rows[selectrow].Cells[5].Value.ToString();
                TXTFECHA.Text = dataListado.Rows[selectrow].Cells[6].Value.ToString();


            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataListado.Rows.Count == 0)
                {
                    MessageBox.Show("Error: no hay información para guardar");
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Guardar archivo Excel",
                    FileName = "Don_Baraton.xlsx"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string filepath = saveFileDialog.FileName;

                // Establecer el contexto de licencia
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Proveedores");

                    // Add Header
                    for (int i = 0; i < dataListado.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataListado.Columns[i].HeaderText;
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                        worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    }

                    // Add Rows
                    for (int i = 0; i < dataListado.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataListado.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = dataListado.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                        }
                    }

                    package.SaveAs(new FileInfo(filepath));
                }

                MessageBox.Show("Los datos se han guardado en un archivo Excel", "Guardar Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar el archivo Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTNCARGAREXCEL_Click(object sender, EventArgs e)
        {
             try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Cargar archivo Excel"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string filepath = openFileDialog.FileName;

                // Establecer el contexto de licencia
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                DataTable dataTable = new DataTable();

                using (ExcelPackage package = new ExcelPackage(new FileInfo(filepath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Add columns to DataTable
                    foreach (ExcelRangeBase firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                    {
                        dataTable.Columns.Add(firstRowCell.Text);
                    }

                    // Add rows to DataTable
                    for (int rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
                    {
                        ExcelRange rowCells = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                        DataRow newRow = dataTable.NewRow();
                        foreach (ExcelRangeBase cell in rowCells)
                        {
                            newRow[cell.Start.Column - 1] = cell.Text;
                        }
                        dataTable.Rows.Add(newRow);
                    }
                }

                // Assign DataTable as the DataSource of the DataGridView
                dataListado.DataSource = dataTable;

                MessageBox.Show("Los datos se han cargado desde el archivo Excel", "Cargar Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Actualizar la base de datos
                ActualizarBaseDeDatos(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar el archivo Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActualizarBaseDeDatos(DataTable dataTable)
        {
            // Asegúrate de ajustar la cadena de conexión según tu base de datos.
            string connectionString = "server=LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE=DonBaraton;INTEGRATED SECURITY=TRUE"; ;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string query = "IF EXISTS (SELECT 1 FROM Proveedores WHERE IdProveedor = @IdProveedor) " +
                                 "BEGIN " +
                                 "UPDATE Proveedores SET Documento = @Documento, RazonSocial = @RazonSocial, Correo = @Correo, Telefono = @Telefono, Estado = @Estado, FechaCreacion = @FechaCreacion WHERE IdProveedor = @IdProveedor " +
                                 "END " +
                                 "ELSE " +
                                 "BEGIN " +
                                 "SET IDENTITY_INSERT Proveedores ON; " +
                                 "INSERT INTO Proveedores (IdProveedor, Documento, RazonSocial, Correo, Telefono, Estado, FechaCreacion) VALUES (@IdProveedor, @Documento, @RazonSocial, @Correo, @Telefono, @Estado, @FechaCreacion); " +
                                 "SET IDENTITY_INSERT Proveedores OFF; " +
                                 "END";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdProveedor", row["IdProveedor"]);
                            command.Parameters.AddWithValue("@Documento", row["Documento"]);
                            command.Parameters.AddWithValue("@RazonSocial", row["RazonSocial"]);
                            command.Parameters.AddWithValue("@Correo", row["Correo"]);
                            command.Parameters.AddWithValue("@Telefono", row["Telefono"]);
                            command.Parameters.AddWithValue("@Estado", row["Estado"]);
                            command.Parameters.AddWithValue("@FechaCreacion", row["FechaCreacion"]);




                            command.ExecuteNonQuery();

                             
                        }
                    }
                }

                MessageBox.Show("La base de datos ha sido actualizada", "Actualizar Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al actualizar la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void proveedor_Load(object sender, EventArgs e)
        {

        }
    }

        }



   





