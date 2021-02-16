using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    class SimpleVector: IVector
    {
        private List<double> val;
        private int size;

        public int Size { get { return size; } }

        public SimpleVector(int razm)
        {
            this.val = new List<double>(razm);
            size = razm;
            for (int i = 0; i < size; i++)
            {
                val.Add(0);
            }
        }
        public List<double> GetAllValues()
        {
            return val;
        }

        public double GetValue(int i)
        {
            return val[i];
        }

        public void AddValue(double chisl)
        {
            val.Add(chisl);
            size++;
        }

        public void SetValue(int h, double chislo)
        {
            val[h] = chislo;
        }

        public double this[int index]
        {
            get { return val[index]; }
            set { val[index] = value; }
        }
    }
}
