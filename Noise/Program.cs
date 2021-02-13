using System;
using System.IO;

namespace Noise
{
    class Program
    {
        static void Main(string[] args)
        {
            //SomethingStdOut();
            //SomethingFile("./out2.raw");

            // using(var outStream = File.Open("./out2.raw", FileMode.Create))
            // {
            //     MainWaveMorph.Main(outStream);
            // }

            // using(var outStream = File.Open("./minBlepSaw2.raw", FileMode.Create))
            // {
            //     GenerateMinBlep.Main(outStream);
            // }

            // using(var outStream = File.Open("./sawAdditive.raw", FileMode.Create))
            // {
            //     SawtoothAdditive.Main(outStream);
            // }

            // using(var outStream = File.Open("./SawTest3.raw", FileMode.Create))
            // {
            //     SawTest3.Main(outStream);
            // }

            // using(var outStream = File.Open("./BuildingBlocks.raw", FileMode.Create))
            // {
            //     BuildingBlocks.Main(outStream);
            // }

            using(var outStream = File.Open("./IntegrateMinBlep.raw", FileMode.Create))
            {
                IntegrateMinBlep.Main(outStream);
            }
        }

        public static void Something3(Stream outStream)
        {
            int sampleRate = 44100;
            int oversample = 4;
            var generator = new SawGenerator();
            SampleFilter sf = new SampleFilter();
            double phase = 0;
            double frequency = 440;
            double volume = 0.8;
            double freqMult = 1.00001f;

            while(frequency < 17000)
            {
                double sample = 0;

                for(int i = 0 ; i < oversample ; i++)
                {
                    //sample += generator.Get(phase);
                    FirFilter.Put(sf, generator.Get(phase));

                    phase += frequency / (sampleRate * oversample);

                    if(phase >= 1)
                        phase -= (int)phase;
                }

                for(int i = 0 ; i < oversample ; i++)
                    sample += FirFilter.Get(sf);
                
                sample /= oversample;

                outStream.Write(BitConverter.GetBytes((Int16)((sample * volume) * (Int16.MaxValue - 1))), 0, 2);
                outStream.Write(BitConverter.GetBytes((Int16)((sample * volume) * (Int16.MaxValue - 1))), 0, 2);

                frequency *= freqMult;
            }
        }

        private static void SomethingFile(string filename)
        {
            using(var outStream = File.Open(filename, FileMode.Create))
            {
                Something3(outStream);
            }
        }

        private static void SomethingStdOut()
        {
            using(var outStream = Console.OpenStandardOutput())
            {
                Something3(outStream);
            }
        }

        private static void Something2(Stream outStream)
        {
            int sampleRate = 44100;
            int waveformSampleBits = 16;
            int waveformSampleCount = 1 << waveformSampleBits;
            double[] waveform = GenerateSaw(waveformSampleCount);
            int accumulatorFractionalBits = 15;
            uint accumulator = 0;
            double frequency = 110;
            double freqMult = 1.00001f;
            uint increment = (uint)(((waveformSampleCount / (double)sampleRate) * frequency) * (1 << accumulatorFractionalBits));
            

            while(frequency < 20000)
            {
                uint waveformIndexMask = uint.MaxValue >> (32 - waveformSampleBits);
                uint accumulatorWholePart = accumulator >> accumulatorFractionalBits;
                uint maskedAccumulatorWholePart = accumulatorWholePart & waveformIndexMask;
                double outputSampleReal = waveform[maskedAccumulatorWholePart];
                short outputSample = (short)Math.Round(outputSampleReal * (short.MaxValue - 1), MidpointRounding.AwayFromZero);

                outStream.Write(BitConverter.GetBytes(outputSample), 0, 2);
                outStream.Write(BitConverter.GetBytes(outputSample), 0, 2);

                frequency *= freqMult;

                increment = (uint)(((waveformSampleCount / (double)sampleRate) * frequency) * (1 << accumulatorFractionalBits));

                accumulator += increment;
            }
        }

        private static void Something1(Stream outStream)
        {
            int sampleRate = 44100;
            int waveformSampleBits = 16;
            int waveformSampleCount = 1 << waveformSampleBits;
            double[] waveform = GenerateSaw(waveformSampleCount);
            int phase = 0;
            int frequencyWholeBits = 15;
            int frequency = 110 << (31 - frequencyWholeBits);
            double freqMult = 1.0000001f;
            double volume = 0.8;

            while(true)
            {
                int sampleIndexSigned = phase >> (31 - waveformSampleBits);
                int sampleIndexUnsigned = sampleIndexSigned >= 0 ? sampleIndexSigned : waveformSampleCount + sampleIndexSigned;
                double realWaveformSample = waveform[sampleIndexUnsigned];
                double realOutputSample = realWaveformSample * short.MaxValue;
                short outputSample = (short)Math.Round(realOutputSample, MidpointRounding.AwayFromZero);

                outStream.Write(BitConverter.GetBytes(outputSample), 0, 2);
                outStream.Write(BitConverter.GetBytes(outputSample), 0, 2);

                frequency = (int)Math.Round(frequency * freqMult, MidpointRounding.AwayFromZero);
                double waveformStridePerSample = (double)waveformSampleCount / sampleRate;
                double waveformFrequencyStridePerSampleUnshifted = frequency * waveformStridePerSample;
                int waveformFrequencyStridePerSampleUnshiftedRounded = (int)Math.Round(waveformFrequencyStridePerSampleUnshifted, MidpointRounding.AwayFromZero);
                int increment = waveformFrequencyStridePerSampleUnshiftedRounded << 1;
                phase += increment;
            }
        }

        static void Main3(string[] args)
        {
            for(int i = 0 ; i < 1000 ; i++)
                Console.WriteLine(NextGaussian());
        }

        static void Main2(string[] args)
        {
            using(var stdout = /*Console.OpenStandardOutput()*/File.Open("./out2.raw", FileMode.Create))
            {
                int sampleRate = 44100;
                int waveformSampleCount = 44100 << 10;
                double[] waveform = GenerateSaw(waveformSampleCount);
                //double[] waveform2 = GenerateSaw(waveformSampleCount/2);
                //double[] waveform3 = GenerateSaw(waveformSampleCount/4);
                //double[] waveform4 = GenerateSaw(waveformSampleCount/8);

                SampleFilter sf = new SampleFilter();
                
                double phase = 0;
                double frequency = 110;
                double volume = 1f;
                bool increased = false;
                double freqMult = 1.00001f;
                //double freqMult = 1.1f;
                int run = 0;

                while(frequency < 20000)
                {                    
                    //var sample = BitConverter.GetBytes((Int16)(waveform[(int)(phase * waveformSampleCount)] * volume));
                    double sample;

                    double realIndex = (phase * waveformSampleCount) + (NextDither() * 1000000);
                    int intIndex = (int)Math.Round(realIndex);

                    if(intIndex < 0)
                        intIndex = -intIndex;

                    intIndex %= waveformSampleCount;
                    int intIndexNext = (intIndex + 1) % waveformSampleCount;


                    sample = waveform[intIndex];
                    var sample2 = waveform[intIndexNext];

                    double interpolationPercent = (realIndex - (int)realIndex);
                    double delta = sample2 - sample;
                    double increment = delta * interpolationPercent;

                    sample = sample + increment;

                    var dither = NextDither();

                    // if(frequency < sampleRate / 2)
                    //     sample = waveform[(int)(phase * waveformSampleCount)] * volume;
                    // else if(frequency < (sampleRate / 2) + (sampleRate / 4))
                    //     sample = waveform2[(int)(phase * waveformSampleCount / 2)] * volume;
                    // else if(frequency < (sampleRate / 2) + (sampleRate / 4) + (sampleRate / 8))
                    //     sample = waveform3[(int)(phase * waveformSampleCount / 4)] * volume;
                    // else
                    //     sample = waveform4[(int)(phase * waveformSampleCount / 8)] * volume;

                    //if(run > 2185)
                    //    run = run;

                    //FirFilter.Put(sf, sample);
                    
                    //sample = FirFilter.Get(sf) * 0.8;

                    stdout.Write(BitConverter.GetBytes((Int16)(((sample * volume) * Int16.MaxValue) + dither)), 0, 2);
                    stdout.Write(BitConverter.GetBytes((Int16)(((sample * volume) * Int16.MaxValue) + dither)), 0, 2);
                    //stdout.Write(BitConverter.GetBytes(sample), 0, 8);
                    //stdout.Write(BitConverter.GetBytes(sample), 0, 8);
                    //Console.WriteLine(waveform[(int)(phase * waveformSampleCount)]);

                    phase += frequency / sampleRate;

                    if(phase >= 1)
                        phase -= (int)phase;

                    //if(phase >= 1)
                    //    phase = 0;

                    //if(run > 0 && (run % 44100) == 0)
                        frequency *= freqMult;

                    //if(frequency >= 19000 || frequency <= 5)
                    //    freqMult = 1/freqMult;

                    /*if(!increased && frequency > 5000)
                    {
                        volume = 1;
                        increased = true;
                    }*/

                    run++;
                }

                // int sampleRate = 44100;

                // float phaseL = 0;
                // float frequencyL = (float)(20 * 2 * Math.PI);
                // float phaseR = 0;
                // float frequencyR = (float)(20 * 2 * Math.PI);

                // while(true)
                // {
                //     stdout.Write(BitConverter.GetBytes((Int16)(Math.Sin(phaseL) * Int16.MaxValue)), 0, 2);
                //     stdout.Write(BitConverter.GetBytes((Int16)(Math.Sin(phaseR) * Int16.MaxValue)), 0, 2);
                    
                //     phaseL += (frequencyL / sampleRate);
                //     phaseR += (frequencyR / sampleRate);
                    
                //     if(phaseL >= 2 * Math.PI)
                //         phaseL -= (float)(2 * Math.PI);

                //     if(phaseR >= 2 * Math.PI)
                //         phaseR -= (float)(2 * Math.PI);

                //     frequencyL += 0.009f;
                //     frequencyR += 0.005f;
                // }
            }
        }

        public static double[] GenerateSin(int sampleCount)
        {
            var returnValue = new double[sampleCount];

            for(int i = 0 ; i < sampleCount ; i++)
            {
                returnValue[i] = Math.Sin((i / (double)sampleCount) * 2 * Math.PI);
            }

            return returnValue;
        }

        public static double[] GenerateSaw(int sampleCount)
        {
            var returnValue = new double[sampleCount];

            for(int i = 0 ; i < sampleCount ; i++)
            {
                //returnValue[(i + (sampleCount / 2)) % sampleCount] = (((double)i / sampleCount) * 2) - 1;
                returnValue[i] = (((double)i / (sampleCount+1)) * 2) - 1;
            }

            return returnValue;
        }

        private static Random _rand = new Random();

        private static double NextGaussian()
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

        private static double NextDither()
        {
            double x;

            do
            {
                x = NextGaussian();
            } while(x >= 1 || x <= -1);

            return x;
        }
    }
}
