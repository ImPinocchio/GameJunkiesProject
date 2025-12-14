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
    public partial class FormLogin : Form
    {
        // Constructor vacío (necesario para el arranque)
        public FormLogin()
        {
            InitializeComponent();
        }

        // Constructor que recibe Usuario (lo mantenemos por compatibilidad)
        public FormLogin(Usuario usuario)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string pass = txtPass.Text;

                UsuarioBL negocio = new UsuarioBL();
                Usuario usuarioLogueado = negocio.ValidarAcceso(email, pass);

                if (usuarioLogueado != null)
                {
                    // --- PASO CRÍTICO: GUARDAR LA SESIÓN GLOBAL ---
                    // Esto permite que ControlJuego pueda acceder al usuario sin que se lo pasemos.
                    Usuario.SesionActual = usuarioLogueado;

                    // Saludo
                    string nombre = !string.IsNullOrEmpty(usuarioLogueado.Nickname)
                                    ? usuarioLogueado.Nickname
                                    : usuarioLogueado.NombreCompleto;

                    MessageBox.Show($"¡Bienvenido {nombre}!", "GameJunkies", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide(); // Ocultamos el Login

                    // Abrimos la tienda
                    FormPrincipal tienda = new FormPrincipal(usuarioLogueado);
                    tienda.ShowDialog();

                    this.Close(); // Al cerrar la tienda, se cierra el programa
                }
                else
                {
                    MessageBox.Show("Correo o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormRegistro registro = new FormRegistro();
            registro.ShowDialog();
        }
    }
}
