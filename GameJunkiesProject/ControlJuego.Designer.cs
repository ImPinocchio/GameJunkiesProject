namespace GameJunkiesProject
{
    partial class ControlJuego
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelFondo = new System.Windows.Forms.Panel();
            this.btnVer = new System.Windows.Forms.Button();
            this.lblRating = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.picPortada = new System.Windows.Forms.PictureBox();
            this.panelFondo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPortada)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFondo
            // 
            this.panelFondo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelFondo.Controls.Add(this.btnVer);
            this.panelFondo.Controls.Add(this.lblRating);
            this.panelFondo.Controls.Add(this.lblTitulo);
            this.panelFondo.Controls.Add(this.picPortada);
            this.panelFondo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFondo.Location = new System.Drawing.Point(0, 0);
            this.panelFondo.Name = "panelFondo";
            this.panelFondo.Size = new System.Drawing.Size(200, 300);
            this.panelFondo.TabIndex = 0;
            // 
            // btnVer
            // 
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Location = new System.Drawing.Point(66, 247);
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(75, 31);
            this.btnVer.TabIndex = 7;
            this.btnVer.Text = "COMPRAR";
            this.btnVer.UseVisualStyleBackColor = true;
            // 
            // lblRating
            // 
            this.lblRating.AutoEllipsis = true;
            this.lblRating.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRating.Location = new System.Drawing.Point(56, 209);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(86, 34);
            this.lblRating.TabIndex = 6;
            this.lblRating.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoEllipsis = true;
            this.lblTitulo.Font = new System.Drawing.Font("Sans Serif Collection", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(3, 184);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(194, 36);
            this.lblTitulo.TabIndex = 5;
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picPortada
            // 
            this.picPortada.Location = new System.Drawing.Point(3, 12);
            this.picPortada.Name = "picPortada";
            this.picPortada.Size = new System.Drawing.Size(194, 169);
            this.picPortada.TabIndex = 4;
            this.picPortada.TabStop = false;
            // 
            // ControlJuego
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelFondo);
            this.Name = "ControlJuego";
            this.Size = new System.Drawing.Size(200, 300);
            this.panelFondo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPortada)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFondo;
        private System.Windows.Forms.Button btnVer;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox picPortada;
    }
}
