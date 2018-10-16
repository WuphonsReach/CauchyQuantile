using System;
using Xunit;

namespace CauchyQuantile.Tests
{
    public class CalculateTests
    {
        [Theory]
        [InlineData(0.0, -1.7e16, -1.5e16)]
        [InlineData(0.5, -0.1, 0.1)]
        [InlineData(1.0, 1.5e16, 1.7e16)]
        public void Calculate_works_for_normal_rho_values(
            double rho, 
            double expectedLow, 
            double expectedHigh
            )
        {
            var result = CauchyQuantile.Calculate(rho);
            Assert.InRange(result, expectedLow, expectedHigh);
        }

        [Theory]
        [InlineData(-0.0001)]
        [InlineData(1.0001)]
        public void Calculate_works_for_abnormal_rho_values(double rho)
        {
            var result = CauchyQuantile.Calculate(rho);
        }
    }
}
