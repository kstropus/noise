using System;

namespace Noise
{
    public static class Utility
    {
        public static double[] GenerateSin(int sampleCount, double startPhase = 0)
        {
            var returnValue = new double[sampleCount];

            for(int i = 0 ; i < sampleCount ; i++)
            {
                returnValue[i] = Math.Sin(((i / (double)sampleCount) + startPhase) * 2 * Math.PI);
            }

            return returnValue;
        }

        public static double[] GenerateSaw(int sampleCount, double startPhase = 0)
        {
            var returnValue = new double[sampleCount];

            for(int i = 0 ; i < sampleCount ; i++)
            {
                double sample = (i / (double)sampleCount) + startPhase;

                sample -= (int)sample;

                returnValue[i] = (sample * 2) - 1;
            }

            return returnValue;
        }

        private static Random _rand = new Random();

        public static double NextGaussian()
        {
            double v1, v2, s;

            do
            {
                v1 = 2.0 * _rand.NextDouble() - 1.0;
                v2 = 2.0 * _rand.NextDouble() - 1.0;
                s = v1 * v1 + v2 * v2;
            } while(s >= 1.0 || s == 0);

            s = Math.Sqrt((-2.0 * Math.Log(s)) / s);

            return v1 * s;
        }

        public static double NextDitherGaussian()
        {
            double x;

            do
            {
                x = NextGaussian();
            } while(x >= 1 || x <= -1);

            return x;
        }

        public static double NextDitherLinear()
        {
            return _rand.NextDouble();
        }
    }
}