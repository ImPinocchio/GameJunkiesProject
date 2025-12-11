using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameJunkiesProject
{
    public partial class FormDetalles : Form
    {
        private Juego juegoSeleccionado;

        // Constructor que recibe el objeto Juego
        public FormDetalles(Juego juego)
        {
            InitializeComponent();
            juegoSeleccionado = juego;

            // --- ESTÉTICA ---
            ConfigurarDiseño();
            AplicarRedondeo(this, 25); // Redondeamos la ventana completa
            AplicarRedondeo(btnComprar, 15); // Botón redondo

            // --- CARGAR DATOS ---
            CargarInformacion();
        }

        private void ConfigurarDiseño()
        {
            // Colores Figma
            this.BackColor = Color.FromArgb(61, 47, 109); // Morado Fondo

            // Estilo Título
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);

            // Estilo Detalles
            lblDetalles.ForeColor = Color.Gainsboro;
            lblDetalles.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            // Estilo Botón Comprar
            btnComprar.BackColor = Color.FromArgb(253, 202, 90); // Amarillo
            btnComprar.ForeColor = Color.Black;
            btnComprar.FlatStyle = FlatStyle.Flat;
            btnComprar.FlatAppearance.BorderSize = 0;
            btnComprar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnComprar.Cursor = Cursors.Hand;

            // Estilo Botón Cerrar (Haremos que parezca un link o botón sutil)
            btnCerrar.Text = "X";
            btnCerrar.BackColor = Color.Transparent;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnCerrar.Cursor = Cursors.Hand;
        }

        private void CargarInformacion()
        {
            lblTitulo.Text = juegoSeleccionado.Name;

            // Armamos un texto descriptivo
            lblDetalles.Text = $"📅 Lanzamiento: {juegoSeleccionado.Released}\n\n" +
                               $"⭐ Calificación: {juegoSeleccionado.Rating}/5\n\n" +
                               $"🎮 Categoría: {ObtenerClasificacion()}\n\n" +
                               $"💰 Precio: $59.99"; // Precio simulado (la API no siempre lo trae)

            try
            {
                if (!string.IsNullOrEmpty(juegoSeleccionado.Background_Image))
                {
                    picPortada.Load(juegoSeleccionado.Background_Image);
                }
            }
            catch
            {
                picPortada.BackColor = Color.Gray;
            }
        }

        private string ObtenerClasificacion()
        {
            if (juegoSeleccionado.Esrb_Rating != null)
                return juegoSeleccionado.Esrb_Rating.Name;
            return "General";
        }

        

        // --- MÉTODO DE REDONDEO (REUTILIZADO) ---
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

        // Evento para mover la ventana sin bordes (Drag & Drop)
        // Puedes agregar esto si quieres poder arrastrar la ventana con el mouse
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            { // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < 50)
                { // Si el mouse está en los primeros 50px de arriba
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
            }
        }

        // --- BOTONES ---

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            this.Close(); // Cierra solo esta ventana modal
        }

        private void btnComprar_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show($"¡Gracias por comprar {juegoSeleccionado.Name}!\nSe ha añadido a tu biblioteca.",
                            "Compra Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
