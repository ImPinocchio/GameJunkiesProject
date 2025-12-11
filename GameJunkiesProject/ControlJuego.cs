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

        // --- COLORES DE TU FIGMA ---
        // 1. Morado Estantes (#5A4A93) -> RGB: 90, 74, 147
        private readonly Color colorNormal = Color.FromArgb(90, 74, 147);

        // 2. Un tono ligeramente más claro para el efecto Hover
        private readonly Color colorHover = Color.FromArgb(110, 94, 167);

        // 3. Amarillo Botón (#FDCA5A) -> RGB: 253, 202, 90
        private readonly Color colorBoton = Color.FromArgb(253, 202, 90);

        public ControlJuego()
        {
            InitializeComponent();

            // --- CONFIGURACIÓN VISUAL ---
            this.Padding = new Padding(4);
            this.BackColor = Color.FromArgb(61, 47, 109); // Fondo oscuro (se funde con el Form)

            panelFondo.BackColor = colorNormal;
            this.Cursor = Cursors.Hand;

            // --- ESTILOS DE TEXTO Y BOTÓN ---
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            lblRating.ForeColor = Color.Gold;

            btnVer.BackColor = colorBoton;
            btnVer.ForeColor = Color.Black;
            btnVer.FlatStyle = FlatStyle.Flat;
            btnVer.FlatAppearance.BorderSize = 0;
            btnVer.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnVer.Cursor = Cursors.Hand;

            // --- AQUÍ ESTÁ LA MAGIA DEL REDONDEO ---
            // 1. Aplicamos el redondeo inicial (Radio 20 como en tu Figma)
            AplicarRedondeo(panelFondo, 20);
            AplicarRedondeo(btnVer, 10); // El botón también lo redondeamos un poco

            // 2. IMPORTANTE: Si el panel cambia de tamaño (Zoom), recalculamos las curvas
            panelFondo.SizeChanged += (s, e) => {
                AplicarRedondeo(panelFondo, 20);
            };

            // Conectamos los eventos de Mouse (Hover)
            AsignarEventosHover(panelFondo);
        }

        private void AsignarEventosHover(Control control)
        {
            control.MouseEnter += EfectoEntrada;
            control.MouseLeave += EfectoSalida;

            // IMPORTANTE: El botón mantiene su propio click, 
            // pero el resto de la tarjeta dispara el evento del botón también.
            if (!(control is Button))
            {
                control.Click += btnVer_Click;
            }

            foreach (Control hijo in control.Controls)
            {
                if (!(hijo is Button))
                {
                    AsignarEventosHover(hijo);
                }
            }
        }

        private void EfectoEntrada(object sender, EventArgs e)
        {
            this.Padding = new Padding(0); // Efecto Zoom
            panelFondo.BackColor = colorHover; // Se ilumina un poco
        }

        private void EfectoSalida(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                this.Padding = new Padding(4);
                panelFondo.BackColor = colorNormal; // Vuelve al Morado Figma
            }
        }

        public void CargarDatos(Juego juego)
        {
            JuegoDatos = juego;
            lblTitulo.Text = juego.Name;
            lblRating.Text = $"⭐ {juego.Rating}"; // Aquí podrías poner el precio si prefieres

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
                picPortada.BackColor = Color.FromArgb(50, 40, 90); // Un morado oscuro si falla la img
            }
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            if (JuegoDatos != null)
            {
                // Creamos una sombra oscura detrás para dar profundidad (Opcional pero pro)
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

                    // Abrimos el detalle
                    FormDetalles detalle = new FormDetalles(JuegoDatos);
                    detalle.StartPosition = FormStartPosition.CenterScreen;
                    detalle.TopMost = true; // Asegura que quede encima de la sombra
                    detalle.ShowDialog(); // Pausa el programa hasta cerrar el detalle

                    sombra.Close(); // Quitamos la sombra al volver
                }
            }
        }
        private void AplicarRedondeo(Control control, int radio)
        {
            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
            GraphicsPath path = new GraphicsPath();

            int d = radio * 2; // Diámetro de la curva

            // Dibujamos los 4 arcos de las esquinas
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90); // Arriba-Izquierda
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y, d, d, 270, 90); // Arriba-Derecha
            path.AddArc(bounds.X + bounds.Width - d, bounds.Y + bounds.Height - d, d, d, 0, 90); // Abajo-Derecha
            path.AddArc(bounds.X, bounds.Y + bounds.Height - d, d, d, 90, 90); // Abajo-Izquierda

            path.CloseFigure();

            // Aplicamos el recorte
            control.Region = new Region(path);
        }


    }
}