using ArbolEmpresaMudanzas.Estructuras;
using ArbolEmpresaMudanzas.Modulo;
using MoveSmart_Modular.Estructuras;
using MoveSmart_Modular.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArbolEmpresaMudanzas.Vistas
{
    public class MapaUI : Form
    {
        // Controles
        private PictureBox pbMapa;
        private Panel panelSide;
        private ComboBox cboDepOrigen, cboMunOrigen, cboDepDestino, cboMunDestino;
        private RichTextBox rtbResultado;
        private List<Ciudad> rutaActual = null;

        // ANCHO FIJO DEL PANEL LATERAL
        private const int ANCHO_PANEL = 340;

        // VALORES BASE PARA ESCALADO
        private const float ANCHO_REF = 1200f;
        private const float ALTO_REF = 800f;

        public MapaUI()
        {
            this.Text = "MoveSmart - Logística Nacional";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            // 1. BARRA LATERAL (FIJA A LA IZQUIERDA)
            panelSide = new Panel();
            panelSide.Dock = DockStyle.Left;
            panelSide.Width = ANCHO_PANEL;
            panelSide.BackColor = Color.FromArgb(30, 30, 40);
            panelSide.Padding = new Padding(15);
            this.Controls.Add(panelSide);

            // --- CONTROLES DEL PANEL ---
            Label lblTitle = new Label { Text = "PLANIFICADOR\nDE RUTAS", ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(15, 20) };
            panelSide.Controls.Add(lblTitle);

            Button btnVolver = new Button { Text = "⬅ VOLVER AL MENÚ", ForeColor = Color.White, BackColor = Color.FromArgb(192, 57, 43), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Size = new Size(150, 35), Location = new Point(15, 90), Cursor = Cursors.Hand };
            btnVolver.Click += (s, e) => this.Close();
            panelSide.Controls.Add(btnVolver);

            int y = 150;

            // ORIGEN
            panelSide.Controls.Add(new Label { Text = "🟢 PUNTO DE PARTIDA", ForeColor = Color.LightGreen, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(15, y), AutoSize = true });
            y += 25;
            cboDepOrigen = CrearCombo(panelSide, y);
            cboDepOrigen.SelectedIndexChanged += (s, e) => CargarMunicipios(cboDepOrigen, cboMunOrigen);
            y += 35;
            cboMunOrigen = CrearCombo(panelSide, y);
            cboMunOrigen.SelectedIndexChanged += (s, e) => pbMapa.Invalidate();
            y += 50;

            // DESTINO
            panelSide.Controls.Add(new Label { Text = "🔴 PUNTO DE LLEGADA", ForeColor = Color.Salmon, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(15, y), AutoSize = true });
            y += 25;
            cboDepDestino = CrearCombo(panelSide, y);
            cboDepDestino.SelectedIndexChanged += (s, e) => CargarMunicipios(cboDepDestino, cboMunDestino);
            y += 35;
            cboMunDestino = CrearCombo(panelSide, y);
            cboMunDestino.SelectedIndexChanged += (s, e) => pbMapa.Invalidate();
            y += 60;

            // BOTÓN
            Button btnCalc = new Button { Text = "CALCULAR RUTA ÓPTIMA", ForeColor = Color.Black, BackColor = Color.Orange, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 11, FontStyle.Bold), Size = new Size(300, 45), Location = new Point(15, y), Cursor = Cursors.Hand };
            btnCalc.Click += BtnCalcular_Click;
            panelSide.Controls.Add(btnCalc);
            y += 60;

            // RESULTADOS (AQUÍ ESTÁ LA MEJORA VISUAL)
            Label lblResTitulo = new Label { Text = "ITINERARIO DETALLADO:", ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(15, y), AutoSize = true };
            panelSide.Controls.Add(lblResTitulo);
            y += 25;

            rtbResultado = new RichTextBox();
            rtbResultado.Location = new Point(15, y);
            rtbResultado.Width = 300;

            // CAMBIO CLAVE 1: Calculamos la altura para que llegue hasta el fondo de la pantalla
            // Restamos 20px para dejar un margen abajo
            rtbResultado.Height = panelSide.Height - y - 20;

            // CAMBIO CLAVE 2: Anclaje. Si estiras la ventana, el cuadro crece hacia abajo.
            rtbResultado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            // CAMBIO CLAVE 3: Barras de desplazamiento verticales activas
            rtbResultado.ScrollBars = RichTextBoxScrollBars.Vertical;

            rtbResultado.BackColor = Color.FromArgb(45, 45, 55);
            rtbResultado.ForeColor = Color.White;
            rtbResultado.Font = new Font("Consolas", 10); // Fuente monoespaciada para que se vea ordenado
            rtbResultado.BorderStyle = BorderStyle.None;
            rtbResultado.ReadOnly = true;
            rtbResultado.Text = "Seleccione origen y destino...";

            panelSide.Controls.Add(rtbResultado);

            // 2. EL MAPA
            pbMapa = new PictureBox();
            pbMapa.Dock = DockStyle.None;
            pbMapa.Location = new Point(ANCHO_PANEL, 0);
            pbMapa.Width = this.ClientSize.Width - ANCHO_PANEL;
            pbMapa.Height = this.ClientSize.Height;
            pbMapa.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbMapa.BackColor = Color.LightBlue;

            try
            {
                string rutaImg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mapa_nicaragua.jpg");
                if (File.Exists(rutaImg))
                {
                    pbMapa.Image = Image.FromFile(rutaImg);
                    pbMapa.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch { }

            pbMapa.Paint += PbMapa_Paint;

            pbMapa.MouseClick += (s, e) => {
                var (ratio, offX, offY) = GetImageScale();
                float realX = (e.X - offX) / ratio * (ANCHO_REF / pbMapa.Image.Width);
                float realY = (e.Y - offY) / ratio * (ALTO_REF / pbMapa.Image.Height);
                MessageBox.Show($"X={(int)realX}, Y={(int)realY}", "Coordenada Base");
            };

            this.Controls.Add(pbMapa);

            CargarDatosIniciales();
        }

        // Evento para asegurar que el cuadro de texto se redimensione si cambias el tamaño de la ventana
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pbMapa != null)
            {
                pbMapa.Width = this.ClientSize.Width - ANCHO_PANEL;
                pbMapa.Height = this.ClientSize.Height;
                pbMapa.Invalidate();
            }
            // No necesitamos redimensionar rtbResultado manualmente porque usamos AnchorStyles.Top | Bottom
        }

        private ComboBox CrearCombo(Panel p, int y)
        {
            ComboBox cb = new ComboBox { Location = new Point(15, y), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10), BackColor = Color.White };
            p.Controls.Add(cb); return cb;
        }

        private void CargarDatosIniciales()
        {
            var deptos = GrafoRutas.Instancia.Departamentos.Keys.OrderBy(x => x).ToArray();
            cboDepOrigen.Items.AddRange(deptos); cboDepDestino.Items.AddRange(deptos);
        }

        private void CargarMunicipios(ComboBox cbD, ComboBox cbM)
        {
            cbM.Items.Clear();
            if (cbD.SelectedItem != null && GrafoRutas.Instancia.Departamentos.ContainsKey(cbD.Text))
            {
                cbM.Items.AddRange(GrafoRutas.Instancia.Departamentos[cbD.Text].OrderBy(c => c.Nombre).ToArray());
                if (cbM.Items.Count > 0) cbM.SelectedIndex = 0;
            }
            pbMapa.Invalidate();
        }

        private void BtnCalcular_Click(object sender, EventArgs e)
        {
            if (cboMunOrigen.SelectedItem == null || cboMunDestino.SelectedItem == null) return;
            int dist; string tipo;
            rutaActual = GrafoRutas.Instancia.CalcularRuta((Ciudad)cboMunOrigen.SelectedItem, (Ciudad)cboMunDestino.SelectedItem, out dist, out tipo);

            if (rutaActual != null)
            {
                string rep = $"🏁 TOTAL: {dist} KM\n🛠 TIPO: {tipo}\n\n⬇ PASO A PASO:\n";
                for (int i = 0; i < rutaActual.Count; i++)
                {
                    string icono = (i == 0) ? "🟢 SALIDA: " : (i == rutaActual.Count - 1 ? "🏁 LLEGADA: " : "  ⬇ ");
                    rep += $"{icono}{rutaActual[i].Nombre}\n";
                }
                rtbResultado.Text = rep;
                pbMapa.Invalidate();
            }
            else { rtbResultado.Text = "⛔ NO HAY RUTA"; MessageBox.Show("No hay conexión."); }
        }

        private (float ratio, float offsetX, float offsetY) GetImageScale()
        {
            if (pbMapa.Image == null) return (1, 0, 0);
            Size imgS = pbMapa.Image.Size; Size pbS = pbMapa.ClientSize;
            float ratio; float offX = 0, offY = 0;
            if ((float)pbS.Width / pbS.Height < (float)imgS.Width / imgS.Height)
            {
                ratio = (float)pbS.Width / imgS.Width; offY = (pbS.Height - (imgS.Height * ratio)) / 2;
            }
            else
            {
                ratio = (float)pbS.Height / imgS.Height; offX = (pbS.Width - (imgS.Width * ratio)) / 2;
            }
            return (ratio, offX, offY);
        }

        private void PbMapa_Paint(object sender, PaintEventArgs e)
        {
            if (pbMapa.Image == null) return;

            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;

            // Como usamos StretchImage y posicionamiento manual, el ratio es simple:
            float ratioX = (float)pbMapa.Width / ANCHO_REF;
            float ratioY = (float)pbMapa.Height / ALTO_REF;

            Ciudad origen = cboMunOrigen.SelectedItem as Ciudad;
            Ciudad destino = cboMunDestino.SelectedItem as Ciudad;

            if (rutaActual != null && rutaActual.Count > 1)
            {
                using (Pen p = new Pen(Color.Red, 5))
                {
                    for (int i = 0; i < rutaActual.Count - 1; i++)
                    {
                        float x1 = rutaActual[i].X * ratioX; float y1 = rutaActual[i].Y * ratioY;
                        float x2 = rutaActual[i + 1].X * ratioX; float y2 = rutaActual[i + 1].Y * ratioY;
                        g.DrawLine(p, x1, y1, x2, y2);
                    }
                }
            }

            foreach (var c in GrafoRutas.Instancia.Mapa)
            {
                bool imp = (c == origen || c == destino || (rutaActual != null && rutaActual.Contains(c)));
                float sx = c.X * ratioX; float sy = c.Y * ratioY;

                if (imp)
                {
                    Color col = (c == origen) ? Color.Lime : (c == destino ? Color.Red : Color.Orange);
                    g.FillEllipse(new SolidBrush(col), sx - 8, sy - 8, 16, 16);
                    g.DrawEllipse(Pens.Black, sx - 8, sy - 8, 16, 16);
                    string txt = c.Nombre; g.DrawString(txt, new Font("Arial", 9, FontStyle.Bold), Brushes.Black, sx + 10, sy - 10);
                }
                else
                {
                    g.FillEllipse(new SolidBrush(Color.FromArgb(100, 50, 50, 50)), sx - 3, sy - 3, 6, 6);
                }
            }
        }
    }
}