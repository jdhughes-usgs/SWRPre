using System;
using System.Collections.Generic;
using System.Text;

namespace SWRPre
{
    public class Point2D
    {
        private static char[] DELIM = new char[] { ',' };

        private double x, y;

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point2D()
        {
            this.x = 0.0;
            this.y = 0.0;
        }

        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        public override string ToString()
        {
            return x + "," + y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point2D)
            {
                Point2D compare = (Point2D)obj;
                return compare.X == this.X && compare.Y == this.Y;
            }

            return false;
        }

        public bool Nearly(object obj)
        {
            if (obj is Point2D)
            {
                double SmallValue = 1.0e-6;
                Point2D compare = (Point2D)obj;
                double dx = Math.Abs(this.X - compare.X);
                double dy = Math.Abs(this.Y - compare.Y);
                if ((dx <= SmallValue) && (dy <= SmallValue))                
                    return true;               
                else
                    return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() % this.Y.GetHashCode();
        }

        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }

        public Point2D RotateAbout(Point2D anchor, double rotation)
        {
            // Translate the point so that anchor is at the origin.
            Point2D atOrigin = Subtract(anchor);

            // Rotate point about origin.
            Point2D rotated = atOrigin.RotateAboutOrigin(rotation);

            // Translate back to the anchor and return.
            return rotated.add(anchor);
        }

        public Point2D RotateAboutOrigin(double r)
        {
            double x = GetX();
            double y = GetY();

            double xp = x * Math.Cos(r) - y * Math.Sin(r);
            double yp = x * Math.Sin(r) + y * Math.Cos(r);

            return new Point2D(xp, yp);
        }

        public Point2D add(Point2D p)
        {
            return new Point2D(GetX() + p.GetX(), GetY() + p.GetY());
        }

        public Point2D Subtract(Point2D p)
        {
            return new Point2D(GetX() - p.GetX(), GetY() - p.GetY());
        }

        public double distance(Point2D p)
        {
            double dx = GetX() - p.GetX();
            double dy = GetY() - p.GetY();

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point2D parse(String s)
        {
            try
            {
                String[] split = s.Split(DELIM);
                double x = Double.Parse(split[0]);
                double y = Double.Parse(split[1]);
                return new Point2D(x, y);
            }
            catch
            {
                return null;
            }
        }

        public double magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        static Random r = new Random();
        public double distance(LineSegment2D segment)
        {
            Point2D p1 = segment.getP1();
            Point2D p2 = segment.getP2();
            double dx = p2.GetX() - p1.GetX();
            double dy = p2.GetY() - p1.GetY();

            if ((dx == 0) && (dy == 0))
                return distance(p1);

            double u = ((GetX() - p1.GetX()) * dx + (GetY() - p1.GetY()) * dy) /
            (dx * dx + dy * dy);

            Point2D p;
            if (u < 0)
            {
                p = p1;
            }
            else if (u > 1)
            {
                p = p2;
            }
            else
            {
                p = new Point2D(p1.GetX() + u * dx, p1.GetY() + u * dy);
            }

            return distance(p);
        }        
    }
}