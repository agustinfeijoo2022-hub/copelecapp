using System;
using System.Drawing;
using System.Windows.Forms;

namespace CopelecApp
{
    public partial class FormMenu : Form
    {
        private string nombreUsuario;
        private string rolUsuario;
        
        private Color colorFondo = Color.FromArgb(44, 62, 80); 
        private Color colorBotonNormal = Color.FromArgb(52, 73, 94);
        private Color colorBotonHover = Color.FromArgb(65, 90, 115);
        private Color colorTexto = Color.White;
        private string tituloPanel = "Panel de Control";

        // Variables para centrar el texto
        private Label lblTituloPanel;
        private Label lblSubtitulo;

        public FormMenu(string nombre, string rol)
        {
            nombreUsuario = Program.NombreActual; 
            rolUsuario = rol; 
            ConfigurarColoresPorRol(); 
            InitializeCustomComponents();
        }

        private void ConfigurarColoresPorRol()
        {
            if (rolUsuario == "Admin") { colorFondo = Color.FromArgb(234, 122, 0); colorBotonNormal = Color.FromArgb(207, 102, 0); colorBotonHover = Color.FromArgb(255, 149, 43); tituloPanel = "Administración General"; }
            else if (rolUsuario == "Supervisor") { colorFondo = Color.FromArgb(41, 128, 185); colorBotonNormal = Color.FromArgb(31, 97, 141); colorBotonHover = Color.FromArgb(52, 152, 219); tituloPanel = "Supervisión de Operaciones"; }
            else if (rolUsuario == "Tecnico") { colorFondo = Color.FromArgb(39, 174, 96); colorBotonNormal = Color.FromArgb(30, 132, 73); colorBotonHover = Color.FromArgb(46, 204, 113); tituloPanel = "Órdenes de Terreno"; }
            else { colorFondo = Color.FromArgb(52, 73, 94); colorBotonNormal = Color.FromArgb(44, 62, 80); colorBotonHover = Color.FromArgb(70, 90, 110); tituloPanel = "Reporte de Incidentes"; }
        }

        private void InitializeCustomComponents()
        {
            this.Text = $"Sistema COPELEC - Módulo {rolUsuario}";
            this.Size = new Size(850, 600); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245); 
            
            this.FormBorderStyle = FormBorderStyle.Sizable; 
            this.MaximizeBox = true;
            this.MinimumSize = new Size(800, 550); 

            // ==========================================
            // BARRA LATERAL (SIDEBAR)
            // ==========================================
            Panel sidebar = new Panel();
            sidebar.BackColor = colorFondo; 
            sidebar.Dock = DockStyle.Left;
            sidebar.Width = 260;
            this.Controls.Add(sidebar);

            Label lblLogo = new Label();
            lblLogo.Text = "⚡ COPELEC"; 
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Arial Black", 18, FontStyle.Bold);
            lblLogo.Location = new Point(20, 20);
            lblLogo.AutoSize = true;
            sidebar.Controls.Add(lblLogo);

            Label lblSubtituloLogo = new Label();
            lblSubtituloLogo.Text = "Eléctrica";
            lblSubtituloLogo.ForeColor = Color.White;
            lblSubtituloLogo.Font = new Font("Arial", 10, FontStyle.Regular);
            lblSubtituloLogo.Location = new Point(25, 55);
            lblSubtituloLogo.AutoSize = true;
            sidebar.Controls.Add(lblSubtituloLogo);

            Label lblBienvenida = new Label();
            lblBienvenida.Text = $"Hola,\n{nombreUsuario}";
            lblBienvenida.ForeColor = Color.White;
            lblBienvenida.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblBienvenida.Location = new Point(20, 100);
            lblBienvenida.AutoSize = true;
            sidebar.Controls.Add(lblBienvenida);

            Label lblRol = new Label();
            lblRol.Text = rolUsuario.ToUpper(); 
            lblRol.ForeColor = Color.FromArgb(255, 230, 200); 
            lblRol.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblRol.Location = new Point(20, 150);
            lblRol.AutoSize = true;
            sidebar.Controls.Add(lblRol);

            int posicionY = 180; 

            if (rolUsuario == "Admin" || rolUsuario == "Supervisor" || rolUsuario == "Tecnico" || rolUsuario == "Operador")
            {
                AgregarBoton(sidebar, "Reportar Falla", posicionY, (s, e) => new FormFallas().ShowDialog());
                posicionY += 45;
            }

            if (rolUsuario == "Tecnico")
            {
                AgregarBoton(sidebar, "Mis Órdenes de Trabajo", posicionY, (s, e) => new FormVerOrdenes().ShowDialog());
                posicionY += 45;
            }

            if (rolUsuario == "Admin" || rolUsuario == "Supervisor")
            {
                AgregarBoton(sidebar, "Ver Fallas Reportadas", posicionY, (s, e) => new FormVerFallas().ShowDialog());
                posicionY += 45;
                AgregarBoton(sidebar, "Gestión de Mantenimiento", posicionY, (s, e) => new FormMantenimiento().ShowDialog());
                posicionY += 45;
            }

            if (rolUsuario == "Admin")
            {
                AgregarBoton(sidebar, "Registrar Usuario", posicionY, (s, e) => new FormRegistro().ShowDialog());
                posicionY += 45;
                AgregarBoton(sidebar, "Listado de Usuarios", posicionY, (s, e) => new FormListado().ShowDialog());
                posicionY += 45;
            }

            AgregarBoton(sidebar, "Mi Perfil", posicionY, (s, e) => new FormPerfil().ShowDialog());
            posicionY += 45;

            // BOTÓN CERRAR SESIÓN ANCLADO AL FONDO
            Button btnSalir = new Button();
            btnSalir.Text = "Cerrar Sesión";
            btnSalir.Size = new Size(260, 45);
            btnSalir.Location = new Point(0, sidebar.Height - 60); 
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.BackColor = Color.FromArgb(192, 57, 43); 
            btnSalir.ForeColor = Color.White;
            btnSalir.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSalir.Click += (s, e) => { 
                Form1 login = new Form1();
                login.Show();
                this.Close(); 
            };
            sidebar.Controls.Add(btnSalir);

            // ==========================================
            // TEXTOS CENTRADOS MATEMÁTICAMENTE
            // ==========================================
            
            lblTituloPanel = new Label();
            lblTituloPanel.Text = "¡Bienvenido al Sistema COPELEC!";
            lblTituloPanel.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTituloPanel.ForeColor = colorFondo; 
            lblTituloPanel.AutoSize = true;
            this.Controls.Add(lblTituloPanel);

            lblSubtitulo = new Label();
            lblSubtitulo.Text = $"Hola {nombreUsuario}, te damos la bienvenida a tu módulo de {tituloPanel}.\nUtilice el menú lateral para acceder a sus funciones.";
            lblSubtitulo.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblSubtitulo.ForeColor = Color.DimGray;
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblSubtitulo);

            // Llamamos al método que los centra la primera vez
            CentrarTextos();

            // Cada vez que se cambie el tamaño de la ventana, volvemos a centrar
            this.Resize += (s, e) => { CentrarTextos(); };
        }

        // MÉTODO PARA CENTRAR EL TEXTO RESPECTO AL ESPACIO LIBRE (DERECHA)
        private void CentrarTextos()
        {
            // El espacio libre empieza después de los 260px del sidebar
            int anchoSidebar = 260; 
            int anchoDisponible = this.ClientSize.Width - anchoSidebar;
            int altoDisponible = this.ClientSize.Height;

            if (anchoDisponible > 0)
            {
                // Centramos el Título
                int xTitulo = anchoSidebar + (anchoDisponible - lblTituloPanel.Width) / 2;
                int yTitulo = (altoDisponible / 2) - 60; // Un poco más arriba del centro
                lblTituloPanel.Location = new Point(xTitulo, yTitulo);

                // Centramos el Subtítulo justo debajo del título
                int xSubtitulo = anchoSidebar + (anchoDisponible - lblSubtitulo.Width) / 2;
                int ySubtitulo = yTitulo + lblTituloPanel.Height + 20; 
                lblSubtitulo.Location = new Point(xSubtitulo, ySubtitulo);
            }
        }

        private void AgregarBoton(Panel sidebar, string texto, int y, EventHandler clickEvent)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Size = new Size(260, 45);
            btn.Location = new Point(0, y);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = colorBotonNormal; 
            btn.ForeColor = colorTexto;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0); 
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => { btn.BackColor = colorBotonHover; };
            btn.MouseLeave += (s, e) => { btn.BackColor = colorBotonNormal; };
            
            btn.Click += clickEvent;
            sidebar.Controls.Add(btn);
        }
    }
}