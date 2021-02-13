using System;
using System.IO;
using System.Linq;

namespace Noise
{
    public static class IntegrateMinBlep
    {
        public static void Main(Stream outStream)
        {
            var minBlep = MinBleps.GenerateMinBlep(16, 64);

            double[] minSquare = new double[minBlep.Length * 2];

            // for(int i = 0 ; i < minBlep.Length ; i++)
            // {
            //     minBlep[i] = (minBlep[i]/* * 0.8*/) * 2 - 1;
            // }

            for(int i = 0 ; i < minBlep.Length ; i++)
            {
                minSquare[i] = minBlep[i] - 0.5;
                minSquare[i + minBlep.Length] = (1 - minBlep[i]) - 0.5;
            }

            // for(int i = 0 ; i < minSquare.Length ; i++)
            // {
            //     minSquare[i] = (minSquare[i] * 0.8) * 2 - 1;
            // }

            var integrated = Integrate(minSquare, 1);
            var differentiated = Differentiate(minSquare);

            var wSinc = WindowedSinc(16, 1);

            var integratedWSinc = Integrate(wSinc, 1);

            var saw = Program.GenerateSaw(8192);

            var convolved = Convolve(saw, wSinc);

            for(int i = 0 ; i < minBlep.Length << 4 ; i++)
            {
                //outStream.Write(BitConverter.GetBytes(minBlep[i % minBlep.Length] * 0.5));
                //outStream.Write(BitConverter.GetBytes(minSquare[i % minSquare.Length] * 0.5));
                //outStream.Write(BitConverter.GetBytes(integrated[i % integrated.Length] * 0.001));
                //outStream.Write(BitConverter.GetBytes(differentiated[i % differentiated.Length] * 200));
                //outStream.Write(BitConverter.GetBytes(wSinc[i % wSinc.Length]));
                //outStream.Write(BitConverter.GetBytes(integratedWSinc[i % integratedWSinc.Length] * 0.01));
                //outStream.Write(BitConverter.GetBytes(saw[i % saw.Length]));
                outStream.Write(BitConverter.GetBytes(convolved[i % convolved.Length] * 0.5));
            }
        }

        public static double[] Convolve(double[] input, double[] convolver)
        {
            double[] output = new double[input.Length];

            for(int i = 0 ; i < input.Length ; i++)
            {
                for(int j = 0 ; j < convolver.Length ; j++)
                {
                    output[(i + j) % input.Length] += convolver[j] * input[i];
                }
            }

            return output;
        }

        public static void Scale(double[] input, double scale)
        {
            for(int i = 0 ; i < input.Length ; i++)
                input[i] *= scale;
        }

        public static double[] Integrate(double[] input, double leak)
        {
            double[] integrated = new double[input.Length];

            integrated[0] = input[0];

            for(int i = 1 ; i < input.Length ; i++)
            {
                integrated[i] = leak * integrated[i - 1] + input[i];
            }

            return integrated;
        }

        public static double[] Differentiate(double[] input)
        {
            double[] differentiated = new double[input.Length];

            differentiated[0] = input[0];

            for(int i = 1 ; i < input.Length ; i++)
            {
                differentiated[i] = input[i] - input[i - 1];
            }

            return differentiated;
        }

        public static double[] WindowedSinc(int zeroCrossings, int overSampling)
        {
            int n = (zeroCrossings * 2 * overSampling) + 1;

            double[] buffer1 = new double[n];
            double[] buffer2 = new double[n];

            // Generate Sinc
            double a = (double)-zeroCrossings;
            double b = (double)zeroCrossings;

            double r;

            for(int i = 0; i < n; i++)
            {
                r = ((double)i) / ((double)(n - 1));
                buffer1[i] = MinBleps.Sinc(a + (r * (b - a)));
            }

            // Window Sinc
            MinBleps.BlackmanWindow(n, buffer2);

            for(int i = 0; i < n; i++)
                buffer1[i] *= buffer2[i];

            return buffer1;
        }
    }
}