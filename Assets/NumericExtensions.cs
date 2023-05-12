using System;

namespace Assets
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180.0) * val;
        }
    }
}
