
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace sr2_GUI
{
    public partial class Form1 : Form
    {
        Graphics b;
        IMatrix current_matrix;
        IMatrix straf;
        IMatrix dir_srok;
        DrawInConsole cons;
        DrawInForm form;
        int size;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                size = Convert.ToInt32(textBox1.Text);
                Console.Clear();
                b = picBox.CreateGraphics();

                b.Clear(BackColor);

                cons = new DrawInConsole();
                form = new DrawInForm(b);

                current_matrix = new SomeMatrix(size, size);
                InitiatorMatrix.RandomMatr(current_matrix, size*size, 100);

                current_matrix.Draw(cons);
                current_matrix.Draw(form);
            }
            else
            {

                MessageBox.Show($"Введите размер в текстовом поле!");

            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                b = picBox.CreateGraphics();
                b.Clear(BackColor);

                cons = new DrawInConsole();
                form = new DrawInForm(b);

                StreamReader file = new StreamReader(@"C:\Users\sekosh\Desktop\ДИПЛОМ\firstmatrix.txt");
                string s = file.ReadToEnd();
                file.Close();
                string[] str = s.Split('\n');
                string[] stolb = str[0].Split(' ');
                size = str.Length;
                current_matrix = new SomeMatrix(size, size);
                int t = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    stolb = str[i].Split(' ');
                    for (int j = 0; j < stolb.Length; j++)
                    {
                        t = Convert.ToInt32(stolb[j]);
                        current_matrix.SetValue(t, i, j);
                    }
                }
                current_matrix.Draw(form);
                current_matrix.Draw(cons);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b = picBox.CreateGraphics();
            b.Clear(BackColor);

            cons = new DrawInConsole();
            form = new DrawInForm(b);

            int[,] a = new int[size, size];
            for (int i=0; i<size; i++)
            {
                for (int j=0; j<size; j++)
                {
                    a[i, j] = Convert.ToInt32(current_matrix.GetValue(i, j));
                }
            }

            Destinations destination = new Destinations();

            int[] vect = new int[size];
            vect = destination.KuhnMunkres(a);

            current_matrix = new SomeMatrix(1, size);

            for (int i = 0; i < size; i++)
                current_matrix.SetValue(vect[i], 0, i);

            current_matrix.Draw(form);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                //штрафы
                b = pictureBox1.CreateGraphics();
                b.Clear(BackColor);
                form = new DrawInForm(b);
                StreamReader file = new StreamReader(@"C:\Users\sekosh\Desktop\ДИПЛОМ\straf.txt");
                string s = file.ReadToEnd();
                file.Close();
                string[] str = s.Split('\n');
                string[] stolb = str[0].Split(' ');
                straf = new SomeMatrix(str.Length, stolb.Length);
                int t = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    stolb = str[i].Split(' ');
                    for (int j = 0; j < stolb.Length; j++)
                    {
                        t = Convert.ToInt32(stolb[j]);
                        straf.SetValue(t, i, j);
                    }
                }
                straf.Draw(form);


                //директивные сроки
                b = pictureBox2.CreateGraphics();
                b.Clear(BackColor);
                form = new DrawInForm(b);
                StreamReader file1 = new StreamReader(@"C:\Users\sekosh\Desktop\ДИПЛОМ\dir_srok.txt");
                string s1 = file1.ReadToEnd();
                file1.Close();
                string[] str1 = s1.Split('\n');
                string[] stolb1 = str1[0].Split(' ');
                dir_srok = new SomeMatrix(str1.Length, stolb1.Length);
                for (int i = 0; i < str1.Length; i++)
                {
                    stolb1 = str1[i].Split(' ');
                    for (int j = 0; j < stolb1.Length; j++)
                    {
                        t = Convert.ToInt32(stolb1[j]);
                        dir_srok.SetValue(t, i, j);
                    }
                }
                dir_srok.Draw(form);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((current_matrix.row_count != straf.row_count)||(current_matrix.row_count != dir_srok.row_count))
            {
                MessageBox.Show($"Несовпадение размеров матриц, введите другие данные!");
            }
        }
    }
}


