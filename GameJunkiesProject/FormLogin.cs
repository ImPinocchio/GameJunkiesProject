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
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Recogemos lo que escribió el usuario
                string email = txtEmail.Text;
                string pass = txtPass.Text;

                // 2. Llamamos al "Gerente" (Capa de Negocio)
                UsuarioBL negocio = new UsuarioBL();
                Usuario usuarioLogueado = negocio.ValidarAcceso(email, pass);

                // 3. Verificamos la respuesta
                if (usuarioLogueado != null)
                {
                    // ¡ÉXITO!
                    MessageBox.Show($"¡Bienvenido {usuarioLogueado.NombreCompleto}!",
                                    "GameJunkies", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Aquí es donde más adelante abriremos la Tienda Principal.
                    // Por ahora, cerramos con éxito.
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // FALLO
                    MessageBox.Show("Correo o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // ERROR TÉCNICO (Ej: No hay internet, BD apagada)
                MessageBox.Show(ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnIrRegistro_Click(object sender, EventArgs e)
        {
            // Abrimos el formulario de registro como un diálogo (bloquea el login hasta que cierres registro)
            FormRegistro registro = new FormRegistro();
            registro.ShowDialog();
        }
    }
}