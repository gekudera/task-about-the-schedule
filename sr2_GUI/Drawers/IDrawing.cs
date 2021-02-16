using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    interface IDrawing
    {
        void DrawBorder(IMatrix matr); //для отрисовки рамки
        void DrawUnit(IMatrix matr, int x, int y); //для отрисовки одной ячейки
        void Print();

    }
}
