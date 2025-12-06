using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ArbolEmpresaMudanzas.Modulo;
using ArbolEmpresaMudanzas.Estructuras;

namespace ArbolEmpresaMudanzas.Vistas
{
    public class VistaArbol : Form
    {
        // Controles Visuales
        private TreeView treeView1;
        private TextBox txtNombre, txtBuscar;
        private MaskedTextBox mskCedula;
        private ComboBox cboCargo;
        private DateTimePicker dtpFecha;
        private Button btnAgregar, btnActualizar, btnEliminar, btnBuscar, btnLimpiar, btnRegresar;
        private Label lblEstado;

        // Lógica del Negocio
        private ArbolEmpresa miEmpresa;
        private Empleado empleadoSeleccionado;

        public VistaArbol()
        {
            ConfigurarDiseño();
            InicializarLogica();
        }

        private void ConfigurarDiseño()
        {
            this.Text = "MoveSmart - Gestión de Personal";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            btnRegresar = new Button() { Text = "⬅ VOLVER AL MENÚ", Location = new Point(900, 20), Size = new Size(150, 40), BackColor = Color.IndianRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnRegresar.Click += (s, e) => this.Close();
            this.Controls.Add(btnRegresar);

            this.Controls.Add(new Label() { Text = "ORGANIGRAMA", Location = new Point(20, 20), Font = new Font("Arial", 10, FontStyle.Bold), AutoSize = true });
            lblEstado = new Label() { Text = "Cargando...", Location = new Point(150, 20), AutoSize = true, ForeColor = Color.Blue };
            this.Controls.Add(lblEstado);

            this.Controls.Add(new Label() { Text = "Buscar:", Location = new Point(20, 50), AutoSize = true });
            txtBuscar = new TextBox() { Location = new Point(70, 48), Width = 150 };
            this.Controls.Add(txtBuscar);

            btnBuscar = new Button() { Text = "🔍", Location = new Point(230, 46), Width = 40, Height = 23, BackColor = Color.Gold, FlatStyle = FlatStyle.Flat };
            btnBuscar.Click += BtnBuscar_Click;
            this.Controls.Add(btnBuscar);

            treeView1 = new TreeView() { Location = new Point(20, 80), Size = new Size(350, 550) };
            treeView1.AfterSelect += TreeView1_AfterSelect;
            treeView1.HideSelection = false;
            this.Controls.Add(treeView1);

            GroupBox grp = new GroupBox() { Text = "Datos del Empleado", Location = new Point(400, 70), Size = new Size(600, 300), Font = new Font("Arial", 10, FontStyle.Bold) };
            int y = 30;

            grp.Controls.Add(new Label() { Text = "Nombre:", Location = new Point(20, y), AutoSize = true });
            txtNombre = new TextBox() { Location = new Point(200, y), Width = 250 };
            grp.Controls.Add(txtNombre); y += 40;

            grp.Controls.Add(new Label() { Text = "Cédula:", Location = new Point(20, y), AutoSize = true });
            mskCedula = new MaskedTextBox() { Mask = "AAA-AAAAAA-AAAAA", Location = new Point(200, y), Width = 250 };
            grp.Controls.Add(mskCedula); y += 40;

            grp.Controls.Add(new Label() { Text = "Cargo:", Location = new Point(20, y), AutoSize = true });
            cboCargo = new ComboBox() { Location = new Point(200, y), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboCargo.Items.AddRange(new string[] { "Chofer de Camión", "Empacador", "Cargador", "Personal de Almacén", "Asistente RRHH", "Reclutador", "Asesor de Ventas", "Atención al Cliente", "Mecánico", "Técnico Mantenimiento", "Contador" });
            cboCargo.SelectedIndex = 0;
            grp.Controls.Add(cboCargo); y += 40;

            grp.Controls.Add(new Label() { Text = "Fecha:", Location = new Point(20, y), AutoSize = true });
            dtpFecha = new DateTimePicker() { Location = new Point(200, y), Width = 250, Format = DateTimePickerFormat.Short };
            grp.Controls.Add(dtpFecha);
            this.Controls.Add(grp);

            int bx = 400, by = 400;
            btnAgregar = CrearBoton("AGREGAR", bx, by, Color.SeaGreen); btnAgregar.Click += BtnAgregar_Click;
            btnActualizar = CrearBoton("ACTUALIZAR", bx + 130, by, Color.Orange); btnActualizar.Click += BtnActualizar_Click;
            btnEliminar = CrearBoton("ELIMINAR", bx + 260, by, Color.IndianRed); btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar = CrearBoton("LIMPIAR", bx + 390, by, Color.Gray); btnLimpiar.Click += (s, e) => LimpiarCampos();
        }

        private Button CrearBoton(string txt, int x, int y, Color c)
        {
            Button b = new Button() { Text = txt, Location = new Point(x, y), Size = new Size(120, 45), BackColor = c, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Arial", 9, FontStyle.Bold) };
            this.Controls.Add(b); return b;
        }

        private void InicializarLogica()
        {
            miEmpresa = new ArbolEmpresa(new Empleado("Carlos Martínez", "Gerente General", "001000", new DateTime(2010, 1, 1)));

            if (miEmpresa.CargarDesdeArchivo())
            {
                lblEstado.Text = "Datos cargados (.bin)";
                lblEstado.ForeColor = Color.Green;
            }
            else
            {
                lblEstado.Text = "Nuevo";
                miEmpresa.AgregarSubordinado("Carlos Martínez", "Ana López", "Gerente Operaciones", "OP001", DateTime.Now);
                miEmpresa.AgregarSubordinado("Carlos Martínez", "Elena Mayorga", "Gerente RRHH", "RR001", DateTime.Now);
                miEmpresa.AgregarSubordinado("Carlos Martínez", "Roberto Díaz", "Gerente Finanzas", "FI001", DateTime.Now);
                miEmpresa.AgregarSubordinado("Carlos Martínez", "Lucía Torres", "Gerente Ventas", "VT001", DateTime.Now);
                miEmpresa.AgregarSubordinado("Carlos Martínez", "Miguel Ángel", "Gerente Almacén", "AL001", DateTime.Now);
                miEmpresa.AgregarSubordinado("Carlos Martínez", "José Ruiz", "Gerente Mantenimiento", "MT001", DateTime.Now);
            }
            RefrescarArbol();
        }

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || !mskCedula.MaskCompleted)
            {
                MessageBox.Show("Faltan datos."); return false;
            }
            if (!Regex.IsMatch(txtNombre.Text, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                MessageBox.Show("Solo letras en nombre."); return false;
            }
            return true;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;
            if (miEmpresa.ExisteCedula(mskCedula.Text)) { MessageBox.Show("Cédula duplicada."); return; }

            string cargo = cboCargo.Text;
            string jefe = ObtenerJefePorCargo(cargo);

            if (miEmpresa.AgregarSubordinado(jefe, txtNombre.Text, cargo, mskCedula.Text, dtpFecha.Value))
            {
                RefrescarArbol(); LimpiarCampos(); MessageBox.Show($"Agregado a: {jefe}");
            }
            else MessageBox.Show("Jefe no encontrado.");
        }

        private string ObtenerJefePorCargo(string c)
        {
            if (c.Contains("Chofer") || c.Contains("Empacador") || c.Contains("Cargador")) return "Ana López";
            if (c.Contains("RRHH") || c.Contains("Reclutador")) return "Elena Mayorga";
            if (c.Contains("Contador")) return "Roberto Díaz";
            if (c.Contains("Ventas") || c.Contains("Cliente")) return "Lucía Torres";
            if (c.Contains("Almacén") || c.Contains("Inventario")) return "Miguel Ángel";
            if (c.Contains("Mecánico") || c.Contains("Técnico")) return "José Ruiz";
            return "Carlos Martínez";
        }

        private void RefrescarArbol()
        {
            treeView1.Nodes.Clear();
            if (miEmpresa.Raiz != null)
            {
                TreeNode r = new TreeNode(miEmpresa.Raiz.ToString());
                r.Tag = miEmpresa.Raiz; // IMPORTANTE: Guardar el objeto en el Tag
                r.ForeColor = Color.DarkBlue;
                treeView1.Nodes.Add(r);
                PintarHijos(r, miEmpresa.Raiz);
                treeView1.ExpandAll();
            }
        }

        private void PintarHijos(TreeNode p, Empleado e)
        {
            foreach (var sub in e.Subordinados)
            {
                TreeNode h = new TreeNode(sub.ToString());
                h.Tag = sub; // IMPORTANTE: Guardar el objeto en el Tag
                if (sub.Cargo.Contains("Gerente")) h.ForeColor = Color.DarkGreen;
                p.Nodes.Add(h);
                PintarHijos(h, sub);
            }
        }

        // ===============================================
        // AQUÍ ESTABAN LOS MÉTODOS FALTANTES (CORREGIDO)
        // ===============================================

        // 1. BUSCAR
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            string busqueda = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(busqueda)) return;

            LimpiarColores(treeView1.Nodes); // Limpiar amarillo anterior
            treeView1.CollapseAll();

            // Buscar en memoria
            Empleado encontrado = miEmpresa.BuscarEmpleado(busqueda);

            if (encontrado != null)
            {
                // Buscar el nodo en el árbol visual que tiene este empleado en su Tag
                TreeNode nodoVisual = BuscarNodoVisual(treeView1.Nodes, encontrado);

                if (nodoVisual != null)
                {
                    treeView1.SelectedNode = nodoVisual;
                    nodoVisual.BackColor = Color.Yellow; // Resaltar
                    nodoVisual.EnsureVisible(); // Bajar scroll y abrir carpetas
                    treeView1.Focus();
                    LlenarCampos(encontrado);
                }
                else
                {
                    MessageBox.Show("Empleado encontrado en datos, pero no en el árbol visual.", "Error Visual");
                }
            }
            else
            {
                MessageBox.Show("No se encontró nadie con ese nombre o cédula.", "Sin resultados");
            }
        }

        private TreeNode BuscarNodoVisual(TreeNodeCollection nodos, Empleado objetivo)
        {
            foreach (TreeNode n in nodos)
            {
                // Comparamos si el objeto en el Tag es el mismo que buscamos
                if (n.Tag == objetivo) return n;

                TreeNode h = BuscarNodoVisual(n.Nodes, objetivo);
                if (h != null) return h;
            }
            return null;
        }

        private void LimpiarColores(TreeNodeCollection nodos)
        {
            foreach (TreeNode n in nodos)
            {
                n.BackColor = Color.White;
                LimpiarColores(n.Nodes);
            }
        }

        // 2. ACTUALIZAR
        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (empleadoSeleccionado == null) { MessageBox.Show("Selecciona a alguien del árbol."); return; }
            if (empleadoSeleccionado.Cargo.Contains("Gerente")) { MessageBox.Show("No puedes editar Gerentes."); return; }
            if (!Validar()) return;

            // Si cambió la cédula, validar que no se repita
            if (empleadoSeleccionado.Cedula != mskCedula.Text && miEmpresa.ExisteCedula(mskCedula.Text))
            {
                MessageBox.Show("La nueva cédula ya existe."); return;
            }

            empleadoSeleccionado.ActualizarDatos(txtNombre.Text, cboCargo.Text, mskCedula.Text, dtpFecha.Value);
            miEmpresa.GuardarEnArchivo();
            RefrescarArbol();
            LimpiarCampos();
            MessageBox.Show("Datos actualizados.");
        }

        // 3. ELIMINAR
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (empleadoSeleccionado == null) return;

            var resp = MessageBox.Show($"¿Eliminar a {empleadoSeleccionado.Nombre}?", "Confirmar", MessageBoxButtons.YesNo);
            if (resp == DialogResult.Yes)
            {
                if (miEmpresa.EliminarEmpleado(empleadoSeleccionado.Nombre))
                {
                    RefrescarArbol();
                    LimpiarCampos();
                    MessageBox.Show("Eliminado.");
                }
                else MessageBox.Show("No se puede eliminar (probablemente es el CEO).");
            }
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Empleado emp)
            {
                empleadoSeleccionado = emp;
                LlenarCampos(emp);
            }
        }

        private void LlenarCampos(Empleado emp)
        {
            txtNombre.Text = emp.Nombre;
            mskCedula.Text = emp.Cedula;
            cboCargo.Text = emp.Cargo;
            dtpFecha.Value = emp.FechaIngreso;
        }

        private void LimpiarCampos()
        {
            empleadoSeleccionado = null;
            txtNombre.Clear();
            mskCedula.Clear();
            cboCargo.SelectedIndex = 0;
            LimpiarColores(treeView1.Nodes);
        }
    }
}