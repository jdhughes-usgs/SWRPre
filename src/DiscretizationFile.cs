using System;
using System.IO;
using System.Text.RegularExpressions;


namespace SWRPre
{
    public class DiscretizationFile
    {
        // These variables are read from the file.
        int nlay, nrow, ncol, nper, itmuni, lenuni;
        int[] laycbd;
        float[] delr, delc;
        float[,] top;
        float[][,] botm;               

        DiscretizationFile(int nlay, int nrow, int ncol, int nper, int itmuni, int lenuni, int[] laycbd, float[] delr, float[] delc, float[,] top,
            float[][,] botm)
        {
            // Store all values.
            this.nlay = nlay;
            this.nrow = nrow;
            this.ncol = ncol;
            this.nper = nper;
            this.itmuni = itmuni;
            this.lenuni = lenuni;
            this.laycbd = laycbd;
            this.delr = delr;
            this.delc = delc;
            this.top = top;
            this.botm = botm;
            //this.stressPeriods = stressPeriods;

            // Make model layer objects.
            //modelLayers = new ModelLayer[nlay];
            int bottomIndex = 0;
            for (int i = 0; i < nlay; i++)
            {
                // Get the upper bound for this layer.
                float[,] upperBound;
                if (i == 0)
                    upperBound = top;
                else
                    upperBound = botm[bottomIndex - 1];                
            }
        }
                
        public static DiscretizationFile fromFile(string file, int unitNumber, bool freeFormat)
        {
            StreamReader sr = null;
            try
            {
                // Open the file for reading.
                sr = File.OpenText(file);

                // Register the reader with the unit number.
                //Unit.registerReader(sr, unitNumber);

                // Discard the comment lines.
                string currentLine = sr.ReadLine();
                while (currentLine.StartsWith("#"))
                {
                    currentLine = sr.ReadLine();
                }

                // The current line is now the NLAY NROW NCOL NPER ITMUNI LENUNI line. Get the values from this line (element one of the file).
                String[] split = currentLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int nlay = int.Parse(split[0]);
                int nrow = int.Parse(split[1]);
                int ncol = int.Parse(split[2]);
                int nper = int.Parse(split[3]);
                int itmuni = int.Parse(split[4]);
                int lenuni = int.Parse(split[5]);

                // Read element two of the file.
                // LAYCBD(NLAY)
                int[] laycbd = new int[nlay];
                split = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);  // ASSUMPTION: fixed width of 2.
                for (int i = 0; i < laycbd.Length; i++)
                    laycbd[i] = int.Parse(split[i]);

                // Item 3:
                /// DELR(NCOL) - U1DREL
                float[] delr = new float[ncol]; // ReadUtil.u1drel(sr, ncol);

                // Item 4: 
                /// DELC(NROW) - U1DREL
                float[] delc = new float[nrow]; // ReadUtil.u1drel(sr, nrow);

                // Read element five of the file.
                // Top(NCOL,NROW) - U2DREL
                float[,] top = new float[ncol, nrow]; // ReadUtil.u2drel(sr, ncol, nrow, freeFormat, 8, nameFilePath);

                // Read element six of the file.
                // BOTM(NCOL,NROW) - U2DREL
                // Item 6 is repeated for each model layer and Quasi-3D confining bed in the grid. 
                // These layer variables are read in sequence going down from the top of the system. 
                // Thus, the number of BOTM arrays must be NLAY plus the number of Quasi-3D confining beds.
                int numQuasi3DConfiningBeds = getNumQuasi3DConfiningBeds(laycbd);
                int numBottomArrays = nlay + numQuasi3DConfiningBeds;
                float[][,] botm = new float[numBottomArrays][,];
                for (int i = 0; i < botm.Length; i++)
                {
                    botm[i] = new float[ncol, nrow]; // ReadUtil.u2drel(sr, ncol, nrow, freeFormat, 8, nameFilePath);
                }                
                // Close the file.
                sr.Close();                
                return new DiscretizationFile(nlay, nrow, ncol, nper, itmuni, lenuni, laycbd, delr, delc, top, botm);
            }
            catch
            {
                if (sr != null)
                {
                    try
                    {
                        sr.Close();
                    }
                    catch { }
                }
                return null;
            }
        }

        private static int getNumQuasi3DConfiningBeds(int[] laycbd)
        {
            // LAYCBDâ€”is a flag, with one value for each model layer, that indicates whether or not a 
            // layer has a Quasi-3D confining bed below it. 0 indicates no confining bed, and not zero 
            // indicates a confining bed. LAYCBD for the bottom layer must be 0.

            // Make a counter for the number of Quasi-3D confining beds.
            int numQuasi3DConfiningBeds = 0;

            // Count the number of confining beds.
            for (int i = 0; i < laycbd.Length; i++)
                if (laycbd[i] != 0)
                    numQuasi3DConfiningBeds++;

            // Return the number of confining beds.
            return numQuasi3DConfiningBeds;
        }
        
        public static bool GetInfoTemporary(string file, out float[] xWidths, out float[] yWidths)
        {
            bool successful;
            StreamReader sr = null;
            try
            {
                // Open the file for reading.
                sr = File.OpenText(file);

                // Discard leading comment lines.
                string line = sr.ReadLine();
                while (line.StartsWith("#"))
                {
                    line = sr.ReadLine();
                }
                Console.WriteLine("The line is " + line);

                // Get the number of rows and columns from the first non-comment line.
                string[] lineSplit = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                int nRows = int.Parse(lineSplit[1]);
                int nCols = int.Parse(lineSplit[2]);

                // Discard the confining-bed line.
                sr.ReadLine();

                // Read the column widths (DELR).
                xWidths = readFloatArray1D(sr, nCols);

                // Read the row widths (DELC).
                yWidths = (readFloatArray1D(sr, nRows));
                Array.Reverse(yWidths);
                
                // Return true to indicate success.
                successful = true;
            }
            catch (Exception e)
            {
                xWidths = null;
                yWidths = null;
                successful = false;

                throw e;
            }

            // Close the file.
            if (sr != null)
            {
                try
                {
                    sr.Close();
                }
                catch { }
            }

            // return the result indicating whether we were successful in reading the file.
            return successful;
        }

        public static float[] readFloatArray1D(StreamReader sr, int numValues)
        {
            bool oc = false;
            int i = 0;
            int j = 0;
            int locat = -999;                        
            int iprn = -999;
            int npl = -999;
            int npi = -999;
            float cnstnt = -999.9f;
            string fmtin = "";
            string[] freeformat = new string[4] { "open/close", "external", "internal", "constant" };
            string ffkeyword = "";

            // Read the line.
            string line = sr.ReadLine();            

            // Split on whitespace and parentheses.
            string[] split = line.Split(new char[] { ' ', '\t', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            //first check for free-format options            
            foreach(string ff in freeformat)
            {
                if (String.Compare(split[0],ff,true) == 0)
                {
                    ffkeyword = split[0];
                }
            }

            // this isn't free format, so...
            if (ffkeyword.Length == 0)
            {               
                locat = int.Parse(line.Substring(0,10));
                cnstnt = float.Parse(line.Substring(10,10));
                string[] st_fmtin = line.Substring(20, 20).Split(new char[] { ' ', '\t', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                fmtin = st_fmtin[0];
                iprn = int.Parse(line.Substring(40,10));                
                // read the horrible fmtin
                
                for (i = 0; i < fmtin.Length; i++)
                {
                    if (!Char.IsNumber(fmtin[i]))
                    {
                        break;
                    }
                }
                npl = int.Parse(fmtin.Substring(0, i));
                for (j = i+1;j<fmtin.Length;j++)
                {
                    //Console.WriteLine(fmtin[j]);
                    if (!Char.IsNumber(fmtin[j]))
                    {
                        break;
                    }
                }
                npi = int.Parse(fmtin.Substring(i+1, j-(i+1)));
                Console.WriteLine("locat " + locat);
                Console.WriteLine("cnstnt " + cnstnt);
                Console.WriteLine("fmtin " + fmtin);
                Console.WriteLine("iprn " + iprn);
                Console.WriteLine("npl,npi"+' '+npl+' '+npi);
            }
            
            // Make and populate an array with the values.
            float[] values = new float[numValues];
                        
            // for constant values
            if ((String.Compare(ffkeyword,freeformat[3],true) == 0) || (locat == 0))
            {
                float value = float.Parse(split[1]);                
                for (i = 0; i < numValues; i++)
                {
                    values[i] = value;
                }
            }
            // everything else not external - we try for fixed format although it can be external - no way of knowing so try...            
            else if ((String.Compare(ffkeyword, freeformat[1], true) != 0))
            {
                fmtin = "free";
                //open a new file if open/close
                StreamReader sr2 = null;
                if (String.Compare(ffkeyword, freeformat[0], true) == 0)
                {
                    
                    sr2 = File.OpenText(split[1]);
                    oc = true;
                }
                    
                i = 0;
                while (i < numValues)
                {
                    string line2 = "";
                    if (oc) line2 = sr2.ReadLine();
                    else line2 = sr.ReadLine();
                    if (String.Compare(fmtin, "free", true) == 0)
                    {
                        split = Regex.Split(line2, @"\s+");
                        
                        foreach (string s in split)
                        {
                            if (!String.IsNullOrEmpty(s))
                            {
                                values[i] = float.Parse(s);
                                i++;
                            }
                        }
                    }
                    else
                    {
                        for (j = 0; j < npl; j++)
                        {
                            string svalue = line2.Substring(j * npi, npi);
                            values[i] = float.Parse(svalue);
                            i++;
                            if (i >= numValues) break;
                        }
                    }
                }
                if (oc) sr2.Close();
            }                       
            return values;
        }
    }
}
