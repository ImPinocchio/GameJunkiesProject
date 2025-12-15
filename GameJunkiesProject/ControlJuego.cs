using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D; // <--- AGREGA ESTO ARRIBA CON LOS OTROS USINGS
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics; // Para Process (Ejecutar juegos)
using System.IO;          // Para File (Buscar archivos)
using GameJunkiesDL;

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
            // Asignar datos a controles visuales y propiedades internas 
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
        private void btnVer_Click(object sender, EventArgs e)
        {
            if (JuegoDatos != null)
            {
                using (Form sombra = new Form())
                {
                    sombra.Opacity = 0.50d;
                    sombra.BackColor = Color.Black;
                    sombra.WindowState = FormWindowState.Maximized;
                    sombra.FormBorderStyle = FormBorderStyle.None;
                    sombra.Show();

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

        // --- EVENTO 2: JUGAR (BIBLIOTECA) - ¡AHORA ES REAL! ---
        private void btnJugar_Click(object sender, EventArgs e)
        {
            // 1. Verificamos ruta y archivo
            if (!string.IsNullOrEmpty(JuegoDatos.RutaEjecutable) && File.Exists(JuegoDatos.RutaEjecutable))
            {
                try
                {
                    // Detectamos si es un Acceso Directo (.lnk) o un Ejecutable (.exe)
                    string extension = Path.GetExtension(JuegoDatos.RutaEjecutable).ToLower();

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = JuegoDatos.RutaEjecutable;
                    startInfo.UseShellExecute = true; // NECESARIO para que Windows entienda el .lnk

                    // --- LÓGICA INTELIGENTE ---
                    if (extension == ".exe")
                    {
                        // Si es un EXE, nosotros le decimos dónde está su "cocina" (su carpeta)
                        startInfo.WorkingDirectory = Path.GetDirectoryName(JuegoDatos.RutaEjecutable);
                    }
                    // Si es .lnk (acceso directo), NO tocamos el WorkingDirectory. 
                    // El acceso directo ya sabe dónde ir. Si lo tocamos, lo rompemos.

                    // Minimizamos el Launcher para evitar conflictos gráficos
                    Form formPrincipal = this.FindForm();
                    if (formPrincipal != null) formPrincipal.WindowState = FormWindowState.Minimized;

                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al lanzar: " + ex.Message);
                    if (this.FindForm() != null) this.FindForm().WindowState = FormWindowState.Normal;
                }
            }
            // 2. Si no existe, buscamos
            else
            {
                DialogResult respuesta = MessageBox.Show(
                   $"No se encuentra el juego {JuegoDatos.Name}.\n¿Quieres buscar el archivo (EXE o Acceso Directo)?",
                   "Vincular Juego",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        // --- CAMBIO AQUÍ: Ahora permitimos .exe Y .lnk ---
                        ofd.Filter = "Aplicaciones (*.exe;*.lnk)|*.exe;*.lnk|Todos los archivos (*.*)|*.*";
                        ofd.Title = "Selecciona el juego o su acceso directo";

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            string ruta = ofd.FileName;
                            JuegoDatos.RutaEjecutable = ruta;

                            // Guardamos en BD
                            try { new BibliotecaDAL().ActualizarRutaJuego(Usuario.SesionActual.IdUsuario, JuegoDatos.Id, ruta); }
                            catch { }

                            // Ejecutamos (Copia de la misma lógica de arriba)
                            try
                            {
                                string ext = Path.GetExtension(ruta).ToLower();
                                ProcessStartInfo psi = new ProcessStartInfo();
                                psi.FileName = ruta;
                                psi.UseShellExecute = true;

                                if (ext == ".exe")
                                {
                                    psi.WorkingDirectory = Path.GetDirectoryName(ruta);
                                }

                                // Minimizamos
                                if (this.FindForm() != null) this.FindForm().WindowState = FormWindowState.Minimized;

                                Process.Start(psi);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al lanzar: " + ex.Message);
                                if (this.FindForm() != null) this.FindForm().WindowState = FormWindowState.Normal;
                            }
                        }
                    }
                }
            }
        }

        // --- MÉTODOS VISUALES (HOVER Y CLICKS EN TARJETA) ---
        private void AsignarEventosHover(Control control)
        {
            control.MouseEnter += EfectoEntrada;
            control.MouseLeave += EfectoSalida;

            if (!(control is Button))
            {
                control.Click -= btnJugar_Click;
                control.Click -= btnVer_Click;

                if (ModoBiblioteca)
                    control.Click += btnJugar_Click;
                else
                    control.Click += btnVer_Click;
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