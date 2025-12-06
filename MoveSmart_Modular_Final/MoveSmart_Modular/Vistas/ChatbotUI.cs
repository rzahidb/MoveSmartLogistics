using System;
using System.Drawing;
using System.Windows.Forms;
using ArbolEmpresaMudanzas.Estructuras; // Importante: Usa tu nueva lógica limpia

namespace ArbolEmpresaMudanzas.Vistas
{
    public class ChatbotUI : Form
    {
        // Controles
        private RichTextBox rtbChat;
        private TextBox txtMensaje;
        private Button btnEnviar;
        private Panel panelInput;

        // Conexión con el cerebro (Tu lógica nueva)
        private ChatbotLogic cerebroBot;

        public ChatbotUI()
        {
            ConfigurarDiseño();
            cerebroBot = new ChatbotLogic(); // Conectamos con la lógica limpia
            MensajeBot("¡Hola! Soy MoveBot. 🚚\nPuedes preguntarme sobre rutas, personal o la empresa.");
        }

        private void ConfigurarDiseño()
        {
            this.Text = "Asistente Virtual - MoveSmart";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Historial del chat
            rtbChat = new RichTextBox
            {
                Dock = DockStyle.Top,
                Height = 480,
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                Padding = new Padding(10)
            };

            // Panel inferior
            panelInput = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(230, 230, 230)
            };

            // Caja de texto
            txtMensaje = new TextBox
            {
                Top = 20,
                Left = 20,
                Width = 350,
                Height = 30,
                Font = new Font("Segoe UI", 12)
            };
            txtMensaje.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) ProcesarMensaje(); };

            // Botón enviar
            btnEnviar = new Button
            {
                Text = "Enviar",
                Top = 18,
                Left = 380,
                Width = 80,
                Height = 35,
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnEnviar.Click += (s, e) => ProcesarMensaje();

            // Agregar controles
            panelInput.Controls.Add(txtMensaje);
            panelInput.Controls.Add(btnEnviar);
            this.Controls.Add(panelInput);
            this.Controls.Add(rtbChat);
        }

        private async void ProcesarMensaje()
        {
            string pregunta = txtMensaje.Text.Trim();
            if (string.IsNullOrEmpty(pregunta)) return;

            // 1. Mostrar mensaje del usuario
            MensajeUsuario(pregunta);
            txtMensaje.Clear();
            btnEnviar.Enabled = false; // Evitar doble click
            txtMensaje.Focus();

            try
            {
                // 2. Enviar a la lógica nueva (sin Google libraries)
                string respuesta = await cerebroBot.EnviarMensajeGemini(pregunta);

                // 3. Mostrar respuesta
                MensajeBot(respuesta);
            }
            catch (Exception ex)
            {
                MensajeBot("❌ Error: " + ex.Message);
            }
            finally
            {
                btnEnviar.Enabled = true;
            }
        }

        private void MensajeUsuario(string texto)
        {
            rtbChat.SelectionColor = Color.Blue;
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChat.AppendText("\n👤 TÚ: ");
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font("Segoe UI", 10);
            rtbChat.AppendText(texto + "\n");
            rtbChat.ScrollToCaret();
        }

        private void MensajeBot(string texto)
        {
            rtbChat.SelectionColor = Color.DarkGreen;
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChat.AppendText("\n🤖 MOVEBOT: ");
            rtbChat.SelectionColor = Color.Black;
            rtbChat.SelectionFont = new Font("Segoe UI", 10);
            rtbChat.AppendText(texto + "\n");
            rtbChat.ScrollToCaret();
        }
    }
}