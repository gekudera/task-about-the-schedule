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
            int N = a.GetLength(0);
            if (N == 0)
                return new int[0];

            int[] lx = new int[N], ly = new int[N];   // функция маркировки вершин в первом и втором разбиениях
            int[] mx = new int[N], my = new int[N];   // mx[u]=v, my[v]=u <==> u and v are currently matched;  -1 значения - несоответствие
            int[] px = new int[N], py = new int[N];   // predecessor arrays.  used in DFS to reconstruct paths.
            int[] stack = new int[N];

            // invariant: lx[u] + ly[v] >= a[u, v]
            // (implies that any perfect matching in subgraph containing only
            // edges u, v for which lx[u]+ly[v]=a[u,v] is the optimal matching.)

            // compute initial labelling function:  lx[i] = max_j(a[i, j]), ly[j] = 0;
            for (int i = 0; i < N; i++)
            {
                lx[i] = a[i, 0];
                for (int j = 0; j < N; j++)
                    if (a[i, j] > lx[i]) lx[i] = a[i, j];
                ly[i] = 0;

                mx[i] = my[i] = -1;
            }

            for (int size = 0; size < N;)
            {
                int s;
                for (s = 0; mx[s] != -1; s++) ;

                // s is an unmatched vertex in the first partition.
                // At the current iteration we will either find an augmenting path
                // starting at s, or we'll extend the equality subgraph so that
                // such a path will exist at the next iteration.

                for (int i = 0; i < N; i++)
                    px[i] = py[i] = -1;
                px[s] = s;

                // DFS
                int t = -1;
                stack[0] = s;
                for (int top = 1; top > 0;)
                {
                    int u = stack[--top];
                    for (int v = 0; v < N; v++)
                    {
                        if (lx[u] + ly[v] == a[u, v] && py[v] == -1)
                        {
                            if (my[v] == -1)
                            {
                                // we've found an augmenting path
                                t = v;
                                py[t] = u;
                                top = 0;
                                break;
                            }

                            py[v] = u;
                            px[my[v]] = v;
                            stack[top++] = my[v];
                        }
                    }
                }

                if (t != -1)
                {
                    // augment along the found path
                    while (true)
                    {
                        int u = py[t];
                        mx[u] = t;
                        my[t] = u;
                        if (u == s) break;
                        t = px[u];
                    }
                    ++size;
                }
                else
                {
                    // No augmenting path exists from s in the current equality graph,
                    // Modify labelling function a bit...

                    int delta = int.MaxValue;
                    for (int u = 0; u < N; u++)
                    {
                        if (px[u] == -1) continue;
                        for (int v = 0; v < N; v++)
                        {
                            if (py[v] != -1) continue;
                            int z = lx[u] + ly[v] - a[u, v];
                            if (z < delta)
                                delta = z;
                        }
                    }

                    for (int i = 0; i < N; i++)
                    {
                        if (px[i] != -1) lx[i] -= delta;
                        if (py[i] != -1) ly[i] += delta;
                    }
                }
            }

            // Verify optimality
            bool correct = true;
            for (int u = 0; u < N; u++)
            {
                for (int v = 0; v < N; v++)
                {
                    correct &= (lx[u] + ly[v] >= a[u, v]);
                    if (mx[u] == v)
                        correct &= (lx[u] + ly[v] == a[u, v]);

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
