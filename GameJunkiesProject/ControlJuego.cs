using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // <--- AGREGA ESTO ARRIBA CON LOS OTROS USINGS

namespace GameJunkiesProject
{
    public partial class ControlJuego : UserControl
    {
        public Juego JuegoDatos { get; private set; }
        public bool ModoBiblioteca { get; set; } = false; // Propiedad para el modo Jugar

        // Colores
        private readonly Color colorNormal = Color.FromArgb(90, 74, 147);
        private readonly Color colorHover = Color.FromArgb(110, 94, 167);
        private readonly Color colorBoton = Color.FromArgb(253, 202, 90);

        public ControlJuego()
        {
            InitializeComponent();

            // Configuración visual inicial
            this.Padding = new Padding(4);
            this.BackColor = Color.FromArgb(61, 47, 109);
            panelFondo.BackColor = colorNormal;
            this.Cursor = Cursors.Hand;

            // Estilos
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblRating.ForeColor = Color.Gold;

            btnVer.BackColor = colorBoton;
            btnVer.ForeColor = Color.Black;
            btnVer.FlatStyle = FlatStyle.Flat;
            btnVer.FlatAppearance.BorderSize = 0;
            btnVer.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnVer.Cursor = Cursors.Hand;

            // Redondeo
            AplicarRedondeo(panelFondo, 20);
            AplicarRedondeo(btnVer, 10);

            panelFondo.SizeChanged += (s, e) => AplicarRedondeo(panelFondo, 20);
            AsignarEventosHover(panelFondo);
        }

        // --- MÉTODO ÚNICO PARA CARGAR DATOS ---
        public void CargarDatos(Juego juego)
        {
            JuegoDatos = juego;
            lblTitulo.Text = juego.Name;
            lblRating.Text = $"⭐ {juego.Rating}";

            // Lógica: Si estamos en biblioteca, el botón es "JUGAR", si no, es "VER"
            if (ModoBiblioteca)
            {
                btnVer.Text = "JUGAR ▶";
                btnVer.BackColor = Color.FromArgb(46, 204, 113); // Verde
                btnVer.ForeColor = Color.White;

                // Reiniciamos eventos para evitar duplicados
                btnVer.Click -= btnVer_Click;
                btnVer.Click -= btnJugar_Click;

                // Asignamos solo el de jugar
                btnVer.Click += btnJugar_Click;
            }
            else
            {
                btnVer.Text = "VER";
                btnVer.BackColor = colorBoton;
                btnVer.ForeColor = Color.Black;

                // Reiniciamos eventos
                btnVer.Click -= btnVer_Click;
                btnVer.Click -= btnJugar_Click;

                // Asignamos solo el de ver detalles
                btnVer.Click += btnVer_Click;
            }

            try
            {
                if (!string.IsNullOrEmpty(juego.Background_Image))
                {
                    picPortada.Load(juego.Background_Image);
                    picPortada.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch
            {
                picPortada.BackColor = Color.FromArgb(50, 40, 90);
            }
        }

        // Evento para Ver Detalles (Tienda)
        private void btnVer_Click(object sender, EventArgs e)
        {
            if (JuegoDatos != null)
            {
                using (Form sombra = new Form())
                {
                    sombra.StartPosition = FormStartPosition.Manual;
                    sombra.FormBorderStyle = FormBorderStyle.None;
                    sombra.Opacity = 0.50d;
                    sombra.BackColor = Color.Black;
                    sombra.WindowState = FormWindowState.Maximized;
                    sombra.TopMost = true;
                    sombra.ShowInTaskbar = false;
                    sombra.Show();

                    FormDetalles detalle = new FormDetalles(JuegoDatos);
                    detalle.StartPosition = FormStartPosition.CenterScreen;
                    detalle.TopMost = true;

                    DialogResult resultado = detalle.ShowDialog();

                    sombra.Close();

                    // Si en el detalle le dieron a "Ir a Pagar", abrimos el carrito
                    if (resultado == DialogResult.Yes)
                    {
                        FormCarrito carrito = new FormCarrito();
                        carrito.ShowDialog();
                    }
                }
            }
        }

        // Evento para Jugar (Biblioteca)
        private void btnJugar_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Iniciando {JuegoDatos.Name}...\n\n(Imagina que el juego se abre en pantalla completa 🎮)",
                   "Ejecutando", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- MÉTODOS VISUALES AUXILIARES ---
        private void AsignarEventosHover(Control control)
        {
            control.MouseEnter += EfectoEntrada;
            control.MouseLeave += EfectoSalida;
            if (!(control is Button))
            {
                // Si haces click en la tarjeta (no en el botón), también actúa según el modo
                if (ModoBiblioteca) control.Click += btnJugar_Click;
                else control.Click += btnVer_Click;
            }

            foreach (Control hijo in control.Controls)
            {
                if (!(hijo is Button)) AsignarEventosHover(hijo);
            }
        }

        private void EfectoEntrada(object sender, EventArgs e)
        {
            this.Padding = new Padding(0);
            panelFondo.BackColor = colorHover;
        }

        private void EfectoSalida(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Padding = new Padding(4);
                panelFondo.BackColor = colorNormal;
            }
        }

        private void AplicarRedondeo(Control control, int radio)
        {
            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
            GraphicsPath path = new GraphicsPath();
            int d = radio * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y + bounds.Height - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - d, d, d, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }
    }
}