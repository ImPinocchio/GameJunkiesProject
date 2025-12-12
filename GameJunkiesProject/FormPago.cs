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

            // Configuramos la lógica visual al iniciar
            ConfigurarLogica();

            // Redondeo
            AplicarRedondeo(this, 20);
        }

        private void ConfigurarLogica()
        {
            this.BackColor = Color.FromArgb(45, 35, 85);
            this.StartPosition = FormStartPosition.CenterParent;

            // Asignamos el total al Label (asegúrate que se llame lblTotal en el diseño)
            if (lblTotal != null)
            {
                lblTotal.Text = $"Total a pagar: ${montoTotal:F2}";
                lblTotal.ForeColor = Color.Gold;
            }

            // --- VINCULACIÓN DE EVENTOS ---
            // Le decimos a los botones del diseñador qué hacer

            if (btnConfirmar != null)
            {
                btnConfirmar.Click -= btnConfirmar_Click; // Evitar duplicados
                btnConfirmar.Click += btnConfirmar_Click;
            }

            if (btnCancelar != null)
            {
                btnCancelar.Click += (s, e) => this.Close();
            }

            if (txtNumeroTarjeta != null)
            {
                txtNumeroTarjeta.TextChanged -= txtNumeroTarjeta_TextChanged;
                txtNumeroTarjeta.TextChanged += txtNumeroTarjeta_TextChanged;
                txtNumeroTarjeta.MaxLength = 19; // 16 nums + 3 espacios
            }

            if (txtCVV != null) txtCVV.PasswordChar = '*';
            if (txtFecha != null) txtFecha.MaxLength = 5;
        }

        // --- BOTÓN CONFIRMAR ---
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // 1. Validar Tarjeta
            string tarjetaLimpia = txtNumeroTarjeta.Text.Replace(" ", "");
            if (tarjetaLimpia.Length != 16 || !tarjetaLimpia.All(char.IsDigit))
            {
                MessageBox.Show("El número de tarjeta debe tener 16 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validar CVV
            if (txtCVV.Text.Length != 3 || !txtCVV.Text.All(char.IsDigit))
            {
                MessageBox.Show("El CVV debe tener 3 dígitos numéricos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Validar Fecha
            if (!ValidarFecha(txtFecha.Text))
            {
                MessageBox.Show("Fecha incorrecta o tarjeta vencida (Use MM/YY).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Validar Titular
            if (string.IsNullOrWhiteSpace(txtTitular.Text))
            {
                MessageBox.Show("Ingrese el nombre del titular.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Todo correcto
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // --- VALIDAR FECHA ---
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

        // --- FORMATO AUTOMÁTICO TARJETA ---
        private void txtNumeroTarjeta_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.TextChanged -= txtNumeroTarjeta_TextChanged; // Pausa evento

            int cursor = txt.SelectionStart;
            string texto = txt.Text.Replace(" ", "");
            string nuevoTexto = "";

            for (int i = 0; i < texto.Length; i++)
            {
                if (i > 0 && i % 4 == 0) nuevoTexto += " ";
                nuevoTexto += texto[i];
            }

            txt.Text = nuevoTexto;
            try
            {
                txt.SelectionStart = nuevoTexto.Length; // Cursor al final
            }
            catch { }

            txt.TextChanged += txtNumeroTarjeta_TextChanged; // Reanuda evento
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