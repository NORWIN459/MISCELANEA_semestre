using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MISCELANEA
{
    public partial class Entrada : Form
    {
        // Importar métodos de la biblioteca user32.dll
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        // Campos privados
        private Login _login;
        private Form currentForm = null;

        public Entrada(Login login)
        {
            InitializeComponent();
            _login = login;
            MostrarInformacion();
            ConfigurarInterfazPorRol();
            timer1.Start();
        }

        public Entrada()
        {
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Mostrar el formulario de inicio y ocultar este formulario
                INICIO inicioForm = new INICIO();
                inicioForm.Show();
                this.Hide();
            }
        }

        private void MostrarInformacion()
        {
            LabelUSUARIO.Text = _login.NombreCompleto;
            Labelrol.Text = _login.Estado;
            if (_login.FotoPerfil != null)
            {
                using (var ms = new System.IO.MemoryStream(_login.FotoPerfil))
                {
                    PictureBox2.Image = Image.FromStream(ms);
                    PictureBox2.BackgroundImageLayout = ImageLayout.None;
                    PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Crear un objeto GraphicsPath con forma circular
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddEllipse(0, 0, PictureBox2.Width, PictureBox2.Height);

                    // Asignar el objeto GraphicsPath como la propiedad Region del PictureBox
                    PictureBox2.Region = new Region(path);
                }
            }
        }

        private void ConfigurarInterfazPorRol()
        {
            if (_login.Estado.Equals("Usuario", StringComparison.OrdinalIgnoreCase))
            {
                btnpreoveedor.Enabled = false;
                btnproductos.Enabled = false;
                btncliente.Enabled = false;
                Btnusurio.Enabled = false;
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                INICIO inicioForm = new INICIO();
                inicioForm.Show();
                this.Hide();
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentForm != null)
                currentForm.Close();
            currentForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            PanelChildForm.Controls.Add(childForm);
            PanelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void HideSubmenu()
        {
            // Implementar lógica para ocultar submenús si es necesario
        }

        private void ShowSubmenu(Panel submenu)
        {
            if (!submenu.Visible)
            {
                HideSubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void btnpreoveedor_Click(object sender, EventArgs e)
        {
            OpenChildForm(new proveedor());
        }

        private void Btnusurio_Click(object sender, EventArgs e)
        {
            OpenChildForm(new usuario());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                lbltime.Text = DateTime.Now.ToString("hh:mm:ss");
                lbldate.Text = DateTime.Now.ToString("dd , MMM yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnproductos_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AgregarProductos());
        }

        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0x112, 0xf012, 0);
            }
        }

        private void btncliente_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Cliente());
        }

        private void PanelSideMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnfami_Click(object sender, EventArgs e)
        {
            OpenChildForm(new compras());
        }
    }
}