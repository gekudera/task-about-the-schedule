﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace sr2_GUI
{
    class DrawInConsole : IDrawing
    {

        private List<string> bufer;
        private string buf_el;
        private string border;
        private int count_col;
        private double[] max;

        public DrawInConsole()
        {
            bufer = new List<string>();
            buf_el = "";
            border = "";
        }

        public void DrawBorder(IMatrix matr)
        {
            count_col = matr.column_count;
                int border_len = matr.column_count * 8;
                while (border_len-- != 0)
                {
                    border += "-";
                }
                border += "\n";
             

            max = new double[matr.column_count];
            for (int x = 0; x < matr.column_count; x++)
            {
                for (int j = 0; j < matr.row_count; j++)
                {
                    if (matr.GetValue(x, j) > max[x])
                    {
                        max[x] = matr.GetValue(x, j);
                    }
                }
            }
            
        }


        public void DrawUnit(IMatrix matr, bool ismark, int x, int y)
        {
            buf_el = "";
            buf_el += String.Format("{0, -4:00}", matr.GetValue(x, y));

            bufer.Add(buf_el);
            
            if (ismark == false)
            bufer[bufer.LastIndexOf(buf_el)] = String.Format("| {0} |", buf_el);
            else
                bufer[bufer.LastIndexOf(buf_el)] = String.Format("|[{0}]|", buf_el);

        }

        public void Print()
        {
            int count = 0;
            bufer.Insert(0, border); //если DrawBorder не вызывалось, то рамка не отрисуется, ведь border  -пустая
            bufer.Add(border);
            for (int i=0; i<bufer.Count; i++)
            {
                Console.Write(bufer[i]);
                if ((i >= count_col) && (i % count_col == 0))
                {
                    Console.Write(" -- " + max[count]);
                    count++;
                    Console.WriteLine();
                }
            }

            bufer.Clear();
            buf_el = "";
            border = "";
        }
    }

}
