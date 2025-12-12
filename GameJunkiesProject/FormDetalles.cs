using GameJunkiesBL;
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
        private string precioCalculado; // Variable para guardar el precio

        public FormDetalles(Juego juego)
        {
            InitializeComponent();
            juegoSeleccionado = juego;

            // 1. Calculamos el precio una sola vez al inicio
            precioCalculado = GenerarPrecioSimulado();

            // --- ESTÉTICA ---
            ConfigurarDiseño();
            AplicarRedondeo(this, 25);
            AplicarRedondeo(btnComprar, 15);

            // --- CARGAR DATOS ---
            CargarInformacion();

            // --- CARGAR SINOPSIS ---
            CargarDescripcionExtra();
        }

        // --- NUEVO MÉTODO: PRECIO BASADO EN ID ---
        private string GenerarPrecioSimulado()
        {
            // Usamos el ID del juego como 'semilla' (Seed).
            Random rnd = new Random(juegoSeleccionado.Id);

            // Generamos un número entre 19 y 69
            int basePrecio = rnd.Next(19, 70);

            return $"${basePrecio}.99";
        }

        private void ConfigurarDiseño()
        {
            this.BackColor = Color.FromArgb(61, 47, 109);

            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);

            lblDetalles.ForeColor = Color.Gainsboro;
            lblDetalles.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            btnComprar.BackColor = Color.FromArgb(253, 202, 90);
            btnComprar.ForeColor = Color.Black;
            btnComprar.FlatStyle = FlatStyle.Flat;
            btnComprar.FlatAppearance.BorderSize = 0;
            btnComprar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnComprar.Cursor = Cursors.Hand;

            // Actualizamos el texto del botón con el precio también
            btnComprar.Text = $"Comprar {precioCalculado}";

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

            lblDetalles.Text = $"📅 Lanzamiento: {juegoSeleccionado.Released}\n" +
                               $"⭐ Calificación: {juegoSeleccionado.Rating}/5\n" +
                               $"🎮 Categoría: {ObtenerClasificacion()}\n" +
                               $"💰 Precio: {precioCalculado}";

            try
            {
                if (!string.IsNullOrEmpty(juegoSeleccionado.Background_Image))
                {
                    picPortada.Load(juegoSeleccionado.Background_Image);
                    picPortada.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch
            {
                picPortada.BackColor = Color.Gray;
            }
        }

        private async void CargarDescripcionExtra()
        {
            try
            {
                lblDetalles.Text += "\n\n⏳ Cargando sinopsis...";

                ServicioJuegos servicio = new ServicioJuegos();
                Juego juegoCompleto = await servicio.ObtenerDetalleJuego(juegoSeleccionado.Id);

                if (juegoCompleto != null && !string.IsNullOrEmpty(juegoCompleto.Description_Raw))
                {
                    lblDetalles.Text = $"📅 Lanzamiento: {juegoSeleccionado.Released}\n" +
                                       $"⭐ Calificación: {juegoSeleccionado.Rating}/5\n" +
                                       $"🎮 Categoría: {ObtenerClasificacion()}\n" +
                                       $"💰 Precio: {precioCalculado}\n\n" +
                                       $"📝 SINOPSIS:\n{juegoCompleto.Description_Raw}";
                }
                else
                {
                    lblDetalles.Text = lblDetalles.Text.Replace("\n\n⏳ Cargando sinopsis...", "\n\n(Sinopsis no disponible)");
                }
            }
            catch
            {
                lblDetalles.Text = lblDetalles.Text.Replace("\n\n⏳ Cargando sinopsis...", "");
            }
        }

        private string ObtenerClasificacion()
        {
            if (juegoSeleccionado.Esrb_Rating != null)
                return juegoSeleccionado.Esrb_Rating.Name;
            return "General";
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < 50)
                {
                    m.Result = (IntPtr)2;
                    return;
                }
            }
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnComprar_Click_1(object sender, EventArgs e)
        {
            string precioLimpio = precioCalculado.Replace("$", "").Trim();
            decimal precioNumerico = 0;
            if (!decimal.TryParse(precioLimpio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out precioNumerico))
            {
                precioNumerico = 59.99m;
            }

            GameJunkiesBL.ServicioCarrito.AgregarJuego(juegoSeleccionado, precioNumerico);

            DialogResult respuesta = MessageBox.Show(
                $"Se agregó '{juegoSeleccionado.Name}' al carrito.\n\n¿Quieres ir a Pagar ahora?",
                "Agregado al Carrito",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                // CAMBIO CLAVE:
                // No abrimos el carrito aquí. Solo avisamos que el usuario dijo "SI"
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.No; // Usuario dijo "NO"
                this.Close();
            }
        }
    }
}
