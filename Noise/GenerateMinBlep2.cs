// using System;
// using System.IO;

// namespace Noise
// {
//     public static class GenerateMinBlep2
//     {
//         public static void Main(Stream outStream)
//         {
//             int minBlepOversample = 128;
//             int minBlepZeroCrossings = 32;
//             double[] minBlep = MinBleps.GenerateMinBlep(minBlepZeroCrossings, minBlepOversample);

//             int sampleRate = 44100;
//             double mPhase = 0;
//             double mPhaseIncrement = 0.0001;
//             int index = 0;
//             int cirBuffSize = minBlep.Length * 3;
//             double[] circularBuffer = new double[cirBuffSize];

//             for(int i = 0 ; i < sampleRate << 4 ; i++)
//             {
//                 mPhase += mPhaseIncrement;
//                 index = (index+1)%cirBuffSize;
                
//                 //Normal minBLEP:
//                 while(mPhase >= 1.0)
//                 {
//                     mPhase -= 1.0;
//                     double exactCrossTime = 1.0-((mPhaseIncrement-mPhase)/mPhaseIncrement);
//                     PopulateCircularBufferForSawtoothWave(exactCrossTime, 1.0);
//                 }
                
//                 circularBuffer[index] += (mPhase-mPhaseIncrement);
//                 double output = 1.7f *(circularBuffer[index] - 0.4f);
//                 circularBuffer[index] = 0.0;
//             }
//         }

//         public static void PopulateCircularBufferForSawtoothWave(double offset, double scale, double[] minBlep, double[] circularBuffer, int minBlepOversample)
//         {
            
//             for(int i = 0; i < (circularBuffer.Length-1); i++)
//             {
//                 double tempIndex = (offset*minBlepOversample)+(i*minBlepOversample);
//                 double tempFraction = tempIndex - Math.Floor(tempIndex);
//                 circularBuffer[(tempIndex+i)%circularBuffer.Length] += (scale-LERP(tempFraction, minBlep[(int)Math.Floor(tempIndex)], minBlep[(int)Math.Ceiling(tempIndex)]) * scale);
//             }
//         }
//     }
// }