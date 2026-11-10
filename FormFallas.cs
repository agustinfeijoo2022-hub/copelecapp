using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CopelecApp
{
    public partial class FormFallas : Form
    {
        private ComboBox cmbEquipo;
        private ComboBox cmbTipo;
        private TextBox txtDesc;
        private Color colorRol = Color.FromArgb(52, 73, 94); // Por defecto Gris

        public FormFallas()
        {
            // Lee el rol guardado en la sesión global
            if (Program.RolActual == "Admin") colorRol = Color.FromArgb(234, 122, 0);
            else if (Program.RolActual == "Supervisor") colorRol = Color.FromArgb(41, 128, 185);
            else if (Program.RolActual == "Tecnico") colorRol = Color.FromArgb(39, 174, 96);
            else if (Program.RolActual == "Operador") colorRol = Color.FromArgb(52, 73, 94);

            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = $"Reportar Falla - {Program.RolActual}";
            this.Size = new Size(450, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ControlPaint.Light(colorRol, 0.85f); // Fondo clarito del color del rol
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblEquipo = new Label();
            lblEquipo.Text = "Equipo / Estación:";
            lblEquipo.Location = new Point(30, 30);
            lblEquipo.AutoSize = true;
            lblEquipo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblEquipo);

            cmbEquipo = new ComboBox();
            cmbEquipo.Items.AddRange(new object[] { "Panel Solar", "Inversor Central", "Transformador de Red", "Línea de Distribución", "Sistema de Monitoreo" });
            cmbEquipo.Location = new Point(170, 30);
            cmbEquipo.Size = new Size(220, 25);
            cmbEquipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEquipo.SelectedIndex = 0;
            this.Controls.Add(cmbEquipo);

            Label lblTipo = new Label();
            lblTipo.Text = "Tipo de Falla:";
            lblTipo.Location = new Point(30, 80);
            lblTipo.AutoSize = true;
            lblTipo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblTipo);

            cmbTipo = new ComboBox();
            cmbTipo.Items.AddRange(new object[] { "Daño físico", "Sobrecalentamiento", "Cortocircuito", "Falla de comunicación", "Robo" });
            cmbTipo.Location = new Point(170, 80);
            cmbTipo.Size = new Size(220, 25);
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.SelectedIndex = 0;
            this.Controls.Add(cmbTipo);

            Label lblDesc = new Label();
            lblDesc.Text = "Descripción:";
            lblDesc.Location = new Point(30, 130);
            lblDesc.AutoSize = true;
            lblDesc.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblDesc);

            txtDesc = new TextBox();
            txtDesc.Location = new Point(170, 130);
            txtDesc.Size = new Size(220, 80);
            txtDesc.Multiline = true;
            this.Controls.Add(txtDesc);

            Button btnReportar = new Button();
            btnReportar.Text = "Reportar Falla";
            btnReportar.Location = new Point(120, 250);
            btnReportar.Size = new Size(200, 45);
            btnReportar.FlatStyle = FlatStyle.Flat;
            btnReportar.FlatAppearance.BorderSize = 0;
            btnReportar.BackColor = colorRol;
            btnReportar.ForeColor = Color.White;
            btnReportar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnReportar.Click += BtnReportar_Click;
            this.Controls.Add(btnReportar);
        }

        private void BtnReportar_Click(object sender, EventArgs e)
        {
            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "INSERT INTO fallas (equipo, tipo_falla, descripcion) VALUES (@equipo, @tipo, @desc)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@equipo", cmbEquipo.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@tipo", cmbTipo.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@desc", txtDesc.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Falla registrada en el sistema.", "Éxito");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }
    }
}