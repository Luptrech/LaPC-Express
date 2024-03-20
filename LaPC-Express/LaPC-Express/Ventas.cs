using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LaPC_Express
{
    //_CONNECT
    public partial class Ventas : Form
    {
        public Conexion _conecc = null;

        public Ventas(Conexion _connec1, string _sId)
        {
            InitializeComponent();
            _conecc = _connec1;
        }
        DataTable dtDatos = null;
        public Ventas(Conexion _conect1, string sId, IContainer components = null)
        {
            InitializeComponent();
            _conecc = _conect1;
            if (_conecc.conectando() == false)
            {
                MessageBox.Show("Conexion Fallida", "Error");
                System.Environment.Exit(0);
            }
            else
            {

            }
            _conecc = _conect1;
            this.components = components;

        }


        public DataGridView DgvListado => dgvListado;

        public void AgregarProductoALaLista(string nombreProducto)
        {
            dgvListado.Rows.Add(nombreProducto);
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            actualizar();
        }
        private void regresarToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        public void txtNameProd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                try
                {
                    string query = "SELECT precio_producto FROM productos WHERE nombre_producto = @nombreProducto";

                    using (MySqlCommand comando = new MySqlCommand(query, _conecc.conn))
                    {

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null)
                        {
                            txtPrecioProduct.Text = resultado.ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el producto en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar el producto en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnListClearData_Click(object sender, EventArgs e)
        {
            int idEliminar;
            if (!int.TryParse(txtIdLocked.Text, out idEliminar))
            {
                MessageBox.Show("Por favor, introduce un ID válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool encontrado = false;
            foreach (DataGridViewRow row in dgvListado.Rows)
            {
                if (row.Cells["Numero_Listado"].Value != null && Convert.ToInt32(row.Cells["Numero_Listado"].Value) == idEliminar)
                {
                    dgvListado.Rows.Remove(row);
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                MessageBox.Show("No se encontró ninguna fila con el ID especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string queryDelete = "DELETE FROM templist WHERE Numero_Listado = @idEliminar";
                using (MySqlCommand comandoDelete = new MySqlCommand(queryDelete, _conecc.conn))
                {
                    comandoDelete.Parameters.AddWithValue("@idEliminar", idEliminar);
                    comandoDelete.ExecuteNonQuery();
                }

                string queryUpdate = "UPDATE templist SET Numero_Listado = Numero_Listado - 1 WHERE Numero_Listado > @idEliminar";
                using (MySqlCommand comandoUpdate = new MySqlCommand(queryUpdate, _conecc.conn))
                {
                    comandoUpdate.Parameters.AddWithValue("@idEliminar", idEliminar);
                    comandoUpdate.ExecuteNonQuery();
                }

                MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el producto de la lista: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Ventas_Search abrir = new Ventas_Search(_conecc, "0", this);
            abrir.Show();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreProducto = txtNameProduct.Text.Trim();
                string precioProducto = txtPrecioProduct.Text.Trim();
                string unidades = string.IsNullOrEmpty(txtCantidadProduct.Text.Trim()) ? "1" : txtCantidadProduct.Text.Trim();

                if (string.IsNullOrEmpty(nombreProducto))
                {
                    MessageBox.Show("Por favor, ingresa el nombre del producto.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string insertar = "INSERT INTO templist (nombre_producto, precio, unidades) VALUES (@nombreProducto, @precioProducto, @unidades)";

                using (MySqlCommand comando = new MySqlCommand(insertar, _conecc.conn))
                {
                    comando.Parameters.AddWithValue("@nombreProducto", nombreProducto);
                    comando.Parameters.AddWithValue("@precioProducto", precioProducto);
                    comando.Parameters.AddWithValue("@unidades", unidades);

                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        actualizar(); 
                    }
                    else
                    {
                        MessageBox.Show("No se pudo agregar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public event EventHandler VentasSearchClosed;

        protected virtual void OnVentasSearchClosed(EventArgs e)
        {
            VentasSearchClosed?.Invoke(this, e);
        }

        private void Ventas_Search_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnVentasSearchClosed(EventArgs.Empty);
        }


        private void actualizar()
        {
            try
            {
                string consulta = "SELECT Numero_Listado, nombre_producto, precio, unidades FROM templist";
                dtDatos = _conecc.llenarTablas(consulta);
                dgvListado.DataSource = dtDatos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la lista: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int idProducto = int.Parse(dgvListado.Rows[e.RowIndex].Cells[0].Value.ToString());

                    txtIdLocked.Text = idProducto.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Ventas_Load_1(object sender, EventArgs e)
        {
            
        }

        private void dgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int idProducto = int.Parse(dgvListado.Rows[e.RowIndex].Cells[0].Value.ToString());

                    txtIdLocked.Text = idProducto.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEndVenta_Click(object sender, EventArgs e)
        {
            try
            {
                string dropQuery = "DROP TABLE IF EXISTS templist";
                using (MySqlCommand comandoDrop = new MySqlCommand(dropQuery, _conecc.conn))
                {
                    comandoDrop.ExecuteNonQuery();

                    string createQuery = "CREATE TABLE templist (" +
                        "Numero_Listado INT AUTO_INCREMENT PRIMARY KEY, " +
                        "nombre_producto VARCHAR(100), " +
                        "precio VARCHAR(100), " +
                        "Unidades VARCHAR(100))";
                    using (MySqlCommand comandoCreate = new MySqlCommand(createQuery, _conecc.conn))
                    {
                        comandoCreate.ExecuteNonQuery();

                        txtNameProduct.Text = "";
                        txtPrecioProduct.Text = "";
                        txtCantidadProduct.Text = "";
                        txtIdLocked.Text = "";

                        MessageBox.Show("Venta finalizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        actualizar();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensajeError = "Error al finalizar la venta: " + ex.GetType().FullName + "\n" + ex.Message;
                MessageBox.Show(mensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }

        private void txtNameProduct_TextChanged(object sender, EventArgs e)
            {
                string nombreProducto = txtNameProduct.Text.Trim();

                if (!string.IsNullOrEmpty(nombreProducto))
                {
                    try
                    {
                        string query = "SELECT precio FROM productos WHERE nombre_producto = @nombreProducto";

                        using (MySqlCommand comando = new MySqlCommand(query, _conecc.conn))
                        {
                            comando.Parameters.AddWithValue("@nombreProducto", nombreProducto);

                            object resultado = comando.ExecuteScalar();

                            if (resultado != null)
                            {
                                txtPrecioProduct.Text = resultado.ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el producto en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar el producto en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un nombre de producto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/Help.html";
            Process.Start(url);
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:8888/AboutUs.html";
            Process.Start(url);
        }

        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas abrir = new Ventas(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void emToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Empleados abrir = new Empleados(_conecc, "0");
            abrir.Show();
            this.Close();
        }

        private void inicioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MenuPrincipal abrir = new MenuPrincipal(_conecc, "0");
            abrir.Show();
            this.Close();
        }
    }
}


