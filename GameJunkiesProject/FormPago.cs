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
    public partial class FormPago : Form
    {
        private decimal montoTotal;

        public FormPago(decimal total)
        {
            InitializeComponent();
            montoTotal = total;

            // Configuramos estilos y eventos al iniciar
            ConfigurarLogicaVisual();

            // Redondeo opcional (si quieres que se vea moderno)
            AplicarRedondeo(this, 20);
            AplicarRedondeo(btnConfirmar, 15);
        }

        private void ConfigurarLogicaVisual()
        {
            // Colores y Estilo
            this.BackColor = Color.FromArgb(45, 35, 85);
            this.StartPosition = FormStartPosition.CenterParent;

            // Mostramos el total
            // Asegúrate de tener un Label llamado 'lblTotal' en tu diseño
            if (lblTotal != null)
            {
                lblTotal.Text = $"Total a pagar: ${montoTotal:F2}";
                lblTotal.ForeColor = Color.Gold;
            }

            // Configuramos inputs (Asumiendo que ya existen en el diseño)
            ConfigurarInput(txtNumeroTarjeta, 19); // 16 nums + 3 espacios
            ConfigurarInput(txtFecha, 5);          // MM/YY
            ConfigurarInput(txtCVV, 3);            // 123

            if (txtCVV != null) txtCVV.PasswordChar = '*';

            // --- CONECTAMOS LOS EVENTOS ---
            // Esto es vital: conectamos el clic con la lógica
            if (btnConfirmar != null)
            {
                btnConfirmar.Click -= btnConfirmar_Click; // Prevenir duplicados
                btnConfirmar.Click += btnConfirmar_Click;
            }

            if (btnCancelar != null)
            {
                btnCancelar.Click += (s, e) => this.Close();
            }

            // Evento mágico para separar números de tarjeta
            if (txtNumeroTarjeta != null)
            {
                txtNumeroTarjeta.TextChanged -= txtNumeroTarjeta_TextChanged;
                txtNumeroTarjeta.TextChanged += txtNumeroTarjeta_TextChanged;
            }
        }

        // Método auxiliar para no repetir código de configuración
        private void ConfigurarInput(TextBox txt, int maxLen)
        {
            if (txt != null)
            {
                txt.MaxLength = maxLen;
                txt.Font = new Font("Segoe UI", 11);
            }
        }

        // --- LÓGICA DEL BOTÓN CONFIRMAR ---
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // 1. Validar Tarjeta
            string tarjetaLimpia = txtNumeroTarjeta.Text.Replace(" ", "");
            if (tarjetaLimpia.Length != 16 || !tarjetaLimpia.All(char.IsDigit))
            {
                MostrarError("El número de tarjeta debe tener 16 dígitos.");
                return;
            }

            // 2. Validar CVV
            if (txtCVV.Text.Length != 3 || !txtCVV.Text.All(char.IsDigit))
            {
                MostrarError("El CVV debe tener 3 dígitos numéricos.");
                return;
            }

            // 3. Validar Fecha
            if (!ValidarFecha(txtFecha.Text))
            {
                MostrarError("La tarjeta está vencida o el formato es incorrecto (Use MM/YY).");
                return;
            }

            // 4. Validar Titular
            if (string.IsNullOrWhiteSpace(txtTitular.Text))
            {
                MostrarError("Por favor ingrese el nombre del titular.");
                return;
            }

            // ¡ÉXITO!
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // --- VALIDACIÓN DE FECHA ---
        private bool ValidarFecha(string fecha)
        {
            try
            {
                if (!fecha.Contains("/")) return false;
                string[] partes = fecha.Split('/');
                int mes = int.Parse(partes[0]);
                int anio = int.Parse(partes[1]) + 2000;

                if (mes < 1 || mes > 12) return false;

                DateTime fechaExp = new DateTime(anio, mes, 1).AddMonths(1).AddDays(-1);
                if (fechaExp < DateTime.Now) return false;

                return true;
            }
            catch { return false; }
        }

        // --- FORMATO AUTOMÁTICO DE TARJETA (4 en 4) ---
        private void txtNumeroTarjeta_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            // Quitamos el evento un momento para no causar loop infinito
            txt.TextChanged -= txtNumeroTarjeta_TextChanged;

            int cursor = txt.SelectionStart; // Guardamos posición del cursor
            string texto = txt.Text.Replace(" ", "");
            string nuevoTexto = "";

            for (int i = 0; i < texto.Length; i++)
            {
                if (i > 0 && i % 4 == 0) nuevoTexto += " ";
                nuevoTexto += texto[i];
            }

            txt.Text = nuevoTexto;

            // Intentamos restaurar el cursor al final (ajuste básico)
            txt.SelectionStart = nuevoTexto.Length;

            // Devolvemos el evento
            txt.TextChanged += txtNumeroTarjeta_TextChanged;
        }

        private void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error en Pago", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void AplicarRedondeo(Control control, int radio)
        {
            if (control == null) return;
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
    }
}