using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sr2_GUI
{
    class Destinations
    {
        int size;         //сколько работ и работников
        IMatrix P;     //матрица производительности
		IMatrix S;     //простейшая матрица из 0 и 1
		SimpleVector V;
		SimpleVector U;
		SimpleVector Solution;

		public Destinations(int siz, IMatrix som)
        {
            this.size = siz;
            this.P = som;

			S = new SomeMatrix(size, size);
			for (int i=0; i<size; i++)
            {
				for (int j = 0; j < size; j++)
					S.SetValue(0, i, j);
            }

			V = new SimpleVector(size);
			U = new SimpleVector(size);
			for (int i = 0; i < size; i++)
			{
				U[i] = 0;
			}
			Solution = new SimpleVector(size);
		}

        public IMatrix DoBoolMatrix()
        {
			double max;
			for (int i = 0; i < size; i++)
			{
				max = 0;
				int index=0;
				for (int j = 0; j < size; j++)
				{
					if (P.GetValue(i,j) >= max)
					{
						max = P.GetValue(i,j);
						index = j;
					}
				}
				S.SetValue(1, i, index);
				V[i] = max;
				for (int j = 0; j < size; j++)
				{
					if ((P.GetValue(i,j) == max) && (j != index))
					{
						S.SetValue(1, i, j);
					}
				}
			}
			return S;
		}

		private IMatrix BoolMatrix()
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (P.GetValue(i, j) == (V[i] + U[j]))
						S.SetValue(1, i, j);
					else S.SetValue(0, i, j);
				}
			}
			return S;
		}


		private void poisk()
		{
			for (int i = 0; i < size; i++)
			{
				Solution[i] = -1;
			}
			int a = 0, b = 0, bezrab = 0, first = 0, noone, count = 1;
			int flag1 = 0;//флаг чтобы он находил только 1 единичку в строчее
			int flag3 = 0;
			int flag2 = 0;
			int flag4 = 0;

			int[] remember = new int[size];
			int[] works = new int[size]; //для записи выбранных работ(чтобы их не выбирали 2 раз)
			for (int i = 0; i < size; i++)
				works[i] = -1;
			int[,] S1 = new int[size,size]; //матрица 5х5 для записи пройденных цепочек

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
					S1[i,j] = 0;
			}

			Stack<int> steck = new Stack<int>();


			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{

					if ((S.GetValue(i,j) == 1) && (a == 0))
					{
						for (int k = 0; k < size; k++)
						{
							if (Solution[k] == j) //проверка на то, что этот столбец еще не взят
							{
								b++;
							}
						}
						if (b == 0)
						{
							Solution[i] = j;
							a = 1;
							Console.Write( (i + 1) + " работник взял " + (j + 1) + " работу\n");
						}
					}
					b = 0;
				}
				a = 0;
				if (Solution[i] == -1)
				{
					Console.Write((i + 1) + " работник не нашел работу\n");
					bezrab = i;
					first = i;
					V[bezrab] = V[bezrab] - 1;
					steck.Push(bezrab);
				}
			}
			SimpleVector Solution1 = new SimpleVector(size);
			for (int i = 0; i < size; i++)
				Solution1[i] = Solution[i]; //создаем копию



			//ПЕРЕНАЗНАЧЕНИЕ
			Console.Write("\nЦЕПОЧКА ПЕРЕНАЗНАЧЕНИЙ: \n");
			count = 1;
			int last_number = -1;
			while ((steck.Count != 0) && (flag2 == 0))
			{
				noone = 0;
				flag1 = 0;
				flag2 = 1;
				flag4 = 0;
				for (int j = 0; j < size; j++)
				{
					flag3 = 0;
					for (int m = 0; m < size; m++)
					{
						if (works[m] == j)
							flag3 = 1;
					}
					if ((S.GetValue(steck.Peek(),j) == 1) && (flag1 == 0) && (flag3 == 0) && ( S1[steck.Peek(),j] != 1 ))
					{
						flag1 = 1;
						Solution1[steck.Peek()] = j;
						if (count>0 && count< size)
						works[count] = j;
						U[j] = U[j] + 1;
						b = j;
						count++;
						Console.Write( "  J" + (steck.Peek() + 1) + " на R" + (j + 1));
						S1[steck.Peek(),j] = 1;
						noone = 2;
						for (int i = 0; i < size; i++)
						{
							if ((Solution[i] == Solution1[steck.Peek()]) && (i != steck.Peek()))
							{
								steck.Push(i);
								V[steck.Peek()] = V[steck.Peek()] - 1;
								Solution1[i] = -2;
								Console.Write(" -> Теперь J" + (steck.Peek() + 1) + " ищет работу!! ");
								if (steck.Peek() == first)
								{
									Console.Write("Цепочка замкнулась, шаг назад \n \n");
									steck.Pop();  // удаляем верхний элемент
									break;
								}
							}
						}
					}

				}
				if (noone == 0)
				{
					a = 0;
					for (int i = 0; i < size; i++)
					{
						if ((S.GetValue(i,b) == 0) && (i != steck.Peek()))
						{
							a++;
						}
					}
					if (a == 4)
					{
						flag4 = 1;
						Console.Write(" Цепь замкнулась для " + (steck.Peek() + 1) + "\n");
						steck.Pop();
					}
					if (flag4 == 0)
					{
						if (steck.Count != 0)
						{
							Console.Write( "J" + (steck.Peek() + 1) + " больше ничего не может. Переходим на шаг назад\n"); // выводим верхний элемент
							if (count>0 && count< size)
							works[count] = -1;
							count--;
							last_number = steck.Peek();
							flag2 = 0;
							Solution1[steck.Peek()] = Solution[steck.Peek()];
							for (int j = 0; j < size; j++)
							{
								S1[steck.Peek(), j] = 0;
							}
							steck.Pop();  // удаляем верхний элемент
						}
						else { break; }
					}
				}

				for (int i = 0; i < size; i++)
				{
					if (Solution1[i] < 0)
						flag2 = 0;
				}

			}

			for (int i = 0; i < size; i++)
				Solution[i] = Solution1[i];

			Console.Write( "\nНАЗНАЧЕНИЕ: \n" );
			if (flag2 == 1)
			{
				for (int i = 0; i < size; i++)
				{
					Console.Write( (i + 1) + "работник на " + (Solution[i] + 1) + "работу\n");
				}
			}
			else
			{
				Console.Write( "Невозможно распределить работников по работам!\n");
			}
		}


		public SimpleVector FindSolution()
        {
			poisk();
			bool fl = false;

			while (fl == false)
			{
				fl = true;
				S = BoolMatrix();
				DrawInConsole cons = new DrawInConsole();
				S.Draw(cons);
				poisk();
				for (int i = 0; i < size; i++)
				{
					if (Solution[i] < 0)
						fl = false;
				}
			}
			return Solution;
		}


	}

}
