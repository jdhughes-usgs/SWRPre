using System;
using System.Collections.Generic;
using System.Text;

namespace SWRPre
{
    public static class GeometryHelper
    {
        public static Range2D CalculateRange(Point2D[] vertices)
        {
            // If the vertex array is empty, return a NaN range.
            if (vertices.Length == 0)
            {
                return new Range2D(new Range1D(double.NaN, double.NaN), new Range1D(double.NaN, double.NaN));
            }

            // Calculate the range.
            double xMin = vertices[0].X;
            double xMax = xMin;
            double yMin = vertices[0].Y;
            double yMax = yMin;
            for (int i = 1; i < vertices.Length; i++)
            {
                xMin = Math.Min(xMin, vertices[i].X);
                xMax = Math.Max(xMax, vertices[i].X);
                yMin = Math.Min(yMin, vertices[i].Y);
                yMax = Math.Max(yMax, vertices[i].Y);
            }

            // Return the result.
            return new Range2D(new Range1D(xMin, xMax), new Range1D(yMin, yMax));
        }
    }
}
