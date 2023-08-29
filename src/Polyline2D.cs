using System;
using System.Collections.Generic;
using System.Text;
//using MapTools;
//using System.Runtime.InteropServices;
//using ScenarioManagerMG5;

namespace SWRPre
{
    public class Polyline2D : Shape2D
    {
        private Range2D range;
        private Point2D[] vertices;
        public const int shapeType = 3;

        public Range2D Range
        {
            get
            {
                if (range == null)
                {
                    range = GeometryHelper.CalculateRange(vertices);
                }
                return range;
            }
        }

        public int NumVertices
        {
            get
            {
                return vertices.Length;
            }
        }

        public void reverseVertices()
        {
            List<Point2D> vertices = new List<Point2D>();
            for (int j = this.NumVertices-1; j >= 0; j--)
            {
                vertices.Add(this.GetVertex(j));
            }
            this.vertices = vertices.ToArray();
            return;
        }

        public Polyline2D(Point2D[] vertices)
        {
            // Store a reference to the vertex array.
            this.vertices = vertices;
        }

        public Point2D GetVertex(int index)
        {
            return vertices[index];
        }

        public LineSegment2D[] splitSegment(LineSegment2D segment)
        {
            Point2D[] intersections = GetIntersections(segment);

            LineSegment2D[] segments = new LineSegment2D[intersections.Length + 1];
            Point2D start = segment.getP1();
            for (int i = 0; i < intersections.Length; i++)
            {
                segments[i] = new LineSegment2D(start, intersections[i]);
                start = intersections[i];
            }

            segments[segments.Length - 1] = new LineSegment2D(start, segment.getP2());

            return segments;
        }

        public Point2D[] GetIntersections(LineSegment2D segment)
        {
            // This method returns intersections in order from start to end point of segment.
            List<Point2D> intersections = new List<Point2D>();

            int numVertices = getNumVertices();
            for (int i = 0; i < numVertices; i++)
            {
                Point2D p1 = GetVertex(i);
                Point2D p2 = GetVertex((i + 1) % numVertices);
                LineSegment2D s = new LineSegment2D(p1, p2);
                if (segment.intersects(s))
                    intersections.Add(segment.intersection(s, true));
            }

            if (segment.getP1().GetX() < segment.getP2().GetX())
            {
                intersections.Sort(new Comparison<Point2D>(xAscendingComparer));
            }
            else if (segment.getP1().GetX() > segment.getP2().GetX())
            {
                intersections.Sort(new Comparison<Point2D>(xDescendingComparer));
            }
            else if (segment.getP1().GetY() < segment.getP2().GetY())
            {
                intersections.Sort(new Comparison<Point2D>(yAscendingComparer));
            }
            else
            {
                intersections.Sort(new Comparison<Point2D>(yDescendingComparer));
            }

            return intersections.ToArray();
        }

        private int xAscendingComparer(Point2D p1, Point2D p2)
        {
            return p1.GetX().CompareTo(p2.GetX());
        }

        private int xDescendingComparer(Point2D p1, Point2D p2)
        {
            return p2.GetX().CompareTo(p1.GetX());
        }

        private int yAscendingComparer(Point2D p1, Point2D p2)
        {
            return p1.GetY().CompareTo(p2.GetY());
        }

        private int yDescendingComparer(Point2D p1, Point2D p2)
        {
            return p2.GetY().CompareTo(p1.GetY());
        }

        public int getNumVertices()
        {
            return vertices.Length;
        }
        
        public Polyline2D[] ClipByCopyAttributes(Polygon2D polygon)
        {
            Polyline2D[] clipped = this.ClipBy(polygon);

            for (int i = 0; i < clipped.Length; i++)
            {
                for (int j = 0; j < this.NumAttributes; j++)
                {
                    clipped[i].SetAttribute(this.GetAttribute(j));
                }
            }

            return clipped;
        }

        public Polyline2D[] ClipBy(Polygon2D polygon)
        {
            // This method returns zero to many polyline features that contain the pieces of this polyline contained in the specified polygon.

            // If the ranges do not overlap, return an empty array.
            if (!this.Range.overlaps(polygon.Range))
            {
                return new Polyline2D[0];
            }

            // Make a list for the clipped lines.
            List<Polyline2D> clippedPolylines = new List<Polyline2D>();

            // Clip all segments.
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                // Make a line segment object.
                Point2D start = vertices[i];
                Point2D end = vertices[i + 1];
                LineSegment2D segment = new LineSegment2D(start, end);

                // Split the segment.
                LineSegment2D[] split = polygon.SplitSegment(segment);

                // The resulting segments may or may not be in the polygon. Only keep those that are in the polygon.
                for (int j = 0; j < split.Length; j++)
                {
                    if (polygon.Contains(split[j].Midpoint()))
                    {
                        clippedPolylines.Add(split[j].ToPolyline());
                    }
                }
            }

            // Reassemble the polylines (at this point, they're just segments) where possible.
            for (int j = 0; j < clippedPolylines.Count; j++)
            {
                // Get the geometry at the current index.
                Polyline2D line1 = clippedPolylines[j];

                // Compare the polyline to the rest of the polylines in the list.
                bool foundConnection = false;
                for (int i = j + 1; i < clippedPolylines.Count && !foundConnection; i++)
                {
                    // Get line 2 and declare the line for the combination of the compared lines.
                    Polyline2D line2 = clippedPolylines[i];
                    Polyline2D combinedPolyline = null;

                    // This is the case when the end of line 1 connects to the beginning of line 2.
                    if (line1.GetVertex(line1.NumVertices - 1).Equals(line2.GetVertex(0)))
                    {
                        // Make the combined polyline.
                        Point2D[] vertices = new Point2D[line1.NumVertices + line2.NumVertices - 1];
                        for (int k = 0; k < line1.NumVertices; k++) {
                            vertices[k] = line1.GetVertex(k);
                        }
                        for (int k = 1; k < line2.NumVertices; k++) {
                            vertices[k + line1.NumVertices - 1] = line2.GetVertex(k);
                        }
                        combinedPolyline = new Polyline2D(vertices);
                    }

                    // This is the case when the end of line 2 connects to the beginning of line 1.
                    else if (line2.GetVertex(line2.NumVertices - 1).Equals(line1.GetVertex(0)))
                    {
                        Point2D[] vertices = new Point2D[line1.NumVertices + line2.NumVertices - 1];
                        for (int k = 0; k < line2.NumVertices; k++) {
                            vertices[k] = line2.GetVertex(k);
                        }
                        for (int k = 1; k < line1.NumVertices; k++) {
                            vertices[k + line2.NumVertices - 1] = line1.GetVertex(k);
                        }
                        combinedPolyline = new Polyline2D(vertices);
                    }

                    // If the geometry was combined, add it to the end of the list, set the flag,
                    // remove the pieces, and step the index back so the next geometry gets checked.
                    if (combinedPolyline != null)
                    {
                        // Remove i first so the position of j doesn't change.
                        clippedPolylines.RemoveAt(i);
                        clippedPolylines.RemoveAt(j);

                        // Add the combined polyline.
                        clippedPolylines.Add(combinedPolyline);

                        // Set the flag to stop iteration.
                        foundConnection = true;

                        // Step the j index back so the polyline that just fell into the j position will be examined.
                        j--;
                    }
                }
            }

            // Return the result.
            return clippedPolylines.ToArray();
        }
       
        public void Write(IO.BigEndianBinaryWriter file)
        {
            //assumes no multipart features

            //shape type 
            file.Write(shapeType);

            //write the bbox            
            file.Write(Range.getXRange().getMin());
            file.Write(Range.getYRange().getMin());
            file.Write(Range.getXRange().getMax());
            file.Write(Range.getYRange().getMax());

            //write the number of parts
            file.Write(1);

            //write the number of points
            file.Write(NumVertices);

            //write offset to the first part
            file.Write(0);

            //write the points
            foreach (Point2D point in vertices)
            {
                file.Write(point.X);
                file.Write(point.Y);
            }
            return;
        }

        private double[] getXCoords()
        {
            // Make an array of the x-coordinates of this shape.
            int n = this.NumVertices;
            double[] xCoords = new double[n];
            for (int i = 0; i < n; i++)
            {
                xCoords[i] = this.vertices[i].X;
            }
            // Return the array.
            return xCoords;
        }

        private double[] getYCoords()
        {
            // Make an array of the y-coordinates of this shape.
            int n = this.NumVertices;
            double[] yCoords = new double[n];
            for (int i = 0; i < n; i++)
            {
                yCoords[i] = this.vertices[i].Y;
            }

            // Return the array.
            return yCoords;
        }

        public double GetLength()
        {
            // Sum the lengths of all segments in the polyline.
            double length = 0.0;
            for (int i = 0; i < this.NumVertices - 1; i++)
            {
                double dx = this.vertices[i].X - this.vertices[i + 1].X;
                double dy = this.vertices[i].Y - this.vertices[i + 1].Y;
                length += Math.Sqrt(dx * dx + dy * dy);
            }

            // Return the result.
            return length;
        }

        internal double GetDistance(Polyline2D polyline)
        {
            double minDistance = double.MaxValue;

            for (int j = 0; j < this.getNumVertices() - 1; j++)
            {
                for (int i = 0; i < polyline.getNumVertices() - 1; i++)
                {
                    LineSegment2D s1 = new LineSegment2D(this.GetVertex(j), this.GetVertex(j + 1));
                    LineSegment2D s2 = new LineSegment2D(polyline.GetVertex(i), polyline.GetVertex(i + 1));

                    double distance = s1.distance(s2);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }
                }
            }
            return minDistance;
        }
    }
}
