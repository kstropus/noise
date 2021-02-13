using System;
using System.IO;

namespace Noise
{
    public static class SawtoothAdditive
    {
        public static void Main(Stream outStream)
        {
            //double[] waveform = GenerateSaw(55, (int)(18000.0 / 55), 100);
            //double[] waveform = GenerateSaw(55 << 1, (int)(18000.0 / (55 << 1)), 100); // 110
            double[] waveform = GenerateSaw(55 << 2, (int)(18000.0 / (55 << 2)), 100); // 220
            //double[] waveform = GenerateSaw(55 << 3, (int)(18000.0 / (55 << 3)), 100); // 440
            //double[] waveform = GenerateSaw(55 << 4, (int)(18000.0 / (55 << 4)), 100); // 880
            //double[] waveform = GenerateSaw(55 << 5, (int)(18000.0 / (55 << 5)), 100); // 1760
            //double[] waveform = GenerateSaw(55 << 6, (int)(18000.0 / (55 << 6)), 100); // 3520
            //double[] waveform = GenerateSaw(55 << 7, (int)(18000.0 / (55 << 7)), 100); // 7040
            //double[] waveform = GenerateSaw(55 << 8, (int)(18000.0 / (55 << 8)), 100); // 14080

            //var waveform = GenerateSin(220, 1, 100, 0.25);

            for(int i = 0 ; i < waveform.Length ; i++)
            {
                outStream.Write(BitConverter.GetBytes(waveform[i]));
            }
        }

        public static double[] GenerateSaw(double frequency, int harmonics = 1, int periods = 1)
        {
            int sampleRate = 44100;
            double[] result = new double[sampleRate * periods];
            double fundamental = frequency;
            double currFrequency = fundamental;
            int polarity = 1;

            for(int i = 0 ; i < harmonics ; i++)
            {
                var nextWave = GenerateSin(currFrequency, 1.0/(i+1), (periods * currFrequency), polarity == 1 ? 0.25 : 0.75);

                for(int j = 0 ; j < nextWave.Length && j < result.Length ; j++)
                {
                    result[j] += nextWave[j];
                }

                currFrequency += fundamental;
                polarity *= -1;
            }

            for(int i = 0 ; i < result.Length ; i++)
                result[i] = result[i] / Math.PI;
            
            for(int i = 0 ; i < result.Length ; i++)
                result[i] = -result[i];

            return result;
        }

        public static double[] GenerateSin(double frequency, double amplitude = 1, double periods = 1, double startPhase = 0, int sampleRate = 44100)
        {
            double[] result = new double[(int)Math.Ceiling(sampleRate / frequency * periods)];

            double phase = startPhase;
            int i = 0;

            while((phase - startPhase) < periods && i < result.Length)
            {
                result[i++] = Math.Sin(2 * Math.PI * (phase - (int)phase)) * amplitude;

                phase += frequency / sampleRate;
            }

            return result;
        }
    }
}