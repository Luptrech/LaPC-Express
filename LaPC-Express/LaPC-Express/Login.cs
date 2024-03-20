using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LaPC_Express
{
    public partial class Login : Form
    {
        public Conexion _conecc = null;
        int contador = 0;
        DataTable dtDatos = new DataTable();
        public Conexion conect1 = new Conexion();
        private Conexion conecc;
        private string v;

        public Login()
    {
        InitializeComponent();
        conect1.conectando();
    }

        public Login(Conexion conecc, string v)
        {
            this.conecc = conecc;
            this.v = v;
        }

        public void LlenarCombo()
        {
            conect1.getCombo("usuario", "", ref cmbUser);
        }
        public void Registro()
        {
            if (cmbUser.Text == "" || txtPassL.Text == "")
            {
                MessageBox.Show("Llene los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassL.Focus();

            }
            else
            {
                MySqlCommand consultar = new MySqlCommand("Select * from Usuario where usuario='" + cmbUser.Text + "' and Pass = '" + txtPassL.Text + "'", conect1.conn);
                MySqlDataReader Ejecuta = consultar.ExecuteReader();

                if (Ejecuta.Read() == true)
                {
                    MessageBox.Show(" Bienvenido " + cmbUser.Text, "Hola", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    MenuPrincipal abrir = new MenuPrincipal(conect1, "0");
                    abrir.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "intentalo de nuevo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassL.Clear();
                    txtPassL.Focus();
                    contador = contador + 1;
                }
                {
                    if (contador >= 3)
                    {
                        MessageBox.Show("¡ERROR DE USUARIO O CONTRASEÑA, SE CERRARÁ.. ADIÓS", "AVISO");
                        Application.Exit();
                    }
                    Ejecuta.Close();
                }
            }
        }
        

        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Registro();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult salir = MessageBox.Show("Realmente quieres salir?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (salir == DialogResult.No)
            {
                return;
            }
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            LlenarCombo();
        }
    }
}
