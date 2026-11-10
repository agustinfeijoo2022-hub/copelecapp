using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormVerOrdenes : Form
    {
        private DataGridView dgvOrdenes;
        private Color colorRol = Color.FromArgb(39, 174, 96); // Verde

        public FormVerOrdenes()
        {
            this.Text = $"Mis Órdenes - {Program.RolActual}";
            this.Size = new Size(700, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f);

            dgvOrdenes = new DataGridView();
            dgvOrdenes.Location = new Point(20, 20);
            dgvOrdenes.Size = new Size(640, 320);
            dgvOrdenes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrdenes.ReadOnly = true;
            dgvOrdenes.BackgroundColor = Color.White;
            this.Controls.Add(dgvOrdenes);
            CargarOrdenes();
        }

        private void CargarOrdenes()
        {
            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT id_orden, tipo_mantenimiento, descripcion, estado FROM ordenes_trabajo WHERE estado = 'Pendiente'";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion))
                    {
                        System.Data.DataTable tabla = new System.Data.DataTable();
                        adapter.Fill(tabla);
                        dgvOrdenes.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }
    }
}