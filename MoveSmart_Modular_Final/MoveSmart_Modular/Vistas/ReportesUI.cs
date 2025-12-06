using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text; // Para construir textos largos
using System.Linq; // Para ordenar listas
using System.Collections.Generic;
using ArbolEmpresaMudanzas.Modulo;
using ArbolEmpresaMudanzas.Estructuras;
using MoveSmart_Modular.Estructuras;

namespace ArbolEmpresaMudanzas.Vistas
{
    public class ReportesUI : Form
    {
        private Button btnRepPersonal, btnRepRutas, btnRepAntiguedad, btnCerrar;
        private RichTextBox rtbSalida; // Cuadro de texto enriquecido para el reporte
        private Label lblTitulo;
        private ArbolEmpresa miEmpresa; // Necesitamos cargar los datos

        public ReportesUI()
        {
            ConfigurarDiseño();
            CargarDatos();
        }

        private void CargarDatos()
        {
            // Cargamos el árbol desde el archivo .bin para tener los datos reales
            miEmpresa = new ArbolEmpresa(new Empleado("Temp", "Temp", "000", DateTime.Now));
            if (!miEmpresa.CargarDesdeArchivo())
            {
                MessageBox.Show("No se encontraron datos de empleados guardados.", "Aviso");
            }
        }

        private void ConfigurarDiseño()
        {
            this.Text = "MoveSmart - Centro de Reportes";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Título
            lblTitulo = new Label() { Text = "GENERADOR DE REPORTES", Location = new Point(20, 20), Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, ForeColor = Color.DarkSlateBlue };
            this.Controls.Add(lblTitulo);

            // Botones (Menú lateral izquierdo)
            int y = 70;
            btnRepPersonal = CrearBoton("👥 PERSONAL POR ÁREA", 20, y, Color.Teal);
            btnRepPersonal.Click += GenerarReportePersonal;

            y += 60;
            btnRepRutas = CrearBoton("🚚 COBERTURA VIAL", 20, y, Color.OrangeRed);
            btnRepRutas.Click += GenerarReporteRutas;

            y += 60;
            btnRepAntiguedad = CrearBoton("📅 ANTIGÜEDAD", 20, y, Color.SteelBlue);
            btnRepAntiguedad.Click += GenerarReporteAntiguedad;

            // Área de Texto (Derecha)
            rtbSalida = new RichTextBox();
            rtbSalida.Location = new Point(250, 70);
            rtbSalida.Size = new Size(500, 450);
            rtbSalida.Font = new Font("Consolas", 10); // Fuente tipo consola para que se alinee bien
            rtbSalida.BackColor = Color.White;
            rtbSalida.ReadOnly = true;
            this.Controls.Add(rtbSalida);

            // Botón Cerrar
            btnCerrar = new Button() { Text = "CERRAR", Location = new Point(650, 530), Size = new Size(100, 30), BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnCerrar.Click += (s, e) => this.Close();
            this.Controls.Add(btnCerrar);
        }

        private Button CrearBoton(string texto, int x, int y, Color color)
        {
            Button b = new Button();
            b.Text = texto;
            b.Location = new Point(x, y);
            b.Size = new Size(200, 50);
            b.BackColor = color;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            this.Controls.Add(b);
            return b;
        }

        // ==========================================
        // LÓGICA DE LOS REPORTES
        // ==========================================

        // REPORTE 1: PERSONAL (Recursivo del Árbol)
        private void GenerarReportePersonal(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine("   REPORTE DE ESTRUCTURA ORGANIZACIONAL   ");
            sb.AppendLine("==========================================");
            sb.AppendLine($"Fecha de emisión: {DateTime.Now}\n");

            if (miEmpresa.Raiz != null)
            {
                RecorrerArbolReporte(miEmpresa.Raiz, sb, 0);
            }
            else
            {
                sb.AppendLine("No hay datos en el sistema.");
            }

            rtbSalida.Text = sb.ToString();
        }

        private void RecorrerArbolReporte(Empleado nodo, StringBuilder sb, int nivel)
        {
            // Usamos indentación para simular la jerarquía visualmente en texto
            string indent = new string(' ', nivel * 4); // 4 espacios por nivel
            string icono = (nivel == 0) ? "👑" : (nodo.Subordinados.Count > 0 ? "📁" : "👤");

            sb.AppendLine($"{indent}{icono} {nodo.Cargo}: {nodo.Nombre} (Cédula: {nodo.Cedula})");

            foreach (var sub in nodo.Subordinados)
            {
                RecorrerArbolReporte(sub, sb, nivel + 1);
            }
        }

        // REPORTE 2: RUTAS (Iteración del Grafo)
        private void GenerarReporteRutas(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine("      REPORTE DE COBERTURA LOGÍSTICA      ");
            sb.AppendLine("==========================================");
            sb.AppendLine("Análisis de conectividad por ciudad:\n");

            var grafo = GrafoRutas.Instancia;
            int totalRutas = 0;

            foreach (var ciudad in grafo.Mapa)
            {
                sb.AppendLine($"📍 CIUDAD: {ciudad.Nombre} ({ciudad.Departamento})");

                if (grafo.Adyacencia.ContainsKey(ciudad))
                {
                    var rutas = grafo.Adyacencia[ciudad];
                    sb.AppendLine($"   Conexiones: {rutas.Count}");

                    foreach (var ruta in rutas)
                    {
                        string tipo = ruta.EsAcuatica ? "🌊 (Agua)" : "🛣️ (Carretera)";
                        sb.AppendLine($"     -> Hacia {ruta.Destino.Nombre}: {ruta.DistanciaKm} km [{tipo}]");
                        totalRutas++;
                    }
                }
                else
                {
                    sb.AppendLine("   ⚠️ SIN CONEXIONES REGISTRADAS");
                }
                sb.AppendLine(new string('-', 30));
            }

            sb.AppendLine($"\nTOTAL DE TRAMOS VIALES: {totalRutas}");
            rtbSalida.Text = sb.ToString();
        }

        // REPORTE 3: ANTIGÜEDAD (Lista Plana y Ordenamiento)
        private void GenerarReporteAntiguedad(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine("        REPORTE DE ANTIGÜEDAD RRHH        ");
            sb.AppendLine("==========================================");
            sb.AppendLine("Ordenado del más antiguo al más reciente:\n");

            // 1. Aplanamos el árbol a una lista simple
            List<Empleado> listaPlana = new List<Empleado>();
            if (miEmpresa.Raiz != null) ObtenerListaPlana(miEmpresa.Raiz, listaPlana);

            // 2. Ordenamos la lista por Fecha (LINQ)
            var listaOrdenada = listaPlana.OrderBy(x => x.FechaIngreso).ToList();

            // 3. Imprimimos
            foreach (var emp in listaOrdenada)
            {
                // Calculamos años de servicio
                int anios = DateTime.Now.Year - emp.FechaIngreso.Year;
                sb.AppendLine($"📅 {emp.FechaIngreso.ToShortDateString()} | {emp.Nombre} ({emp.Cargo})");
                sb.AppendLine($"   --> Antigüedad: {anios} años\n");
            }

            rtbSalida.Text = sb.ToString();
        }

        private void ObtenerListaPlana(Empleado nodo, List<Empleado> lista)
        {
            lista.Add(nodo);
            foreach (var sub in nodo.Subordinados) ObtenerListaPlana(sub, lista);
        }
    }
}