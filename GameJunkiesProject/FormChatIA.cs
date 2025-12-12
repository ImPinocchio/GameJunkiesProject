using GameJunkiesBL;
using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameJunkiesProject
{
    public partial class FormChatIA : Form
    {
        private ServicioIA _servicioIA;
        private ServicioJuegos _servicioJuegos;

        public FormChatIA()
        {
            InitializeComponent();
            _servicioIA = new ServicioIA();
            _servicioJuegos = new ServicioJuegos();

            // 1. Configuramos el diseño para que se adapte a TU tamaño
            ConfigurarDiseñoAdaptable();

            // 2. Redondeo
            AplicarRedondeo(this, 25);

            // 3. Mensaje de bienvenida
            AgregarBurbuja("¡Hola! Soy JunkieBot 🤖. ¿No sabes qué jugar? Pregúntame.", false);
        }

        private void ConfigurarDiseñoAdaptable()
        {
            // --- CONFIGURACIÓN BASE ---
            this.BackColor = Color.FromArgb(61, 47, 109);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            // NOTA: YA NO forzamos el tamaño. Usará el que tú definiste en el Diseñador.

            // --- 1. BOTÓN CERRAR (Esquina Superior Derecha) ---
            if (btnCerrar != null)
            {
                btnCerrar.Text = "X";
                // Lo pegamos a la derecha basándonos en el ancho actual
                btnCerrar.Location = new Point(this.ClientSize.Width - 40, 15);
                btnCerrar.Size = new Size(30, 30);
                // ANCLAJE: Si la ventana crece, el botón se mueve con ella
                btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

                btnCerrar.BackColor = Color.Transparent;
                btnCerrar.ForeColor = Color.White;
                btnCerrar.FlatStyle = FlatStyle.Flat;
                btnCerrar.FlatAppearance.BorderSize = 0;

                btnCerrar.Click -= BtnCerrar_Click;
                btnCerrar.Click += BtnCerrar_Click;
            }

            // --- 2. PANEL DE CHAT (Centro - Se estira) ---
            if (panelChat != null)
            {
                panelChat.Location = new Point(20, 60);

                // CAMBIO: Restamos 140 en lugar de 120 para dejar más espacio antes del TextBox
                int altoPanel = this.ClientSize.Height - 140;

                int anchoPanel = this.ClientSize.Width - 40;

                panelChat.Size = new Size(anchoPanel, altoPanel);
                // ... resto del código igual ...
            }

            // --- 3. CAJA DE TEXTO (Abajo Izquierda - Se estira) ---
            if (txtMensaje != null)
            {
                // La ponemos pegada al fondo
                txtMensaje.Location = new Point(20, this.ClientSize.Height - 45);
                // Calculamos el ancho (Ancho total - espacio del botón enviar - márgenes)
                txtMensaje.Size = new Size(this.ClientSize.Width - 130, 30);

                // ANCLAJE: Se mantiene abajo y se estira a los lados
                txtMensaje.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                txtMensaje.Font = new Font("Segoe UI", 10);
            }

            // --- 4. BOTÓN ENVIAR (Abajo Derecha - Fijo) ---
            if (btnEnviar != null)
            {
                btnEnviar.Text = "Enviar";
                // Lo ponemos a la derecha del textbox
                btnEnviar.Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 48);
                btnEnviar.Size = new Size(80, 35);

                // ANCLAJE: Se mantiene abajo a la derecha
                btnEnviar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                btnEnviar.BackColor = Color.FromArgb(253, 202, 90);
                btnEnviar.ForeColor = Color.Black;
                btnEnviar.FlatStyle = FlatStyle.Flat;
                btnEnviar.FlatAppearance.BorderSize = 0;
                btnEnviar.Cursor = Cursors.Hand;

                btnEnviar.Click -= btnEnviar_Click;
                btnEnviar.Click += btnEnviar_Click;
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            string pregunta = txtMensaje.Text.Trim();
            if (string.IsNullOrEmpty(pregunta)) return;

            // 1. Mostrar mi mensaje
            AgregarBurbuja(pregunta, true);
            txtMensaje.Text = "";
            txtMensaje.Enabled = false;

            Label lblEscribiendo = new Label() { Text = "JunkieBot está pensando...", ForeColor = Color.Gray, AutoSize = true, Padding = new Padding(10) };
            panelChat.Controls.Add(lblEscribiendo);
            panelChat.ScrollControlIntoView(lblEscribiendo);

            // 2. Llamar a Gemini
            string respuestaRaw = await _servicioIA.EnviarMensaje(pregunta);

            panelChat.Controls.Remove(lblEscribiendo);

            // 3. Procesar respuesta
            string respuestaFinal = respuestaRaw;
            string imagenUrl = "";

            if (respuestaRaw.Contains("[JUEGO:"))
            {
                try
                {
                    int inicio = respuestaRaw.IndexOf("[JUEGO:") + 7;
                    int fin = respuestaRaw.IndexOf("]", inicio);
                    string nombreJuego = respuestaRaw.Substring(inicio, fin - inicio).Trim();

                    respuestaFinal = respuestaRaw.Replace($"[JUEGO: {nombreJuego}]", "");

                    var resultados = await _servicioJuegos.BuscarJuegos(nombreJuego);
                    if (resultados.Count > 0)
                    {
                        imagenUrl = resultados[0].Background_Image;
                    }
                }
                catch { }
            }

            AgregarBurbuja(respuestaFinal, false, imagenUrl);

            txtMensaje.Enabled = true;
            txtMensaje.Focus();
        }

        private void AgregarBurbuja(string texto, bool esUsuario, string urlImagen = null)
        {
            MensajeChat msj = new MensajeChat
            {
                Texto = texto,
                EsUsuario = esUsuario,
                ImagenUrl = urlImagen
            };

            ItemChat burbuja = new ItemChat();
            // Ajuste dinámico: Si el panel cambia de tamaño, la burbuja se adapta
            burbuja.Width = panelChat.ClientSize.Width - 25;
            burbuja.ConfigurarMensaje(msj);

            panelChat.Controls.Add(burbuja);
            panelChat.ScrollControlIntoView(burbuja);
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < 50) { m.Result = (IntPtr)2; return; }
            }
        }
    }
}