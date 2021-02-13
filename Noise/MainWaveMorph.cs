using System;
using System.IO;

namespace Noise
{
    public class MainWaveMorph
    {
        public static void Main(Stream outStream)
        {
            int sampleRate = 44100;
            int waveformSampleCount = 44100;
            double[] waveformSaw = Utility.GenerateSaw(waveformSampleCount, 0.5);
            double[] waveformSin = Utility.GenerateSin(waveformSampleCount);
            
            double previousSample = 0;

            for(int i = 0 ; i < 100 ; i++)
            {
                for(int j = 0 ; j < waveformSampleCount ; j++)
                {
                    double sample;

                    sample = (waveformSaw[(j * 10000) % waveformSampleCount] * (i/100.0)) + (waveformSin[(j * 10000) % waveformSampleCount] * (1 - i/100.0));

                    double smoothSample = (sample + previousSample) / 2;

                    outStream.Write(BitConverter.GetBytes((short)(smoothSample * (short.MaxValue - 1))));
                    outStream.Write(BitConverter.GetBytes((short)(smoothSample * (short.MaxValue - 1))));

                    previousSample = smoothSample;
                }
            }
        }
    }
}