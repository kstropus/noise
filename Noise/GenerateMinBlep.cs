using System;
using System.IO;

namespace Noise
{
    public static class GenerateMinBlep
    {
        public static void Main(Stream outStream)
        {
            // double[] minBlep = MinBleps.GenerateMinBlep(6, 4);

            // for(int i = 0 ; i < minBlep.Length ; i++)
            // {
            //     outStream.Write(BitConverter.GetBytes((Int16)((minBlep[i] - 1) * 0.7 * Int16.MaxValue)));
            // }

            int minBlepOversample = 1024;
            int minBlepZeroCrossings = 7;
            double[] minBlepA = MinBleps.GenerateMinBlep(minBlepZeroCrossings, minBlepOversample);
            double[] minBlep = new double[minBlepA.Length - 1];
            Array.Copy(minBlepA, 1, minBlep, 0, minBlepA.Length - 1);

            int sampleRate = 44100;
            double phase = 0;
            double frequency = 440 << 2;
            double freqMult = 1.0594631;
            int minBlepBufferLength = minBlep.Length * 3;
            double[] minBlepBuffer = new double[minBlepBufferLength];
            int minBlepBufferIndex = 0;
            int counter = 0;

            while(frequency < 17000)
            {
                double sample = ((phase - (frequency / sampleRate)) * 2) - 1;

                double minBleppedSample = (minBlepBuffer[minBlepBufferIndex] - (frequency/22050)) + sample;

                minBlepBuffer[minBlepBufferIndex] = 0;

                outStream.Write(BitConverter.GetBytes(minBleppedSample * 0.8));

                minBlepBufferIndex++;

                if(minBlepBufferIndex >= minBlepBufferLength)
                    minBlepBufferIndex = 0;

                phase += frequency / sampleRate;

                if(phase >= 1)
                {
                    phase -= (int)phase;

                    double stepSample = (sampleRate / frequency) * phase * minBlepOversample;
                    double exactCrossTime = 1.0 - (((frequency / sampleRate) - phase) / (frequency / sampleRate));
                    double stepSample2 = exactCrossTime * minBlepOversample;
                    
                    for(int j = 0 ; j < (minBlep.Length / minBlepOversample) ; j++)
                    {
                        // double minBlep1 = minBlep[(int)(stepSample + j * minBlepOversample)];
                        // double minBlep2 = minBlep[(int)(stepSample + j * minBlepOversample) + 1];
                        // double minBlepSample = (minBlep1 * (1 - stepSample)) + (minBlep2 * stepSample);

                        //minBlepBuffer[(minBlepBufferIndex + j) % minBlepBufferLength] += -((minBlep[(int)(stepSample + j * minBlepOversample)] * 2) - 2);

                        int tempIndex = (int)(exactCrossTime * minBlepOversample + (/*Utility.NextDitherGaussian()*/0)) + (j * minBlepOversample); 
                        double minBlep1 = minBlep[tempIndex];
                        double minBlep2 = tempIndex < minBlep.Length - 1 ? minBlep[tempIndex + 1] : minBlep1;
                        double blep = minBlep1 * (1 - exactCrossTime) + minBlep2 * (exactCrossTime);
                        minBlepBuffer[(minBlepBufferIndex + j) % minBlepBufferLength] += -((blep * 2) - 2); 
                        //minBlepBuffer[(minBlepBufferIndex + j) % minBlepBufferLength] += -((minBlep[tempIndex] * 2) - 2); 
                    }
                }

                counter++;

                if(counter > 10000)
                {
                    counter = 0;

                    frequency *= freqMult;
                }
            }
        }
    }
}