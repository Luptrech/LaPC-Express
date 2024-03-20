using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace LaPC_Express
{
    public class Conexion
    {
        public MySqlConnection conn = null;
        string sconexion = "server=localhost; Database = LaPC_Express; Uid=root; password=";

        public bool conectando()
        {
            try
            {
                conn = new MySqlConnection(sconexion);
                conn.Open();
            }
            catch (Exception Ex)
            {
                System.Windows.Forms.MessageBox.Show(Ex.ToString());
                return false;
            }
            return true;
        }
        public void getCombo(string strTable, string filtro, ref System.Windows.Forms.ComboBox cmb)
        {
            DataTable dtDatos = new DataTable();
            string strQuery = "Select * from " + strTable + " " + filtro;
            MySqlCommand cmd = new MySqlCommand(strQuery, conn);
            cmd.Prepare();

            MySqlDataAdapter mdaDatos = new MySqlDataAdapter(cmd);
            mdaDatos.Fill(dtDatos);
            cmb.ValueMember = "id";
            cmb.DisplayMember = "usuario";
            cmb.DataSource = dtDatos;
        }
        public DataTable llenarTablas(string sL)
        {
            string strQuery = sL;
            MySqlCommand cmd = new MySqlCommand(strQuery, conn);
            cmd.Prepare();

            MySqlDataAdapter mdaDatos = new MySqlDataAdapter(cmd);
            DataTable dtDatos = new DataTable();
            mdaDatos.Fill(dtDatos);
            return dtDatos;
        }
        public void Insertar(string strCadena)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = strCadena;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("El Registro se ha guardado exitosamente..");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

            }
        }
        public void Modificar(string strCadena)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = strCadena;
                cmd.CommandText = strCadena;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("El registro se ha modificado correctamente");
            }

            catch (Exception ex)

            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            
        }
        
        public void Eliminar(string strCadena)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = strCadena;
                cmd.CommandText = strCadena;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                System.Windows.Forms.MessageBox.Show("El registro se ha eliminado correctamente");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void InsertarCon(MySqlCommand comand)
        {
            try
            {
                comand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

    }
}
