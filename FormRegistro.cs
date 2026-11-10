using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormRegistro : Form
    {
        private TextBox txtNombre;
        private TextBox txtCorreo;
        private TextBox txtContrasena;
        private ComboBox cmbRol;

        public FormRegistro()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Registrar Usuario";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblNombre = new Label();
            lblNombre.Text = "Nombre:";
            lblNombre.Location = new Point(50, 40);
            lblNombre.AutoSize = true;
            this.Controls.Add(lblNombre);

            txtNombre = new TextBox();
            txtNombre.Location = new Point(150, 37);
            txtNombre.Size = new Size(180, 20);
            this.Controls.Add(txtNombre);

            Label lblCorreo = new Label();
            lblCorreo.Text = "Correo:";
            lblCorreo.Location = new Point(50, 80);
            lblCorreo.AutoSize = true;
            this.Controls.Add(lblCorreo);

            txtCorreo = new TextBox();
            txtCorreo.Location = new Point(150, 77);
            txtCorreo.Size = new Size(180, 20);
            this.Controls.Add(txtCorreo);

            Label lblContrasena = new Label();
            lblContrasena.Text = "Contraseña:";
            lblContrasena.Location = new Point(50, 120);
            lblContrasena.AutoSize = true;
            this.Controls.Add(lblContrasena);

            txtContrasena = new TextBox();
            txtContrasena.Location = new Point(150, 117);
            txtContrasena.Size = new Size(180, 20);
            txtContrasena.PasswordChar = '*';
            this.Controls.Add(txtContrasena);

            Label lblRol = new Label();
            lblRol.Text = "Rol:";
            lblRol.Location = new Point(50, 160);
            lblRol.AutoSize = true;
            this.Controls.Add(lblRol);

            cmbRol = new ComboBox();
            cmbRol.Location = new Point(150, 157);
            cmbRol.Size = new Size(180, 20);
            cmbRol.Items.AddRange(new object[] { "Admin", "Supervisor", "Tecnico", "Operador" });
            cmbRol.SelectedIndex = 0;
            this.Controls.Add(cmbRol);

            Button btnGuardar = new Button();
            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(150, 210);
            btnGuardar.Size = new Size(100, 30);
            btnGuardar.Click += BtnGuardar_Click;
            this.Controls.Add(btnGuardar);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrEmpty(txtContrasena.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios");
                return;
            }

            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO usuario (nombre, correo, contrasena, rol, activo) VALUES (@nombre, @correo, @pass, @rol, 1)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                        cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                        cmd.Parameters.AddWithValue("@pass", txtContrasena.Text);
                        cmd.Parameters.AddWithValue("@rol", cmbRol.SelectedItem.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario registrado correctamente");
                        this.Close();
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