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
            ConexionBD conexion = new ConexionBD("BaseDatos\\Lab3-1ra-clase.accdb");
            conexion.Abrir();

        }
        private void CargarDatos()
        {
            // Ruta a la base de datos Access
            string databasePath = ("BaseDatos\\Lab3-1ra-clase.accdb");

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

        private void Cargar_Click(object sender, EventArgs e)
        {
                // Ruta a la base de datos Access
                string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

                // Crea una instancia de la clase Conexion
                ConexionBD conexionBD = new ConexionBD(databasePath);

                // Obtén los valores de los TextBox
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                string precio = txtPrecio.Text;
                string stock = txtStock.Text;

                // Validar los valores (podrías agregar más validaciones según sea necesario)
                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(descripcion) ||
                    string.IsNullOrWhiteSpace(precio) ||
                    string.IsNullOrWhiteSpace(stock) )
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                // Crear la consulta SQL para insertar datos
                string query = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES (?, ?, ?, ?, ?)";

                try
                {
                    // Abre la conexión
                    conexionBD.Abrir();

                    // Crear el comando SQL
                    using (OleDbCommand command = new OleDbCommand(query, conexionBD.ObtenerConexion()))
                    {
                        // Añadir parámetros al comando
                        command.Parameters.AddWithValue("?", nombre);
                        command.Parameters.AddWithValue("?", descripcion);
                        command.Parameters.AddWithValue("?", precio);
                        command.Parameters.AddWithValue("?", stock);

                        // Ejecutar el comando
                        command.ExecuteNonQuery();
                    }

                    // Mensaje de éxito
                    MessageBox.Show("Datos cargados correctamente.");

                    // Limpiar los TextBox
                    txtNombre.Clear();
                    txtDescripcion.Clear();
                    txtPrecio.Clear();
                    txtStock.Clear();

                    // Recargar los datos en el DataGridView
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    // Mostrar mensaje de error
                    MessageBox.Show("Error al cargar los datos: " + ex.Message);
                }
                finally
                {
                    // Asegurarse de cerrar la conexión
                    conexionBD.Cerrar();
                }           
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
     
                // Ruta a la base de datos Access
                string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

                // Crea una instancia de la clase Conexion
                ConexionBD conexionBD = new ConexionBD(databasePath);

                // Obtén los valores de los TextBox
                string nombre = txtNombre.Text.Trim();

                // Construir la consulta SQL con filtros
                // Usamos parámetros para evitar inyecciones SQL
                string query = "SELECT * FROM Productos WHERE 1=1";

                // Agregar filtros si los valores están presentes
                if (!string.IsNullOrEmpty(nombre))
                {
                    query += " AND Nombre LIKE ?";
                }

                try
                {
                    // Abre la conexión
                    conexionBD.Abrir();

                    // Crear el adaptador de datos
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conexionBD.ObtenerConexion()))
                    {
                        // Añadir parámetros si son necesarios
                        if (!string.IsNullOrEmpty(nombre))
                        {
                            dataAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + nombre + "%");
                        }

                        // Crear un DataTable para almacenar los resultados
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        // Asignar el DataTable al DataGridView
                        dgvInventario.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    // Mostrar mensaje de error
                    MessageBox.Show("Error al buscar los datos: " + ex.Message);
                }
                finally
                {
                    // Asegurarse de cerrar la conexión
                    conexionBD.Cerrar();
                }
            

        }
    }
}
