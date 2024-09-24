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
    public partial class frmActualizarDatos : Form
    {
        private int codigoProducto;
        public frmActualizarDatos(int codigoProducto)
        {
            InitializeComponent();
            this.codigoProducto = codigoProducto;
            CargarDatosProducto();
        }

        private void frmActualizarDatos_Load(object sender, EventArgs e)
        {

        }
        private void CargarDatosProducto()
        {
            // Ruta a la base de datos Access
            string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

            // Crea una instancia de la clase Conexion
            ConexionBD conexionBD = new ConexionBD(databasePath);

            try
            {
                // Abre la conexión
                conexionBD.Abrir();

                // Crear la consulta SQL para obtener los datos del producto
                string query = "SELECT * FROM Productos WHERE Código = ?";

                using (OleDbCommand command = new OleDbCommand(query, conexionBD.ObtenerConexion()))
                {
                    command.Parameters.AddWithValue("?", codigoProducto);
                    OleDbDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Cargar los datos del producto en los TextBox
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtDescripcion.Text = reader["Descripcion"].ToString();
                        txtPrecio.Text = reader["Precio"].ToString();
                        txtStock.Text = reader["Stock"].ToString();
                        txtCategoria.Text = reader["Categoria"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del producto: " + ex.Message);
            }
            finally
            {
                conexionBD.Cerrar();
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            // Código para actualizar el producto seleccionado en la base de datos
            // Similar a la lógica para insertar, pero con un UPDATE

            // Ruta a la base de datos Access
            string databasePath = "BaseDatos\\Lab3-1ra-clase.accdb";

            // Crea una instancia de la clase Conexion
            ConexionBD conexionBD = new ConexionBD(databasePath);

            try
            {
                // Abre la conexión
                conexionBD.Abrir();

                // Crear la consulta SQL para actualizar los datos del producto
                string query = "UPDATE Productos SET Nombre = ?, Descripcion = ?, Precio = ?, Stock = ?, Categoria = ? WHERE Código = ?";

                using (OleDbCommand command = new OleDbCommand(query, conexionBD.ObtenerConexion()))
                {
                    command.Parameters.AddWithValue("?", txtNombre.Text);
                    command.Parameters.AddWithValue("?", txtDescripcion.Text);
                    command.Parameters.AddWithValue("?", txtPrecio.Text);
                    command.Parameters.AddWithValue("?", txtStock.Text);
                    command.Parameters.AddWithValue("?", txtCategoria.Text);
                    command.Parameters.AddWithValue("?", codigoProducto);

                    // Ejecutar el comando
                    command.ExecuteNonQuery();
                }

                // Si se actualizan los datos correctamente, cerrar el formulario
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos: " + ex.Message);
            }
            finally
            {
                conexionBD.Cerrar();
            }
        }
    }
}
