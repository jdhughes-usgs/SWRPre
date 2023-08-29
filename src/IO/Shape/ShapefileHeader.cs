using System;
using System.Diagnostics;

namespace SWRPre.IO
{
	/// <summary>
	/// Class that represents a shape file header record.
	/// </summary>
	public class ShapefileHeader
	{
		public int FileCode = -1;
        public int ShapeCode = 9994;
        public int FileLength = -1;
        public int Version = 1000;       
        public Range2D Range;        
        public double[] VRange;
        public ShapeTypes ShapeType = ShapeTypes.NullShape;
        			
		public ShapefileHeader(BigEndianBinaryReader shpBinaryReader)
		{
			if (shpBinaryReader == null)
				throw new ArgumentNullException("shpBinaryReader");

			FileCode = shpBinaryReader.ReadInt32BE();	
			if (FileCode != ShapeCode)
				throw new Exception("The first four bytes of this file indicate this is not a shape file.");

			// skip 5 unsed bytes
			shpBinaryReader.ReadInt32BE();
			shpBinaryReader.ReadInt32BE();
			shpBinaryReader.ReadInt32BE();
			shpBinaryReader.ReadInt32BE();
			shpBinaryReader.ReadInt32BE();

			FileLength = shpBinaryReader.ReadInt32BE();

			Version = shpBinaryReader.ReadInt32();			
			int shapeType = shpBinaryReader.ReadInt32();
            
			//read in and store the range
			double[] coords = new double[4];
			for (int i = 0; i < 4; i++)
				coords[i] = shpBinaryReader.ReadDouble();			
            Range = new Range2D(new Range1D(coords[0], coords[2]), new Range1D(coords[1], coords[3]));
			
            //read in and store the vertical range
            double minZ = shpBinaryReader.ReadDouble();
            double maxZ = shpBinaryReader.ReadDouble();            
            VRange = new double[] { minZ, maxZ };

			// skip the m bounding box.
			for (int i = 0; i < 2; i++)
				shpBinaryReader.ReadDouble();	
		}
		
		public ShapefileHeader() { }
						
		public void Write(BigEndianBinaryWriter file) 
		{
			if (file == null)
				throw new ArgumentNullException("file");
			if (FileLength==-1)
				throw new InvalidOperationException("The header properties need to be set before writing the header record.");
			int pos = 0;
			file.WriteIntBE(ShapeCode);
			pos += 4;
			for (int i = 0; i < 5; i++)
			{
				file.WriteIntBE(0);//Skip unused part of header
				pos += 4;
			}
			file.WriteIntBE(FileLength);
			pos += 4;
			file.Write(Version);
			pos += 4;

            file.Write(int.Parse(Enum.Format(typeof(ShapeTypes), ShapeType, "d")));		    
            pos += 4;
			
            // Write the bounding box
            //file.Write(Bounds.XMin);
            //file.Write(Bounds.YMin);
            //file.Write(Bounds.XMax);
            //file.Write(Bounds.YMax);
            file.Write(Range.getXRange().getMin());
            file.Write(Range.getXRange().getMax());
            file.Write(Range.getYRange().getMin());
            file.Write(Range.getYRange().getMax());


			pos += 8 * 4;

            if (VRange != null)
            {
                file.Write(VRange[0]);
                file.Write(VRange[1]);
                pos += 16;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    file.Write(0.0); // Skip if no vertical envelope is set
                    pos += 8;
                }		
            }

			// Skip remaining unused bytes
			for (int i = 0; i < 2; i++)
			{
				file.Write(0.0); // Skip unused part of header
				pos += 8;
			}

		}
        public enum ShapeTypes
        {
            
            NullShape = 0,

            Point = 1,

            LineString = 3,

            Polygon = 5,

            MultiPoint = 8,

            PointZ = 11,

            LineStringZ = 13,

            PolygonZ = 15,

            MultiPointZ = 18,

            PointM = 21,

            LineStringM = 23,
        
            PolygonM = 25,

            MultiPointM = 28,

            MultiPatch = 31,

            PointZM = 35,

            LineStringZM = 36,

            PolygonZM = 37,

            MultiPointZM

        }        
	}
}
