using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    class Destinations
    {
        public int[] KuhnMunkres(int[,] a)
        {
            int N = a.GetLength(0);   //размер
            if (N == 0)
                return new int[0];

            int[] V = new int[N], U = new int[N];     // для записи промежуточных вычислений U изначально равна 0, V - максимальным числам
            int[] mx = new int[N], my = new int[N];   // mx[u]=v, my[v]=u <==> u and v are currently matched;  -1 значения - несоответствие
            int[] px = new int[N], py = new int[N];   //px - для записи какую V на сколько уменьшать, py - для записи какую U на сколько увеличивать
            int[] stack = new int[N];                 //для записи цепочки переназначений

            // Заполняем U и V
            for (int i = 0; i < N; i++)
            {
                V[i] = a[i, 0];
                for (int j = 0; j < N; j++)
                    if (a[i, j] > V[i])
                    {
                        V[i] = a[i, j];   //находим максимальные элементы в каждой строчке для массива V
                    }
                U[i] = 0;
                mx[i] = my[i] = -1;
            }

            for (int size = 0; size < N;)  //пока не заполнили все решение
            {
                int s;
                for (s = 0; mx[s] != -1; s++);
                Console.Write("\n\n s = " + s);

                for (int i = 0; i < N; i++)
                    px[i] = py[i] = -1;
                px[s] = s;

                int t = -1;
                stack[0] = s;
                for (int top = 1; top > 0;)
                {
                    int u = stack[--top];
                    for (int v = 0; v < N; v++)
                    {
                        if ((V[u] + U[v] == a[u, v]) && (py[v] == -1)) //пытаемся назначить работника на работу, при этом проверяем что работа не занята
                        {
                            if (my[v] == -1) 
                            {
                                Console.Write("\nУсловие (V[u] + U[v] == a[u, v]) && (py[v] == -1) выполняется для номера "+ v);
                                t = v;
                                py[t] = u;
                                top = 0;
                                break;
                            }
 
                            py[v] = u;
                            px[my[v]] = v;
                            stack[top++] = my[v];
                            Console.Write("\nstack: ");
                            for (int g=0; g<stack.Length; g++)
                                Console.Write(stack[g]+ " ");
                        }
                    }
                }

                if (t != -1)
                {
                    Console.Write("\nНомер назначен");
                    while (true)
                    {
                        int u = py[t];
                        mx[u] = t;
                        my[t] = u;        //записываем в my значение, чтобы больше ее не выбрали
                        if (u == s) break;
                        t = px[u];
                    }
                    ++size;

                    Console.Write("  mx  = ");
                    for (int k = 0; k < N; k++)
                        Console.Write(mx[k] + " ");
                }
                else
                {
                    //не существует пути и цепочки переназначений
                    Console.Write("\nЦепочка не найдена");
                    int delta = int.MaxValue;
                    for (int u = 0; u < N; u++)
                    {
                        if (px[u] == -1) continue;
                        for (int v = 0; v < N; v++)
                        {
                            if (py[v] != -1) continue;
                            int z = V[u] + U[v] - a[u, v];
                            if (z < delta)
                                delta = z;
                        }
                    }

                    for (int i = 0; i < N; i++)
                    {
                        if (px[i] != -1) V[i] -= delta;
                        if (py[i] != -1) U[i] += delta;
                    }

                    Console.Write("\n-----------------------------------\nV: ");
                    for (int k=0; k<N; k++)
                        Console.Write(V[k] + " ");
                    Console.Write("U: ");
                    for (int k = 0; k < N; k++)
                        Console.Write(U[k] + " ");


                }
            }

            //Проверка оптимальности
            bool correct = true;
            for (int u = 0; u < N; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    correct &= (V[u] + U[v] >= a[u, v]);
                    if (mx[u] == v)
                        correct &= (V[u] + U[v] == a[u, v]);

                    if (!correct)
                    {
                        throw new Exception(
                            "*** Internal error: optimality conditions are not satisfied ***\n" +
                            "Most probably an overflow occurred");
                    }
                }
            }

            return mx;
        }

	}

}
