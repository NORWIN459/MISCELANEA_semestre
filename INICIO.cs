using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;



namespace MISCELANEA
{
    public partial class INICIO : Form


    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private SqlConnection conexion = new SqlConnection("server= LAPTOP-H5ATVQM6\\SQLSERVER;DATABASE = DonBaraton; INTEGRATED SECURITY=TRUE");

        public INICIO()
        {
            InitializeComponent();
        }

        private void INICIO_Load(object sender, EventArgs e)
        {
            conexionbd conexion = new conexionbd();
            conexion.abrir();

            // Establece el estilo plano para el botón
            btnLogin.FlatStyle = FlatStyle.Popup;

            // Personalizar el estilo del TextBox de usuario
            txtusuario.BackColor = Color.White;
            txtusuario.ForeColor = Color.Black;
            txtusuario.BorderStyle = BorderStyle.FixedSingle;
            txtusuario.Font = new Font("Arial", 12);

            txtcontr.BackColor = Color.White;
            txtcontr.ForeColor = Color.Black;
            txtcontr.BorderStyle = BorderStyle.FixedSingle;
            txtcontr.Font = new Font("Arial", 12);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var login = new Login();
            if (login.IniciarSesion(txtusuario.Text, txtcontr.Text))
            {
                MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var arranque = new arranque(login);
                arranque.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrectos.");
            }
        }

        private void titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0x112, 0xf012, 0);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPassword.Checked)
            {
                txtcontr.PasswordChar = '\0'; // Muestra la contraseña
            }
            else
            {
                txtcontr.PasswordChar = '*'; // Oculta la contraseña
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var recuperar = new RecuperarContraseña();
            recuperar.Show();
        }

        // Métodos no utilizados o eventos no implementados pueden ser eliminados o completados según sea necesario
        private void titleBar_Paint(object sender, PaintEventArgs e) { }

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}