using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DistributionFunctions
{
    public abstract class Distribution
    {
        public abstract int S { get; }
        public virtual double Probability(double x)
        {
            return -1;
        }
    }

    #region Continious
    public class ExponentialDistribution : Distribution
    {
        private double lambda;
        public override int S
        {
            get { return 2; }
        }
        public double Lambda
        {
            get { return lambda; }
            set
            {
                if (value > 0)
                    lambda = value;
                else
                    throw new Exception("Параметр 'лямбда' экспоненциального распределения не может быть отрицательным.");
            }
        }

        public override double Probability(double x)
        {
            if (x < 0)
                return 0;
            else
            {
                return (1 - Math.Exp(-lambda * x));
            }
        }

        public ExponentialDistribution(double lambda)
        {
            Lambda = lambda;
        }
    }

    public class NormalDistribution : Distribution
    {
        private double sigma;
        public double expectedValue;
        const double eps = 0.0001;
        const double CriticalPoint = 3.90;
        public override int S
        {
            get { return 3; }
        }
        public double Sigma
        {
            get { return sigma; }
            set
            {
                if (value > 0)
                    sigma = value;
                else
                    throw new Exception("Среднеквадратическое отклонение 'сигма' не может быть отрицательным.");
            }
        }

        public override double Probability(double x)
        {
            if ((x - expectedValue) / sigma >= CriticalPoint)
                return 1;
            if ((x - expectedValue) / sigma <= -CriticalPoint)
                return 0;

            double sum = 0;
            double point = -CriticalPoint + eps;
            while (point < x )//(x - expectedValue) / sigma)
            {
                sum += Math.Exp(-Math.Pow((point - expectedValue), 2) / (2 * Math.Pow(sigma, 2))) * (2 * eps);
                point += 2 * eps;
            }

            return sum / (Math.Sqrt(2 * Math.PI) * sigma);
        }

        public NormalDistribution(double expectedValue, double sigma)
        {
            this.expectedValue = expectedValue;
            Sigma = sigma;
        }
    }

    public class TriangleSimpsonDistribution : Distribution
    {
        public double a;
        public double b;
        private double l;
        public override int S
        {
            get { return 1; }
        }
        public double L
        {
            get { return l; }
            set
            {
                if ((value >= a) && (value <= b))
                    l = value;
                else
                    throw new Exception("Параметр треугольного закона распределения 'l' не может быть отрицательным.");
            }
        }

        public override double Probability(double x)
        {
            if (x < a)
                return 0;

            if ((a <= x) && (x <= l))
                return (Math.Pow((x - a), 2) / ((b - a) * (l - a)));

            if ((l < x) && (x <= b))
                return (1 - Math.Pow((b - x), 2) / ((b - a) * (b - l)));

            else
                return 1;
        }

        public TriangleSimpsonDistribution(double a, double b, double l)
        {
            this.a = a;
            this.b = b;
            if (b <= a)
                throw new Exception("Правая граница должна иметь бОльшую координату.");
            L = l;
        }
    }

    public class UniformDistribution : Distribution
    {
        public double a;
        public double b;
        public override int S
        {
            get { return 1; }
        }
        public override double Probability(double x)
        {
            if (x < a)
                return 0;
            if (x >= b)
                return 1;
            else
                return ((x - a) / (b - a));
        }

        public UniformDistribution(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
    }  
    #endregion
}
