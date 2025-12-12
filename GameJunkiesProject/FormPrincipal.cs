using GameJunkiesBL;
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

namespace GameJunkiesProject
{
    public partial class FormPrincipal : Form
    {
        private Usuario usuarioActual;

        // Esta lista solo guarda lo que se ve ACTUALMENTE en pantalla
        private List<Juego> listaJuegosActual = new List<Juego>();

        // Variable para controlar en qué página estamos
        private int paginaActual = 1;

        public FormPrincipal(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;

            // --- CONFIGURACIÓN VISUAL ---
            this.Text = $"GameJunkies - {usuario.Nickname}";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(61, 47, 109);
            flowLayoutPanel1.BackColor = Color.FromArgb(61, 47, 109);

            // Configuramos botones y buscador
            ConfigurarBuscador();
            txtBuscar.KeyDown += txtBuscar_KeyDown;

            // Conectamos los botones de paginación
            // (Asegúrate de haber creado btnAnterior y btnSiguiente en el diseño)
            btnAnterior.Click += btnAnterior_Click;
            btnSiguiente.Click += btnSiguiente_Click;

            // --- NUEVO: CONECTAR BOTÓN DE CARRITO ---
            // Asegúrate de haber creado 'btnVerCarrito' en el diseño
            // Si no lo tienes aún en el diseño, comenta esta línea temporalmente.
            if (Controls.ContainsKey("btnVerCarrito") || btnVerCarrito != null)
            {
                btnVerCarrito.Click += btnVerCarrito_Click;
            }

            // --- INICIO ALEATORIO ---
            // Empezamos en una página al azar entre la 1 y la 10 para ver juegos distintos
            Random rnd = new Random();
            paginaActual = rnd.Next(1, 11);

            // Cargamos esa página
            CargarPagina();
        }

        // --- EVENTO DEL BOTÓN CARRITO ---
        private void btnVerCarrito_Click(object sender, EventArgs e)
        {
            // Creamos y mostramos la ventana del carrito
            FormCarrito carrito = new FormCarrito();
            carrito.ShowDialog(); // ShowDialog hace que no puedas tocar la ventana de atrás hasta cerrar esta
        }

        private async void CargarPagina()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Desactivamos botones mientras carga
                btnAnterior.Enabled = false;
                btnSiguiente.Enabled = false;

                ServicioJuegos servicio = new ServicioJuegos();

                // Pedimos a la API la página exacta
                listaJuegosActual = await servicio.ObtenerJuegosPopulares(paginaActual);

                // Mostramos los juegos
                MostrarJuegosEnPantalla(listaJuegosActual);

                // Actualizamos etiqueta de página
                if (Controls.ContainsKey("lblPagina"))
                {
                    lblPagina.Text = $"Página: {paginaActual}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;

                // Reactivamos botones
                btnSiguiente.Enabled = true;

                // Solo activamos "Anterior" si estamos en página mayor a 1
                if (paginaActual > 1)
                {
                    btnAnterior.Enabled = true;
                }
                else
                {
                    btnAnterior.Enabled = false;
                }
            }
        }

        // --- BOTONES DE NAVEGACIÓN ---
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (paginaActual > 1)
            {
                paginaActual--; // Restamos 1 a la página
                CargarPagina(); // Recargamos
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual++; // Sumamos 1 a la página
            CargarPagina(); // Recargamos
        }


        // --- MÉTODO VISUAL ---
        private void MostrarJuegosEnPantalla(List<Juego> juegosAVisualizar)
        {
            flowLayoutPanel1.SuspendLayout();

            // SIEMPRE LIMPIAMOS para que no se acumulen
            flowLayoutPanel1.Controls.Clear();

            foreach (Juego j in juegosAVisualizar)
            {
                ControlJuego tarjeta = new ControlJuego();
                tarjeta.CargarDatos(j);
                tarjeta.Margin = new Padding(10);
                flowLayoutPanel1.Controls.Add(tarjeta);
            }

            flowLayoutPanel1.ResumeLayout();

            // Hacemos scroll arriba al cambiar de página
            flowLayoutPanel1.AutoScrollPosition = new Point(0, 0);
        }

        // --- LÓGICA DE BÚSQUEDA ---
        private async void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string texto = txtBuscar.Text.Trim();
                string placeholder = "Busca tu nueva adicción...";

                // Si borra la búsqueda, volvemos a la página actual de populares
                if (string.IsNullOrEmpty(texto) || texto == placeholder)
                {
                    MostrarJuegosEnPantalla(listaJuegosActual);
                    PanelPaginacionVisible(true); // Mostramos botones
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                try
                {
                    ServicioJuegos servicio = new ServicioJuegos();
                    List<Juego> resultados = await servicio.BuscarJuegos(texto);

                    if (resultados.Count > 0)
                    {
                        MostrarJuegosEnPantalla(resultados);
                        PanelPaginacionVisible(false); // Ocultamos botones de paginación durante búsqueda
                    }
                    else
                    {
                        MessageBox.Show("No encontramos juegos con ese nombre :(");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar: " + ex.Message);
                }
                finally { this.Cursor = Cursors.Default; }
            }
        }

        // Método auxiliar para ocultar/mostrar botones
        private void PanelPaginacionVisible(bool visible)
        {
            btnAnterior.Visible = visible;
            btnSiguiente.Visible = visible;
            if (Controls.ContainsKey("lblPagina")) Controls["lblPagina"].Visible = visible;
        }

        private void ConfigurarBuscador()
        {
            txtBuscar.Text = "Busca tu nueva adicción...";
            txtBuscar.ForeColor = Color.Gray;

            txtBuscar.Enter += (s, e) => {
                if (txtBuscar.Text == "Busca tu nueva adicción...")
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.White;
                }
            };

            txtBuscar.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = "Busca tu nueva adicción...";
                    txtBuscar.ForeColor = Color.Gray;
                }
            };

            txtBuscar.TextChanged += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text) && listaJuegosActual.Count > 0)
                {
                    MostrarJuegosEnPantalla(listaJuegosActual);
                    PanelPaginacionVisible(true);
                }
            };
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}