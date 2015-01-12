using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public sealed class ChiSquareCriterion: Criterion
    {
        private int k = 15;
        /// <summary>
        /// Подсчет критического значения по критерию Пирсона
        /// </summary>
        /// <param name="r">Количество степеней свободы</param>
        /// <param name="alpha">Вероятность</param>
        /// <returns>критическое значение по критерию Пирсона</returns>
        public double Critical(int r, double alpha)
        {
            double d = 0;
            if ((0.5 <= alpha) && (alpha <= 0.999))
                d = 2.0637 * Math.Pow((Math.Log(1 / (1 - alpha)) - 0.16), 0.4274) - 1.5774;
            if((0.001<= alpha) && (alpha < 0.5))
                d = -2.0637 * Math.Pow((Math.Log(1 / alpha) - 0.16), 0.4274) + 1.5774;

            double A = d * Math.Sqrt(2);
            double B = (d * d - 1) * 2/3;
            double C = d * ((d * d - 7) / (9 * Math.Sqrt(2)));
            double D = (6 * d * d * d * d + 14 * d * d - 32) / 405;
            double E = d * (9 * d * d * d * d + 256 * d * d - 433) / (4860 * Math.Sqrt(2));

            return (r) + A * Math.Sqrt(r) + B + C / Math.Sqrt(r) + D / (r) + E / ((r) * Math.Sqrt(r));
        }

        /// <summary>
        /// Проверка гипотезы о ЗР по критерию Пирсона
        /// </summary>
        /// <param name="Selection">Случайная выборка</param>
        /// <param name="distribution">Предполагаемый закон распределения</param>
        /// <returns>Возвращает истину, если гипотеза верна</returns>
        public override bool HypoTest(double[] Selection, DistributionFunctions.Distribution distribution)
        {
            double[] spaces = new double[k + 1];
            for (int i = 0; i < spaces.Length; i++)
                spaces[i] = Selection.Min() + (Selection.Max() - Selection.Min()) * i / k;

            int[] counts = new int[k];
            for (int i = 0; i < Selection.Length; i++)
                for (int j = 0; j < (spaces.Length - 1); j++)
                    if ((Selection[i] >= spaces[j]) && (Selection[i] <= spaces[j + 1]))
                        counts[j]++;

            double[] frequency = new double[k];
            for (int i = 0; i < frequency.Length; i++)
                frequency[i] = (double)counts[i] / Selection.Length;

            double[] Probability = new double[k];
            for (int i = 0; i < k; i++)
                Probability[i] = distribution.Probability(spaces[i + 1]) - distribution.Probability(spaces[i]);

            double[] U = new double[k];
            for (int i = 0; i < k; i++)
                U[i] = Math.Pow((frequency[i] - Probability[i]), 2) / Probability[i];

            int r = k - distribution.S;
            if ((U.Sum() * Selection.Length) < Critical(r, 0.9))
                return true; 
            else
                return false;
        }

        
    }
}
