using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LaPC_Express
{
    public partial class CatalogoForm : Form
    {
        public Conexion _connec = null;
        DataTable dtDatos = null;

        public CatalogoForm(Conexion _connec1, string _sId)
        {
            InitializeComponent();
            _connec = _connec1;
            if (_connec.conectando() == false)
            {
                MessageBox.Show("Conexion Fallida", "Error");
                System.Environment.Exit(0);
            }
        }


        private void dgvCatalogo_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCatalogo.Rows[e.RowIndex];
                txtID_Ct.Text = row.Cells["Id_Producto_Ct"].Value.ToString();
                txtProd_Ct.Text = row.Cells["nombre_producto_ct"].Value.ToString();
                txtPrecio_Ct.Text = row.Cells["valor_producto_ct"].Value.ToString();
                txtHay_Ct.Text = row.Cells["Inventario_Producto_Ct"].Value.ToString();
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreProducto = txtProd_Ct.Text;
                string precioProducto = txtPrecio_Ct.Text;
                string existenciasProducto = txtHay_Ct.Text;

                string query = $"INSERT INTO catalogo (nombre_producto_ct, valor_producto_ct, Inventario_Producto_Ct) VALUES ('{nombreProducto}', '{precioProducto}', '{existenciasProducto}')";

                _connec.Modificar(query);

                CargarProductos();

                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditarProducto_Click(object sender, EventArgs e)
        {
            string idProducto = txtID_Ct.Text;
            string nombreProducto = txtProd_Ct.Text;
            string precioProducto = txtPrecio_Ct.Text;
            string existenciasProducto = txtHay_Ct.Text;

            try
            {
                string query = $"UPDATE catalogo SET nombre_producto_ct = '{nombreProducto}', valor_producto_ct = '{precioProducto}', Inventario_Producto_Ct = '{existenciasProducto}' WHERE Id_Producto_Ct = '{idProducto}'";
                _connec.Modificar(query);
                CargarProductos();
                MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CatalogoForm_Load_1(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            try
            {
                string query = "SELECT Id_Producto_Ct, nombre_producto_ct, valor_producto_ct, Inventario_Producto_Ct FROM catalogo";
                dtDatos = _connec.llenarTablas(query);
                dgvCatalogo.DataSource = dtDatos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBorrarProducto_Click_1(object sender, EventArgs e)
        {
            if (dgvCatalogo.SelectedRows.Count > 0)
            {
                string nombreProducto = dgvCatalogo.SelectedRows[0].Cells["nombre_producto_ct"].Value.ToString();
                string valorProducto = dgvCatalogo.SelectedRows[0].Cells["valor_producto_ct"].Value.ToString();

                DialogResult result = MessageBox.Show($"¿Estás seguro de que deseas eliminar el producto '{nombreProducto}' con valor '{valorProducto}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string query = $"DELETE FROM catalogo WHERE nombre_producto_ct = '{nombreProducto}' AND valor_producto_ct = '{valorProducto}'";
                        _connec.Eliminar(query);
                        CargarProductos();
                        MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void regresarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void emToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Empleados abrir = new Empleados(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MenuPrincipal abrir = new MenuPrincipal(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_connec, "0");
            abrir.Show();
            this.Close();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
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
