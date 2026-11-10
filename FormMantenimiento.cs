using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormMantenimiento : Form
    {
        private Color colorRol = Color.FromArgb(41, 128, 185); // Azul por defecto (Supervisor)

        public FormMantenimiento()
        {
            if (Program.RolActual == "Admin") colorRol = Color.FromArgb(234, 122, 0);
            else if (Program.RolActual == "Supervisor") colorRol = Color.FromArgb(41, 128, 185);

            this.Text = $"Mantenimiento - {Program.RolActual}";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f);

            Label lblTitulo = new Label();
            lblTitulo.Text = "Seleccione el tipo de mantenimiento:";
            lblTitulo.Location = new Point(50, 20);
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblTitulo);

            Button btnPreventivo = new Button();
            btnPreventivo.Text = "Mantenimiento Preventivo";
            btnPreventivo.Location = new Point(50, 60);
            btnPreventivo.Size = new Size(280, 40);
            btnPreventivo.FlatStyle = FlatStyle.Flat;
            btnPreventivo.FlatAppearance.BorderSize = 0;
            btnPreventivo.BackColor = colorRol;
            btnPreventivo.ForeColor = Color.White;
            btnPreventivo.Click += (s, e) => { CrearOrden("Preventivo"); };
            this.Controls.Add(btnPreventivo);

            Button btnCorrectivo = new Button();
            btnCorrectivo.Text = "Mantenimiento Correctivo";
            btnCorrectivo.Location = new Point(50, 120);
            btnCorrectivo.Size = new Size(280, 40);
            btnCorrectivo.FlatStyle = FlatStyle.Flat;
            btnCorrectivo.FlatAppearance.BorderSize = 0;
            btnCorrectivo.BackColor = colorRol;
            btnCorrectivo.ForeColor = Color.White;
            btnCorrectivo.Click += (s, e) => { CrearOrden("Correctivo"); };
            this.Controls.Add(btnCorrectivo);
        }

        private void CrearOrden(string tipo)
        {
            string descripcion = PedirDescripcion(tipo);
            if (!string.IsNullOrEmpty(descripcion))
            {
                string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        conexion.Open();
                        string query = "INSERT INTO ordenes_trabajo (tipo_mantenimiento, descripcion) VALUES (@tipo, @desc)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@tipo", tipo);
                            cmd.Parameters.AddWithValue("@desc", descripcion);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show($"Orden de Trabajo {tipo} creada.");
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private string PedirDescripcion(string tipo)
        {
            Form prompt = new Form()
            {
                Width = 400, Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = $"Nueva Orden: {tipo}",
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = ControlPaint.Light(colorRol, 0.85f)
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Descripción de la tarea:", AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340 };
            Button confirmation = new Button() { Text = "Guardar", Left = 260, Width = 100, Top = 90, DialogResult = DialogResult.OK, BackColor = colorRol, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }
    }
}