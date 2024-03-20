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
    public partial class Empleados : Form
    {
        public Conexion _connec = null;

        DataTable dtDatos = null;
        public Empleados(Conexion _conect1, string sId, IContainer components = null)
        {
            InitializeComponent();
            _connec = _conect1;
            if (_connec.conectando() == false)
            {
                MessageBox.Show("Conexion Fallida", "Error");
                System.Environment.Exit(0);
            }
            else
            {

            }
            _connec = _conect1;
            this.components = components;

        }
        public void limpiar()
        {
            txtID.Clear();
            txtName.Clear();
            txtUser.Clear();
            txtPassword.Clear();
            txtNumero.Clear();
            txtDireccion.Clear();
            txtSueldo.Clear();
            dateFecha.ResetText();
        }
        private void actualizar()



        {
            string aC = "SELECT empleados.Id_Empleado AS Id_Empleado, " +
                "empleados.Nombre_Empleado," +
                "empleados.User," +
                "empleados.Pass, " +
                "empleados.Numero," +
                "empleados.Direccion," +
                "empleados.Salario," +
                "empleados.Fecha_Ingreso" +
                " FROM empleados ";
            dtDatos = _connec.llenarTablas(aC);
            dgvEmpleados.DataSource = dtDatos;
        }

        private void Empleados_Load(object sender, EventArgs e)
        {
            actualizar();
        }

        private void regresarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void inicioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MenuPrincipal abrir = new MenuPrincipal(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void CatálogoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CatalogoForm abrir = new CatalogoForm(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            string modificar = "update empleados set nombre_empleado = '" + txtName.Text + " ', user='" + txtUser.Text + "', pass='" + txtPassword.Text + "', numero='" + txtNumero.Text + "', direccion='" + txtDireccion.Text + "', salario='" + txtSueldo.Text + "', fecha_ingreso='" + dateFecha.Value.ToString("yyy-MM-dd") + "'" +
                " where id_empleado = '" + txtIDuser.Text + "'";
            _connec.Modificar(modificar);

            limpiar();
            actualizar();
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlCommand comando = null;
            try
            {
                string insertar = " insert into empleados(nombre_empleado, user, pass, numero, direccion, salario, fecha_ingreso) " +
                    " values (@nombre, @usuario, @contrasena, @telefono,@direccion, @sueldo, @fecha_ingreso)";

                using (comando = new MySql.Data.MySqlClient.MySqlCommand(insertar, _connec.conn))
                {
                    MySqlCommand agregar = new MySqlCommand(insertar, _connec.conn);

                    agregar.Parameters.AddWithValue("@id_empleado", txtIDuser.Text);
                    agregar.Parameters.AddWithValue("@nombre", txtName.Text);
                    agregar.Parameters.AddWithValue("@usuario", txtUser.Text);
                    agregar.Parameters.AddWithValue("@contrasena", txtPassword.Text);
                    agregar.Parameters.AddWithValue("@telefono", txtNumero.Text);
                    agregar.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                    agregar.Parameters.AddWithValue("@sueldo", txtSueldo.Text);
                    agregar.Parameters.AddWithValue("@fecha_ingreso", dateFecha.Value.ToString("yyyy-MM-dd"));


                    _connec.InsertarCon(agregar);
                    MessageBox.Show("Usuario Guardado", "Nuevo Usuario");
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            actualizar();
        }

        private void dgvEmpleados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {

                string id = dgvEmpleados.Rows[e.RowIndex].Cells[0].Value.ToString();
                Empleados usu = new Empleados(_connec, id);

                usu.txtIDuser.Text = dgvEmpleados.CurrentRow.Cells[0].Value.ToString();
                usu.txtName.Text = dgvEmpleados.CurrentRow.Cells[1].Value.ToString();
                usu.txtUser.Text = dgvEmpleados.CurrentRow.Cells[2].Value.ToString();
                usu.txtPassword.Text = dgvEmpleados.CurrentRow.Cells[3].Value.ToString();
                usu.txtNumero.Text = dgvEmpleados.CurrentRow.Cells[4].Value.ToString();
                usu.txtDireccion.Text = dgvEmpleados.CurrentRow.Cells[5].Value.ToString();
                usu.txtSueldo.Text = dgvEmpleados.CurrentRow.Cells[6].Value.ToString();
                usu.dateFecha.Text = dgvEmpleados.CurrentRow.Cells[7].Value.ToString();
                usu.Show();
                actualizar();
                this.Close();
            }
        }

        private void emToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Empleados abrir = new Empleados(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/AboutUs.html";
            Process.Start(url);
        }

        private void ayudaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/Help.html";
            Process.Start(url);
        }

    }
}