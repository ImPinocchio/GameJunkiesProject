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
using GameJunkiesDL; // Elimine o comente esta línea si no existe el proyecto o ensamblado GameJunkiesDL

namespace GameJunkiesProject
{
    public partial class FormCarrito : Form
    {
        private Usuario comprador; // Variable para guardar quién compra

        // Modifica el constructor así:
        public FormCarrito(Usuario usuario)
        {
            InitializeComponent();
            comprador = usuario; // Guardamos al usuario que nos pasan

            ConfigurarDiseño();
            AplicarRedondeo(this, 20);
            AplicarRedondeo(btnPagar, 15);
            CargarItems();
        }

        private void CargarItems()
        {
            // Limpiamos la lista visual para evitar duplicados al recargar
            lstProductos.Items.Clear();

            // Pedimos la lista real al Servicio (Capa BL)
            var lista = ServicioCarrito.ObtenerCarrito();

            // Si no hay nada, mostramos mensaje y bloqueamos botones
            if (lista.Count == 0)
            {
                lstProductos.Items.Add("Tu carrito está vacío... ¡Ve a llenar tu biblioteca!");
                btnPagar.Enabled = false;
                btnVaciar.Enabled = false;
                lblTotal.Text = "TOTAL: $0.00";
            }
            else
            {
                // Recorremos la lista y la mostramos en texto
                foreach (ItemCarrito item in lista)
                {
                    // Formato visual: "Undertale (x1) - $59.99"
                    // Multiplicamos precio por cantidad por si llevaras 2 copias del mismo
                    string linea = $"{item.JuegoSeleccionado.Name} (x{item.Cantidad}) - ${item.PrecioFinal * item.Cantidad}";
                    lstProductos.Items.Add(linea);
                }

                btnPagar.Enabled = true;
                btnVaciar.Enabled = true;

                // Calculamos el total usando el método del servicio
                decimal total = ServicioCarrito.CalcularTotal();
                lblTotal.Text = $"TOTAL: ${total:F2}"; // F2 asegura 2 decimales (ej: 10.50)
            }
        }

        // --- BOTONES DE ACCIÓN ---

        private void btnPagar_Click(object sender, EventArgs e)
        {
            decimal totalAPagar = ServicioCarrito.CalcularTotal();

            // 1. Pedimos datos de pago (Tarjeta simulada)
            FormPago formularioPago = new FormPago(totalAPagar);
            DialogResult resultado = formularioPago.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                try
                {
                    // 2. GUARDAMOS EN BASE DE DATOS (MySQL)
                    var itemsCarrito = ServicioCarrito.ObtenerCarrito();

                    // Instanciamos la DAL que acabamos de crear
                    TransaccionDAL transaccion = new TransaccionDAL();

                    // Registramos la compra real
                    transaccion.RegistrarCompra(comprador, itemsCarrito, totalAPagar, "Tarjeta de Crédito");

                    // 3. Limpieza y Éxito
                    ServicioCarrito.VaciarCarrito();

                    MessageBox.Show("¡Compra realizada con éxito! 🎉\n\nTus juegos ya están disponibles en tu Biblioteca listos para descargar.",
                                    "¡A Jugar!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en la base de datos: " + ex.Message);
                }
            }
        }

        private void btnVaciar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Seguro que quieres eliminar todos los juegos del carrito?", "Vaciar Carrito", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                ServicioCarrito.VaciarCarrito();
                CargarItems(); // Recargamos la vista para mostrar que está vacío
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- DISEÑO VISUAL (Colores de Figma) ---
        private void ConfigurarDiseño()
        {
            // Configuración de la Ventana
            this.BackColor = Color.FromArgb(61, 47, 109); // Fondo Morado Oscuro
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; // Sin bordes de Windows
            this.Size = new Size(400, 500); // Tamaño fijo

            // Configuración del ListBox (Lista de juegos)
            lstProductos.BackColor = Color.FromArgb(90, 74, 147); // Morado un poco más claro
            lstProductos.ForeColor = Color.White;
            lstProductos.Font = new Font("Segoe UI", 11);
            lstProductos.BorderStyle = BorderStyle.None;

            // Posicionamos manualmente por si no lo hiciste en el diseñador
            lstProductos.Location = new Point(20, 60);
            lstProductos.Size = new Size(360, 300);

            // Configuración del Label Total
            lblTotal.ForeColor = Color.Gold; // Dorado para el dinero
            lblTotal.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTotal.Location = new Point(20, 380);
            lblTotal.AutoSize = true;

            // Botón Pagar
            btnPagar.Text = "PAGAR AHORA";
            btnPagar.BackColor = Color.FromArgb(253, 202, 90); // Amarillo Botón
            btnPagar.ForeColor = Color.Black;
            btnPagar.FlatStyle = FlatStyle.Flat;
            btnPagar.FlatAppearance.BorderSize = 0;
            btnPagar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnPagar.Size = new Size(140, 40);
            btnPagar.Location = new Point(240, 430);
            btnPagar.Cursor = Cursors.Hand;
            // Vinculamos evento si no existe
            btnPagar.Click -= btnPagar_Click;
            btnPagar.Click += btnPagar_Click;

            // Botón Vaciar
            btnVaciar.Text = "Vaciar";
            btnVaciar.BackColor = Color.IndianRed; // Rojo suave para acciones destructivas
            btnVaciar.ForeColor = Color.White;
            btnVaciar.FlatStyle = FlatStyle.Flat;
            btnVaciar.FlatAppearance.BorderSize = 0;
            btnVaciar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnVaciar.Size = new Size(80, 30);
            btnVaciar.Location = new Point(20, 440);
            btnVaciar.Cursor = Cursors.Hand;
            btnVaciar.Click -= btnVaciar_Click;
            btnVaciar.Click += btnVaciar_Click;

            // Botón Cerrar (X)
            btnCerrar.Text = "X";
            btnCerrar.BackColor = Color.Transparent;
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnCerrar.Location = new Point(360, 10);
            btnCerrar.Size = new Size(30, 30);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.Click -= btnCerrar_Click;
            btnCerrar.Click += btnCerrar_Click;

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "Tu Carrito 🛒";
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;
            this.Controls.Add(lblTitulo);
        }

        // Método para redondear bordes (Igual que en tus otros forms)
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

        // Permite mover la ventana arrastrándola (Drag & Drop)
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x84) // Trap WM_NCHITTEST
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < 50) // Zona superior arrastrable
                {
                    m.Result = (IntPtr)2; // HTCAPTION 
                    return;
                }
            }
        }
    }
}
