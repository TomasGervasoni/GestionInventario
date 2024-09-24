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
    public partial class frmModificar : Form
    {
        public frmModificar()
        {
            InitializeComponent();
            CargarDatos(); // Llama a cargar los datos cuando se inicie el formulario
        }
        
        private void frmModificar_Load(object sender, EventArgs e)
        {

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
                conexionBD.Abrir();
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
            finally
            {
                conexionBD.Cerrar();
            }
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count > 0)
            {
                // Obtener el Código del producto seleccionado
                int codigoProducto = Convert.ToInt32(dgvInventario.SelectedRows[0].Cells["Código"].Value);

                // Confirmar eliminación
                DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar este producto?", "Confirmación", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Ruta a la base de datos Access
                    string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

                    // Crea una instancia de la clase Conexion
                    ConexionBD conexionBD = new ConexionBD(databasePath);

                    try
                    {
                        // Abrir la conexión
                        conexionBD.Abrir();

                        // Crear la consulta SQL para eliminar
                        string query = "DELETE FROM Productos WHERE Código = ?";

                        using (OleDbCommand command = new OleDbCommand(query, conexionBD.ObtenerConexion()))
                        {
                            // Añadir el parámetro
                            command.Parameters.AddWithValue("?", codigoProducto);
                            command.ExecuteNonQuery();
                        }

                        // Mostrar mensaje de éxito
                        MessageBox.Show("Producto eliminado correctamente.");

                        // Recargar los datos en el DataGridView
                        CargarDatos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                    }
                    finally
                    {
                        conexionBD.Cerrar();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Ruta a la base de datos Access
            string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

            // Crea una instancia de la clase Conexion
            ConexionBD conexionBD = new ConexionBD(databasePath);

            // Obtén los valores de los TextBox
            string nombre = txtNombre.Text.Trim();
            string codigo = txtCodigo.Text.Trim();
            string categoria = txtCategoría.Text.Trim();

            // Construir la consulta SQL con filtros
            string query = "SELECT * FROM Productos WHERE 1=1";

            // Agregar filtros si los valores están presentes
            if (!string.IsNullOrEmpty(nombre))
            {
                query += " AND Nombre LIKE ?";
            }
            if (!string.IsNullOrEmpty(codigo))
            {
                query += " AND Código = ?";
            }
            if (!string.IsNullOrEmpty(categoria))
            {
                query += " AND Categoria LIKE ?";
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
                    if (!string.IsNullOrEmpty(codigo))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", codigo);
                    }
                    if (!string.IsNullOrEmpty(categoria))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", "%" + categoria + "%");
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

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto para actualizar.");
                return;
            }

            // Obtener el ID o código del producto seleccionado
            int codigoProducto = Convert.ToInt32(dgvInventario.SelectedRows[0].Cells["Código"].Value);

            // Crear una instancia del formulario frmActualizarDatos
            frmActualizarDatos frmActualizar = new frmActualizarDatos(codigoProducto);

            // Mostrar el formulario frmActualizarDatos
            if (frmActualizar.ShowDialog() == DialogResult.OK)
            {
                // Actualizar el DataGridView después de modificar el producto
                btnBuscar_Click(sender, e); // Vuelve a ejecutar la búsqueda actual
            }
        }
    }
}
