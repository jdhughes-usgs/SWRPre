using System;
using System.Collections.Generic;
using System.Text;

namespace SWRPre
{
    public class Range1D
    {
        static Random r = new Random();
        double min, max;

        public Range1D()
        {
            this.min = Double.MaxValue;
            this.max = Double.MinValue;
        }

        public Range1D(double min, double max)
        {
            this.min = (min < max ? min : max);
            this.max = (max > min ? max : min);
        }

        public double constrain(double value)
        {
            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }

        public bool contains(double value)
        {
            return (min <= value) && (value <= max);
        }

        public bool overlaps(Range1D range)
        {
            if (min < range.min)
                return (max >= range.min);
            else
                return (range.max >= min);
        }

        public double getOverlap(Range1D range)
        {
            if (min < range.min)
                if (max > range.max)
                    return range.size();
                else
                    if (max < range.min)
                        return 0.0;
                    else
                        return max - range.min;
            else
                if (max < range.max)
                    return size();
                else
                    if (min > range.max)
                        return 0.0;
                    else
                        return range.max - min;
        }

        public double random()
        {
            return r.NextDouble() * size() + getMin();
        }
       
        public double size()
        {
            return getMax() - getMin();
        }
        
        public double getMin()
        {
            return min;
        }
        
        public double getMax()
        {
            return max;
        }
        
        public static double mapValue(double value, Range1D fromRange, Range1D toRange, bool reverse)
        {
            // Determine the relative position of the value along the from-range.
            double relative = (value - fromRange.getMin()) / fromRange.size();

            // If reversing is requested, reverse the relative value.
            if (reverse)
                relative = 1 - relative;

            // Return the mapped value.
            return relative * toRange.size() + toRange.getMin();
        }

        public static double mapScale(double value, Range1D fromRange, Range1D toRange)
        {
            return value * toRange.size() / fromRange.size();
        }

        public String toString()
        {
            return "[" + min + "," + max + "]";
        }

        public Range1D union(Range1D range)
        {
            // Get the lower minimum of this and the other range.
            double minValue = (this.getMin() < range.getMin() ? this.getMin() : range.getMin());
            double maxValue = (this.getMax() > range.getMax() ? this.getMax() : range.getMax());

            // Return the combined range.
            return new Range1D(minValue, maxValue);
        }

        public Range1D buffer(double bufferRatio)
        {
            double bufferSize = this.size() * bufferRatio;
            double bufferedMin = this.getMin() - bufferSize;
            double bufferedMax = this.getMax() + bufferSize;
            return new Range1D(bufferedMin, bufferedMax);
        }
    }

}
