using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoBilheteria
{
    public partial class Form1 : Form
    {
        const int FILEIRAS = 15;
        const int POLTRONAS = 40;

        // 0 = vaga
        // 1 = inteira
        // 2 = meia
        int[,] lugares = new int[FILEIRAS, POLTRONAS];

        Button[,] botoes = new Button[FILEIRAS, POLTRONAS];

        Label lblResultado;
        Button btnFaturamento;

        public Form1()
        {
            InitializeComponent();

            CriarInterface();
        }

        private void CriarInterface()
        {
            Text = "Projeto Bilheteria";
            Width = 1000;
            Height = 700;

            Panel painel = new Panel();
            painel.Location = new Point(10, 10);
            painel.Size = new Size(950, 520);
            painel.AutoScroll = true;

            Controls.Add(painel);

            int largura = 22;
            int altura = 25;

            for (int i = 0; i < FILEIRAS; i++)
            {
                for (int j = 0; j < POLTRONAS; j++)
                {
                    Button btn = new Button();

                    btn.Width = largura;
                    btn.Height = altura;

                    btn.Left = j * largura;
                    btn.Top = i * altura;

                    btn.Text = "";
                    btn.BackColor = Color.LightGreen;

                    btn.Tag = new Point(i, j);

                    btn.Click += Poltrona_Click;

                    painel.Controls.Add(btn);

                    botoes[i, j] = btn;
                }
            }

            btnFaturamento = new Button();

            btnFaturamento.Text = "Faturamento";
            btnFaturamento.Location = new Point(10, 550);
            btnFaturamento.Size = new Size(120, 35);

            btnFaturamento.Click += Faturamento_Click;

            Controls.Add(btnFaturamento);

            lblResultado = new Label();

            lblResultado.Location = new Point(150, 545);
            lblResultado.Size = new Size(700, 60);

            lblResultado.Font = new Font("Arial", 11);

            Controls.Add(lblResultado);
        }

        private void Poltrona_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            Point posicao = (Point)btn.Tag;

            int fileira = posicao.X;
            int poltrona = posicao.Y;

            if (fileira < 0 || fileira >= FILEIRAS ||
                poltrona < 0 || poltrona >= POLTRONAS)
            {
                MessageBox.Show("Coordenadas inválidas.");
                return;
            }

            if (lugares[fileira, poltrona] == 0)
            {
                DialogResult resposta =
                    MessageBox.Show(
                        "Deseja reservar como MEIA entrada?\n\n" +
                        "Sim = Meia entrada\n" +
                        "Não = Inteira",
                        "Tipo de reserva",
                        MessageBoxButtons.YesNoCancel);

                if (resposta == DialogResult.Yes)
                {
                    lugares[fileira, poltrona] = 2;

                    btn.BackColor = Color.Gold;
                }
                else if (resposta == DialogResult.No)
                {
                    lugares[fileira, poltrona] = 1;

                    btn.BackColor = Color.Red;
                }
            }
            else
            {
                string estado =
                    lugares[fileira, poltrona] == 1
                    ? "Ocupada (Inteira)"
                    : "Ocupada (Meia entrada)";

                MessageBox.Show(
                    $"Fileira {fileira + 1}, Poltrona {poltrona + 1}\n" +
                    $"Estado: {estado}");
            }
        }

        private double ValorFileira(int fileira)
        {
            if (fileira <= 4)
                return 50.00;

            if (fileira <= 9)
                return 30.00;

            return 15.00;
        }

        private void Faturamento_Click(object sender, EventArgs e)
        {
            int ocupados = 0;
            double total = 0;

            for (int i = 0; i < FILEIRAS; i++)
            {
                for (int j = 0; j < POLTRONAS; j++)
                {
                    if (lugares[i, j] != 0)
                    {
                        ocupados++;

                        double valor = ValorFileira(i);

                        if (lugares[i, j] == 2)
                            valor /= 2;

                        total += valor;
                    }
                }
            }

            lblResultado.Text =
                $"Qtde de lugares ocupados: {ocupados}\n" +
                $"Valor da bilheteria: R$ {total:N2}";
        }
    }
}