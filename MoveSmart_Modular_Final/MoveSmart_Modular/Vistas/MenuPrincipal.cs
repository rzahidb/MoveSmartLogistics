using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArbolEmpresaMudanzas.Vistas
{
    public class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            ConfigurarDiseño();
        }

        private void ConfigurarDiseño()
        {
            // 1. Configuración de la Ventana
            this.Text = "MoveSmart Logistics - Panel Principal";
            this.Size = new Size(600, 700); // AUMENTÉ LA ALTURA para que quepan 5 botones
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 2. Encabezado
            Panel panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 100;
            panelHeader.BackColor = Color.DarkSlateBlue;
            this.Controls.Add(panelHeader);

            Label lblTitulo = new Label();
            lblTitulo.Text = "SISTEMA MOVESMART\nLOGISTICS";
            lblTitulo.Dock = DockStyle.Fill;
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            panelHeader.Controls.Add(lblTitulo);

            Label lblSubtitulo = new Label();
            lblSubtitulo.Text = "Seleccione un módulo de gestión:";
            lblSubtitulo.Location = new Point(0, 115);
            lblSubtitulo.Width = 600;
            lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitulo.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblSubtitulo.ForeColor = Color.DimGray;
            this.Controls.Add(lblSubtitulo);

            // --- BOTONES ---
            int y = 160;
            int espacio = 85; // Un poco más pegados para que quepan todos

            // 1. GESTIÓN DE PERSONAL
            Button btnPersonal = CrearBoton("👥 GESTIÓN DE PERSONAL", y, Color.Teal);
            btnPersonal.Click += (s, e) =>
            {
                this.Hide();
                new VistaArbol().ShowDialog();
                this.Show();
            };
            this.Controls.Add(btnPersonal);

            // 2. OPTIMIZACIÓN DE RUTAS
            Button btnRutas = CrearBoton("🚚 OPTIMIZACIÓN DE RUTAS", y + espacio, Color.OrangeRed);
            btnRutas.Click += (s, e) =>
            {
                this.Hide();
                new MapaUI().ShowDialog();
                this.Show();
            };
            this.Controls.Add(btnRutas);

            // 3. REPORTES
            Button btnReportes = CrearBoton("📊 CENTRO DE REPORTES\n(Estadísticas y Listados)", y + (espacio * 2), Color.Purple);
            btnReportes.Click += (s, e) =>
            {
                this.Hide();
                new ReportesUI().ShowDialog();
                this.Show();
            };
            this.Controls.Add(btnReportes);

            // 4. ASISTENTE IA (¡ESTE ES EL NUEVO!)
            Button btnIA = CrearBoton("🤖 ASISTENTE IA\n(Chatbot Inteligente)", y + (espacio * 3), Color.FromArgb(0, 102, 204)); // Azul Google
            btnIA.Click += (s, e) =>
            {
                this.Hide();
                new ChatbotUI().ShowDialog(); // Abre la ventana del chat
                this.Show();
            };
            this.Controls.Add(btnIA);

            // 5. SALIR
            Button btnSalir = CrearBoton("CERRAR SESIÓN", y + (espacio * 4), Color.Gray);
            btnSalir.Height = 50;
            btnSalir.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnSalir);
        }

        private Button CrearBoton(string texto, int y, Color color)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(100, y),
                Size = new Size(400, 70),
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }
    }
}