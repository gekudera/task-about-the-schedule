﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    interface IMatrix
    {
        int row_count { get; }
        int column_count { get; }
        double GetValue(int i, int j);                          //отдает значение матрицы
        void SetValue(double chisl, int i, int j);                 //записывает значение

        void Draw(IDrawing drawer);

        void MarkUnit(int[] vect);

        double this[int rowIndex, int columnIndex] { get; set; }
    }
}
