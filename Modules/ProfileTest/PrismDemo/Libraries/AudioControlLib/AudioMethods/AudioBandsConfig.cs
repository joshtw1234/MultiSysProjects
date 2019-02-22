using AudioControlLib.Structures;
using System;

namespace AudioControlLib.AudioMethods
{
    class AudioBandsConfig
    {
        /// <summary>
        /// Source C++ FFT.h
        /// Get Next power of number.
        /// @param x Number to check
        /// @return A power of two number
        /// </summary>
        public static int NextPowerOfTwo(int x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }

        /// <summary>
        /// Initialize EQ 
        ///
        /// Recommended frequencies are ...
        ///  lowfreq  = 880  Hz
        ///  highfreq = 5000 Hz
        ///  Set mixfreq to whatever rate your system is using (eg 48Khz)
        /// </summary>
        public static void init_3band_state(EQSTATE es, int lowfreq, int highfreq, int mixfreq)
        {
            // Clear state 

            // Set Low/Mid/High gains to unity

            es.lg = 5.0;       // updated to fine tune
            es.mg = 1.0;
            es.hg = 8.0;       // updated to fine tune output

            // Calculate filter cutoff frequencies

            es.lf = 2 * Math.Sin(Math.PI * ((double)lowfreq / (double)mixfreq));
            es.hf = 2 * Math.Sin(Math.PI * ((double)highfreq / (double)mixfreq));
        }
        const double vsa = (1.0 / 4294967295.0);   // Very small amount (Denormal Fix)
        /// <summary>
        /// Gets the high value based on a sample and a EQSTATE.
        ///
        /// sample can be any range you like :)
        /// Note that the output will depend on the gain settings for each band 
        /// (especially the bass) so may require clipping before output, but you 
        /// knew that anyway :)
        /// </summary>
        public static double do_3band(EQSTATE es, double sample)
        {
            // Locals

            double l, m, h;      // Low / Mid / High - Sample Values

            // Filter #1 (lowpass)

            es.f1p0 += (es.lf * (sample - es.f1p0)) + vsa;
            es.f1p1 += (es.lf * (es.f1p0 - es.f1p1));
            es.f1p2 += (es.lf * (es.f1p1 - es.f1p2));
            es.f1p3 += (es.lf * (es.f1p2 - es.f1p3));

            l = es.f1p3;

            // Filter #2 (highpass)

            es.f2p0 += (es.hf * (sample - es.f2p0)) + vsa;
            es.f2p1 += (es.hf * (es.f2p0 - es.f2p1));
            es.f2p2 += (es.hf * (es.f2p1 - es.f2p2));
            es.f2p3 += (es.hf * (es.f2p2 - es.f2p3));

            h = es.sdm3 - es.f2p3;

            // Calculate midrange (signal - (low + high))

            m = es.sdm3 - (h + l);

            // Scale, Combine and store

            l *= es.lg;
            m *= es.mg;
            h *= es.hg;

            // Shuffle history buffer 

            es.sdm3 = es.sdm2;
            es.sdm2 = es.sdm1;
            es.sdm1 = sample;

            return (h);
        }

        /// <summary>
        /// Gets the low value based on a sample and a EQSTATE.
        ///
        /// sample can be any range you like :)
        /// Note that the output will depend on the gain settings for each band 
        /// (especially the bass) so may require clipping before output, but you 
        /// knew that anyway :)
        /// </summary>
        public static double do_3bandLow(EQSTATE es, double sample)
        {
            // Locals

            double l, m, h;      // Low / Mid / High - Sample Values

            // Filter #1 (lowpass)

            es.f1p0 += (es.lf * (sample - es.f1p0)) + vsa;
            es.f1p1 += (es.lf * (es.f1p0 - es.f1p1));
            es.f1p2 += (es.lf * (es.f1p1 - es.f1p2));
            es.f1p3 += (es.lf * (es.f1p2 - es.f1p3));

            l = es.f1p3;

            // Filter #2 (highpass)
            es.f2p0 += (es.hf * (sample - es.f2p0)) + vsa;
            es.f2p1 += (es.hf * (es.f2p0 - es.f2p1));
            es.f2p2 += (es.hf * (es.f2p1 - es.f2p2));
            es.f2p3 += (es.hf * (es.f2p2 - es.f2p3));

            h = es.sdm3 - es.f2p3;

            // Calculate midrange (signal - (low + high))
            m = es.sdm3 - (h + l);

            // Scale, Combine and store
            l *= es.lg;
            m *= es.mg;
            h *= es.hg;

            // Shuffle history buffer 
            es.sdm3 = es.sdm2;
            es.sdm2 = es.sdm1;
            es.sdm1 = sample;

            return (l);
        }
    }
}
