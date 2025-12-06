using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArbolEmpresaMudanzas.Vistas
{
    public class LoginForm : Form
    {
        private TextBox txtUsuario, txtPassword;
        private Button btnLogin;

        // LIMITAR A 3 INTENTOS
        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        public LoginForm() { ConfigurarDiseñoModerno(); }

        private void ConfigurarDiseñoModerno()
        {
            this.Text = "Login"; this.Size = new Size(780, 330); this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None; this.BackColor = Color.FromArgb(15, 15, 15); this.Opacity = 0.95;

            Panel pIzq = new Panel { BackColor = Color.FromArgb(0, 122, 204), Dock = DockStyle.Left, Width = 250 };
            Label l1 = new Label { Text = "MOVE\nSMART", Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.White, Location = new Point(50, 100), AutoSize = true };
            pIzq.Controls.Add(l1); this.Controls.Add(pIzq);

            Label lTitle = new Label { Text = "INICIAR SESIÓN", ForeColor = Color.DimGray, Font = new Font("Segoe UI", 15), Location = new Point(310, 30), AutoSize = true };
            this.Controls.Add(lTitle);

            Label lU = new Label { Text = "USUARIO", ForeColor = Color.DimGray, Location = new Point(310, 80), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true };
            this.Controls.Add(lU);
            txtUsuario = new TextBox { BackColor = Color.FromArgb(15, 15, 15), ForeColor = Color.LightGray, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 12), Text = "admin", Location = new Point(310, 100), Size = new Size(400, 20) };
            this.Controls.Add(txtUsuario);
            this.Controls.Add(new Panel { BackColor = Color.DimGray, Size = new Size(400, 1), Location = new Point(310, 125) });

            Label lP = new Label { Text = "CONTRASEÑA", ForeColor = Color.DimGray, Location = new Point(310, 150), Font = new Font("Segoe UI", 9, FontStyle.Bold), AutoSize = true };
            this.Controls.Add(lP);
            txtPassword = new TextBox { BackColor = Color.FromArgb(15, 15, 15), ForeColor = Color.LightGray, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 12), PasswordChar = '●', Location = new Point(310, 170), Size = new Size(400, 20) };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(null, null); };
            this.Controls.Add(txtPassword);
            this.Controls.Add(new Panel { BackColor = Color.DimGray, Size = new Size(400, 1), Location = new Point(310, 195) });

            btnLogin = new Button { Text = "ACCEDER", BackColor = Color.FromArgb(40, 40, 40), ForeColor = Color.LightGray, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 12), Location = new Point(310, 230), Size = new Size(400, 40), Cursor = Cursors.Hand };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) => { btnLogin.BackColor = Color.FromArgb(0, 122, 204); btnLogin.ForeColor = Color.White; };
            btnLogin.MouseLeave += (s, e) => { btnLogin.BackColor = Color.FromArgb(40, 40, 40); btnLogin.ForeColor = Color.LightGray; };
            this.Controls.Add(btnLogin);

            Label lClose = new Label { Text = "X", ForeColor = Color.Gray, Font = new Font("Arial", 12, FontStyle.Bold), Location = new Point(750, 5), AutoSize = true, Cursor = Cursors.Hand };
            lClose.Click += (s, e) => Application.Exit();
            this.Controls.Add(lClose);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234")
            {
                this.Hide(); new MenuPrincipal().ShowDialog(); this.Close();
            }
            else
            {
                intentosFallidos++;
                int restantes = MAX_INTENTOS - intentosFallidos;
                if (intentosFallidos >= MAX_INTENTOS)
                {
                    MessageBox.Show("⛔ BLOQUEO: 3 intentos fallidos.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show($"Datos incorrectos. Quedan {restantes} intentos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear(); txtPassword.Focus();
                }
            }
        }
    }
}