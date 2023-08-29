using System;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SWRPre.IO
{	
	public class ShapefileReader
	{        		

		public ShapefileHeader ShpHeader = null;
        public DbaseFileHeader DbfHeader = null;
		private string _filename;
        private BigEndianBinaryReader _shpBinaryReader;
        private DbaseFileReader _dbfBinaryReader;
        private IEnumerator _dbfEnumerator;
		
		/// <summary>
		/// Initializes a new instance of the Shapefile class with the given parameters.
		/// </summary>
		/// <param name="filename">The filename of the shape file to read (with .shp).</param>
		/// <param name="geometryFactory">The GeometryFactory to use when creating Geometry objects.</param>
		public ShapefileReader(string filename)
		{           
			if (filename == null)
				throw new ArgumentNullException("filename");
			
            _filename = filename;		
			
            //shp
            FileStream stream = new FileStream(filename, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
			BigEndianBinaryReader shpBinaryReader = new BigEndianBinaryReader(stream);
			ShpHeader = new ShapefileHeader(shpBinaryReader);
            _shpBinaryReader = shpBinaryReader;		

            //dbf
            string dbfname = filename.Substring(0, filename.Length - 3) + "dbf";
            DbaseFileReader dbfBinaryReader = new DbaseFileReader(dbfname);
            DbfHeader = dbfBinaryReader.GetHeader();
            _dbfBinaryReader = dbfBinaryReader;
            _dbfEnumerator = _dbfBinaryReader.GetEnumerator();
            return;
	


		}

        		
		public Polyline2D[] ReadAll()
		{
			List<Polyline2D> polylines = new List<Polyline2D>();
            IO.ShapefileHeader.ShapeTypes shapeType = ShpHeader.ShapeType;
			//make sure we are at the right place
            _shpBinaryReader.BaseStream.Seek(100,SeekOrigin.Begin);
            while (true)
            {
                if (_shpBinaryReader.PeekChar() != -1)
                {                    
                    //try
                    //{
                        //read the header for this entry
                        int recordNumber = _shpBinaryReader.ReadInt32BE();
                        int contentLength = _shpBinaryReader.ReadInt32BE();
                        //read this polyline
                        Polyline2D poly = ReadShp();
                        polylines.Add(poly);

                    //}
                    //catch (Exception) { break; }                    
                }
                else                
                    break;               
            }
            return polylines.ToArray();				
        }
        //read the next dbf entry
        public ShapeAttribute[] ReadDbf()
        {
            bool more = _dbfEnumerator.MoveNext();
            ShapeAttribute[] shp_attributes;
            if (more)
            {
                ArrayList attribute_values = (ArrayList)_dbfEnumerator.Current;
                if (attribute_values.Count != DbfHeader.Fields.Length)
                    throw new ArgumentOutOfRangeException("DBF record does not have the correct number of fields");
                shp_attributes = new ShapeAttribute[attribute_values.Count];
                for (int i = 0; i < attribute_values.Count; i++)
                {
                    ShapeAttribute a = new ShapeAttribute(DbfHeader.Fields[i].Name, attribute_values[i]);
                    shp_attributes[i] = a;
                }
            }
            else
                throw new ArgumentOutOfRangeException("DBF file does not have the same number of records as SHP");            
            return shp_attributes;
        }

        public Polyline2D ReadShp()
        {
            int shapeTypeNum = _shpBinaryReader.ReadInt32();
            if (shapeTypeNum != 3)
                throw new System.NotImplementedException("Only Polylines of shape type '3' are supported");            
            Range2D range = new Range2D(_shpBinaryReader);
            int nparts = _shpBinaryReader.ReadInt32();
            if (nparts > 1)
                throw new System.NotImplementedException("Only single-part polylines are supported");
            int npoints = _shpBinaryReader.ReadInt32();
            //read the part number
            int pnum = _shpBinaryReader.ReadInt32();
            //read the points
            List<Point2D> vertices = new List<Point2D>();
            for (int i = 0; i < npoints; i++)
            {
                double x = _shpBinaryReader.ReadDouble();
                double y = _shpBinaryReader.ReadDouble();
                vertices.Add(new Point2D(x, y));
            }
            //a new polyline instance
            Polyline2D poly = new Polyline2D(vertices.ToArray());
            //NEED TO DO SOMETHING WITH THE BBOX...

            //read the dbf attributes and add to polyline
            ShapeAttribute[] attributes = ReadDbf();
            foreach (ShapeAttribute a in attributes)
                poly.SetAttribute(a);
            
            return poly;
        }

		        
	}
}
