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

        // Interruptor: False = Tienda (Comprar), True = Biblioteca (Jugar)
        public bool ModoBiblioteca { get; set; } = false;

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

            // Estilos de texto
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblRating.ForeColor = Color.Gold;

            // Configuración base del botón
            btnVer.FlatStyle = FlatStyle.Flat;
            btnVer.FlatAppearance.BorderSize = 0;
            btnVer.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnVer.Cursor = Cursors.Hand;

            // Redondeo
            AplicarRedondeo(panelFondo, 20);
            AplicarRedondeo(btnVer, 10);

            // Eventos de redimensionado
            panelFondo.SizeChanged += (s, e) => AplicarRedondeo(panelFondo, 20);

            // Asignar eventos de Hover y Click a toda la tarjeta
            AsignarEventosHover(panelFondo);
        }

        public void CargarDatos(Juego juego)
        {
            JuegoDatos = juego;
            lblTitulo.Text = juego.Name;
            lblRating.Text = $"⭐ {juego.Rating}";

            // Lógica para cambiar el Botón según dónde estemos
            if (ModoBiblioteca)
            {
                // MODO BIBLIOTECA: Botón Verde "JUGAR"
                btnVer.Text = "JUGAR ▶";
                btnVer.BackColor = Color.FromArgb(46, 204, 113);
                btnVer.ForeColor = Color.White;

                // Reiniciamos eventos y asignamos JUGAR
                btnVer.Click -= btnVer_Click;
                btnVer.Click -= btnJugar_Click;
                btnVer.Click += btnJugar_Click;
            }
            else
            {
                // MODO TIENDA: Botón Amarillo "VER"
                btnVer.Text = "VER";
                btnVer.BackColor = colorBoton;
                btnVer.ForeColor = Color.Black;

                // Reiniciamos eventos y asignamos VER
                btnVer.Click -= btnVer_Click;
                btnVer.Click -= btnJugar_Click;
                btnVer.Click += btnVer_Click;
            }

            // Cargar imagen
            try
            {
                if (!string.IsNullOrEmpty(juego.Background_Image))
                {
                    picPortada.Load(juego.Background_Image);
                    picPortada.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch { picPortada.BackColor = Color.FromArgb(50, 40, 90); }
        }

        // --- EVENTO 1: VER DETALLES (TIENDA) ---
        // Este es el código que te faltaba
        private void btnVer_Click(object sender, EventArgs e)
        {
            if (JuegoDatos != null)
            {
                using (Form sombra = new Form())
                {
                    // ... (Configuración de sombra igual) ...
                    sombra.Opacity = 0.50d;
                    sombra.BackColor = Color.Black;
                    sombra.WindowState = FormWindowState.Maximized;
                    sombra.FormBorderStyle = FormBorderStyle.None;
                    sombra.Show();

                    // --- AQUÍ ESTÁ EL CAMBIO ---
                    // Pasamos 'ModoBiblioteca' al constructor
                    FormDetalles detalle = new FormDetalles(JuegoDatos, ModoBiblioteca);

                    detalle.StartPosition = FormStartPosition.CenterScreen;
                    detalle.TopMost = true;

                    DialogResult resultado = detalle.ShowDialog();
                    sombra.Close();

                    if (resultado == DialogResult.Yes)
                    {
                        FormCarrito carrito = new FormCarrito(Usuario.SesionActual);
                        carrito.ShowDialog();
                    }
                }
            }
        }

        // --- EVENTO 2: JUGAR (BIBLIOTECA) ---
        private void btnJugar_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Iniciando {JuegoDatos.Name}...\n\n(Imagina que el juego se abre en pantalla completa 🎮)",
                            "GameJunkies Launcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- MÉTODOS VISUALES (HOVER Y CLICKS EN TARJETA) ---
        // Este método es CRÍTICO para que puedas dar click en la imagen, no solo en el botón
        private void AsignarEventosHover(Control control)
        {
            control.MouseEnter += EfectoEntrada;
            control.MouseLeave += EfectoSalida;

            // Si el control NO es el botón, le asignamos el click de la tarjeta
            if (!(control is Button))
            {
                // Quitamos eventos previos para no acumularlos
                control.Click -= btnJugar_Click;
                control.Click -= btnVer_Click;

                // Asignamos el evento correcto según el modo
                if (ModoBiblioteca)
                    control.Click += btnJugar_Click;
                else
                    control.Click += btnVer_Click;
            }

            // Recursividad para hijos (la imagen, los labels, etc.)
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