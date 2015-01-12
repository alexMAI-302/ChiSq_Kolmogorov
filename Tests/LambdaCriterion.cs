using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributionFunctions;

namespace Tests
{
    public sealed class LambdaCriterion : Criterion
    {
        /// <summary>
        /// Подсчет вероятности P(lambda)
        /// </summary>
        /// <param name="Lambda">Параметр lambda</param>
        /// <returns>вероятность P(lambda)</returns>
        public double LambdaCriterionValue(double Lambda)
        {
            int k = 0;
            double Sum = 0;
            const double eps = 0.0000000001;
            double last = double.MinValue;
            while (Math.Abs( Sum - last) > eps)
            {
                last = Sum;
                Sum += (Math.Pow(-1, k) * Math.Exp(-2 * k * k * Lambda * Lambda) + Math.Pow(-1, (-(k + 1))) * Math.Exp(-2 * (k + 1) * (k + 1) * Lambda * Lambda));
                k++;
            }
            if ((1 - Sum) > 1)
                return 1;
            return (1 - Sum);
        }

        /// <summary>
        /// Проверка гипотезы о ЗР по критерию Колмогорова
        /// </summary>
        /// <param name="Selection">Случайная выборка</param>
        /// <param name="distribution">Предполагаемый закон распределения</param>
        /// <returns>Возвращает истину, если гипотеза верна</returns>
        public override bool HypoTest(double[] Selection, DistributionFunctions.Distribution distribution)
        {
            double[] spaces = new double[15+1];
            for (int i = 0; i < spaces.Length; i++)
                spaces[i] = Selection.Min() + (Selection.Max() - Selection.Min()) * i / 15;

            int[] counts = new int[15];
            for (int i = 0; i < Selection.Length; i++)
                for (int j = 0; j < (spaces.Length-1); j++)
                    if ((Selection[i] >= spaces[j]) && (Selection[i] <= spaces[j + 1]))
                        counts[j]++;
            
            double[] frequency = new double[15];
            for (int i = 0; i < frequency.Length; i++)
                frequency[i] = (double)counts[i] / Selection.Length;
            
            double[] DistFuncPract = new double[15];
            DistFuncPract[0] = frequency[0];
            for (int i = 1; i < DistFuncPract.Length; i++)
                DistFuncPract[i] = DistFuncPract[i-1] + frequency[i];
            

            double[] Probability = new double[15];
            for (int i = 0; i < 15; i++)
                Probability[i] = distribution.Probability(spaces[i + 1]);
            
            double[] D = new double[15];
            for (int i = 0; i < 15; i++)
                D[i] = Math.Abs(DistFuncPract[i] - Probability[i]);

            if (LambdaCriterionValue(D.Max() * Math.Sqrt(Selection.Length)) >= 0.74)
                return true;
            else
            {
                return false; 
            }
        }

        
    }
}
