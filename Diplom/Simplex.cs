using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom
{
    public class Simplex
    {
        public Double[,] lastTable;
        public Double[] SimplexQuiz(List<Double> function, List<List<String>> lim, bool minmax)
        {

            Double[,] table = makeTable(function, lim, minmax);
            PrintTable(table);
            while (!checkOptimal(table, minmax))
            {
                int rCol = resolveCol(table, minmax);
                int rRow = resolveRow(table, rCol);
                table = NewTable(table, rRow, rCol);
                PrintTable(table);
                Console.WriteLine("iteration-------------------");
            }
            double[] answer = CheckTableForX(function.Count, table);
            for(int i = 0; i < answer.Length;i++)
            {
                Console.WriteLine($"X {i} = {answer[i]}");
            }
            lastTable = table;
            return answer;





        }


        public static Double[,] makeTable(List<Double> function, List<List<String>> lim, bool minmax)
        {
            int l = lim.Count;
            int rows = l + 1;
            int cols = function.Count + l + 1;
            Double[,] table = new double[rows, cols];
            int i = 0;
            foreach (List<string> limit in lim)
            {




                if (limit[limit.Count - 2] == ">=")
                {
                    limit[limit.Count - 2] = "<=";
                    for(int j = 0; j < limit.Count; j++)
                    {
                        if (j != limit.Count - 2)
                        {
                            if (!limit[j].Contains('-')) 
                            {
                                limit[j] = "-" + limit[j];
                            }
                        }
                    
                    }
                }




                for (int indexer = 0; indexer < limit.Count - 2; indexer++)
                {

                    table[i, indexer] = Convert.ToDouble(limit[indexer]);

                }
                table[i, function.Count + i] = 1;
                table[i, cols - 1] = Convert.ToDouble(limit[limit.Count - 1]);
                i++;
            }
            for (i = 0; i < cols; i++)
            {
                if (i < function.Count)
                {
                    table[rows - 1, i] = -function[i];
                }
                else
                {
                    table[rows - 1, i] = 0;
                }

            }
            return table;

        }

        public bool checkOptimal(double[,] table, bool minmax)
        {
            bool flag = false;
            if (minmax)
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[table.GetLength(0) - 1, i] < 0)
                    {
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < table.GetLength(1); i++)
                {
                    if (table[table.GetLength(0) - 1, i] > 0)
                    {
                        flag = false;
                        break;
                    }
                    else
                    {
                        flag = true;
                    }

                }
            }
            return flag;

        }

        public int resolveCol(double[,] table, bool minMax)
        {
            int col = 0;
            double minOrMax = table[table.GetLength(0) - 1, 0];
            for (int i = 1; i < table.GetLength(1); i++)
            {
                double nextElement = table[table.GetLength(0) - 1, i];
                if (minMax)
                {
                    if ((minOrMax >= nextElement))
                    {
                        minOrMax = nextElement;
                        col = i;
                    }
                }
                else
                {
                    if ((minOrMax <= nextElement))
                    {
                        minOrMax = nextElement;
                        col = i;
                    }
                }
            }
            return col;
        }

        public int resolveRow(double[,] table, int rCol)
        {
            int row = 0;
            double min = double.MaxValue;
            for (int i = 0; i < table.GetLength(0) - 1; i++)
            {
                double temp = table[i, table.GetLength(1) - 1] / table[i, rCol];
                if (temp >= 0 && temp <= min)
                {
                    min = temp;
                    row = i;
                }

            }
            return row;
        }


        public Double[,] NewTable(Double[,] table, int resolveRow, int resolveCol)
        {
            Double[,] newTable = (Double[,])table.Clone();
            int cols = table.GetLength(1);
            int rows = table.GetLength(0);

            for (int i = 0; i < cols; i++)
            {
                newTable[resolveRow, i] = newTable[resolveRow, i] / table[resolveRow, resolveCol];

            }
            for (int i = 0; i < rows; i++)
            {
                if (i != resolveRow)
                {
                    double mulI = -newTable[i, resolveCol];

                    for (int j = 0; j < cols; j++)
                    {
                        newTable[i, j] += mulI * newTable[resolveRow, j];
                    }
                }
            }
            return newTable;

        }

        public double[] CheckTableForX(int numOfCoef, double[,] table)
        {
            double[] answer = new double[numOfCoef+1];
            int rows = table.GetLength(0);
            int cols = table.GetLength(1);
            for (int i = 0; i < numOfCoef; i++)
            {
                int numberOfNonZero = 0;
                int posOfNonZero = 0;
                for (int j = 0; j < rows; j++)
                {
                    if (table[j, i] != 0)
                    {
                        numberOfNonZero++;
                        posOfNonZero = j;
                    }
                }
                if (numberOfNonZero <= 1)
                {
                    answer[i] = Math.Round(table[posOfNonZero, cols - 1],10);
                }

            }
            answer[answer.Length - 1] = table[rows-1, cols-1];
            return answer;

        }





        public void PrintTable(Double[,] table)
        {
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    Console.Write($"[{table[i, j]}]");
                }
                Console.WriteLine();
            }
        }



    }

}
