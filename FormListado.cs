using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormListado : Form
    {
        private DataGridView dgvUsuarios;

        public FormListado()
        {
            InitializeCustomComponents();
            CargarUsuarios();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Listado de Usuarios";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            dgvUsuarios = new DataGridView();
            dgvUsuarios.Location = new Point(20, 20);
            dgvUsuarios.Size = new Size(640, 320);
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.ReadOnly = true;
            this.Controls.Add(dgvUsuarios);
        }

        private void CargarUsuarios()
        {
            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT id_usuario, nombre, correo, rol, activo FROM usuario";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion))
                    {
                        System.Data.DataTable tabla = new System.Data.DataTable();
                        adapter.Fill(tabla);
                        dgvUsuarios.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}