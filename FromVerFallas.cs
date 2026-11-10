using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormVerFallas : Form
    {
        private DataGridView dgvFallas;
        private Button btnActualizar;
        private Color colorRol = Color.FromArgb(41, 128, 185); 

        public FormVerFallas()
        {
            if (Program.RolActual == "Admin") colorRol = Color.FromArgb(234, 122, 0);
            else if (Program.RolActual == "Supervisor") colorRol = Color.FromArgb(41, 128, 185);

            this.Text = $"Gestión de Fallas - {Program.RolActual}";
            this.Size = new Size(750, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f);

            dgvFallas = new DataGridView();
            dgvFallas.Location = new Point(20, 20);
            dgvFallas.Size = new Size(690, 350);
            dgvFallas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvFallas.ReadOnly = true;
            dgvFallas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFallas.MultiSelect = false;
            dgvFallas.BackgroundColor = Color.White;
            this.Controls.Add(dgvFallas);

            btnActualizar = new Button();
            btnActualizar.Text = "Actualizar Estado de Falla";
            btnActualizar.Location = new Point(250, 390);
            btnActualizar.Size = new Size(220, 40);
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.BackColor = colorRol;
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnActualizar.Click += BtnActualizar_Click;
            this.Controls.Add(btnActualizar);
            CargarFallas();
        }

        private void CargarFallas()
        {
            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT id_falla, equipo, tipo_falla, descripcion, estado FROM fallas";
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conexion))
                    {
                        System.Data.DataTable tabla = new System.Data.DataTable();
                        adapter.Fill(tabla);
                        dgvFallas.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (dgvFallas.SelectedRows.Count == 0) return;
            int idFalla = Convert.ToInt32(dgvFallas.SelectedRows[0].Cells["id_falla"].Value);
            string estadoActual = dgvFallas.SelectedRows[0].Cells["estado"].Value.ToString();
            string nuevoEstado = MostrarDialogoEstado(estadoActual);

            if (nuevoEstado != null)
            {
                string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                    {
                        conexion.Open();
                        string query = "UPDATE fallas SET estado=@estado WHERE id_falla=@id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                            cmd.Parameters.AddWithValue("@id", idFalla);
                            cmd.ExecuteNonQuery();
                            CargarFallas();
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private string MostrarDialogoEstado(string estadoActual)
        {
            Form prompt = new Form()
            {
                Width = 300, Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Cambiar Estado", StartPosition = FormStartPosition.CenterScreen,
                BackColor = ControlPaint.Light(colorRol, 0.85f)
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Nuevo estado:", AutoSize = true };
            ComboBox comboEstados = new ComboBox() { Left = 20, Top = 50, Width = 240 };
            comboEstados.Items.AddRange(new object[] { "Pendiente", "En Reparación", "Resuelta" });
            comboEstados.Text = estadoActual; 
            Button confirmation = new Button() { Text = "Guardar", Left = 160, Width = 100, Top = 90, DialogResult = DialogResult.OK, BackColor = colorRol, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(comboEstados);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? comboEstados.SelectedItem.ToString() : null;
        }
    }
}