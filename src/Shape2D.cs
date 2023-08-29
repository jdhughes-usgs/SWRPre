using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using MapTools;
//using USGS.Puma;


namespace SWRPre
{
    public class Shape2D
    {
        public List<ShapeAttribute> attributes;
        
        public Shape2D()
        {
            // Make the attribute list.
            attributes = new List<ShapeAttribute>();
        }

        public void SetAttribute(ShapeAttribute attribute)
        {
            this.SetAttribute(attribute.Name, attribute.Value);
        }

        public void SetAttribute(string name, object value)
        {
            // If the attribute exists, remove it.
            int existingAttribute_idx = -1;
            ShapeAttribute existingAttribute = attributes.Find(
                delegate(ShapeAttribute attribute)
                {
                    return attribute.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                });
            if (existingAttribute != null)
            {
                // If the attribute exists, get the position and remove it
                existingAttribute_idx = attributes.FindIndex(
                    delegate(ShapeAttribute attribute)
                    {
                        return attribute.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                    });
                attributes.Remove(existingAttribute);
            }
            // Add the new attribute.
            if (existingAttribute_idx != -1)
                attributes.Insert(existingAttribute_idx,new ShapeAttribute(name,value));
            else
                attributes.Add(new ShapeAttribute(name, value));
            return;

        }

        public object GetAttributeValue(string name, object defaultValue)
        {
            // Get the attribute.
            ShapeAttribute attribute = this.GetAttribute(name);

            // If the attribute is null, return the default value.
            if (attribute == null)
            {
                return defaultValue;
            }

            // Otherwise, return the attribute value.
            else
            {
                return attribute.Value;
            }
        }

        public object GetAttributeValue(string name)
        {
            return GetAttributeValue(name, null);
        }

        public ShapeAttribute GetAttribute(string name)
        {
            ShapeAttribute existingAttribute = attributes.Find(
                delegate(ShapeAttribute attribute)
                {
                    return attribute.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                });

            return existingAttribute;
        }

        public ShapeAttribute GetAttribute(int index)
        {
            return attributes[index];
        }

        public int NumAttributes
        {
            get
            {
                return attributes.Count;
            }
        }

        public void castListToString()
        {                     
            for (int i=0;i<NumAttributes;i++)
            {
                ShapeAttribute attrib = attributes[i];
                Type type = attrib.Value.GetType();
                if (type == typeof(List<int>))
                {                                        
                    string cast = listToString((List<int>)attrib.Value);                    
                    SetAttribute(attrib.Name, cast);                   
                }
                else if (type == typeof(List<double>))
                {                   
                    string cast = listToString((List<double>)attrib.Value);
                    SetAttribute(attrib.Name, cast);
                }                
            }
            return;
        }
        
        private static string listToString(List<int> list)
        {
            if (list.Count == 0) { return ""; }
            else if (list.Count == 1)
            {
                return list[0].ToString();
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                for (int c = 0; c < list.Count - 1; c++)
                {
                    builder.Append(list[c]).Append(" ");
                }
                builder.Append(list[list.Count - 1]);
                string lstr = builder.ToString();
                return lstr;
            }
        }

        private static string listToString(List<double> list)
        {
            if (list.Count == 0) { return ""; }
            else if (list.Count == 1)
            {
                return list[0].ToString();
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                for (int c = 0; c < list.Count; c++)
                {
                    if (list[c] > 0.0)
                    {
                        string str = Math.Round(list[c], 1).ToString();
                        builder.Append(str).Append(" ");
                    }
                    else
                    {
                        builder.Append(list[c]).Append(" ");
                    }
                }
                //builder.Append(distList[distList.Count - 1]);
                builder.Remove(builder.Length - 1, 1);
                string lstr = builder.ToString();
                return lstr;
            }
        }
        
        public static string[] GetShapefileFieldNames(string shapefilePath)
        {            
            string dbfPath = shapefilePath.Substring(0, shapefilePath.Length - 3) + "dbf";
            IO.DbaseFileReader dbf = new IO.DbaseFileReader(dbfPath);
            IO.DbaseFileHeader dbfHeader = dbf.GetHeader();
            int numFields = dbfHeader.NumFields;
            string[] fieldNames = new string[numFields];

            for (int i=0;i<numFields;i++)
            {
                IO.DbaseFieldDescriptor desc = dbfHeader.Fields[i];
                fieldNames[i] = desc.Name;
            }

            return fieldNames;
        }

        public ArrayList GetAttributes()
        {
            
            ArrayList values = new ArrayList();
            foreach (ShapeAttribute a in this.attributes)
            {
                values.Add(a.Value);
            }
            return values;
        }
        
        protected static void CleanForShapefile(string baseFilename)
        {
            string[] extensions = { "shp", "shx", "sbn", "sbx", "dbf", "prj" };

            for (int i = 0; i < extensions.Length; i++)
            {
                string filename = baseFilename + "." + extensions[i];
                if (File.Exists(filename))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteError("Error in cleaning up a shapefile. Unable to delete " + filename + ". ERROR: " + e.Message + ".");
                    }
                }
            }
        }
    }
}
