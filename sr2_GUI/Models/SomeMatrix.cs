using System.Drawing;

namespace sr2_GUI
{
    class SomeMatrix : IMatrix
    {
        private int row, col;
        private IVector[] matr;

        public SomeMatrix(int r, int c)
        {
            matr = new IVector[r];
            this.row = r;
            this.col = c;
            for (int i = 0; i < r; i++)
            {
                matr[i] = new SimpleVector(c);
            }
        }


        public int row_count { get { return row; } }
        public int column_count { get { return col; } }

        public void SetValue(double chisl, int i, int j)
        {
            matr[i].SetValue(j, chisl);
        }

        public double GetValue(int i, int j)
        {
            if ((i < row) && (j < col))
            { 
                return (matr[i].GetValue(j));
            }
            else
            {
                return 0;
            }
        }

        public void Draw(IDrawing drawer)
        {             
            drawer.DrawBorder(this);

            for (int i = 0; i < row_count; i++)
            {
                for (int j = 0; j < column_count; j++)
                {
                    drawer.DrawUnit(this, i, j);
                }
            }
            drawer.Print();

        }

    }
}
