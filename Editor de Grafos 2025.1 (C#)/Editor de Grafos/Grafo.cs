using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Editor_de_Grafos
{
    public class Grafo : GrafoBase, iGrafo
    {
        bool[] visitado;


        public void IsArvore()
        {
            int n = getN();
            bool[] visitado = new bool[n];

            // Função auxiliar para detectar ciclos
            bool TemCiclo(int v, int pai)
            {
                visitado[v] = true;

                for (int i = 0; i < n; i++)
                {
                    if (getAresta(v, i) != null)
                    {
                        if (!visitado[i])
                        {
                            if (TemCiclo(i, v))
                                return true;
                        }
                        else if (i != pai)
                        {
                            return true; // ciclo encontrado
                        }
                    }
                }

                return false;
            }

            // 1. Verificar ciclos
            if (TemCiclo(0, -1))
            {
                MessageBox.Show("O grafo NÃO é uma Árvore (possui ciclo).", "Resultado");
                return;
            }

            // 2. Verificar se é conexo (todos visitados)
            for (int i = 0; i < n; i++)
            {
                if (!visitado[i])
                {
                    MessageBox.Show("O grafo NÃO é uma Árvore (não é conexo).", "Resultado");
                    return;
                }
            }

            // 3. Verificar número de arestas = n - 1
            int qtdArestas = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (getAresta(i, j) != null)
                        qtdArestas++;
                }
            }

            if (qtdArestas != n - 1)
            {
                MessageBox.Show($"O grafo NÃO é uma Árvore (possui {qtdArestas} arestas, mas deveria ter {n - 1}).", "Resultado");
            }
            else
            {
                MessageBox.Show("O grafo É uma Árvore!", "Resultado");
            }
        }



        public void AGM(int v)
        {
            int custoTotal = 0;
            int n = getN();
            int[] chave = new int[n];
            int[] pai = new int[n];
            bool[] naAGM = new bool[n];

            for (int i = 0; i < n; i++)
            {
                chave[i] = int.MaxValue;
                pai[i] = -1;
                naAGM[i] = false;
            }

            chave[v] = 0;

            for (int count = 0; count < n - 1; count++)
            {
                int u = -1;
                int menor = int.MaxValue;

                for (int i = 0; i < n; i++)
                {
                    if (!naAGM[i] && chave[i] < menor)
                    {
                        menor = chave[i];
                        u = i;
                    }
                }

                if (u == -1)
                    break;

                naAGM[u] = true;

                for (int i = 0; i < n; i++)
                {
                    Aresta a = getAresta(u, i);
                    if (a != null && !naAGM[i] && a.getPeso() < chave[i])
                    {
                        chave[i] = a.getPeso();
                        pai[i] = u;
                    }
                }
            }


            // Colorir as arestas da AGM
            for (int i = 0; i < n; i++)
            {
                int u = pai[i];
                if (u != -1)
                {
                    Aresta a1 = getAresta(u, i);
                    Aresta a2 = getAresta(i, u);

                    if (a1 != null) a1.setCor(Color.Red);
                    if (a2 != null) a2.setCor(Color.Red);

                    custoTotal += chave[i];
                }
            }

            MessageBox.Show($"Custo total da AGM: {custoTotal}", "Árvore Geradora Mínima");
        }


        public void CaminhoMinimo(int i, int j)
        {
            int n = getN();
            int[] dist = new int[n];
            int[] precedente = new int[n];
            bool[] fechado = new bool[n];

            // Inicializar
            for (int k = 0; k < n; k++)
            {
                dist[k] = int.MaxValue;
                precedente[k] = -1;
                fechado[k] = false;
            }
            dist[i] = 0;

            // Algoritmo principal
            while (true)
            {
                int u = -1;
                int menor = int.MaxValue;

                for (int k = 0; k < n; k++)
                {
                    if (!fechado[k] && dist[k] < menor)
                    {
                        menor = dist[k];
                        u = k;
                    }
                }

                if (u == -1 || u == j)
                    break;

                fechado[u] = true;

                for (int v = 0; v < n; v++)
                {
                    Aresta aresta = getAresta(u, v);
                    if (!fechado[v] && aresta != null)
                    {
                        int peso = aresta.getPeso();
                        if (dist[u] + peso < dist[v])
                        {
                            dist[v] = dist[u] + peso;
                            precedente[v] = u;
                        }
                    }
                }
            }

            // Resetar cores
            for (int k = 0; k < n; k++)
            {
                getVertice(k).setCor(Color.Green);
                for (int l = 0; l < n; l++)
                {
                    if (getAresta(k, l) != null)
                        getAresta(k, l).setCor(Color.Black);
                }
            }

            // Se não há caminho até o destino
            if (dist[j] == int.MaxValue)
                return;

            // Pintar caminho de i até j
            int atual = j;
            while (atual != -1 && atual != i)
            {
                int anterior = precedente[atual];
                if (anterior == -1)
                    break;

                getVertice(atual).setCor(Color.Red);
                getAresta(anterior, atual)?.setCor(Color.Red);
                atual = anterior;
            }
            getVertice(i).setCor(Color.Red); // origem também em vermelho
        }

        public void NumeroCromatico()
        {
            int n = getN();
            int[] corVertice = new int[n];
            int maxCor = 0;

            Color[] cores = new Color[]
            {
                    Color.Red, Color.Blue, Color.Green, Color.Yellow,
            };


            for (int i = 0; i < n; i++)
                corVertice[i] = -1;

            // Algoritmo guloso
            corVertice[0] = 0;
            for (int u = 1; u < n; u++)
            {
                bool[] coresUsadas = new bool[n];
                for (int v = 0; v < n; v++)
                {
                    if (getAresta(u, v) != null || getAresta(v, u) != null)
                    {
                        if (corVertice[v] != -1)
                            coresUsadas[corVertice[v]] = true;
                    }
                }

                for (int c = 0; c < n; c++)
                {
                    if (!coresUsadas[c])
                    {
                        corVertice[u] = c;
                        break;
                    }
                }
            }
 
            // Pintar arestas com base na coloração dos vértices
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Aresta a = getAresta(i, j);
                    if (a != null)
                    {
                        if (corVertice[i] != corVertice[j])
                        {
                            // Colore com a cor do vértice de origem
                            a.setCor(cores[corVertice[i] % cores.Length]);
                        }
                        else
                        {
                            // Se os dois têm mesma cor (conflito), destaque!
                            a.setCor(Color.Black);
                        }
                    }
                }
            }

            MessageBox.Show($"Número cromático X(G) = {maxCor + 1}", "Coloração de Vértices");
        }



        public void CompletarGrafo()
        {
            for (int i = 0; i < getN(); i++)
            {
                for (int j = 0; j < getN(); j++)
                {
                    if (i != j && getAresta(i, j) == null)
                    {
                        setAresta(i, j, 1);
                        setAresta(j, i, 1);
                    }
                }
            }
        }

        public bool IsEuleriano()
        {
            for (int i = 0; i < getN(); i++)
            {
                if (grau(i) % 2 != 0) ;
                return false;
            }
            return true;
        }

        public bool IsUnicursal()
        {
            int cont = 0;
            for (int i = 0; i < getN(); i++)
            {
                if (grau(i) % 2 != 0) ;
                cont++;
            }
            if (cont == 2)
                return true;
            return false;
        }

        public void Largura(int v)
        {
            getVertice(0).setCor(Color.Red);
            Fila f = new Fila(matAdj.GetLength(0));
            f.enfileirar(v);   
            visitado[v] = true; 
            while (!f.vazia())
            {
                v = f.desenfileirar(); 
                for (int i = 0; i < matAdj.GetLength(0); i++)
                {  
                    if (matAdj[v, i] != null && !visitado[i])
                    {
                        matAdj[v, i].setCor(Color.Red);
                        visitado[i] = true; 
                        f.enfileirar(i);
                    }
                }
            }
        }


        public String ParesOrdenados()
        {
            string resp = "E = {";
            for (int i = 0; i < getN(); i++){
                for (int j = 0; j < getN(); j++){

                    if (getAresta(i, j) != null)
                    {
                        resp += $"({i} , {j}),";
                    }
                        
                }
            }
            return resp.Substring(0, resp.Length - 1) + "}";
           


            // getAresta(vi, vj) == null => aresta não existe
            // getVertice(v) => object "Vertice"
            //getVertice(0).setCor(Color.Red);
            //getVertice(0).setRotulo("BH");
            //getAresta(0, 1).setPeso(5);

        }

        public void Profundidade(int v)
        {

            visitado[v] = true;
            getVertice(0).setCor(Color.Red);
            for (int i = 0; i < matAdj.GetLength(0); i++)
            {

                if (matAdj[v, i] != null && !visitado[i])
                {
                    matAdj[v, i].setCor(Color.Red);


                    if (matAdj[i, v] != null)
                        matAdj[i, v].setCor(Color.Red);
                    Profundidade(i);
                }
            }

        }
        public void ResetarVisitados()
        {
            visitado = new bool[n];
            for (int i = 0; i < visitado.Length; i++)
            {
                visitado[i] = false;
            }
        }
    }
}
