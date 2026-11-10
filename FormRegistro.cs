using System;
using System.Drawing;
using System.Text.RegularExpressions;
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
        private Color colorRol = Color.FromArgb(234, 122, 0);

        public FormRegistro() { InitializeCustomComponents(); }

        private void InitializeCustomComponents()
        {
            this.Text = "Registrar Usuario";
            this.Size = new Size(450, 400); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblNombre = new Label() { Text = "Nombre:", Location = new Point(40, 40), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtNombre = new TextBox() { Location = new Point(150, 40), Size = new Size(220, 25), Font = new Font("Segoe UI", 10) };
            
            Label lblCorreo = new Label() { Text = "Correo:", Location = new Point(40, 90), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtCorreo = new TextBox() { Location = new Point(150, 90), Size = new Size(220, 25), Font = new Font("Segoe UI", 10) };
            
            Label lblContrasena = new Label() { Text = "Contraseña:", Location = new Point(40, 140), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            txtContrasena = new TextBox() { Location = new Point(150, 140), Size = new Size(220, 25), Font = new Font("Segoe UI", 10), PasswordChar = '*' };
            Label lblInfoPass = new Label() { Text = "(8-12 caract. 1 Mayús, 1 Núm, 1 Símbolo)", Location = new Point(150, 168), AutoSize = true, Font = new Font("Segoe UI", 8, FontStyle.Italic), ForeColor = Color.DimGray };

            Label lblRol = new Label() { Text = "Rol:", Location = new Point(40, 200), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            cmbRol = new ComboBox() { Location = new Point(150, 200), Size = new Size(220, 25), Font = new Font("Segoe UI", 10), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbRol.Items.AddRange(new object[] { "Admin", "Supervisor", "Tecnico", "Operador" });
            cmbRol.SelectedIndex = 0;

            Button btnGuardar = new Button() { Text = "Guardar Usuario", Location = new Point(100, 270), Size = new Size(250, 45), FlatStyle = FlatStyle.Flat, BackColor = colorRol, ForeColor = Color.White, Font = new Font("Segoe UI", 11, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.Click += BtnGuardar_Click;

            this.Controls.Add(lblNombre); this.Controls.Add(txtNombre);
            this.Controls.Add(lblCorreo); this.Controls.Add(txtCorreo);
            this.Controls.Add(lblContrasena); this.Controls.Add(txtContrasena); this.Controls.Add(lblInfoPass);
            this.Controls.Add(lblRol); this.Controls.Add(cmbRol);
            this.Controls.Add(btnGuardar);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text)) { MessageBox.Show("Todos los campos son obligatorios."); return; }

            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(txtCorreo.Text, patronCorreo)) { MessageBox.Show("Formato de correo inválido."); return; }

            string password = txtContrasena.Text;
            if (password.Length < 8 || password.Length > 12) { MessageBox.Show("La contraseña debe tener entre 8 y 12 caracteres."); return; }
            if (!Regex.IsMatch(password, @"[A-Z]") || !Regex.IsMatch(password, @"[a-z]") || !Regex.IsMatch(password, @"[0-9]") || !Regex.IsMatch(password, @"[\W_]"))
            { MessageBox.Show("La contraseña debe tener mayúscula, minúscula, número y símbolo."); return; }

            // ENCRIPTACIÓN DE LA CONTRASEÑA CON BCRYPT
            string hashContrasena = BCrypt.Net.BCrypt.HashPassword(txtContrasena.Text);

            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO usuario (nombre, correo, contrasena, rol, activo) VALUES (@nombre, @correo, @pass, @rol, 1)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@correo", txtCorreo.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", hashContrasena);
                        cmd.Parameters.AddWithValue("@rol", cmbRol.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario registrado correctamente.");
                        this.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) MessageBox.Show("Este correo ya está registrado.");
                else MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}