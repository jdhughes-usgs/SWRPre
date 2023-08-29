using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWRPre.IO
{
    public class BoundBox
    {
        public double XMin, YMin;
        public double XMax, YMax;

        public BoundBox()
        {
            XMin = 1.0e+30;
            YMin = 1.0e+30;
            XMax = -1.0e+30;
            YMax = -1.0e+30;
        }
        public BoundBox(double xMin, double xMax, double yMin, double yMax)
        {
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
        }
        public BoundBox(BigEndianBinaryReader file)
        {
            XMin = file.ReadDouble();
            YMin = file.ReadDouble();
            XMax = file.ReadDouble();
            YMax = file.ReadDouble();
        }


        public BoundBox(double[] coords)
        {
            XMin = coords[0];
            YMin = coords[1];
            XMax = coords[2];
            YMax = coords[3];
        }
        public void CheckSetPoint(Point2D point)
        {
            if (point.X > XMax)
                XMax = point.X;
            if (point.X < XMin)
                XMin = point.X;
            if (point.Y > YMax)
                YMax = point.Y;
            if (point.Y < YMin)
                YMin = point.Y;
        }
        public void CheckSetBoundBox(BoundBox other)
        {
            if (XMin > other.XMin)
                XMin = other.XMin;
            if (XMax < other.XMax)
                XMax = other.XMax;
            if (YMax < other.YMax)
                YMax = other.YMax;
            if (YMin > other.YMin)
                YMin = other.YMin;
        }

    }
}
