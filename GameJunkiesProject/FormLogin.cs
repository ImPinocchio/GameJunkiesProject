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

        // Constructor que recibe Usuario (lo agregamos porque tu código lo pide para la redirección)
        public FormLogin(Usuario usuario)
        {
            InitializeComponent();
            // Aquí podrías usar 'usuario' si quisieras mostrar algo en la nueva ventana
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
                    // Saludo (esto ya lo tienes bien)
                    string nombre = !string.IsNullOrEmpty(usuarioLogueado.Nickname)
                                    ? usuarioLogueado.Nickname
                                    : usuarioLogueado.NombreCompleto;

                    MessageBox.Show($"¡Bienvenido {nombre}!", "GameJunkies", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // --- AQUÍ ESTÁ LA CORRECCIÓN CLAVE ---
                    this.Hide(); // Ocultamos el Login

                    // IMPORTANTE: Aquí debe decir 'FormPrincipal', NO 'FormLogin'
                    FormPrincipal tienda = new FormPrincipal(usuarioLogueado);

                    tienda.ShowDialog(); // Abrimos la tienda
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
