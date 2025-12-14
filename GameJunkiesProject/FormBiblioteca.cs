using GameJunkiesBL;
using GameJunkiesDL;
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
            CargarMisJuegos(); // <--- Aquí ocurre la magia
        }

        private void CargarMisJuegos()
        {
            // 1. Limpiamos el panel por si acaso
            flowPanelBiblioteca.Controls.Clear();

            // 2. Verificamos que haya alguien logueado
            if (Usuario.SesionActual == null) return;

            // 3. Vamos a la Base de Datos a buscar los juegos
            BibliotecaDAL dal = new BibliotecaDAL();
            List<Juego> misJuegos = dal.ObtenerJuegosUsuario(Usuario.SesionActual.IdUsuario);

            // 4. Si no tiene juegos, mostramos aviso
            if (misJuegos.Count == 0)
            {
                Label aviso = new Label();
                aviso.Text = "Aún no tienes juegos. ¡Ve a la tienda!";
                aviso.ForeColor = Color.Gray;
                aviso.AutoSize = true;
                aviso.Font = new Font("Segoe UI", 14);
                flowPanelBiblioteca.Controls.Add(aviso);
                return;
            }

            // 5. Dibujamos cada juego
            foreach (Juego juego in misJuegos)
            {
                ControlJuego tarjeta = new ControlJuego();

                // --- ESTA ES LA LÍNEA MÁGICA ---
                tarjeta.ModoBiblioteca = true; // <--- ¡Esto cambia el botón a JUGAR!

                tarjeta.CargarDatos(juego);
                flowPanelBiblioteca.Controls.Add(tarjeta);
            }
        }

        private void ConfigurarDiseño()
        {
            this.Text = "Mi Biblioteca";
            this.BackColor = Color.FromArgb(45, 35, 85);
            flowPanelBiblioteca.BackColor = Color.FromArgb(45, 35, 85);
            // Asegúrate de que tu FlowLayoutPanel se llame 'flowLayoutPanel1' en el diseño
            // Si se llama diferente, cambia el nombre aquí.
        }
    }
}
