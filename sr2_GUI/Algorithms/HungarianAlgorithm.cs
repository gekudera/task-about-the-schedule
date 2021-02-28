using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    public sealed class HungarianAlgorithm     
    {
        private List<int> u, v, p, way;
        private int n;
        private static int INF = 10000;

        //создаем список заданного размера для простоты
        public static List<int> clist(int size, int init = 0)
        {
            List<int> v = new List<int>();
            for (int i = 0; i < size; i++)
            {
                v.Add(init);
            }
            return v;
        }

        //конструктор
        public HungarianAlgorithm(int n)
        {
            this.n = n;
            u = clist(n + 1);
            v = clist(n + 1);
            p = clist(n + 1);
            way = clist(n + 1);
        }

        //решение
        public int[] Solve(List<List<int>> a)
        {
            int price = 0;

            for (int i = 0; i < n; i++)
            {
                p[0] = i;
                int j0 = 0;
                List<int> minv = clist(n + 1, INF);
                List<bool> used = new List<bool>();
                for (int z = 0; z < n + 1; z++)
                {
                    used.Add(false);
                }
                do
                {
                    used[j0] = true;
                    int i0 = p[j0], delta = INF, j1 = -1;
                    for (int j = 0; j < n; j++)
                        if (!used[j])
                        {
                            int cur = a[i0][j] - u[i0] - v[j];
                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = j0;
                            }
                            if (minv[j] < delta)
                            {
                                delta = minv[j];
                                j1 = j;
                            }
                        }
                    for (int j = 0; j <= n; ++j)
                        if (used[j])
                        {
                            u[p[j]] += delta;
                            v[j] -= delta;
                        }
                        else
                            minv[j] -= delta;
                    j0 = j1;
                } while (p[j0] != 0);
                do
                {
                    int j1 = way[j0];
                    p[j0] = p[j1];
                    j0 = j1;
                } while (j0 != 0);
            }

            int[] solve = new int[n];
            for (int j = 0; j < n; j++)
            {
                if (p[j+1] == 0)
                {
                    solve[j] = n;
                }
                else
                {
                    solve[j] = p[j+1];
                }
            }

            price = -v[0]; //минимум
            Console.WriteLine("price = " + price);

            return solve;
        }
    }
}
