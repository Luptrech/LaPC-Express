using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LaPC_Express
{
    public partial class Ventas_Search : Form
    {
        public Conexion _conecc = null;
        private Conexion _connec;
        private Ventas _ventas;

        public Ventas_Search(Conexion _conect1, string _sId, Ventas ventas)
        {
            InitializeComponent();
            _connec = _conect1;
            _ventas = ventas;
        }

        private void MostrarTodosLosProductos()
        {
            try
            {
                string query = "SELECT * FROM productos";
                DataTable dtProductos = _connec.llenarTablas(query);
                dgvShowAll.DataSource = dtProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Ventas_Search_Load(object sender, EventArgs e)
        {
            MostrarTodosLosProductos();
        }

        private void dgvSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells["nombre_producto"]; // Obtener la celda de nombre_producto

                if (cell.Value != null)
                {
                    string nombreProducto = cell.Value.ToString();
                    txtProd.Text = nombreProducto;
                }
            }
        }

        private void dgvShowAll_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells["nombre_producto"]; // Obtener la celda de nombre_producto

                if (cell.Value != null)
                {
                    string nombreProducto = cell.Value.ToString();
                    txtProd.Text = nombreProducto;
                }
            }
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtProd.Clear();
            txtBusqueda.Clear();
        }
        //Boton de agregar

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            // Obtener el nombre del producto desde el TextBox de Ventas_Search
            string nombreProducto = txtProd.Text;

            // Verificar si la ventana de Ventas está abierta
            Ventas ventanaVentas = Application.OpenForms.OfType<Ventas>().FirstOrDefault();
            if (ventanaVentas != null)
            {
                // Asignar el nombre del producto al TextBox de Ventas
                ventanaVentas.txtNameProduct.Text = nombreProducto;
                Close();
            }
            else
            {
                MessageBox.Show("La ventana Ventas no está abierta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //END

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBusqueda.Text.Trim();

            try
            {
                string query = "SELECT * FROM productos WHERE nombre_producto LIKE @filtro";
                MySqlCommand comando = new MySqlCommand(query, _connec.conn);
                comando.Parameters.AddWithValue("@filtro", $"%{filtro}%");

                DataTable dtProductos = new DataTable();
                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                adaptador.Fill(dtProductos);

                dgvSearch.DataSource = dtProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
