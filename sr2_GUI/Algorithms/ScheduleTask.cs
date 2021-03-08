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
        IMatrix straf_matr;

        public int size { get; set; }

        public ScheduleTask(IVector str, IVector dir_sro, IVector srok_vi)
        {
            this.straf = str;
            this.dir_srok = dir_sro;
            this.srok_vip = srok_vi;
            size = str.Size;
            straf_matr = new SomeMatrix(size, size);
        }


        public IMatrix MakeStrafMatrix(int[,] queue)
        {
            int time;                                //для сравнения прошедшего времени с директивными сроками.
            int size1 = queue.Length / size;

            for (int i=0; i<size1; i++)
            {
                time = 0;
                for (int j=0; j<size; j++)
                {
                    time += (int)srok_vip[queue[i,j]];
                    Console.Write(" #  time =" + time);
                    if (time >= dir_srok[queue[i,j]])
                    {
                        //какая по порядку выпб номер заявки
                        straf_matr[j, queue[i, j]] = (time - dir_srok[queue[i, j]]) * straf[queue[i, j]];       
                        Console.Write(", straf(" + j + ";" + queue[i, j] + ")" + "= (" + time + "-" + dir_srok[queue[i, j]] + ")*" + straf[queue[i, j]] + " = " + straf_matr[j, queue[i, j]]);
                    }
                    else
                    {
                        straf_matr.SetValue(0, j, queue[i, j]);
                    }
                    Console.WriteLine();
                }
            }

            return straf_matr;
        }


    }
}
