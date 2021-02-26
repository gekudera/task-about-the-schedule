using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    class ScheduleTask
    {
        IVector straf;
        IVector dir_srok;
        IVector srok_vip;

        public int size { get; set; }

        public ScheduleTask(IVector str, IVector dir_sro, IVector srok_vi)
        {
            this.straf = str;
            this.dir_srok = dir_sro;
            this.srok_vip = srok_vi;

            size = str.Size;
        }


        public IMatrix MakeStrafMatrix(int[,] queue)
        {
            IMatrix straf_matr = new SomeMatrix(size,size);
            int time;                                //для сравнения прошедшего времени с директивными сроками.

            for (int i=0; i<size; i++)
            {
                time = 0;
                for (int j=0; j<size; j++)
                {
                    time += (int)srok_vip[queue[i,j]];
                    Console.Write(" #  time =" + time);
                    if (time >= dir_srok[queue[i,j]])
                    {
                        straf_matr[queue[i,j],j] = (time - dir_srok[queue[i, j]]) * straf[queue[i, j]];
                        Console.Write(", straf = (" + time + "-" + dir_srok[queue[i, j]] + ")*" + straf[queue[i, j]]);
                    }
                }
                Console.WriteLine();
            }

            return straf_matr;
        }


    }
}
