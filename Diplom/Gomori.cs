using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    internal class Gomori : Simplex
    {
        List<List<Double>> answers = new List<List<double>>();

        public void SolveGomori(List<Double> function, List<List<String>> lim, bool minmax, List<List<String>> defaultLim)
        {
            double[] answer = SimplexQuiz(function, lim, minmax);
            double[,] table = lastTable;
            for (int i = 0; i < table.GetLength(1); i++)
            {
                table[table.GetLength(0) - 1, i] = table[table.GetLength(0) - 1, i] * -1;
            }
            while (CheckDeci(answer) == true)
            {
                table = MakeGomoriTable(lastTable);

                int rCol = ResolveCol(table);
                int rRow = table.GetLength(0) - 2;
                table = NewTable(table, rRow, rCol);
                answer = CheckTableForX(function.Count, table);
                lastTable = table;
            }
            for (int i = 0; i < answer.Length-1; i++)
            {
                Console.WriteLine($"X-{i+1} = {answer[i]}");
            }
            Console.WriteLine($"F = {-Math.Round(answer[answer.Length - 1])}");
        }
        public double[,] MakeGomoriTable(double[,] table)
        {
            double[,] newTable = new double[table.GetLength(0) + 1, table.GetLength(1) + 1];

            for (int i = 0; i < table.GetLength(0) - 1; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    newTable[i, j] = table[i, j];
                }
            }
            int newLimRow = FindNewLimitRow(table);
            for (int i = 0; i < table.GetLength(1); i++)
            {
                double deci = table[newLimRow, i] - Math.Floor(table[newLimRow, i]);
                newTable[newTable.GetLength(0) - 2, i] = deci * -1;
            }
            for (int i = 0; i < table.GetLength(1); i++)
            {
                newTable[newTable.GetLength(0) - 1, i] = table[table.GetLength(0) - 1, i];
            }
            for (int i = 0; i < newTable.GetLength(0); i++)
            {
                newTable[i, newTable.GetLength(1) - 1] = newTable[i, newTable.GetLength(1) - 2];
                newTable[i, newTable.GetLength(1) - 2] = 0;
            }
            newTable[newTable.GetLength(0) - 2, newTable.GetLength(1) - 2] = 1;




            return newTable;
        }

        public int ResolveCol(double[,] table)
        {
            int rCol = 0;
            double min = Double.MaxValue;
            int rRow = table.GetLength(0) - 2;
            for (int i = 0; i < table.GetLength(1) - 2; i++)
            {
                double f = table[table.GetLength(0) - 1, i];
                double x = table[rRow, i];

                if (x != 0)
                {
                    if (f / x < min)
                    {
                        min = f / x;
                        rCol = i;
                    }


                }
            }

            return rCol;
        }

        public int FindNewLimitRow(double[,] table)
        {
            int newLimRow = 0;
            double maxDeci = Double.MinValue;
            for (int i = 0; i < table.GetLength(0) - 1; i++)
            {
                double number = table[i, table.GetLength(1) - 1];
                if (Math.Round(number - Math.Floor(number),10) > maxDeci)
                {
                    maxDeci = number - Math.Floor(number);
                    newLimRow = i;

                }
            }

            return newLimRow;



        }


        static bool CheckDeci(double[] answer)
        {
            bool flag = false;
            for (int i = 0; i < answer.Length - 1; i++)
            {

                if (Math.Round(answer[i],10) != Math.Round(answer[i]))
                {

                    flag = true;
                    break;

                }

            }
            return flag;
        }
    }

}
