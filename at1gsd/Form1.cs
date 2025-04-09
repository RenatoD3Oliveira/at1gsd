using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace at1gsd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string usuario = UsuarioTxt.Text.Trim();
            string email = EmailTxt.Text.Trim();
            string senha = SenhaTxt.Text.Trim();
            string ConfSenha = ConfirmarSenhaTxt.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (senha != ConfSenha)
            {
                MessageBox.Show("As senhas não coincidem!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (SqlConnection con = new Conecte().ReturnConnection())
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Verifica se o usuário já existe
                string verificarUsuario = "SELECT COUNT(*) FROM Usuarios WHERE usuario = @Usuario";
                using (SqlCommand cmdVerificar = new SqlCommand(verificarUsuario, con))
                {
                    cmdVerificar.Parameters.AddWithValue("@Usuario", usuario);
                    int usuarioExistente = (int)cmdVerificar.ExecuteScalar();

                    if (usuarioExistente > 0)
                    {
                        MessageBox.Show("Usuário já existe! Escolha outro nome de usuário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                string senhaHash = GerarHash(senha);

                // Insere novo usuário
                string inserirUsuario = "INSERT INTO Usuarios (usuario, email, senha) VALUES (@Usuario, @Email, @Senha)";
                using (SqlCommand cmdInserir = new SqlCommand(inserirUsuario, con))
                {
                    cmdInserir.Parameters.AddWithValue("@Usuario", usuario);
                    cmdInserir.Parameters.AddWithValue("@Email", email);
                    cmdInserir.Parameters.AddWithValue("@Senha",  senhaHash);
                    cmdInserir.ExecuteNonQuery();
                }

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UsuarioTxt.Clear();
                EmailTxt.Clear();
                SenhaTxt.Clear();
            }


        }
        private string GerarHash(string senha)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Formato hexadecimal
                }
                return builder.ToString();
            }
        }


        private void SenhaTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void confirmar_senha_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
