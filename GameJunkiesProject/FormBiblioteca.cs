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
    public partial class FormBiblioteca : Form
    {
        public FormBiblioteca()
        {
            InitializeComponent();
            ConfigurarDiseño();
            CargarMisJuegos();
        }

        private void CargarMisJuegos()
        {
            flowPanelBiblioteca.Controls.Clear();

            // 1. Obtenemos los juegos desde el servicio de biblioteca
            List<Juego> misJuegos = ServicioBiblioteca.ObtenerMisJuegos();

            if (misJuegos.Count == 0)
            {
                // Si no hay juegos, mostramos un label o mensaje
                Label lblVacio = new Label();
                lblVacio.Text = "Aún no tienes juegos.\n¡Ve a la tienda y compra algo!";
                lblVacio.ForeColor = Color.Gray;
                lblVacio.Font = new Font("Segoe UI", 16);
                lblVacio.AutoSize = true;
                lblVacio.Padding = new Padding(50);
                flowPanelBiblioteca.Controls.Add(lblVacio);
            }
            else
            {
                foreach (Juego j in misJuegos)
                {
                    ControlJuego tarjeta = new ControlJuego();

                    // --- AQUÍ ACTIVAMOS EL MODO BIBLIOTECA ---
                    tarjeta.ModoBiblioteca = true;

                    tarjeta.CargarDatos(j);
                    tarjeta.Margin = new Padding(10);
                    flowPanelBiblioteca.Controls.Add(tarjeta);
                }
            }
        }

        private void ConfigurarDiseño()
        {
            this.Text = "Mi Biblioteca de Juegos";
            this.BackColor = Color.FromArgb(61, 47, 109);
            this.WindowState = FormWindowState.Maximized; // Pantalla completa como el principal

            flowPanelBiblioteca.BackColor = Color.FromArgb(61, 47, 109);
            flowPanelBiblioteca.AutoScroll = true;
        }
    }
}
