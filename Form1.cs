using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Timer = System.Windows.Forms.Timer;

namespace CopelecApp
{
    public partial class Form1 : Form
    {
        private Label lblCorreo;
        private Label lblContrasena;
        private TextBox txtCorreo;
        private TextBox txtContrasena;
        private Button btnIngresar;
        private Label lblError; 
        private Panel header;
        
        private Timer timerExito;
        private string rolParaMenu = "";
        private Color colorActual = Color.FromArgb(234, 122, 0); 

        public Form1() { InitializeCustomComponents(); }

        private void InitializeCustomComponents()
        {
            this.Text = "Login - COPELEC";
            // Reduje un poco la altura porque ya no está el botón de registro
            this.Size = new Size(400, 480); 
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245); 
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            timerExito = new Timer();
            timerExito.Interval = 1500; 
            timerExito.Tick += TimerExito_Tick;

            header = new Panel();
            header.BackColor = colorActual; 
            header.Dock = DockStyle.Top;
            header.Height = 110;
            this.Controls.Add(header);

            Label lblLogo = new Label();
            lblLogo.Text = "⚡ COPELEC"; 
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Arial Black", 20, FontStyle.Bold);
            lblLogo.Location = new Point(85, 25);
            lblLogo.AutoSize = true;
            header.Controls.Add(lblLogo);

            Label lblSubtituloLogo = new Label();
            lblSubtituloLogo.Text = "Eléctrica";
            lblSubtituloLogo.ForeColor = Color.White;
            lblSubtituloLogo.Font = new Font("Arial", 10, FontStyle.Regular);
            lblSubtituloLogo.Location = new Point(170, 65);
            lblSubtituloLogo.AutoSize = true;
            header.Controls.Add(lblSubtituloLogo);

            lblCorreo = new Label();
            lblCorreo.Text = "Correo:";
            lblCorreo.Location = new Point(50, 140);
            lblCorreo.AutoSize = true;
            lblCorreo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblCorreo);

            txtCorreo = new TextBox();
            txtCorreo.Location = new Point(50, 165);
            txtCorreo.Size = new Size(280, 25);
            txtCorreo.Font = new Font("Segoe UI", 10);
            this.Controls.Add(txtCorreo);

            lblContrasena = new Label();
            lblContrasena.Text = "Contraseña:";
            lblContrasena.Location = new Point(50, 210);
            lblContrasena.AutoSize = true;
            lblContrasena.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(lblContrasena);

            txtContrasena = new TextBox();
            txtContrasena.Location = new Point(50, 235);
            txtContrasena.Size = new Size(280, 25);
            txtContrasena.Font = new Font("Segoe UI", 10);
            txtContrasena.PasswordChar = '*';
            this.Controls.Add(txtContrasena);

            btnIngresar = new Button();
            btnIngresar.Text = "Ingresar";
            btnIngresar.Location = new Point(50, 290);
            btnIngresar.Size = new Size(280, 40);
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.BackColor = colorActual; 
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnIngresar.Cursor = Cursors.Hand;
            btnIngresar.Click += new EventHandler(this.btnIngresar_Click);
            
            btnIngresar.MouseEnter += (s, e) => { btnIngresar.BackColor = ControlPaint.Light(colorActual, 0.2f); };
            btnIngresar.MouseLeave += (s, e) => { btnIngresar.BackColor = colorActual; };
            
            this.Controls.Add(btnIngresar);

            lblError = new Label();
            // Lo subimos un poco para que quede bien centrado
            lblError.Location = new Point(50, 345); 
            lblError.Size = new Size(280, 35);
            lblError.TextAlign = ContentAlignment.MiddleCenter;
            lblError.ForeColor = Color.White; 
            lblError.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblError.Visible = false; 
            this.Controls.Add(lblError);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            lblError.Visible = false; 
            btnIngresar.Enabled = false; 

            string cadenaConexion = "server=localhost;port=3306;database=copelecdb;uid=root;pwd=Copelec123;";

            try
            {
                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    
                    string query = "SELECT nombre, rol, contrasena FROM usuario WHERE correo=@correo";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@correo", txtCorreo.Text);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nombreReal = reader["nombre"].ToString();
                                string rolReal = reader["rol"].ToString();
                                string passGuardada = reader["contrasena"].ToString();

                                bool passwordCorrecta = false;

                                // MODO TRANSICIÓN (Acepta Hash o Texto Plano)
                                if (passGuardada.StartsWith("$2a$") || passGuardada.StartsWith("$2b$") || passGuardada.StartsWith("$2y$"))
                                {
                                    if (BCrypt.Net.BCrypt.Verify(txtContrasena.Text, passGuardada))
                                    {
                                        passwordCorrecta = true;
                                    }
                                }
                                else
                                {
                                    if (txtContrasena.Text == passGuardada)
                                    {
                                        passwordCorrecta = true;
                                    }
                                }

                                if (passwordCorrecta)
                                {
                                    rolParaMenu = rolReal;
                                    Program.RolActual = rolReal;
                                    Program.NombreActual = nombreReal;
                                    Program.CorreoActual = txtCorreo.Text;

                                    if (rolReal == "Admin") colorActual = Color.FromArgb(234, 122, 0);
                                    else if (rolReal == "Supervisor") colorActual = Color.FromArgb(41, 128, 185);
                                    else if (rolReal == "Tecnico") colorActual = Color.FromArgb(39, 174, 96);
                                    else if (rolReal == "Operador") colorActual = Color.FromArgb(52, 73, 94);
                                    
                                    btnIngresar.BackColor = colorActual;
                                    header.BackColor = colorActual;
                                    
                                    lblError.Text = "¡Bienvenido al sistema COPELEC!";
                                    lblError.BackColor = Color.FromArgb(39, 174, 96); 
                                    lblError.Visible = true;
                                    
                                    timerExito.Start();
                                }
                                else
                                {
                                    lblError.Text = "La contraseña es incorrecta";
                                    lblError.BackColor = Color.FromArgb(192, 57, 43); 
                                    lblError.Visible = true;
                                    btnIngresar.Enabled = true;
                                    txtContrasena.Clear(); 
                                    txtContrasena.Focus(); 
                                }
                            }
                            else
                            {
                                lblError.Text = "Este usuario no existe en el sistema";
                                lblError.BackColor = Color.FromArgb(192, 57, 43); 
                                lblError.Visible = true;
                                btnIngresar.Enabled = true;
                                txtCorreo.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblError.Text = "Error de conexión a la base de datos";
                lblError.BackColor = Color.FromArgb(192, 57, 43); 
                lblError.Visible = true;
                btnIngresar.Enabled = true;
            }
        }

        private void TimerExito_Tick(object sender, EventArgs e)
        {
            timerExito.Stop(); 
            this.Hide();       
            FormMenu menu = new FormMenu(Program.CorreoActual, rolParaMenu);
            menu.Show();
        }
    }
}