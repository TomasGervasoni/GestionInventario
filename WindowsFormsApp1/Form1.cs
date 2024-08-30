using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConexionBD objConexion = new ConexionBD("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Alumno\\Downloads\\Lab3-1ra-clase.accdb");

            objConexion.Abrir();

        }
    }
}
