using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MISCELANEA
{
    public partial class arranque : Form
    {
        private byte contador = 0;
        private Login _login;

        public arranque(Login login)
        {
            InitializeComponent();
            _login = login;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Value < 100)
            {
                contador += 25;
                ProgressBar.Value = contador;

                switch (contador)
                {
                    case 25:
                        label3.Text = "25%";
                        break;
                    case 50:
                        label3.Text = "50%";
                        break;
                    case 75:
                        label3.Text = "75%";
                        break;
                    case 100:
                        label3.Text = "100%";
                        break;
                }
            }
            else
            {
                timer1.Stop(); // Detener el temporizador una vez que el progreso llega al 100%
                var entradaForm = new Entrada(_login);
                entradaForm.Show();
                this.Hide();
            }
        }
    }
}
