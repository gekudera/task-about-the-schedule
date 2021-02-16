using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    class InitiatorMatrix
    {
        public static void RandomMatr(IMatrix sm, int not_nul, int max)
        {
            Random rnd = new Random();
            int until = not_nul;

            for (int i = 0; i < sm.row_count; i++)
            {
                for (int j = 0; j < sm.column_count; j++)
                {
                    if (until != 0)
                    {
                        sm.SetValue(rnd.Next(1, 100), i, j);
                        until--;
                    }
                    else
                        sm.SetValue(0, i, j);
                }
            }

            //тасование
            int k, m;
            double temp;
            for (int i = sm.row_count - 1; i >= 1; i--)
            {
                for (int j = sm.column_count - 1; j >= 1; j--)
                {
                    k = rnd.Next(i + 1);
                    m = rnd.Next(j + 1);

                    temp = sm.GetValue(k, m);
                    sm.SetValue(sm.GetValue(i, j), k, m);
                    sm.SetValue(temp, i, j);
                }
            }
        }
    }
}
