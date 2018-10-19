using System;
using Xunit;

namespace CauchyQuantile.Tests
{
    public class RimworldUnityExample
    {
        private Random random = new Random();

        /* Old code:
         *  this.MaxTempOffsetC = (float)this.random.Next(-23, -12);
         * New code:
         *  this.MaxTempOffsetC = GetOffset(-60, -20, -10, 5.0)
         */

        public float GetOffset(int min, int peak, int max, float gamma)
        {
            // min/peak/max should all be >= 0, with min<peak<max
            // gamma should be limited to 1/10th width to 1/6th width (width=abs(max-min))
            var offset = (float)this.random.Next(min, max);
            // try 10x to get a Couchy value within min/max range
            for (var i = 0; i < 10; i++)
            {
                var rho = (float)random.NextDouble();
                var candidate = (float)((float)peak + (float)gamma * Math.Tan(Math.PI * (rho - 0.5)));
                if (min <= candidate && candidate <= max)
                {
                    offset = candidate;
                    break;
                }                    
            }
            return offset;
        }

        [Theory]

        [InlineData(-60, -20, -10, 5.0)]

        //[InlineData(0, 0, 60, 1.0)]
        //[InlineData(0, 10, 60, 1.0)]
        //[InlineData(0, 20, 60, 1.0)]
        //[InlineData(0, 30, 60, 1.0)]
        //[InlineData(0, 40, 60, 1.0)]
        //[InlineData(0, 50, 60, 1.0)]
        //[InlineData(0, 60, 60, 1.0)]
//
        //[InlineData(0, 0, 60, 5.0)]
        //[InlineData(0, 10, 60, 5.0)]
        //[InlineData(0, 20, 60, 5.0)]
        //[InlineData(0, 30, 60, 5.0)]
        //[InlineData(0, 40, 60, 5.0)]
        //[InlineData(0, 50, 60, 5.0)]
        //[InlineData(0, 60, 60, 5.0)]
//
        //[InlineData(0, 0, 60, 7.0)]
        //[InlineData(0, 10, 60, 7.0)]
        //[InlineData(0, 20, 60, 7.0)]
        //[InlineData(0, 30, 60, 7.0)]
        //[InlineData(0, 40, 60, 7.0)]
        //[InlineData(0, 50, 60, 7.0)]
        //[InlineData(0, 60, 60, 7.0)]
//
        //[InlineData(0, 0, 60, 10.0)]
        //[InlineData(0, 10, 60, 10.0)]
        //[InlineData(0, 20, 60, 10.0)]
        //[InlineData(0, 30, 60, 10.0)]
        //[InlineData(0, 40, 60, 10.0)]
        //[InlineData(0, 50, 60, 10.0)]
        //[InlineData(0, 60, 60, 10.0)]
//
        public void Test_calculation(int min, int peak, int max, float gamma)
        {
            int samplesWithinGamma = 0;
            int samplesWithinHalfGamma = 0;
            int totalSamples = 0;
            float? minOffset = null;
            float? maxOffset = null;

            for (var i = 0; i < 10000; i++)
            {
                var offset = GetOffset(min, peak, max, gamma);

                totalSamples++;
                if (Math.Abs(offset - (float)peak) <= gamma) samplesWithinGamma++;
                if (Math.Abs(offset - (float)peak) <= gamma/2) samplesWithinHalfGamma++;

                if (minOffset is null || offset < minOffset) minOffset = offset;
                if (maxOffset is null || offset > maxOffset) maxOffset = offset;
                
                Assert.True(
                    ((float)min <= offset) && (offset <= (float)max),
                    $"min:{min} <= {offset:N2} <= max:{max}"
                    );
            }

            var percentWithinGamma = (float)samplesWithinGamma / (float)totalSamples;
            var percentWithinHalfGamma = (float)samplesWithinHalfGamma / (float)totalSamples;
            Assert.True(
                (0.45 <= percentWithinGamma && percentWithinGamma <= 0.75),
                $"% of samples ({percentWithinGamma:P2}) within gamma:{gamma:N2} of the peak:{peak:N2} was not 45%-75%"
                );
        }
    }
}