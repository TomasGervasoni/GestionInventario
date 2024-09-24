using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
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
        private string GenerarReporteExcel()
        {
            // Establecer el contexto de la licencia para EPPlus
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // O LicenseContext.Commercial si tienes una licencia comercial

            // Obtener la ruta de la carpeta "exel" dentro del directorio del proyecto
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory; // Directorio del proyecto en ejecución
            string folderPath = Path.Combine(projectDirectory, "exel"); // Ruta de la carpeta "exel"

            // Crear la carpeta "exel" si no existe
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Definir la ruta para guardar el archivo Excel en la carpeta "exel"
            string filePath = Path.Combine(folderPath, "ReporteInventario.xlsx");

            // Crear paquete Excel
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crear hoja de trabajo
                var worksheet = excel.Workbook.Worksheets.Add("Inventario");

                // Escribir cabeceras
                worksheet.Cells[1, 1].Value = "Código";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Descripcion";
                worksheet.Cells[1, 4].Value = "Precio";
                worksheet.Cells[1, 5].Value = "Stock";

                // Conectar a la base de datos y obtener los productos
                string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";
                ConexionBD conexionBD = new ConexionBD(databasePath);

                try
                {
                    conexionBD.Abrir();
                    string query = "SELECT * FROM Productos";
                    using (OleDbCommand command = new OleDbCommand(query, conexionBD.ObtenerConexion()))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            int row = 2; // Empezar a escribir desde la fila 2

                            // Escribir datos en el Excel
                            while (reader.Read())
                            {
                                worksheet.Cells[row, 1].Value = reader["Código"].ToString();
                                worksheet.Cells[row, 2].Value = reader["Nombre"].ToString();
                                worksheet.Cells[row, 3].Value = reader["Descripcion"].ToString();
                                worksheet.Cells[row, 4].Value = reader["Precio"].ToString();
                                worksheet.Cells[row, 5].Value = reader["Stock"].ToString();
                                row++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el reporte: " + ex.Message);
                }
                finally
                {
                    conexionBD.Cerrar();
                }

                // Guardar archivo Excel en la ruta definida en la carpeta "exel"
                FileInfo excelFile = new FileInfo(filePath);
                excel.SaveAs(excelFile);
            }

            return filePath; // Devolver la ruta del archivo generado
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
                string categoria = txtCategoria.Text;

            // Validar los valores (podrías agregar más validaciones según sea necesario)
            if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(descripcion) ||
                    string.IsNullOrWhiteSpace(precio) ||
                    string.IsNullOrWhiteSpace(stock) ||
                    string.IsNullOrWhiteSpace(categoria))
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
                    command.Parameters.AddWithValue("?", categoria);

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
                txtCategoria.Clear();

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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //crea una instancia del formulario frmModificar
            frmModificar frmModificar = new frmModificar();

            //Oculta el formulario frmInventario
            this.Hide();

            // Muestra el formulario frmModificar y verifica si se ha cerrado con DialogResult.OK
            frmModificar.ShowDialog();
           
            //actualiza la grilla
            CargarDatos();
            
            //al cerrar el formulario vuelve al formulario anterior
            this.Show();
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
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

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            // Generar el reporte y obtener la ruta del archivo
            string reportePath = GenerarReporteExcel();

            // Mostrar el archivo en Excel
            if (File.Exists(reportePath))
            {
                System.Diagnostics.Process.Start(reportePath); // Esto abrirá el archivo con la aplicación predeterminada (normalmente Excel)
            }

            // Mostrar mensaje con la ruta del archivo generado
            MessageBox.Show($"Reporte generado: {reportePath}");
        }
    }
}
