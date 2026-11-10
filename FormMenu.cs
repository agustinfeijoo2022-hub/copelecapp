using System;
using System.Drawing;
using System.Windows.Forms;

namespace CopelecApp
{
    public partial class FormMenu : Form
    {
        private string nombreUsuario;
        private string rolUsuario;

        public FormMenu(string nombre, string rol)
        {
            nombreUsuario = nombre;
            rolUsuario = rol;
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Menú Principal - Copelec";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblBienvenida = new Label();
            lblBienvenida.Text = $"Bienvenido, {nombreUsuario} ({rolUsuario})";
            lblBienvenida.Location = new Point(50, 30);
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Arial", 12, FontStyle.Bold);
            this.Controls.Add(lblBienvenida);

            Button btnMonitoreo = new Button();
            btnMonitoreo.Text = "Monitoreo de Paneles";
            btnMonitoreo.Location = new Point(150, 90);
            btnMonitoreo.Size = new Size(200, 40);
            this.Controls.Add(btnMonitoreo);

            Button btnRegistrar = new Button();
            btnRegistrar.Text = "Registrar Usuario";
            btnRegistrar.Location = new Point(150, 150);
            btnRegistrar.Size = new Size(200, 40);
            btnRegistrar.Click += (s, e) => { new FormRegistro().ShowDialog(); };
            this.Controls.Add(btnRegistrar);

            Button btnListado = new Button();
            btnListado.Text = "Listado de Usuarios";
            btnListado.Location = new Point(150, 210);
            btnListado.Size = new Size(200, 40);
            btnListado.Click += (s, e) => { new FormListado().ShowDialog(); };
            this.Controls.Add(btnListado);

            Button btnSalir = new Button();
            btnSalir.Text = "Cerrar Sesión";
            btnSalir.Location = new Point(150, 270);
            btnSalir.Size = new Size(200, 40);
            btnSalir.Click += BtnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            // Cerrar el menú actual
            this.Close();

            // Volver al login
            Form1 login = new Form1();
            login.Show();
        }
    }
}