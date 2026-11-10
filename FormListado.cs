using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormListado : Form
    {
        private DataGridView dgvUsuarios;
        private Color colorRol = Color.FromArgb(234, 122, 0);

        public FormListado()
        {
            InitializeCustomComponents();
            CargarUsuarios();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Gestión de Usuarios (Admin)";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            dgvUsuarios = new DataGridView();
            dgvUsuarios.Location = new Point(20, 20);
            dgvUsuarios.Size = new Size(740, 350);
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.BackgroundColor = Color.White;
            this.Controls.Add(dgvUsuarios);

            Button btnEliminar = new Button();
            btnEliminar.Text = "Eliminar Usuario";
            btnEliminar.Location = new Point(300, 390); // Centrado
            btnEliminar.Size = new Size(180, 40);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.BackColor = Color.FromArgb(192, 57, 43);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);
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
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0) { MessageBox.Show("Seleccione un usuario."); return; }

            int idUsuario = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["id_usuario"].Value);
            string nombreUsuario = dgvUsuarios.SelectedRows[0].Cells["nombre"].Value.ToString();

            if (idUsuario == 1) { MessageBox.Show("No puede eliminar al administrador principal."); return; }

            DialogResult confirmacion = MessageBox.Show($"¿Eliminar al usuario {nombreUsuario}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        conexion.Open();
                        string query = "DELETE FROM usuario WHERE id_usuario=@id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@id", idUsuario);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Usuario eliminado correctamente.");
                            CargarUsuarios(); 
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }
    }
}