using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmInventario : Form
    {
        public frmInventario()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void frmInventario_Load(object sender, EventArgs e)
        {
            // Crea una instancia de la clase Conexion
            ConexionBD conexion = new ConexionBD("C:\\Users\\Alumno\\Downloads\\Lab3-1ra-clase.accdb");
            conexion.Abrir();

        }
        private void CargarDatos()
        {
            // Ruta a la base de datos Access
            string databasePath = ("C:\\Users\\Alumno\\Downloads\\Lab3-1ra-clase.accdb");

            // Crea una instancia de la clase Conexion
            ConexionBD conexionBD = new ConexionBD(databasePath);

            try
            {
                // Abre la conexion

                // Crea un adaptador de datos para llenar un DataTable
                string query = "SELECT * FROM Productos";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conexionBD.ObtenerConexion());
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Asigna el DataTable al DataGridView
                dgvInventario.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }
    }
}
