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
    public partial class RegProd : Form
    {
        public Conexion _conecc = null;

        public RegProd(Conexion _connec1, string _sId)
        {
            InitializeComponent();
            _conecc = _connec1;
        }
        public RegProd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRf_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void RegProd_Load(object sender, EventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void inicioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MenuPrincipal abrir = new MenuPrincipal(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void catálogoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void emToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Empleados abrir = new Empleados(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void facturaciónToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_conecc, "0");
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
    }
}
