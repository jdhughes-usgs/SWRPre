using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SWRPre
{
    public class Range2D
    {
        Range1D xRange, yRange;
        public Range2D()
        {
            this.xRange = new Range1D();
            this.yRange = new Range1D();
        }

        public Range2D(IO.BigEndianBinaryReader file)
        {
            double xmin = file.ReadDouble();
            double ymin = file.ReadDouble();
            double xmax = file.ReadDouble();
            double ymax = file.ReadDouble();
            this.xRange = new Range1D(xmin, xmax);
            this.yRange = new Range1D(ymin, ymax);
        }

        public Range2D(Range1D xRange, Range1D yRange)
        {
            this.xRange = xRange;
            this.yRange = yRange;
        }       

        public Range1D getXRange()
        {
            return xRange;
        }

        public Range1D getYRange()
        {
            return yRange;
        }

        public String toString()
        {
            return "x:" + xRange + ",y:" + yRange;
        }

        public bool overlaps(Range2D range)
        {
            return getXRange().overlaps(range.getXRange()) &&
            getYRange().overlaps(range.getYRange());
        }

        public void checkSetRange(Range2D other)
        {
            xRange = xRange.union(other.xRange);
            yRange = yRange.union(other.yRange);
        }

        public bool contains(Point2D p)
        {
            return getXRange().contains(p.X) && getYRange().contains(p.Y);
        }
    }
}
