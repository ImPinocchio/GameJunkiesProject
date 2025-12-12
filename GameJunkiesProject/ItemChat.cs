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
    public partial class ItemChat : UserControl
    {
        public ItemChat()
        {
            InitializeComponent();
        }

        public void ConfigurarMensaje(MensajeChat mensaje)
        {
            // 1. CONFIGURACIÓN DEL TEXTO
            // Le damos un ancho máximo al label para que no se salga de la burbuja.
            // Restamos 50px para dejar buen margen a los lados.
            int anchoDisponible = this.Width - 50;

            lblTexto.MaximumSize = new Size(anchoDisponible, 0);
            lblTexto.AutoSize = true;
            lblTexto.Text = mensaje.Texto;
            lblTexto.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // 2. POSICIONAMIENTO Y COLORES
            int margenSuperior = 15;
            int margenIzquierdo = 20;

            if (mensaje.EsUsuario)
            {
                this.BackColor = Color.FromArgb(45, 35, 85); // Azul oscuro (Usuario)
                lblTexto.ForeColor = Color.White;
                // Si es usuario, lo movemos un poquito a la derecha visualmente
                lblTexto.Location = new Point(margenIzquierdo + 5, margenSuperior);
            }
            else
            {
                this.BackColor = Color.FromArgb(61, 47, 109); // Morado (Bot)
                lblTexto.ForeColor = Color.Gainsboro;
                lblTexto.Location = new Point(margenIzquierdo, margenSuperior);
            }

            // 3. CONFIGURACIÓN DE IMAGEN
            int alturaImagen = 0;
            int espacioEntreTextoEImagen = 0;

            if (!string.IsNullOrEmpty(mensaje.ImagenUrl))
            {
                pbImagen.Visible = true;
                try
                {
                    pbImagen.Load(mensaje.ImagenUrl);
                    pbImagen.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Ubicamos la imagen 15px debajo del texto
                    espacioEntreTextoEImagen = 15;
                    pbImagen.Location = new Point(margenIzquierdo, lblTexto.Bottom + espacioEntreTextoEImagen);

                    // Ajustamos el tamaño de la imagen
                    pbImagen.Size = new Size(this.Width - 40, 150);
                    alturaImagen = 150;
                }
                catch
                {
                    pbImagen.Visible = false;
                }
            }
            else
            {
                pbImagen.Visible = false;
            }

            // 4. CÁLCULO FINAL DE ALTURA (CRÍTICO)
            // Altura = (Margen Arriba) + (Alto Texto) + (Espacio Img) + (Alto Img) + (MARGEN ABAJO GENEROSO)
            int margenInferior = 30; // Aquí aumentamos el espacio final para que no se corte

            this.Height = margenSuperior + lblTexto.Height + espacioEntreTextoEImagen + alturaImagen + margenInferior;
        }
    }
}

