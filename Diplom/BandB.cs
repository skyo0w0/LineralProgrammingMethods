using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Diplom
{
    internal class BandB : Simplex
    {
        List<List<Double>> answers = new List<List<double>>();
        public void SolveBandB(List<Double> function, List<List<String>> lim, bool minmax, List<List<String>> defaultLim)
        {
            double[] answer = SimplexQuiz(function, lim, minmax);
            int xChangedCount = 0;

            if (CheckLimits(lim, answer) && CheckDeci(answer) == true)
            {
                List<List<Double>> newLims = new List<List<Double>>();
                for (int i = 0; i < answer.Length - 1; i++)
                {
                    if (answer[i] != Math.Round(answer[i]))
                    {
                        newLims.Add(new List<double> { Math.Floor(answer[i]), Math.Ceiling(answer[i]), i });
                        xChangedCount++;
                    }
                }
                List<List<String>> newLimits = new List<List<String>>(defaultLim);
                for (int i = 0; i < Math.Pow(2, newLims.Count); i++)
                {
                    if (newLims.Count == lim.Count - defaultLim.Count)
                    {
                        newLimits = new List<List<String>>(lim);
                    }
                    else
                    {
                        newLimits = new List<List<string>>(defaultLim);
                    }
                    double[] combination = new double[newLims.Count];
                    for (int k = 0; k < newLims.Count; k++)
                    {
                        if (((i >> k) & 1) == 1)
                        {
                            combination[k] = newLims[k][1];
                            newLimits.Add(MakeNewLimit(newLims, k, 1, function));
                        }
                        else
                        {
                            combination[k] = newLims[k][0];
                            newLimits.Add(MakeNewLimit(newLims, k, 0, function));
                        }
                    }
                    if (xChangedCount == 1)
                    {
                        for (int j = 0; j < function.Count; j++)
                        {
                            if (j != newLims[0][newLims[0].Count - 1] && function.Count + j < lim.Count)
                            {
                                newLimits.Add(lim[function.Count + j]);
                            }
                        }
                    }
                    SolveBandB(function, newLimits, minmax, defaultLim);
                }
            }
            else if (CheckLimits(lim, answer) && CheckDeci(answer) == false)
            {
                answers.Add(answer.ToList());
            }

        }


        public void FindAnswer()
        {

            double maxValue = 0;
            int index = 0;

            for (int i = 0; i < answers.Count; i++)
            {
                double valueOfAnswer = answers[i][answers[i].Count - 1];

                if (answers[i][answers[i].Count - 1] > maxValue)
                {
                    maxValue = valueOfAnswer;
                    index = i;
                }
            }

            for (int i = 0; i < answers[index].Count - 1; i++)
            {

                Console.WriteLine($"X{i+1} -  {answers[index][i]}");
            }
            Console.WriteLine($"L - {answers[index][answers[index].Count - 1]}");
        }

        static List<string> MakeNewLimit(List<List<Double>> newLims, int place, int placeofX, List<Double> function)
        {
            string[] limit = new string[function.Count + 2];
            for (int i = 0; i < limit.Length; i++)
            {
                if (i == newLims[place][2])
                {
                    limit[i] = "1";
                }

                else if (i == limit.Length - 2)
                {
                    if (placeofX == 0)
                    {
                        limit[i] = "<=";
                    }
                    else
                    {
                        limit[i] = ">=";
                    }

                }
                else if (i == limit.Length - 1)
                {
                    limit[i] = Convert.ToString(newLims[place][placeofX]);
                }
                else
                {
                    limit[i] = "0";
                }
            }
            return limit.ToList();
        }

        static bool CheckLimits(List<List<String>> lim, double[] answer)
        {
            bool flag = true;
            foreach (List<string> limits in lim)
            {
                double summ = 0;
                int countOfNonZero = 0;
                for (int i = 0; i < limits.Count - 2; i++)
                {
                    if (Convert.ToDouble(limits[i]) != 0)
                    {
                        summ += Convert.ToDouble(limits[i]) * answer[i];
                        countOfNonZero++;
                    }

                }
                if (countOfNonZero == 1)
                {
                    switch (limits[limits.Count - 2])
                    {
                        case "<=":
                            if (summ > Convert.ToDouble(limits[limits.Count - 1]))
                            {
                                flag = false;
                            }
                            break;
                        case ">=":
                            if (summ < Convert.ToDouble(limits[limits.Count - 1]))
                            {
                                flag = false;
                            }
                            break;
                    }
                }

            }
            return flag;
        }

        static bool CheckDeci(double[] answer)
        {
            bool flag = false;
            for (int i = 0; i < answer.Length - 1; i++)
            {

                if (answer[i] != Math.Round(answer[i]))
                {

                    flag = true;
                    break;

                }

            }

            return flag;
        }
    }
}
