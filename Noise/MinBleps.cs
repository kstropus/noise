using System;

namespace Noise
{
    // MinBLEP Generation Code
    // By Daniel Werner
    // This Code Is Public Domain
    // Converted to C# by Keith Stropus
    public class MinBleps
    {
        // SINC Function
        // inline float SINC(float x)
        // {
        //     float pix;

        //     if(x == 0.0f)
        //         return 1.0f;
        //     else
        //     {
        //         pix = PI * x;
        //     return sinf(pix) / pix;
        //     }
        // }

        // SINC Function
        public static double Sinc(double x)
        {
            double pix;

            if(x == 0.0)
                return 1.0;
            else
            {
                pix = Math.PI * x;
                return Math.Sin(pix) / pix;
            }
        }

        // Generate Blackman Window
        // inline void BlackmanWindow(int n, float *w)
        // {
        //     int m = n - 1;
        //     int i;
        //     float f1, f2, fm;

        //     fm = (float)m;
        //     for(i = 0; i <= m; i++)
        //     {
        //         f1 = (2.0f * PI * (float)i) / fm;
        //         f2 = 2.0f * f1;
        //         w[i] = 0.42f - (0.5f * cosf(f1)) + (0.08f * cosf(f2));
        //     }
        // }

        // Generate Blackman Window
        public static void BlackmanWindow(int n, double[] w)
        {
            int m = n - 1;
            double f1, f2, fm;

            fm = (double)m;
            for(int i = 0 ; i <= m ; i++)
            {
                f1 = (2.0 * Math.PI * (double)i) / fm;
                f2 = 2.0 * f1;
                w[i] = 0.42 - (0.5 * Math.Cos(f1)) + (0.08 * Math.Cos(f2));
            }
        }

        // Discrete Fourier Transform
        // void DFT(int n, float *realTime, float *imagTime, float *realFreq, float *imagFreq)
        // {
        //     int k, i;
        //     float sr, si, p;

        //     for(k = 0; k < n; k++)
        //     {
        //         realFreq[k] = 0.0f;
        //         imagFreq[k] = 0.0f;
        //     }

        //     for(k = 0; k < n; k++)
        //     {
        //         for(i = 0; i < n; i++)
        //         {
        //             p = (2.0f * PI * (float)(k * i)) / n;
        //             sr = cosf(p);
        //             si = -sinf(p);
        //             realFreq[k] += (realTime[i] * sr) - (imagTime[i] * si);
        //             imagFreq[k] += (realTime[i] * si) + (imagTime[i] * sr);
        //         }
        //     }
        // }

        // Discrete Fourier Transform
        public static void Dft(int n, double[] realTime, double[] imagTime, double[] realFreq, double[] imagFreq)
        {
            double sr, si, p;

            for(int k = 0 ; k < n ; k++)
            {
                realFreq[k] = 0.0;
                imagFreq[k] = 0.0;
            }

            for(int k = 0 ; k < n ; k++)
            {
                for(int i = 0 ; i < n ; i++)
                {
                    p = (2.0 * Math.PI * (double)(k * i)) / n;
                    sr = Math.Cos(p);
                    si = -Math.Sin(p);
                    realFreq[k] += (realTime[i] * sr) - (imagTime[i] * si);
                    imagFreq[k] += (realTime[i] * si) + (imagTime[i] * sr);
                }
            }
        }

        // Inverse Discrete Fourier Transform
        // void InverseDFT(int n, float *realTime, float *imagTime, float *realFreq, float *imagFreq)
        // {
        //     int k, i;
        //     float sr, si, p;

        //     for(k = 0; k < n; k++)
        //     {
        //         realTime[k] = 0.0f;
        //         imagTime[k] = 0.0f;
        //     }

        //     for(k = 0; k < n; k++)
        //     {
        //         for(i = 0; i < n; i++)
        //         {
        //             p = (2.0f * PI * (float)(k * i)) / n;
        //             sr = cosf(p);
        //             si = -sinf(p);
        //             realTime[k] += (realFreq[i] * sr) + (imagFreq[i] * si);
        //             imagTime[k] += (realFreq[i] * si) - (imagFreq[i] * sr);
        //         }

        //         realTime[k] /= n;
        //         imagTime[k] /= n;
        //     }
        // }

        // Inverse Discrete Fourier Transform
        public static void InverseDft(int n, double[] realTime, double[] imagTime, double[] realFreq, double[] imagFreq)
        {
            double sr, si, p;

            for(int k = 0 ; k < n ; k++)
            {
                realTime[k] = 0.0;
                imagTime[k] = 0.0;
            }

            for(int k = 0 ; k < n ; k++)
            {
                for(int i = 0 ; i < n ; i++)
                {
                    p = (2.0 * Math.PI * (double)(k * i)) / n;
                    sr = Math.Cos(p);
                    si = -Math.Sin(p);
                    realTime[k] += (realFreq[i] * sr) + (imagFreq[i] * si);
                    imagTime[k] += (realFreq[i] * si) - (imagFreq[i] * sr);
                }

                realTime[k] /= n;
                imagTime[k] /= n;
            }
        }

        // Complex Absolute Value
        // inline float cabs(float x, float y)
        // {
        //     return sqrtf((x * x) + (y * y));
        // }

        // Complex Absolute Value
        public static double Cabs(double x, double y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        // Complex Exponential
        // inline void cexp(float x, float y, float *zx, float *zy)
        // {
        //     float expx;

        //     expx = expf(x);
        //     *zx = expx * cosf(y);
        //     *zy = expx * sinf(y);
        // }

        // Complex Exponential
        public static void Cexp(double x, double y, out double zx, out double zy)
        {
            double expx;

            expx = Math.Exp(x);
            zx = expx * Math.Cos(y);
            zy = expx * Math.Sin(y);
        }

        // Compute Real Cepstrum Of Signal
        // void RealCepstrum(int n, float *signal, float *realCepstrum)
        // {
        //     float *realTime, *imagTime, *realFreq, *imagFreq;
        //     int i;

        //     realTime = new float[n];
        //     imagTime = new float[n];
        //     realFreq = new float[n];
        //     imagFreq = new float[n];

        //     // Compose Complex FFT Input

        //     for(i = 0; i < n; i++)
        //     {
        //         realTime[i] = signal[i];
        //         imagTime[i] = 0.0f;
        //     }

        //     // Perform DFT

        //     DFT(n, realTime, imagTime, realFreq, imagFreq);

        //     // Calculate Log Of Absolute Value

        //     for(i = 0; i < n; i++)
        //     {
        //         realFreq[i] = logf(cabs(realFreq[i], imagFreq[i]));
        //         imagFreq[i] = 0.0f;
        //     }

        //     // Perform Inverse FFT

        //     InverseDFT(n, realTime, imagTime, realFreq, imagFreq);

        //     // Output Real Part Of FFT
        //     for(i = 0; i < n; i++)
        //         realCepstrum[i] = realTime[i];

        //     delete realTime;
        //     delete imagTime;
        //     delete realFreq;
        //     delete imagFreq;
        // }

        // Compute Real Cepstrum Of Signal
        public static void RealCepstrum(int n, double[] signal, double[] realCepstrum)
        {
            double[] realTime;
            double[] imagTime;
            double[] realFreq;
            double[] imagFreq;
            
            realTime = new double[n];
            imagTime = new double[n];
            realFreq = new double[n];
            imagFreq = new double[n];

            // Compose Complex FFT Input
            for(int i = 0 ; i < n ; i++)
            {
                realTime[i] = signal[i];
                imagTime[i] = 0.0;
            }

            //     // Perform DFT
            Dft(n, realTime, imagTime, realFreq, imagFreq);

            // Calculate Log Of Absolute Value
            for(int i = 0; i < n; i++)
            {
                realFreq[i] = Math.Log(Cabs(realFreq[i], imagFreq[i]));
                imagFreq[i] = 0.0f;
            }

            // Perform Inverse FFT

            InverseDft(n, realTime, imagTime, realFreq, imagFreq);

            // Output Real Part Of FFT
            for(int i = 0; i < n; i++)
                realCepstrum[i] = realTime[i];
        }

        // Compute Minimum Phase Reconstruction Of Signal
        // void MinimumPhase(int n, float *realCepstrum, float *minimumPhase)
        // {
        //     int i, nd2;
        //     float *realTime, *imagTime, *realFreq, *imagFreq;

        //     nd2 = n / 2;
        //     realTime = new float[n];
        //     imagTime = new float[n];
        //     realFreq = new float[n];
        //     imagFreq = new float[n];

        //     if((n % 2) == 1)
        //     {
        //         realTime[0] = realCepstrum[0];

        //         for(i = 1; i < nd2; i++)
        //             realTime[i] = 2.0f * realCepstrum[i];

        //         for(i = nd2; i < n; i++)
        //             realTime[i] = 0.0f;
        //     }
        //     else
        //     {
        //         realTime[0] = realCepstrum[0];

        //         for(i = 1; i < nd2; i++)
        //             realTime[i] = 2.0f * realCepstrum[i];

        //         realTime[nd2] = realCepstrum[nd2];

        //         for(i = nd2 + 1; i < n; i++)
        //             realTime[i] = 0.0f;
        //     }

        //     for(i = 0; i < n; i++)
        //         imagTime[i] = 0.0f;

        //     DFT(n, realTime, imagTime, realFreq, imagFreq);

        //     for(i = 0; i < n; i++)
        //         cexp(realFreq[i], imagFreq[i], &realFreq[i], &imagFreq[i]);

        //     InverseDFT(n, realTime, imagTime, realFreq, imagFreq);

        //     for(i = 0; i < n; i++)
        //         minimumPhase[i] = realTime[i];

        //     delete realTime;
        //     delete imagTime;
        //     delete realFreq;
        //     delete imagFreq;
        // }

        // Compute Minimum Phase Reconstruction Of Signal
        public static void MinimumPhase(int n, double[] realCepstrum, double[] minimumPhase)
        {
             int nd2;
             double[] realTime, imagTime, realFreq, imagFreq;

             nd2 = n / 2;
             realTime = new double[n];
             imagTime = new double[n];
             realFreq = new double[n];
             imagFreq = new double[n];

            if((n % 2) == 1)
            {
                realTime[0] = realCepstrum[0];

                for(int i = 1; i < nd2; i++)
                    realTime[i] = 2.0 * realCepstrum[i];

                for(int i = nd2; i < n; i++)
                    realTime[i] = 0.0;
            }
            else
            {
                realTime[0] = realCepstrum[0];

                for(int i = 1; i < nd2; i++)
                    realTime[i] = 2.0 * realCepstrum[i];

                realTime[nd2] = realCepstrum[nd2];

                for(int i = nd2 + 1; i < n; i++)
                    realTime[i] = 0.0;
            }

            for(int i = 0; i < n; i++)
                imagTime[i] = 0.0;

            Dft(n, realTime, imagTime, realFreq, imagFreq);

            for(int i = 0; i < n; i++)
            {
                Cexp(realFreq[i], imagFreq[i], out double realFreqi, out double imagFreqi);

                realFreq[i] = realFreqi;
                imagFreq[i] = imagFreqi;
            }

            InverseDft(n, realTime, imagTime, realFreq, imagFreq);

            for(int i = 0; i < n; i++)
                minimumPhase[i] = realTime[i];
        }

        // Generate MinBLEP And Return It In An Array Of Floating Point Values
        // float *GenerateMinBLEP(int zeroCrossings, int overSampling)
        // {
        //     int i, n;
        //     float r, a, b;
        //     float *buffer1, *buffer2, *minBLEP;

        //     n = (zeroCrossings * 2 * overSampling) + 1;

        //     buffer1 = new float[n];
        //     buffer2 = new float[n];

        //     // Generate Sinc
        //     a = (float)-zeroCrossings;
        //     b = (float)zeroCrossings;

        //     for(i = 0; i < n; i++)
        //     {
        //         r = ((float)i) / ((float)(n - 1));
        //         buffer1[i] = SINC(a + (r * (b - a)));
        //     }

        //     // Window Sinc
        //     BlackmanWindow(n, buffer2);

        //     for(i = 0; i < n; i++)
        //         buffer1[i] *= buffer2[i];

        //     // Minimum Phase Reconstruction
        //     RealCepstrum(n, buffer1, buffer2);
        //     MinimumPhase(n, buffer2, buffer1);

        //     // Integrate Into MinBLEP
        //     minBLEP = new float[n];
        //     a = 0.0f;

        //     for(i = 0; i < n; i++)
        //     {
        //         a += buffer1[i];
        //         minBLEP[i] = a;
        //     }

        //     // Normalize
        //     a = minBLEP[n - 1];
        //     a = 1.0f / a;

        //     for(i = 0; i < n; i++)
        //         minBLEP[i] *= a;

        //     delete buffer1;
        //     delete buffer2;
        //     return minBLEP;
        // }

        // Generate MinBLEP And Return It In An Array Of Floating Point Values
        public static double[] GenerateMinBlep(int zeroCrossings, int overSampling)
        {
            int n;
            double r, a, b;
            double[] buffer1, buffer2, minBlep;

            n = (zeroCrossings * 2 * overSampling) + 1;

            buffer1 = new double[n];
            buffer2 = new double[n];

            // Generate Sinc
            a = (double)-zeroCrossings;
            b = (double)zeroCrossings;

            for(int i = 0; i < n; i++)
            {
                r = ((double)i) / ((double)(n - 1));
                buffer1[i] = Sinc(a + (r * (b - a)));
            }

            // Window Sinc
            BlackmanWindow(n, buffer2);

            for(int i = 0; i < n; i++)
                buffer1[i] *= buffer2[i];

            // Minimum Phase Reconstruction
            RealCepstrum(n, buffer1, buffer2);
            MinimumPhase(n, buffer2, buffer1);

            // Integrate Into MinBLEP
            minBlep = new double[n];
            a = 0.0;

            for(int i = 0; i < n; i++)
            {
                a += buffer1[i];
                minBlep[i] = a;
            }

            // Normalize
            a = minBlep[n - 1];
            a = 1.0 / a;

            for(int i = 0; i < n; i++)
                minBlep[i] *= a;

            return minBlep;
        }
    }
}