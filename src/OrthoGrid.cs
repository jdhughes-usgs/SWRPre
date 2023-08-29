using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SWRPre
{
    public class OrthoGrid
    {
        public const string KEY_COL = "column";
        public const string KEY_ROW = "row";

        private float[] xWidths;
        private float[] yWidths;
        private float[] xCoordinates;
        private float[] yCoordinates;
        private Point2D anchor;
        private double rotation;
        private bool southernHemisphere;

        public OrthoGrid(float[] xWidths, float[] yWidths, Point2D anchor, double rotation,
            bool southernHemisphere)
        {
            // Store the data.
            this.xWidths = xWidths;
            this.yWidths = yWidths;
            this.anchor = anchor;
            this.rotation = rotation;
            this.southernHemisphere = southernHemisphere;
        }

        public int NCols
        {
            get
            {
                return xWidths.Length;
            }
        }

        public int NRows
        {
            get
            {
                return yWidths.Length;
            }
        }

        public float[] XCoordinates
        {
            get
            {
                if (xCoordinates == null)
                {
                    calculateXCoordinates();
                }
                return xCoordinates;
            }
        }

        public float[] YCoordinates
        {
            get
            {
                if (yCoordinates == null)
                {
                    calculateYCoordinates();
                }
                return yCoordinates;
            }
        }

        private void calculateXCoordinates()
        {
            int nCols = this.NCols;
            xCoordinates = new float[nCols + 1];

            xCoordinates[0] = (float)anchor.X;
            for (int i = 1; i < nCols + 1; i++)
            {
                xCoordinates[i] = xCoordinates[i - 1] + xWidths[i - 1];
            }
        }

        private void calculateYCoordinates()
        {
            int nRows = this.NRows;
            yCoordinates = new float[nRows + 1];

            yCoordinates[nRows] = (float)anchor.Y;
            if (this.southernHemisphere)
            {
                for (int i = 1; i < nRows + 1; i++)
                {
                    yCoordinates[i] = yCoordinates[i - 1] + yWidths[i - 1];
                }
            }
            else
            {
                for (int i = nRows - 1; i >= 0; i--)
                {
                    yCoordinates[i] = yCoordinates[i + 1] + yWidths[i];
                }
            }
        }

        public Polygon2D[] GetPolygons()
        {
            int nCols = this.NCols;
            int nRows = this.NRows;

            // Get the cell coordinates.
            float[] xCoordinates = this.XCoordinates;
            float[] yCoordinates = this.YCoordinates;
            
            List<Polygon2D> polygons = new List<Polygon2D>();
            
            // Make a polygon for each cell.           
            for (int j = 0; j < nRows; j++)            
            {
                for (int i = 0; i < nCols; i++)                
                {

                    // Make the polygon.
                    double x0 = xCoordinates[i];
                    double x1 = xCoordinates[i + 1];
                    double y0 = yCoordinates[j];
                    double y1 = yCoordinates[j + 1];
                    Point2D p0 = new Point2D(x0, y0);
                    Point2D p1 = new Point2D(x1, y0);
                    Point2D p2 = new Point2D(x1, y1);
                    Point2D p3 = new Point2D(x0, y1);
                    if (rotation != 0.0)
                    {
                        p0 = p0.RotateAbout(anchor, rotation);
                        p1 = p1.RotateAbout(anchor, rotation);
                        p2 = p2.RotateAbout(anchor, rotation);
                        p3 = p3.RotateAbout(anchor, rotation);
                    }

                    Polygon2D polygon = new Polygon2D(new Point2D[] { p0, p1, p2, p3,p0 });

                    // Attach the row and column.
                    polygon.SetAttribute(KEY_COL, i + 1);
                    polygon.SetAttribute(KEY_ROW, j + 1);
                    polygons.Add(polygon);
                }
            }
            return polygons.ToArray();
        }

        public Polygon2D getBoundingBox()
        {
            double x0 = xCoordinates[0];
            double x1 = xCoordinates[xCoordinates.Length - 1];
            double y0 = yCoordinates[0];
            double y1 = yCoordinates[yCoordinates.Length - 1];
            Point2D p0 = new Point2D(x0, y0);
            Point2D p1 = new Point2D(x1, y0);
            Point2D p2 = new Point2D(x1, y1);
            Point2D p3 = new Point2D(x0, y1);
            if (rotation != 0.0)
            {
                p0 = p0.RotateAbout(anchor, rotation);
                p1 = p1.RotateAbout(anchor, rotation);
                p2 = p2.RotateAbout(anchor, rotation);
                p3 = p3.RotateAbout(anchor, rotation);
            }

            Polygon2D polygon = new Polygon2D(new Point2D[] { p0, p1, p2, p3 });

            return polygon;
        }

        public void ToFile(string filename)
        {           
            IO.ShapefileWriter shpWtr = new IO.ShapefileWriter();
            shpWtr.Write(filename, GetPolygons());
            return;
        }
    }
}
