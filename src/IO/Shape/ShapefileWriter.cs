using System;
using System.Collections;
using System.Diagnostics;

namespace SWRPre.IO
{
    

	public class ShapefileWriter
	{                       
        public ShapefileWriter(){}        		
		public void Write(string filename, Polygon2D[] polygons)
		{
			System.IO.FileStream shpStream = new System.IO.FileStream(filename + ".shp", System.IO.FileMode.Create);
			System.IO.FileStream shxStream = new System.IO.FileStream(filename + ".shx", System.IO.FileMode.Create);
			BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
			BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);
            IO.ShapefileHeader.ShapeTypes shapeType = ShapefileHeader.ShapeTypes.Polygon;
            
            //create and build dbfHeader
            DbaseFileHeader dbfHeader = new DbaseFileHeader();
            dbfHeader.BuildHeader(polygons[0]);
            dbfHeader.NumRecords = polygons.Length;

            //create dbfWriter
            DbaseFileWriter dbfWriter = new DbaseFileWriter(filename + ".dbf");
            //write the dbf header
            dbfWriter.Write(dbfHeader);

			int numShapes = polygons.Length;
			
            // calc the length of the shp file, so it can put in the header.                      
			int shpLength = 50; //file header length in WORDS            
            Range2D shpRange = new Range2D();
			for (int i = 0; i < numShapes; i++) 
			{
				Polygon2D poly = polygons[i];
                int length = GetRecordLength(poly);                
                shpRange.checkSetRange(poly.Range);

				shpLength += 4; // length of header in WORDS
				shpLength += length; // length of shape in words
			}

			int shxLength = 50 + (4*numShapes); //length in WORDS
            

			// write the .shp header
			ShapefileHeader shpHeader = new ShapefileHeader();
			shpHeader.FileLength = shpLength; //number of 16-bit WORDS            
            shpHeader.Range = shpRange;            
            shpHeader.VRange = new double[] { 0.0, 0.0 };
                                    

			// assumes Geometry type of the first item will the same for all other items
			// in the collection.
            shpHeader.ShapeType = shapeType;
			shpHeader.Write(shpBinaryWriter);

			// write the .shx header
			ShapefileHeader shxHeader = new ShapefileHeader();
			shxHeader.FileLength = shxLength;//number of 16-bit WORDS			
            shxHeader.Range = shpHeader.Range;
            shxHeader.VRange = shpHeader.VRange;


            shxHeader.ShapeType = shpHeader.ShapeType;
            shxHeader.Write(shxBinaryWriter);

			// write the individual records.
			int _pos = 50; // header length in WORDS
			for (int i = 0; i < numShapes; i++) 
			{
                Polygon2D poly = polygons[i];
                int recordLength = GetRecordLength(poly);				
				shpBinaryWriter.WriteIntBE(i+1);
				shpBinaryWriter.WriteIntBE(recordLength);
								
				shxBinaryWriter.WriteIntBE(_pos);
				shxBinaryWriter.WriteIntBE(recordLength);
				
                dbfWriter.Write(poly.GetAttributes());
				
                
				//handler.Write(body, shpBinaryWriter,  geometryFactory);
                poly.Write(shpBinaryWriter);
                _pos += 4; // length of header in WORDS
				_pos += recordLength; // length of shape in WORDS
			}

			shxBinaryWriter.Flush();
			shxBinaryWriter.Close();
			shpBinaryWriter.Flush();
			shpBinaryWriter.Close();
            dbfWriter.Close();
			
			//WriteDummyDbf(filename + ".dbf", numShapes);
            return;
		}

        public void Write(string filename, Polyline2D[] polylines)
        {
            System.IO.FileStream shpStream = new System.IO.FileStream(filename + ".shp", System.IO.FileMode.Create);
            System.IO.FileStream shxStream = new System.IO.FileStream(filename + ".shx", System.IO.FileMode.Create);
            BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
            BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);
            IO.ShapefileHeader.ShapeTypes shapeType = ShapefileHeader.ShapeTypes.LineString;

            //create and build dbfHeader
            DbaseFileHeader dbfHeader = new DbaseFileHeader();
            dbfHeader.BuildHeader(polylines[0]);
            dbfHeader.NumRecords = polylines.Length;

            //create dbfWriter
            DbaseFileWriter dbfWriter = new DbaseFileWriter(filename + ".dbf");
            //write the dbf header
            dbfWriter.Write(dbfHeader);

            int numShapes = polylines.Length;

            // calc the length of the shp file, so it can put in the header.                      
            int shpLength = 50; //file header length in WORDS            
            Range2D shpRange = new Range2D();
            for (int i = 0; i < numShapes; i++)
            {
                Polyline2D poly = polylines[i];
                int length = GetRecordLength(poly);                
                shpRange.checkSetRange(poly.Range);
                shpLength += 4; // length of header in WORDS
                shpLength += length; // length of shape in words
            }

            int shxLength = 50 + (4 * numShapes); //length in WORDS


            // write the .shp header
            ShapefileHeader shpHeader = new ShapefileHeader();
            shpHeader.FileLength = shpLength; //number of 16-bit WORDS            
            shpHeader.Range = shpRange;            
            shpHeader.VRange = new double[] { 0.0, 0.0 };


            // assumes Geometry type of the first item will the same for all other items
            // in the collection.
            shpHeader.ShapeType = shapeType;
            shpHeader.Write(shpBinaryWriter);

            // write the .shx header
            ShapefileHeader shxHeader = new ShapefileHeader();
            shxHeader.FileLength = shxLength;//number of 16-bit WORDS            
            shxHeader.Range = shpHeader.Range;
            shxHeader.VRange = shpHeader.VRange;
            shxHeader.ShapeType = shpHeader.ShapeType;
            shxHeader.Write(shxBinaryWriter);

            // write the individual records.
            int _pos = 50; // header length in WORDS
            for (int i = 0; i < numShapes; i++)
            {
                Polyline2D poly = polylines[i];
                int recordLength = GetRecordLength(poly);
                shpBinaryWriter.WriteIntBE(i + 1);
                shpBinaryWriter.WriteIntBE(recordLength);

                shxBinaryWriter.WriteIntBE(_pos);
                shxBinaryWriter.WriteIntBE(recordLength);

                dbfWriter.Write(poly.GetAttributes());


                //handler.Write(body, shpBinaryWriter,  geometryFactory);
                poly.Write(shpBinaryWriter);
                _pos += 4; // length of header in WORDS
                _pos += recordLength; // length of shape in WORDS
            }

            shxBinaryWriter.Flush();
            shxBinaryWriter.Close();
            shpBinaryWriter.Flush();
            shpBinaryWriter.Close();
            dbfWriter.Close();

            //WriteDummyDbf(filename + ".dbf", numShapes);
            return;
        }
        public int GetRecordLength(Polygon2D poly)
        {
            int numParts = 1;
            int nozLength = 22 + (2 * numParts) + (poly.NumVertices * 8);
            //int zLength = 8 + (4 * NumVertices);
            //int mLength = 8 + (4 * NumVertices);
            int bodyLength = nozLength; //+ zLength + mLength;
            return bodyLength;
        }

        public int GetRecordLength(Polyline2D poly)
        {
            int numParts = 1;
            int nozLength = 22 + (2 * numParts) + (poly.NumVertices * 8);
            //int zLength = 8 + (4 * NumVertices);
            //int mLength = 8 + (4 * NumVertices);
            int bodyLength = nozLength; //+ zLength + mLength;
            return bodyLength;
        }

	}
}
