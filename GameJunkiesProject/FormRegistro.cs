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
    public partial class FormRegistro : Form
    {
        public FormRegistro()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Recopilamos los datos del formulario
                Usuario nuevoUsuario = new Usuario();

                nuevoUsuario.NombreCompleto = txtNombre.Text.Trim(); // .Trim() quita espacios al inicio/final
                nuevoUsuario.Nickname = txtNick.Text.Trim();         // <--- AQUÍ GUARDAMOS EL NICK
                nuevoUsuario.Email = txtEmail.Text.Trim();
                nuevoUsuario.Password = txtPass.Text.Trim();

                // El Rol se asigna solo en la Base de Datos como 'Cliente'

                // 2. Llamamos al Gerente (BL) para que valide y guarde
                UsuarioBL negocio = new UsuarioBL();
                bool resultado = negocio.RegistrarUsuario(nuevoUsuario);

                // 3. Verificamos el resultado
                if (resultado)
                {
                    MessageBox.Show("¡Cuenta creada exitosamente!\nAhora puedes iniciar sesión.",
                                    "Bienvenido a GameJunkies",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    this.Close(); // Cerramos el registro para volver al Login
                }
                else
                {
                    MessageBox.Show("No se pudo registrar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Este error saltará si el correo ya existe (porque pusimos UNIQUE en SQL)
                // O si la BL detecta campos vacíos.
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close(); // Simplemente cierra la ventana
        }
    }
}
