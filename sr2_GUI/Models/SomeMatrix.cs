using System.Drawing;

namespace sr2_GUI
{
    class SomeMatrix : IMatrix
    {
        private int row, col;
        private IVector[] matr;
        private int[] marked_unit;

        public SomeMatrix(int r, int c)
        {
            matr = new IVector[r];
            this.row = r;
            this.col = c;
            for (int i = 0; i < r; i++)
            {
                matr[i] = new SimpleVector(c);
            }
            marked_unit = new int[r];
            for (int j = 0; j < r; j++)
                marked_unit[j] = -1;
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
                int marked_un = marked_unit[i];
                for (int j = 0; j < column_count; j++)
                {
                    if (j == marked_un)
                        drawer.DrawUnit(this, true, i, j);
                    else
                        drawer.DrawUnit(this, false, i, j);
                }
            }
            drawer.Print();

        }

        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                if (rowIndex >= row_count || columnIndex >= column_count) return 0;
                return matr[rowIndex][columnIndex];
            }
            set
            {
                if (!(rowIndex >= row_count || columnIndex >= column_count))
                    matr[rowIndex][columnIndex] = value;
            }
        }

        public void MarkUnit(int[] vector)
        {
                for (int i = 0; i < vector.Length; i++)
                { 
                    marked_unit[i] = vector[i];
                }
        }
    }
}
