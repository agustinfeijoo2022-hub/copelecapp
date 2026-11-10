using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class Form1 : Form
    {
        private Label lblCorreo;
        private Label lblContrasena;
        private TextBox txtCorreo;
        private TextBox txtContrasena;
        private Button btnIngresar;

        public Form1()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Login - Copelec";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblCorreo = new Label();
            lblCorreo.Text = "Correo:";
            lblCorreo.Location = new Point(50, 50);
            lblCorreo.AutoSize = true;
            this.Controls.Add(lblCorreo);

            txtCorreo = new TextBox();
            txtCorreo.Location = new Point(120, 47);
            txtCorreo.Size = new Size(160, 20);
            this.Controls.Add(txtCorreo);

            lblContrasena = new Label();
            lblContrasena.Text = "Contraseña:";
            lblContrasena.Location = new Point(50, 90);
            lblContrasena.AutoSize = true;
            this.Controls.Add(lblContrasena);

            txtContrasena = new TextBox();
            txtContrasena.Location = new Point(120, 87);
            txtContrasena.Size = new Size(160, 20);
            txtContrasena.PasswordChar = '*';
            this.Controls.Add(txtContrasena);

            btnIngresar = new Button();
            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(120, 130);
            btnIngresar.Size = new Size(100, 30);
            btnIngresar.Click += new EventHandler(this.btnIngresar_Click);
            this.Controls.Add(btnIngresar);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT COUNT(*) FROM usuario WHERE correo=@correo AND contrasena=@pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                        cmd.Parameters.AddWithValue("@pass", txtContrasena.Text);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Bienvenido al sistema Copelec");
                            this.Hide();
                            FormMenu menu = new FormMenu(txtCorreo.Text, "Admin");
                            menu.ShowDialog();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Credenciales incorrectas");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }
    }
}