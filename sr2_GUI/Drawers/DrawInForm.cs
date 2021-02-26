using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace sr2_GUI
{
    class DrawInForm : IDrawing
    {
        int startx=10, starty=10; //поля для запоминания точки старта
        int curX = 10, curY = 10; //текущее положение точки
        int w = 45, h = 45; //длина и высота ячейки

        Brush myBrush = new SolidBrush(Color.Red); //инструменты
        Font MyFont = new Font("Arial", 16);
        StringFormat StrFormat;
        Pen p = new Pen(Color.Black, 6);
        Pen gray = new Pen(Color.Gray, 2);
        Graphics g;

        private Dictionary<Rectangle, string> bufer; //буфер данных
        private List<Point> points;
        private int count;
        private int col_count = 0;


        public DrawInForm(Graphics graph)
        {
            g = graph;
            bufer = new Dictionary<Rectangle, string>();
            points = new List<Point>();
            count = 0;
        }

        public void DrawBorder(IMatrix matr)
        {
            col_count = matr.column_count;
                int dlin = matr.row_count;
                int shir = matr.column_count;
                Point p1 = new Point(startx, starty);
                Point p2 = new Point(startx, (dlin * h) + starty);
                Point p3 = new Point((shir * w) + startx, (dlin * h) + starty);
                Point p4 = new Point((shir * w) + startx, starty);
                points.Add(p1);
                points.Add(p2);
                points.Add(p3);
                points.Add(p4);
            
        }

        public void DrawUnit(IMatrix matr, int x, int y)
        {
            StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            string data_unit = string.Format("{0,1:00}", matr.GetValue(x, y)); ;
            
            Rectangle rect_unit = new Rectangle(curX, curY, w, h);
            curX = curX + w;

            bufer.Add(rect_unit, data_unit);

            count++;
            if (count == col_count)
            {
                curX = startx;
                curY = curY + h;
                count = 0;
            }
        }

        public void Print()
        {
             g.DrawLine(p, points[0], points[1]);
             g.DrawLine(p, points[1], points[2]);
             g.DrawLine(p, points[2], points[3]);
             g.DrawLine(p, points[3], points[0]);
            
            foreach (var item in bufer)
            {
                g.DrawString(item.Value, MyFont, myBrush, item.Key, StrFormat); //отрисовка самих ячеек
            }

            foreach (var item in bufer)
            {
                g.DrawRectangle(gray, item.Key); //рамка для ячейки
            }
            
            g.Dispose();
            bufer.Clear();
            points.Clear();
           
        }

    }
}
