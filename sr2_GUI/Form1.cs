
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

        //в будущем объединить в структуру
        ScheduleTask schedule;
        int size = 0;

        DrawInConsole cons;
        DrawInForm form;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            size = Convert.ToInt32(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b = picBox.CreateGraphics();
            b.Clear(BackColor);

            cons = new DrawInConsole();
            form = new DrawInForm(b);

            List<List<int>> a = new List<List<int>>();
            for (int i = 0; i < size; i++)
            {
                a.Add(new List<int>());
                for (int j = 0; j < size; j++)
                {
                    a[i].Add(Convert.ToInt32(current_matrix.GetValue(i, j)));
                }
            }

            HungarianAlgorithm algorithm = new HungarianAlgorithm(size);

            int[] vect = new int[size];
            vect = algorithm.Solve(a);

            current_matrix = new SomeMatrix(1, size);

            for (int i = 0; i < size; i++)
                current_matrix.SetValue(vect[i], 0, i);

            current_matrix.Draw(form);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (size != 0)
            {
                current_matrix = new SomeMatrix(size, size);
                int sdvig = 0;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (j + sdvig < size)
                            current_matrix.SetValue(j + sdvig, i, j);
                        else
                            current_matrix.SetValue(j + sdvig - size, i, j);
                    }
                    sdvig++;
                }
                Console.WriteLine("Матрица очередей размером " + size);
                cons = new DrawInConsole();
                current_matrix.Draw(cons);
            }
            else
            {
                MessageBox.Show("Размер не указан! Ввведите цифру в текстбокс или загрузите штрафы, директивные сроки и сроки выполнения из файла!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            b = picBox.CreateGraphics();
            b.Clear(BackColor);

            cons = new DrawInConsole();
            form = new DrawInForm(b); 

            int[,] que = new int[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    que[i, j] = (int)current_matrix.GetValue(i, j);

            current_matrix = schedule.MakeStrafMatrix(que);

            current_matrix.Draw(cons);
            current_matrix.Draw(form);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //штрафы
            StreamReader file = new StreamReader(@"C:\Users\sekosh\Desktop\ДИПЛОМ\straf.txt");
            string s = file.ReadToEnd();
            file.Close();
            string[] str = s.Split('\n');
            string[] stolb = str[0].Split(' ');
            IVector straf = new SimpleVector(str.Length);
            IVector dir_srok = new SimpleVector(str.Length);
            IVector srok_vip = new SimpleVector(str.Length);
            int t = 0;

            for (int i = 0; i < str.Length; i++)
            {
                stolb = str[i].Split(' ');
                for (int j = 0; j < stolb.Length;)
                {
                    t = Convert.ToInt32(stolb[j]);
                    straf.SetValue(i, t);

                    j++;
                    t = Convert.ToInt32(stolb[j]);
                    dir_srok.SetValue(i, t);

                    j++;
                    t = Convert.ToInt32(stolb[j]);
                    srok_vip.SetValue(i, t);

                    j++;
                }
            }

            b = pictureBox1.CreateGraphics();
            b.Clear(BackColor);
            form = new DrawInForm(b);
            straf.Draw(form);

            b = pictureBox2.CreateGraphics();
            b.Clear(BackColor);
            form = new DrawInForm(b);
            dir_srok.Draw(form);

            b = pictureBox3.CreateGraphics();
            b.Clear(BackColor);
            form = new DrawInForm(b);
            srok_vip.Draw(form);

            size = str.Length;
            schedule = new ScheduleTask(straf, dir_srok, srok_vip);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                IVector straf = new SimpleVector(size);
                IVector dir_srok = new SimpleVector(size);
                IVector srok_vip = new SimpleVector(size);

                Random rand = new Random();
                for (int i=0; i< size; i++)
                {
                    straf.SetValue(i, rand.Next(0, 20));
                    dir_srok.SetValue(i, rand.Next(0, 20));
                    srok_vip.SetValue(i, rand.Next(0, 20));
                }

                b = pictureBox1.CreateGraphics();
                b.Clear(BackColor);
                form = new DrawInForm(b);
                straf.Draw(form);

                b = pictureBox2.CreateGraphics();
                b.Clear(BackColor);
                form = new DrawInForm(b);
                dir_srok.Draw(form);

                b = pictureBox3.CreateGraphics();
                b.Clear(BackColor);
                form = new DrawInForm(b);
                srok_vip.Draw(form);

                schedule = new ScheduleTask(straf, dir_srok, srok_vip);

            }
            else
            {
                MessageBox.Show("Введите в текстбокс количество работ!");
            }
        }
    }
}


