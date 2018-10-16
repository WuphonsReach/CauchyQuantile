using System;

namespace CauchyQuantile
{
    public static class CauchyQuantile
    {
        /// <summary>Calculate the Cauchy quantile value.</summary>
        /// <param name="rho">Input value in the range [0,1]</param>
        /// <param name="x0">Location parameter of peak</param>
        /// <param name="scale">Scale parameter (half-width at half-maximum)</param>
        /// <returns></returns>
        public static double Calculate(
            double rho,
            double x0 = 0,
            double scale = 1
            )
        {
            return x0 + scale * Math.Tan(Math.PI * (rho - 0.5));
        }
    }
}
