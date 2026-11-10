using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormPerfil : Form
    {
        private Color colorRol = Color.FromArgb(234, 122, 0);

        public FormPerfil()
        {
            if (Program.RolActual == "Admin") colorRol = Color.FromArgb(234, 122, 0);
            else if (Program.RolActual == "Supervisor") colorRol = Color.FromArgb(41, 128, 185);
            else if (Program.RolActual == "Tecnico") colorRol = Color.FromArgb(39, 174, 96);
            else if (Program.RolActual == "Operador") colorRol = Color.FromArgb(52, 73, 94);

            this.Text = "Mi Perfil";
            this.Size = new Size(400, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblTitulo = new Label() { Text = "Mi Perfil", Location = new Point(30, 20), AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = colorRol };
            Label lblNombre = new Label() { Text = $"Nombre: {Program.NombreActual}", Location = new Point(30, 70), AutoSize = true, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            Label lblCorreo = new Label() { Text = $"Correo: {Program.CorreoActual}", Location = new Point(30, 110), AutoSize = true, Font = new Font("Segoe UI", 11) };
            Label lblRol = new Label() { Text = $"Rol en el sistema: {Program.RolActual}", Location = new Point(30, 150), AutoSize = true, Font = new Font("Segoe UI", 11) };
            Label lblPass = new Label() { Text = $"Contraseña: ************", Location = new Point(30, 190), AutoSize = true, Font = new Font("Segoe UI", 11) };
            
            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblNombre);
            this.Controls.Add(lblCorreo);
            this.Controls.Add(lblRol);
            this.Controls.Add(lblPass);

            Button btnCambiarPass = new Button();
            btnCambiarPass.Text = "Cambiar mi contraseña";
            btnCambiarPass.Location = new Point(30, 240);
            btnCambiarPass.Size = new Size(320, 45);
            btnCambiarPass.FlatStyle = FlatStyle.Flat;
            btnCambiarPass.FlatAppearance.BorderSize = 0;
            btnCambiarPass.BackColor = colorRol;
            btnCambiarPass.ForeColor = Color.White;
            btnCambiarPass.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnCambiarPass.Cursor = Cursors.Hand;
            btnCambiarPass.Click += BtnCambiarPass_Click;
            this.Controls.Add(btnCambiarPass);
        }

        private void BtnCambiarPass_Click(object sender, EventArgs e)
        {
            string nuevaPass = MostrarDialogoPassword();

            if (!string.IsNullOrEmpty(nuevaPass))
            {
                if (nuevaPass.Length < 8 || nuevaPass.Length > 12) { MessageBox.Show("Debe tener entre 8 y 12 caracteres."); return; }
                if (!Regex.IsMatch(nuevaPass, @"[A-Z]") || !Regex.IsMatch(nuevaPass, @"[a-z]") || !Regex.IsMatch(nuevaPass, @"[0-9]") || !Regex.IsMatch(nuevaPass, @"[\W_]"))
                { MessageBox.Show("Debe tener mayúscula, minúscula, número y símbolo."); return; }

                // AQUI SE HASHEA LA CLAVE NUEVA
                string hashNuevaPass = BCrypt.Net.BCrypt.HashPassword(nuevaPass);

                string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE usuario SET contrasena=@pass WHERE correo=@correo";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@pass", hashNuevaPass);
                            cmd.Parameters.AddWithValue("@correo", Program.CorreoActual);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Tu contraseña ha sido actualizada.", "Éxito");
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private string MostrarDialogoPassword()
        {
            Form prompt = new Form() { Width = 400, Height = 200, FormBorderStyle = FormBorderStyle.FixedDialog, Text = "Cambiar mi contraseña", StartPosition = FormStartPosition.CenterScreen, BackColor = ControlPaint.Light(colorRol, 0.85f) };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Nueva contraseña:", AutoSize = true };
            Label lblAyuda = new Label() { Left = 20, Top = 45, Text = "(8-12 caract. 1 Mayús, 1 Núm, 1 Símbolo)", AutoSize = true, ForeColor = Color.DimGray, Font = new Font("Segoe UI", 8, FontStyle.Italic) };
            TextBox textBox = new TextBox() { Left = 20, Top = 70, Width = 340, PasswordChar = '*' };
            Button confirmation = new Button() { Text = "Guardar", Left = 260, Width = 100, Top = 120, DialogResult = DialogResult.OK };
            confirmation.FlatStyle = FlatStyle.Flat; confirmation.FlatAppearance.BorderSize = 0; confirmation.BackColor = colorRol; confirmation.ForeColor = Color.White;

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox); prompt.Controls.Add(confirmation); prompt.Controls.Add(textLabel); prompt.Controls.Add(lblAyuda);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}