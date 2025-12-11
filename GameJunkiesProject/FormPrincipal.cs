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

        // --- NUEVA VARIABLE: LA RESERVA ---
        // Aquí guardaremos TODOS los juegos originales que trajo la API
        private List<Juego> listaOriginalJuegos = new List<Juego>();

        public FormPrincipal(Usuario usuario)
        {
            InitializeComponent();
            usuarioActual = usuario;

            // Configuración Visual
            this.Text = $"GameJunkies - {usuario.Nickname}";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(61, 47, 109);
            flowLayoutPanel1.BackColor = Color.FromArgb(61, 47, 109);

            // Configuramos el evento del Buscador
            // (Si no lo hiciste en el diseñador, lo hacemos aquí por código)
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            

            CargarEstante();
        }

        private async void CargarEstante()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ServicioJuegos servicio = new ServicioJuegos();

                // 1. Guardamos los datos en nuestra lista "Original"
                listaOriginalJuegos = await servicio.ObtenerJuegosPopulares();

                // 2. Mostramos todo al principio
                MostrarJuegosEnPantalla(listaOriginalJuegos);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // --- MÉTODO AUXILIAR PARA PINTAR LAS TARJETAS ---
        // Separamos esto para poder reusarlo cuando filtremos
        private void MostrarJuegosEnPantalla(List<Juego> juegosAVisualizar)
        {
            // Detenemos el redibujado para que no parpadee (Suspensión)
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            foreach (Juego j in juegosAVisualizar)
            {
                ControlJuego tarjeta = new ControlJuego();
                tarjeta.CargarDatos(j);
                tarjeta.Margin = new Padding(10);
                flowLayoutPanel1.Controls.Add(tarjeta);
            }

            // Reanudamos el dibujado
            flowLayoutPanel1.ResumeLayout();
        }

        // --- EVENTO DE BÚSQUEDA ---
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string textoBusqueda = txtBuscar.Text.ToLower().Trim();
            string placeholder = "busca tu nueva adicción..."; // El texto en minúsculas

            // Si está vacío O si es el texto del placeholder, mostramos todo
            if (string.IsNullOrEmpty(textoBusqueda) || textoBusqueda == placeholder)
            {
                MostrarJuegosEnPantalla(listaOriginalJuegos);
            }
            else
            {
                // Filtro normal
                List<Juego> listaFiltrada = listaOriginalJuegos
                    .Where(j => j.Name.ToLower().Contains(textoBusqueda))
                    .ToList();

                MostrarJuegosEnPantalla(listaFiltrada);
            }
        }
        private void ConfigurarBuscador()
        {
            // Estado inicial: Texto gris y mensaje
            txtBuscar.Text = "Busca tu nueva adicción...";
            txtBuscar.ForeColor = Color.Gray;

            // Cuando haces clic (Entrar): Se borra el mensaje
            txtBuscar.Enter += (s, e) => {
                if (txtBuscar.Text == "Busca tu nueva adicción...")
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.White; // Color normal al escribir
                }
            };

            // Cuando te vas (Salir): Si está vacío, vuelve el mensaje
            txtBuscar.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = "Busca tu nueva adicción...";
                    txtBuscar.ForeColor = Color.Gray;
                }
            };
        }

        private void FormPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


    }
}