using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LaPC_Express
{
    public partial class MenuPrincipal : Form
    {
        public Conexion _conecc = null;

        public MenuPrincipal(Conexion _connec1, string _sId)
        {
            InitializeComponent();
            _conecc = _connec1;
        }
        
       


        //StartupPage
        private void MenuPrincipal_Load(object sender, EventArgs e)
        {

        //    string rutaCmd = @"C:\Kamilo\http.cmd";

        //    if (System.IO.File.Exists(rutaCmd))
        //    {

        //        Process.Start(rutaCmd);
        //    }
        //    else
        //    {

        //        MessageBox.Show("El archivo CMD no existe en la ubicación especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        }


        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void emToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Empleados abrir = new Empleados(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void pantallasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CatalogoForm abrir = new CatalogoForm(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuPrincipal abrir = new MenuPrincipal(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void acercaDeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/AboutUs.html"; 
            Process.Start(url);
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/Help.html";
            Process.Start(url);
        }
        private void regresarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
