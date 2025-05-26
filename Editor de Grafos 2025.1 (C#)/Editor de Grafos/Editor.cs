using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Editor_de_Grafos
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();            
        }

        #region Botoes de Algoritmo do Menu
        private void BtParesOrd_Click(object sender, EventArgs e)
        {
            string pares = g.ParesOrdenados();
            MessageBox.Show(pares, "Pares Ordenados", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtGrafoEuleriano_Click(object sender, EventArgs e)
        {
            if(g.IsEuleriano())
                MessageBox.Show("O grafo e Euleriano!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("O grafo não e Euleriano!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtGrafoUnicursal_Click(object sender, EventArgs e)
        {
            if (g.IsUnicursal())
                MessageBox.Show("O grafo e unicursal!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("O grafo não e unicursal!", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtBuscaProfundidade_Click(object sender, EventArgs e)
        {

        }

        #endregion --------------------------------------------------------------------------------------------------

        #region botoes restantes do menu

        private void BtNovo_Click(object sender, EventArgs e)
        {
            g.limpar();
        }

        private void BtAbrir_Click(object sender, EventArgs e)
        {
            if(OPFile.ShowDialog() == DialogResult.OK)
            {
                g.abrirArquivo(OPFile.FileName);
                g.Refresh();
            }
        }

        private void BtSalvar_Click(object sender, EventArgs e)
        {
            if(SVFile.ShowDialog() == DialogResult.OK)
            {
                g.SalvarArquivo(SVFile.FileName);
            }
        }

        private void BtSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtPeso_Click(object sender, EventArgs e)
        {
            if(BtPeso.Checked)
            {
                BtPeso.Checked = false;
                g.setExibirPesos(false);

            }
            else
            {
                BtPeso.Checked = true;
                g.setExibirPesos(true);
            }
            g.Refresh();
        }

        private void BtPesoAleatorio_Click(object sender, EventArgs e)
        {
            if(BtPesoAleatorio.Checked)
            {
                BtPesoAleatorio.Checked = false;
                g.setPesosAleatorios(false);
            }
            else
            {
                BtPesoAleatorio.Checked = true;
                g.setPesosAleatorios(true);
            }
        }

        private void BtSobre_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editor de Grafos - 2025/1\n\n" +
                "Desenvolvido por:\nNatan Macedo de Magalhaes\n" +
                "Profª. Roselene Costa\n" +
                "Prof. Virgilio Borges de Oliveira\n" +
                "MATRÍCULA-NOME DO ALUNO (co-autor)\n\n" + 
                "Algoritmos e Estruturas de Dados II\nFaculdade COTEMIG\nSomente para fins didáticos.", "Sobre o Editor de Grafos...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion --------------------------------------------------------------------------------------------------

        private void completarGrafoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.CompletarGrafo();
            g.setVerticeMarcado(null);
        }

        private void profundidadeToolStripMenuItem_Click(object sender, EventArgs e) {
            g.ResetarVisitados();
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Digite o vértice inicial (0 a {g.getN() - 1}):",
                "Busca em Profundidade",
                "0"
            );

            if (int.TryParse(input, out int v) && v >= 0 && v < g.getN())
            {
                g.Profundidade(v);
            }
            else
            {
                MessageBox.Show("Vértice inválido!");
            }
        }

        private void algoritmosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void larguraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            g.ResetarVisitados();
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Digite o vértice inicial (0 a {g.getN() - 1}):",
                "Busca em Largura",
                "0"
            );

            if (int.TryParse(input, out int v) && v >= 0 && v < g.getN())
            {
                g.Largura(v);
            }
            else
            {
                MessageBox.Show("Vértice inválido!");
            }
        }

        private void caminhoMinimoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            string inputOrigem = Interaction.InputBox(
                $"Digite o vértice de origem (0 a {g.getN() - 1}):",
                "Caminho Mínimo - Origem",
                "0"
            );

            string inputDestino = Interaction.InputBox(
                $"Digite o vértice de destino (0 a {g.getN() - 1}):",
                "Caminho Mínimo - Destino",
                "1"
            );

            if (int.TryParse(inputOrigem, out int origem) && int.TryParse(inputDestino, out int destino))
            {
                if (origem >= 0 && origem < g.getN() && destino >= 0 && destino < g.getN())
                {
                    g.CaminhoMinimo(origem, destino);
                }
                else
                {
                    MessageBox.Show("Vértices fora do intervalo válido!");
                }
            }
            else
            {
                MessageBox.Show("Entrada inválida!");
            }
        }

        private void numCromaticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            g.NumeroCromatico();
        }

        private void aGMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Digite o vértice inicial (0 a {g.getN() - 1}):",
                "Árvore Geradora Mínima",
                "0"
            );

            if (int.TryParse(input, out int v) && v >= 0 && v < g.getN())
            {
                g.AGM(v);
            }
            else
            {
                MessageBox.Show("Vértice inválido!");
            }
        }

        private void arvoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (g.getN() == 0)
            {
                MessageBox.Show("O grafo está vazio!");
                return;
            }

            g.IsArvore();
        }
    }
}

