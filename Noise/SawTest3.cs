using System;
using System.IO;

namespace Noise
{
    public static class SawTest3
    {
        public static void Main(Stream outStream)
        {
            var sinGen = new SinGenerator2(44100);

            double phase = 0;

            for(int i = 0 ; i < 44100 << 2 ; i++)
            {
                //outStream.Write(BitConverter.GetBytes(sinGen.Sample(phase)));
                outStream.Write(BitConverter.GetBytes(MinBleps.Sinc(phase)));

                phase += 0.001;
            }
        }
    }

    public class SinGenerator2
    {
        public SinGenerator2(int resolution)
        {
            _sinQuadrant1Samples = new double[resolution];
            BuildSamples();
        }

        public double Sample(double phase)
        {
            phase -= (int)phase;
            phase *= 4;

            if(phase < 1)
                return _sinQuadrant1Samples[(int)Math.Round(phase * (_sinQuadrant1Samples.Length - 1))];
            else if(phase < 2)
                return _sinQuadrant1Samples[(int)Math.Round((1 - (phase - 1)) * (_sinQuadrant1Samples.Length - 1))];
            else if(phase < 3)
                return -_sinQuadrant1Samples[(int)Math.Round((phase - 2) * (_sinQuadrant1Samples.Length - 1))];
            else
                return -_sinQuadrant1Samples[(int)Math.Round((1 - (phase - 3)) * (_sinQuadrant1Samples.Length - 1))];
        }

        private void BuildSamples()
        {
            for(int i = 0 ; i < _sinQuadrant1Samples.Length ; i++)
                _sinQuadrant1Samples[i] = Math.Sin(2 * Math.PI * ((double)i / _sinQuadrant1Samples.Length / 4));
        }

        private readonly double[] _sinQuadrant1Samples;
    }
}