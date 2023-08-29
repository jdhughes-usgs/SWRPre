
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace SWRPre
{
    public class SwrProcessor
    {
        public enum SegmentationType
        {
            NoClip,
            Exact,
            Equal
        }
       
        private const string KEY_IREACH = "REACH";
        private const string KEY_IEQN = "EQN";
        private const string KEY_ICENUM = "RCH_GRP";
        private const string KEY_KRCH_LAY = "LAYER";
        private const string KEY_IRCH_ROW = "ROW";
        private const string KEY_JRCH_COL = "COLUMN";
        private const string KEY_RLEN = "LENGTH";
        private const string KEY_REACH_GROUP_LENGTH = "GRP_LEN";
        private const string KEY_NCONN = "NCONN";
        private const string KEY_CONN = "CONN";
        private const string KEY_DIST = "CONN_DIST";

        private const double MIN_REACH_LENGTH = 0.1;
        
        public static string[] CreateFiles(string discretizationPath, string shapefilePath, 
            Point2D anchorPoint, double rotation, bool southernHemisphere,
            double minimumElementLength, string fieldId,
            string fieldNConn, string fieldConn, SegmentationType segmentationType, 
            double reachGroupLength, bool warnFlag,
            string reachDirection, bool xmlFlag, ref string status, int startReachNumber)
        {
            List<string> result = new List<string>();
           
            //status tracking
            int progress = 0;
            Status st = new Status();
            st.Show();

            //assert that the dis and shapefiles exist
            if (File.Exists(discretizationPath) == false)
            {
                st.Close();
                throw new System.Exception("Error - discretization file not found at path: "+discretizationPath);
            }

            if (File.Exists(shapefilePath) == false)
            {
                st.Close();
                throw new System.Exception("Error - shapefile not found at path: " + shapefilePath);
            }

            // Determine the base file name.
            string discretizationDirectory = Path.GetDirectoryName(discretizationPath);
            string shapefileDirectory = Path.GetDirectoryName(shapefilePath);

            //write xml parameters file            
            if (xmlFlag)
            {
                status = DateTime.Now + " - Writing parameters file";
                st.writeTask(status);
                //st.Update();
                string xmlFile = discretizationDirectory + "\\SWRPre.xml";
                try
                {

                    writeParameters(discretizationPath, shapefilePath, 
                        anchorPoint, rotation, southernHemisphere,
                        minimumElementLength, fieldId, 
                        fieldNConn, fieldConn, segmentationType, reachGroupLength, warnFlag,
                        reachDirection, xmlFlag, xmlFile, startReachNumber);
                    result.Add(xmlFile);
                }
                catch (Exception e)
                {
                    st.Close();
                    throw e;
                }
            }

            // Get the discretization information from the discretization file.
            status = DateTime.Now + " - Loading Discritization File";
            st.writeTask(status);
            float[] xWidths, yWidths;
            DiscretizationFile.GetInfoTemporary(discretizationPath, out xWidths, out yWidths);                           

            // Get the polylines from the shapefile.            
            status = DateTime.Now + " - Loading Polyline Shapefile";
            st.writeTask(status);  
            IO.ShapefileReader shprdr = new IO.ShapefileReader(shapefilePath);
            Polyline2D[] polylines = shprdr.ReadAll();            
                                  
            //error checking...
            status = DateTime.Now + " - Checking Polylines";
            st.writeTask(status);
            List<string[]> errList = new List<string[]>();

            //check for duplicate fieldKey names
            errList.Add(checkPolylinesForDuplicates(polylines,fieldId));

            //check the connectivity
            errList.Add(checkConnectivity(polylines,fieldId,fieldNConn,fieldConn));
            
            //check zero_length polylines
            errList.Add(checkPolylineLength(polylines,fieldId));

            string errorFile = "SWRPre.err";
            bool errorFound = writeErrorFile(errList,errorFile);
            if (errorFound)
            {
                st.Close();
                throw new System.Exception("Error(s) found in the polyline shapefile.\n See " + errorFile + " for explanation");
            }

            // Make the grid.
            status = DateTime.Now + " - Creating Grid";
            st.writeTask(status);
            progress += 1;
            st.updateProgress(progress);
            OrthoGrid grid = new OrthoGrid(xWidths, yWidths, anchorPoint, rotation, southernHemisphere);
            
            // Write the grid shapefile.
            string[] disPath = discretizationPath.Split('\\');
            string disFilename = disPath[disPath.Length - 1];
            string gridPath = shapefileDirectory + "\\" + disFilename.Substring(0,disFilename.Length-4)+"_grid";
            
            result.Add(gridPath);
            status = DateTime.Now + " - Writing Grid Polygon Shapefile";
            st.writeTask(status);
            grid.ToFile(gridPath);            
            
            // Segment the polylines by the grid.
            Polyline2D[] segmentedPolylines = segmentPolylinesForSwr(polylines, grid, fieldId, fieldNConn, fieldConn, segmentationType,
                    reachGroupLength, reachDirection, ref status, ref progress, st, startReachNumber);
            
                      
            // Write the SWR dataset.
            status = DateTime.Now + " - Writing SWR Dataset";
            st.writeTask(status);
            string swrPath = getOutputFilename(discretizationPath);
            result.Add(swrPath);
            try
            {
                writeSwrDataset(segmentedPolylines, swrPath);
            }
            catch (Exception e)
            {
                st.Close();
                throw e;
            }

            // Cast list attributes to strings in place
            try
            {
                segmentedPolylines = castListAttributes(segmentedPolylines);
            }
            catch (Exception e)
            {
                st.Close();
                throw e;
            }

            // Write the polyline shapefile.
            status = DateTime.Now + " - Writing Reach Polyline Shapefile";
            st.writeTask(status);
            progress = 100;
            string polylinePath = shapefilePath.Substring(0,shapefilePath.Length-4) + "_SWRpolylines";
            result.Add(polylinePath);
            try
            {
                IO.ShapefileWriter shpwrt = new IO.ShapefileWriter();
                shpwrt.Write(polylinePath, segmentedPolylines);
            }
            catch (Exception e)
            {
                st.Close();
                throw e;
            }
            
            status = DateTime.Now + " - Done";            
            st.writeTask(status);
            st.updateProgress(progress);

            StringBuilder sb = new StringBuilder();
            st.writeTask("Output written successfully to the following paths:\n\n");
            for (int i = 0; i < result.Count; i++)
            {
                st.writeTask(result[i] + "\n");
            }                                
            return result.ToArray();            
        }
                
        private static Polyline2D[] castListAttributes(Polyline2D[] polylines)
        {            
            for (int i=0;i<polylines.Length;i++) 
                polylines[i].castListToString();                            
            return polylines;
        }

        private static string getOutputFilename(string discretizationPath)
        {
            // Get the directory of the discretization file.
            string disFolder = Path.GetDirectoryName(discretizationPath);

            // Append the filename to the directory name.
            string datasetFilename = disFolder + "\\SWRPre_dataset4.txt";

            // Return the result.
            return datasetFilename;
        }

        private static Polyline2D[] segmentPolylinesForSwr(Polyline2D[] polylines, OrthoGrid grid, string fieldKey,
                   string fieldNConn, string fieldConn, SegmentationType segmentationType, double reachGroupLength,string reachDirection,
                   ref string status, ref int progress, Status st, int startReachNumber)
        {
            // Get the polygons from the grid.
            Polygon2D[] polygons = grid.GetPolygons();

            // Clip the polylines by the bounding box of the grid.
            Polygon2D boundingBox = grid.getBoundingBox();
            List<Polyline2D> newPolylines = new List<Polyline2D>();
            for (int i = 0; i < polylines.Length; i++)
            {
                Polyline2D[] clipped = polylines[i].ClipByCopyAttributes(boundingBox);
                newPolylines.AddRange(clipped);
            }
            polylines = newPolylines.ToArray();
            if (polylines.Length == 0)
            {
                throw new System.NullReferenceException("no polylines found within the domain. " +
                                                        "check offset, discritization file and " +
                                                        "polyine projection");
            }

            //get the preferred ordering reference point
            double refX = -1.0, refY = -1.0;
            Range2D boundingBoxCoords = boundingBox.Range;
            
            switch (reachDirection)
            {
                case "Top left":
                    refX = boundingBoxCoords.getXRange().getMin();
                    refY = boundingBoxCoords.getYRange().getMax();
                    break;
                case "Bottom left":
                    refX = boundingBoxCoords.getXRange().getMin();
                    refY = boundingBoxCoords.getYRange().getMin();
                    break;
                case "Bottom right":
                    refX = boundingBoxCoords.getXRange().getMax();
                    refY = boundingBoxCoords.getYRange().getMin();
                    break;
                case "Top right":
                    refX = boundingBoxCoords.getXRange().getMax();
                    refY = boundingBoxCoords.getYRange().getMax();
                    break;
                case ("None"):
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException("Error processing preferred ordering direction");
            }
            
            if (reachDirection != "None")
            {
                status = DateTime.Now+" - Reordering Polylines with Preferred Direction";
                st.writeTask(status);
                progress += 1;
                st.updateProgress(progress);
                Point2D referencePoint = new Point2D(refX, refY);
                Polyline2D[] reorderedPolylines = reorderPolylines(referencePoint, polylines, fieldKey, fieldConn);
                polylines = reorderedPolylines;
            }

            // Make a list for the segments.
            List<Polyline2D> allSegments = new List<Polyline2D>();

            // Iterate over all polylines.            
            int reachNumber = startReachNumber;
            int reachGroupNumber = startReachNumber;
            double targetReachGrouplength = reachGroupLength;
            int prevSrcPoly = -1;
            status = DateTime.Now+" - Segmenting Polylines Against the Grid";
            st.writeTask(status);
            double maxProgress = 80.0;
           
            for (int k = 0; k < polylines.Length; k++)
            {
                //status stuff               
                double delta = progress + (Convert.ToDouble(k+1) / Convert.ToDouble(polylines.Length)) * maxProgress ;
                st.updateProgress(Convert.ToInt32(delta));
                status = DateTime.Now + " - Segmenting Polyline " + (k + 1) + " of " + polylines.Length;
                st.writeTask(status);

                // Make a list for the segments of this polyline.
                List<Polyline2D> thisPolylineSegments = new List<Polyline2D>();                

                // For each polygon, find the contained polyline segments.
                for (int j = 0; j < polygons.Length; j++)
                {
                    // Get the segments.
                    Polyline2D[] segments = polylines[k].ClipBy(polygons[j]);
                    // Assign the source polyline and the row and column of the clipping polygon.
                    for (int i = 0; i < segments.Length; i++)
                    {
                        Point2D first = segments[i].GetVertex(0);
                        Point2D last = segments[i].GetVertex(segments[i].NumVertices - 1);
                        if (!first.Nearly(last))
                        
                        {
                            // Set an attribute indicating the source polyline index.
                            segments[i].SetAttribute("SRC_PLY", k);

                            for (int ii = 0; ii < polylines[k].NumAttributes; ii++)
                            {
                                ShapeAttribute att = polylines[k].GetAttribute(ii);

                                if ((att.Name == KEY_IREACH) || (att.Name == KEY_IREACH) || (att.Name == KEY_IEQN) || (att.Name == KEY_ICENUM) ||
                                (att.Name == KEY_KRCH_LAY) || (att.Name == KEY_IRCH_ROW) || (att.Name == KEY_JRCH_COL) ||
                                (att.Name == KEY_RLEN) || (att.Name == KEY_REACH_GROUP_LENGTH) ||
                                (att.Name == KEY_NCONN) || (att.Name == KEY_CONN) || (att.Name == KEY_DIST))

                                    segments[i].SetAttribute("SRC_" + att.Name, att.Value);
                                else

                                    segments[i].SetAttribute(att.Name, att.Value);

                            }

                            // Set an attribute indicating the row and column in the grid in which this reach is located.
                            segments[i].SetAttribute(KEY_JRCH_COL, polygons[j].GetAttributeValue(OrthoGrid.KEY_COL));
                            segments[i].SetAttribute(KEY_IRCH_ROW, polygons[j].GetAttributeValue(OrthoGrid.KEY_ROW));
                            thisPolylineSegments.Add(segments[i]);
                        }
                    }
                    
                    // Add the segments to the list of segments for this polyline.
                    //thisPolylineSegments.AddRange(segments);
                }                

                Polyline2D firstSegment = null;               
                Point2D startingPointInPolyline = polylines[k].GetVertex(0);
                for (int i = 0; i < thisPolylineSegments.Count && firstSegment == null; i++)
                {
                    Point2D firstPointInSegment = thisPolylineSegments[i].GetVertex(0);
                    Point2D lastPointInSegment = thisPolylineSegments[i].GetVertex(thisPolylineSegments[i].NumVertices - 1);
                    if (firstPointInSegment.Equals(lastPointInSegment))
                    {
                        Console.WriteLine("zero-length segment - skipping");
                    }

                    //if (firstPointInSegment.Equals(startingPointInPolyline) || lastPointInSegment.Equals(startingPointInPolyline))
                    else if (firstPointInSegment.Nearly(startingPointInPolyline) || lastPointInSegment.Nearly(startingPointInPolyline))
                    {
                        firstSegment = thisPolylineSegments[i];
                    }
                }
                

                // If the reach group length is zero, use the no-clip segmentation type.
                if (reachGroupLength == 0.0)
                {
                    segmentationType = SegmentationType.NoClip;
                }

                // Make a list for the modified segments from this polyline.
                List<Polyline2D> modifiedThisPolylineSegments = new List<Polyline2D>();

                // Process the segments according to the segmentation type.
                if (segmentationType == SegmentationType.NoClip)
                {
                    // Start at the first segment.
                    Polyline2D currentSegment = firstSegment;

                    // Number the first segment and add it to the reordered list.
                    
                    currentSegment.SetAttribute(KEY_IREACH, reachNumber);
                    currentSegment.SetAttribute(KEY_ICENUM, reachNumber);
                    reachNumber++;
                    modifiedThisPolylineSegments.Add(currentSegment);

                    // Follow the connections to the end of the line.
                    // In the perferred order direction
                    while (currentSegment != null)
                    {
                        // Find the next segment.
                        Point2D endOfThisSegment = currentSegment.GetVertex(currentSegment.NumVertices - 1);
                        Polyline2D nextSegment = null;
                        for (int j = 0; j < thisPolylineSegments.Count && nextSegment == null; j++)
                        {                            
                            //for the damn 15 - 17 digit precision mess...
                            if (thisPolylineSegments[j].GetVertex(0).Nearly(endOfThisSegment))
                                {                                    
                                    nextSegment = thisPolylineSegments[j];
                                }
                        }

                        // Number the segment and add it to the reordered list.
                        if (nextSegment != null)
                        {
                            nextSegment.SetAttribute(KEY_IREACH, reachNumber);
                            nextSegment.SetAttribute(KEY_ICENUM, reachNumber);
                            reachNumber++;
                            try
                            {
                                modifiedThisPolylineSegments.Add(nextSegment);
                            }
                            catch
                            {
                                Console.WriteLine(modifiedThisPolylineSegments.Capacity+" "+modifiedThisPolylineSegments.Count);
                                throw new System.ArgumentOutOfRangeException("Could not add new segment to polyline list - list is full");
                            }                                
                        }

                        // Advance the segment.
                        currentSegment = nextSegment;
                    }
                }

                // This is for exact and equal segmentation types.
                else
                {
                    // If the type is equal, adjust the reach group length.
                    double polylineLength = 0.0;
                    if (segmentationType == SegmentationType.Equal)
                    {
                        polylineLength = polylines[k].GetLength();
                        if (polylineLength < targetReachGrouplength)
                        {
                            reachGroupLength = targetReachGrouplength;
                        }
                        else
                        {
                            int numSegmentsLow = (int)Math.Floor(polylineLength / targetReachGrouplength);
                            if (numSegmentsLow == 0)
                            {
                                numSegmentsLow++;
                            }
                            int numSegmentsHigh = numSegmentsLow + 1;
                            double modifiedLengthHigh = polylineLength / numSegmentsLow;
                            double modifiedLengthLow = polylineLength / numSegmentsHigh;
                            double modifiedLengthHighDiff = Math.Abs(reachGroupLength - modifiedLengthHigh);
                            double modifiedLengthLowDiff = Math.Abs(reachGroupLength - modifiedLengthLow);
                            if (modifiedLengthHighDiff < modifiedLengthLowDiff)
                            {
                                reachGroupLength = modifiedLengthLow;
                            }
                            else
                            {
                                reachGroupLength = modifiedLengthHigh;
                            }
                            Console.WriteLine("The reach group length is " + reachGroupLength);
                        }
                        
                    }

                    // Start at the first segment.
                    Polyline2D currentSegment = firstSegment;
                    double lengthOfThisGroup = 0.0;                   
                    while (currentSegment != null)
                    {
                        // This is the case where we clip the beginning and reset the group.
                        if (currentSegment.GetLength() + lengthOfThisGroup > reachGroupLength)
                        {
                            Polyline2D[] splitSegment = splitPolylineByLength(currentSegment, reachGroupLength - lengthOfThisGroup);
                            Polyline2D segmentForThisGroup = splitSegment[0];
                            
                            segmentForThisGroup.SetAttribute(KEY_IREACH, reachNumber++);
                            segmentForThisGroup.SetAttribute(KEY_ICENUM, reachGroupNumber);
                            modifiedThisPolylineSegments.Add(segmentForThisGroup);
                            
                           
                            //store this segements source polyline index for reach grouping later
                            prevSrcPoly = (int)segmentForThisGroup.GetAttributeValue("SRC_PLY",0);
                            
                            reachGroupNumber++;
                            lengthOfThisGroup = 0.0;
                            currentSegment = splitSegment[1];
                        }

                        // Otherwise, process this segment and find the next segment.
                        else
                        {
                            // Process this segment.
                            //only keep segement if it has some length
                            double length = currentSegment.GetLength();
                            if (length > MIN_REACH_LENGTH)
                            {
                                currentSegment.SetAttribute(KEY_IREACH, reachNumber++);
                                currentSegment.SetAttribute(KEY_ICENUM, reachGroupNumber);
                                modifiedThisPolylineSegments.Add(currentSegment);
                            }
                            lengthOfThisGroup += currentSegment.GetLength();
                        

                            // Find the next segment. 
                            Point2D endOfThisSegment = currentSegment.GetVertex(currentSegment.NumVertices - 1);
                            Polyline2D nextSegment = null;
                            for (int j = 0; j < thisPolylineSegments.Count && nextSegment == null; j++)
                            {
                                length = thisPolylineSegments[j].GetLength();
                                if ((thisPolylineSegments[j].GetVertex(0).Equals(endOfThisSegment)) && (length > MIN_REACH_LENGTH))
                                {
                                    nextSegment = thisPolylineSegments[j];
                                }
                            }
                            currentSegment = nextSegment;
                        }
                    }

                    //if this segment is too short and we are still on the same polyline
                    //then add it to the previous reach group
                    if ((lengthOfThisGroup < reachGroupLength / 2.0) && (prevSrcPoly == k))
                    {
                        
                        for (int j = modifiedThisPolylineSegments.Count - 1; j >= 0; j--)
                        {
                            int groupNumber = (int)modifiedThisPolylineSegments[j].GetAttributeValue(KEY_ICENUM);
                            if ((groupNumber == reachGroupNumber))
                            {
                                modifiedThisPolylineSegments[j].SetAttribute(KEY_ICENUM, reachGroupNumber - 1);
                            }
                        }
                    }

                    // Otherwise, increment the reach group number to account for the last group.
                    else
                    {
                        reachGroupNumber++;
                    }
                }

                // Set the connections.
                for (int i = 0; i < modifiedThisPolylineSegments.Count; i++)
                {
                    //set up nconn and conn attributes
                    modifiedThisPolylineSegments[i].SetAttribute(KEY_NCONN, 0);
                    modifiedThisPolylineSegments[i].SetAttribute(KEY_CONN, new List<int>());

                    //set up the distance of connection attribute
                    modifiedThisPolylineSegments[i].SetAttribute(KEY_DIST, new List<double>());

                    int startConnection = i == 0 ? 0 : (int)modifiedThisPolylineSegments[i - 1].GetAttributeValue(KEY_IREACH, 0);
                    int endConnection = i == modifiedThisPolylineSegments.Count - 1 ? 0 :
                        (int)modifiedThisPolylineSegments[i + 1].GetAttributeValue(KEY_IREACH, 0);
                    
                    //if a valid start, increment nconn and add to conn
                    if (i != 0)
                    {
                        addReach(modifiedThisPolylineSegments[i], startConnection, 0.0);

                    }
                    if (i != modifiedThisPolylineSegments.Count - 1)
                    {
                        addReach(modifiedThisPolylineSegments[i], endConnection, 0.0);
                    }
                }

                // Add the segments from this polyline to the list of all segments.
                allSegments.AddRange(modifiedThisPolylineSegments);
            }


            //Explicit NConn Connectivity
            status = DateTime.Now+" - Setting Explicit Polyline Connectivity";
            st.writeTask(status);
            progress = Convert.ToInt32(maxProgress);
            maxProgress = 15.0;
            for (int k = 0; k < polylines.Length; k++)
            {
                //status
                status = DateTime.Now + " - Setting Connections for Polyline "+(k+1)+" of "+polylines.Length;
                st.writeTask(status);
                double delta = progress + (Convert.ToDouble(k + 1) / Convert.ToDouble(polylines.Length)) * maxProgress;
                st.updateProgress(Convert.ToInt32(delta));
                
                // Get the appropriate attributes for this polyline.
                string polylineKey = getAttributeForConnection(polylines[k], fieldKey);
                Console.WriteLine("Setting the explicit connections for reach " + polylineKey);
                //int polylineNConn = int.Parse(getAttributeForConnection(polylines[k], fieldNConn));
                string nConnStr = getAttributeForConnection(polylines[k], fieldNConn);
                int polylineNConn = -1;
                if (nConnStr.Length == 0)
                {
                    polylineNConn = 0;
                }
                else
                {
                    polylineNConn = Convert.ToInt32(nConnStr, 10);
                }
                string polylineConn = getAttributeForConnection(polylines[k], fieldConn);
                string[] conn = parseConn(polylineConn);
                
                Console.WriteLine("The connection key is " + polylineKey);
                Console.WriteLine("The number of connections is " + polylineNConn);
                Console.WriteLine("The connections are " + polylineConn);
                //loop over each connection
                for (int c = 0; c < polylineNConn; c++)
                {
                    //find the source reach index for this connection
                    int indexOfConnecting = -1;
                    for (int j = 0; j < polylines.Length && indexOfConnecting < 0; j++)
                    {
                        if (getAttributeForConnection(polylines[j], fieldKey).Equals(conn[c]))
                        {
                            indexOfConnecting = j;
                        }
                    }
                    if (indexOfConnecting >= 0)
                    {
                        Console.WriteLine("The index of the start-connecting polyline is " + indexOfConnecting);

                        // Loop through all segments. Find the closest distance between two segments of the two polylines.
                        double minDistance = double.MaxValue;
                        int indexOfSegmentInThis = -1;
                        int indexOfSegmentInConnected = -1;

                        List<int> first = GetSegmentIndexesBySourceIndex(allSegments, k, "SRC_PLY");
                        List<int> second = GetSegmentIndexesBySourceIndex(allSegments, indexOfConnecting, "SRC_PLY");
                        //for (int y = 0; y < allSegments.Count; y++)
                        for (int y = 0; y < first.Count; y++)
                        {
                            //for (int x = 0; x < allSegments.Count; x++)
                            for (int x = 0; x < second.Count; x++)
                            {
                                try
                                {
                                    //int sourceIndexOfFirst = (int)allSegments[y].GetAttributeValue("SRC_PLY");
                                    //int sourceIndexOfSecond = (int)allSegments[x].GetAttributeValue("SRC_PLY");
                                    //if (sourceIndexOfFirst == k && sourceIndexOfSecond == indexOfConnecting)
                                    //if (sourceIndexOfSecond == indexOfConnecting)
                                    //{
                                        //double distance = allSegments[x].GetDistance(allSegments[y]);
                                        double distance = allSegments[second[x]].GetDistance(allSegments[first[y]]);
                                        if (distance < minDistance)
                                        {
                                            indexOfSegmentInThis = first[y];
                                            indexOfSegmentInConnected = second[x];
                                            minDistance = distance;
                                        }
                                    //}
                                }
                                catch { }
                            }
                        }
                        //check for circular connections
                        double minCircDistance = double.MaxValue;
                        int indexOfSegInThisCirc = -1;
                        int indexOfSegInConnCirc = -1;
                        for (int y = 0; y < first.Count; y++)
                        {
                            for (int x = 0; x < second.Count; x++)
                            {
                                double distance = allSegments[second[x]].GetDistance(allSegments[first[y]]);
                                //if this segment is close and atleast one of the two segments are different the previous found segments 
                                if ((distance < minCircDistance) && ((first[y] != indexOfSegmentInThis) || (second[x] != indexOfSegmentInConnected)))
                                {
                                    indexOfSegInThisCirc = first[y];
                                    indexOfSegInConnCirc = second[x];
                                    minCircDistance = distance;
                                }
                            }
                        }

                        // If the connection is valid, make it.
                        if (indexOfSegmentInThis >= 0)
                        {
                            Polyline2D segmentInThis = allSegments[indexOfSegmentInThis];
                            Polyline2D segmentInConnected = allSegments[indexOfSegmentInConnected];
                            int reachIdOfThis = (int)segmentInThis.GetAttributeValue(KEY_IREACH);
                            int reachIdOfConnected = (int)segmentInConnected.GetAttributeValue(KEY_IREACH);
                            addReach(segmentInThis, reachIdOfConnected, minDistance);
                            //addReach(segmentInConnected, reachIdOfThis, minDistance);
                        }
                        
                        //If minCircDist is < or = minDistance, these must be a circular conn - make it
                        if (minCircDistance <= minDistance)
                        {
                            Polyline2D segmentInThis = allSegments[indexOfSegInThisCirc];
                            Polyline2D segmentInConnected = allSegments[indexOfSegInConnCirc];
                            int reachIdOfThis = (int)segmentInThis.GetAttributeValue(KEY_IREACH);
                            int reachIdOfConnected = (int)segmentInConnected.GetAttributeValue(KEY_IREACH);
                            addReach(segmentInThis, reachIdOfConnected, minCircDistance);
                        }
                    }
                }
            }
            
            // Set the "CONNECTION" and "LENGTH" attributes. Also calculate the total length of each reach group.
            int groupArraySize = segmentationType == SegmentationType.NoClip ? reachNumber : reachGroupNumber;
            double[] reachGroupLengths = new double[groupArraySize];
            for (int i = 0; i < allSegments.Count; i++) {
                // Get the reach group and add the length of the segment to the appropriate group.
                int reachGroup = (int)allSegments[i].GetAttributeValue(KEY_ICENUM);
                double length = allSegments[i].GetLength();
                reachGroupLengths[reachGroup] += length;

                // Set the length attribute.
                allSegments[i].SetAttribute(KEY_RLEN, length);
            }

            // Set the reach group length according to the group.           
            for (int i = 0; i < allSegments.Count; i++)
            {
                // Get the reach group.
                int reachGroup = (int)allSegments[i].GetAttributeValue(KEY_ICENUM);

                // Set the reach group length.
                allSegments[i].SetAttribute(KEY_REACH_GROUP_LENGTH, reachGroupLengths[reachGroup]);
            }

            // Return the segments as an array.

            return allSegments.ToArray();
        }

        private static List<int> GetSegmentIndexesBySourceIndex(List<Polyline2D> allSegments, int index, string sourceAttribute)
        {
            List<int> foundIndexes = new List<int>();
            for (int k = 0; k < allSegments.Count; k++)
            {
                if ((int)allSegments[k].GetAttributeValue(sourceAttribute) == index)
                {
                    foundIndexes.Add(k);
                }
            }
            return foundIndexes;
        }

        private static Polyline2D[] reorderPolylines(Point2D referencePoint, Polyline2D[] polylines,string fieldKey, string fieldConn)
        {
            //reorders the polylines to make as sequentially numbered (where possible), starting at the polyline nearest the preferred reference point
            
            //min distance to the reference point
            double[] polylineMinDistance = new double[polylines.Length];
            //flag for determining the order of the polyline vertices
            bool[] polylineDirection = new bool[polylines.Length];
            //sorted index of polylines
            int[] polylineIdx = new int[polylines.Length];
            //flag for determining which polylines have been visited
            bool[] visited = new bool[polylines.Length];
            double min = 1.0e+20;
            int minIdx = -1;            
            
            //loop over all polylines and populate the minimum distance and
            //the direction arrays
            for (int i = 0; i < polylines.Length; i++)
            {
                //get the start and end distance to the reference point
                Point2D startPoint = polylines[i].GetVertex(0);
                Point2D endPoint = polylines[i].GetVertex(polylines[i].NumVertices - 1);
                double startDist = referencePoint.distance(startPoint);
                double endDist = referencePoint.distance(endPoint);
                
                //track which end is closer to the ref point
                polylineDirection[i] = startDist > endDist ? false : true;
                polylineMinDistance[i] = startDist > endDist ? endDist : startDist;

                //find the minimum index
                if (startDist < min)
                {
                    min = startDist;
                    minIdx = i;
                }
                if (endDist < min)
                {
                    min = endDist;
                    minIdx = i;
                }

                //initialize the visited array
                visited[i] = false;
                //initialize the index array
                polylineIdx[i] = i;
                
            }
            //sort the polyines by minimum distance to ref point
            //polylineIdx stores the original index of the polyline array
            Array.Sort(polylineMinDistance, polylineIdx);

            //build new re-order polyline list
            int startIdx = 0;
            List<Polyline2D> reorderedPolylines = new List<Polyline2D>();
            while (true)
            {
                //get the original array index of the nearest unvistied polyline
                startIdx = findNearestUnvisitedIdx(polylineIdx, polylineMinDistance, visited);
                if (startIdx == -1) break;
                
                Polyline2D polylineStart = polylines[startIdx];
                //if this polyline needs to be reordered...
                if (polylineDirection[startIdx] == false)
                {
                    polylineStart.reverseVertices();
                    //reorderedPolylines.Add(polylineStart);
                }
                //add this polyline to the new list
                reorderedPolylines.Add(polylineStart);
                
                //follow this polyline's connections
                followPolylines(startIdx, polylineIdx, polylines, ref reorderedPolylines, ref visited, fieldKey, fieldConn);
            }  
            return reorderedPolylines.ToArray();
            

        }

        private static void sortConnByEndDist(Polyline2D[] polylines, int idx, ref string[] thisConn,string fieldKey)
        {
            double[] endDist = new double[thisConn.Length];
            Point2D thisEnd = polylines[idx].GetVertex(polylines[idx].NumVertices-1);
            Point2D thisConnEnd = new Point2D();
            Point2D thisConnStart = new Point2D();
            for (int i=0;i<thisConn.Length;i++)
            {
                foreach (Polyline2D connPoly in polylines)
                {
                    if (Int32.Parse(connPoly.GetAttributeValue(fieldKey).ToString()) == Int32.Parse(thisConn[i]))
                    {                        
                        thisConnEnd = connPoly.GetVertex(connPoly.NumVertices-1);
                        thisConnStart = connPoly.GetVertex(0);
                        break;
                    }
                }
                double end2end = thisEnd.distance(thisConnEnd);
                double end2start = thisEnd.distance(thisConnEnd);                    
                endDist[i] = end2end < end2start ? end2end : end2start;                
            }
            Array.Sort(endDist,thisConn);
            return;
        }

        private static void followPolylines(int startIdx, int[] polylineIdx, Polyline2D[] polylines,ref  List<Polyline2D> reorderedPolylines,
                                           ref bool[] visited,  string fieldKey, string fieldConn)
        {
            
            Polyline2D thisPolyline = polylines[startIdx];
            
            //mark this polyline as visited
            visited[startIdx] = true;
            //get the conn string attribute
            string[] thisConn = parseConn(thisPolyline.GetAttributeValue(fieldConn, "").ToString());

            sortConnByEndDist(polylines,startIdx,ref thisConn,fieldKey);
            
            //loop over each connection
            for (int j = 0; j < thisConn.Length; j++)
            {
                //get the original polyline array index of the connection
                int connIdx = getConnectedIdx(fieldKey, int.Parse(thisConn[j]), polylineIdx, polylines);
                if(connIdx == -1)
                {
                    string wrn = "Reach "+thisConn[j]+" in CONN attribute of reach "+thisPolyline.GetAttributeValue(KEY_IREACH)+" not found";
                    throw new System.ArgumentOutOfRangeException(wrn);                    

                }
                if (!visited[connIdx])
                {
                    //determine if thisConn polyline needs to be reordered,
                    //based on the proximity to the nearest connection of thisPolyline
                    Polyline2D thisConnPolyline = polylines[connIdx];
                    if (!checkOrder(thisPolyline,thisConnPolyline))
                        thisConnPolyline.reverseVertices();
                    
                    //add thisConnPolyline to the reordered list
                    reorderedPolylines.Add(thisConnPolyline);
                    
                    //recursively follow the connections of thisConnPolyline
                    followPolylines(connIdx, polylineIdx, polylines,  ref reorderedPolylines, ref visited, fieldKey, fieldConn);
                }
            }
            return;   
        }

        private static bool checkOrder(Polyline2D current, Polyline2D conn)
        {
            Point2D currentStart = current.GetVertex(0);
            Point2D connStart = conn.GetVertex(0);
            Point2D currentEnd = current.GetVertex(current.NumVertices - 1);
            Point2D connEnd = conn.GetVertex(conn.NumVertices - 1);
            
            double start2startDist = currentStart.distance(connStart);
            double start2endDist = currentStart.distance(connEnd);
            double end2startDist = currentEnd.distance(connStart);
            double end2endDist = currentEnd.distance(connEnd);
            
            double startMin = start2startDist < start2endDist ? start2startDist : start2endDist;
            double endMin = end2startDist < end2endDist ? end2startDist : end2endDist;

            if (startMin < endMin)
            {
                if (start2endDist < start2startDist)
                    return false;
            }
            else
            {
                if (end2endDist < end2startDist)
                    return false;
            }
            return true;
        }
    
        private static int findNearestUnvisitedIdx(int[] polylineIdx, double[] polylineMinDistance, bool[] visited)
        {
            //find the index of the polyline from the  original array of the that hasn't been visited and 
            //is closest to the origin
            double min = 1.0e+20;
            int minIdx = -1;
            for (int i = 0; i < polylineMinDistance.Length; i++)
            {
                if ((!visited[polylineIdx[i]]) && (polylineMinDistance[i] < min))
                {
                    min = polylineMinDistance[i];
                    minIdx = polylineIdx[i];
                }
            }
            return minIdx;
        }

        private static int getConnectedIdx(string fieldKey, int connReach, int[] polylineIdx, Polyline2D[] polylines)
        {
            //find the index of the polyline from the original polyline array with the matching connReach number
            int indexOfConnecting = -1;
            for (int j = 0; j < polylines.Length && indexOfConnecting < 0; j++)
            {
                int thisReachNum = int.Parse(getAttributeForConnection(polylines[polylineIdx[j]], fieldKey).ToString()); ;
                if (thisReachNum == connReach)
                {
                    indexOfConnecting = polylineIdx[j];
                    
                }
            }
            return indexOfConnecting;
        }

        private static void addReach(Polyline2D thisPolyLine, int iReach, double dist)
        {
            //get the current conn list, add the new iReach to it, then reset the attribute
            List<int> thisConn = (List<int>)thisPolyLine.GetAttributeValue(KEY_CONN, new List<int>());
            //check to see if this connection already exists, if so skip it
            foreach (int idx in thisConn)
            {
                if (idx == iReach) return;
            }
            thisConn.Add(iReach);
            thisPolyLine.SetAttribute(KEY_CONN, thisConn);
            
            //increment nconn
            thisPolyLine.SetAttribute(KEY_NCONN,
                (int)thisPolyLine.GetAttributeValue(KEY_NCONN, 0) + 1);
            
            //get the current conn list, add the new dist to it, then reset the attribute
            List<double> thisDist = (List<double>)thisPolyLine.GetAttributeValue(KEY_DIST, new List<double>());
            thisDist.Add(dist);
            thisPolyLine.SetAttribute(KEY_DIST, thisDist);
            return;
        }

        private static string[] parseConn(string polylineConn)
        {            
            //split on multiple whitespaces and commas
            string[] line = Regex.Split(polylineConn, @",|\s+", RegexOptions.IgnorePatternWhitespace);
            if (line[0]=="") return new string[0];
            else return line;
        }

        private static Polyline2D[] splitPolylineByLength(Polyline2D polyline, double lengthOfFirst)
        {
            if (lengthOfFirst >= polyline.GetLength())
            {
                throw new ArgumentException("You're dumb.");
            }

            double totalLengthOfFirst = 0.0;
            List<Point2D> verticesOfFirst = new List<Point2D>();
            List<Point2D> verticesOfSecond = new List<Point2D>();
            int i = 1;
            Point2D lastVertex = polyline.GetVertex(0);
            verticesOfFirst.Add(lastVertex);
            while (i < polyline.NumVertices)
            {
                Point2D currentVertex = polyline.GetVertex(i);
                double legLength = lastVertex.distance(currentVertex);

                if (legLength + totalLengthOfFirst > lengthOfFirst)
                {
                    // Calculate the last vertex and add it to the list of the first.
                    double requiredLength = lengthOfFirst - totalLengthOfFirst;
                    double ratio = requiredLength / legLength;
                    double dx = currentVertex.GetX() - lastVertex.GetX();
                    double dy = currentVertex.GetY() - lastVertex.GetY();
                    double x = lastVertex.GetX() + dx * ratio;
                    double y = lastVertex.GetY() + dy * ratio;
                    verticesOfFirst.Add(new Point2D(x, y));

                    // Build the list of the second.
                    verticesOfSecond.Add(new Point2D(x, y));
                    while (i < polyline.NumVertices)
                    {
                        verticesOfSecond.Add(polyline.GetVertex(i));
                        i++;
                    }
                }
                else
                {
                    // Add the vertex to the first.
                    verticesOfFirst.Add(currentVertex);

                    // Add the leg length to the total length.
                    totalLengthOfFirst += legLength;

                    // Advance the vertex.
                    lastVertex = currentVertex;
                }
                i++;
            }

            // Make the child lines.
            Polyline2D first = new Polyline2D(verticesOfFirst.ToArray());
            Polyline2D second = new Polyline2D(verticesOfSecond.ToArray());

            // Copy the attributes from the parent to the children.
            for (int j = 0; j < polyline.NumAttributes; j++)
            {
                first.SetAttribute(polyline.GetAttribute(j));
                second.SetAttribute(polyline.GetAttribute(j));
            }

            return new Polyline2D[] { first, second };
        }

        private static string getAttributeForConnection(Polyline2D polyline, string key)
        {
            // A connection key is always provided as a string.
            // An empty string is used to denote "no connection."

            // Get the attribute from the shape.
            ShapeAttribute attribute = polyline.GetAttribute(key);

            // If the attribute is null, return the empty string.
            if (attribute == null)
            {
                return "";
            }
            else
            {
                string attributeString = attribute.Value + "";
                if (attributeString.Equals("0"))
                {
                    attributeString = "";
                }
                return attributeString;
            }
        }

        private static void writeSwrDataset(Polyline2D[] polylines, string filename)
        {
            // Open the output file for writing.
            //StreamWriter sw = File.CreateText(filename);
            StreamWriter sw = new StreamWriter(filename);
            sw.AutoFlush = true;

            // 4a
            sw.WriteLine(@"#                                       LAY        ROW        COL");
            sw.WriteLine(@"#   IRCH4A IROUTETYPE     IRGNUM       KRCH       IRCH       JRCH           RLEN");
            for (int i = 0; i < polylines.Length; i++)
            {
                // Get the polyline from the set.
                Polyline2D polyline = polylines[i];

                // Write the reach number, padded to the right to 8 characters.
                int ireach = (int)polyline.GetAttributeValue(KEY_IREACH, 0);
                int ieqn = 3; // (int)polyline.GetAttributeValue(KEY_IEQN, 0);
                int icenum = (int)polyline.GetAttributeValue(KEY_ICENUM, 0);
                int krch = 1; // (int)polyline.GetAttributeValue(KEY_KRCH, 0);
                int irch = (int)polyline.GetAttributeValue(KEY_IRCH_ROW, 0);
                int jrch = (int)polyline.GetAttributeValue(KEY_JRCH_COL, 0);
                float rlen = (float)polyline.GetLength();
                sw.WriteLine("{0,10:g} {1,10:g} {2,10:g} {3,10:g} {4,10:g} {5,10:g} {6,15:e4}",ireach,ieqn,icenum,krch,irch,jrch,rlen);
            }

            //4b
            sw.WriteLine("\n\n#   IRCH4B      NCONN      ICONN(1)...ICONN(NCONN)");
            for (int i=0;i<polylines.Length;i++)
            {
                Polyline2D polyline = polylines[i];
                int ireach = (int)polyline.GetAttributeValue(KEY_IREACH, 0);
                int nconn = (int)polyline.GetAttributeValue(KEY_NCONN, 0);
                sw.Write("{0,10:g} {1,10:g} ", ireach,nconn);

                List<int> conn = (List<int>)polyline.GetAttributeValue(KEY_CONN, new List<int>());
                foreach (int c in conn)
                    sw.Write("{0,10:g} ", c);
                // Close the line X 2
                sw.WriteLine();
            }
            // Close the output file.
            sw.Close();
        }

        private static bool writeErrorFile(List<string[]> errList, string errorFile)
        {
            StreamWriter sw = new StreamWriter(errorFile);
            sw.AutoFlush = true;
            bool error = false;
            foreach (string[] errString in errList)
            {
                if (errString.Length > 0)
                {
                    error = true;
                    foreach (string err in errString)
                    {
                        sw.WriteLine(err);
                    }
                }
            }
            sw.Close();
            return error;
        }

        private static string[] checkPolylineLength(Polyline2D[] polylines,string fieldId)
        {
            List<string> errList = new List<string>();
            foreach (Polyline2D poly in polylines)
            {                
                int numVert = poly.NumVertices;
                if (numVert == 1)
                {
                    string err = "Single vertex polyline: " + poly.GetAttribute(0).Value.ToString();
                    errList.Add(err);
                }
                Point2D start = poly.GetVertex(0);
                if ((start.Equals(poly.GetVertex(poly.NumVertices - 1))) &&
                    numVert == 2)
                {
                    //string err = "Zero-length polyline with two vertices: "+poly.GetAttribute(0).Value.ToString();
                    string err = "Zero-length polyline with two vertices: " + poly.GetAttributeValue(fieldId, " ");
                    errList.Add(err);
                }
                //check for duplicate vertices
                for (int i = 0; i < numVert; i++)
                {
                    for (int j = i + 1; j < numVert; j++)
                    {
                        Point2D ivert = poly.GetVertex(i);
                        Point2D jvert = poly.GetVertex(j);
                        if (ivert.Nearly(jvert))
                        {
                            string err = "Polyline with 'nearly' duplicate verticies: " + poly.GetAttribute(0).Value.ToString();
                            errList.Add(err);
                        }
                    }
                }


            }
            return errList.ToArray();
        }
       
        private static string[] checkConnectivity(Polyline2D[] polylines,string fieldId, 
                                                      string fieldNConn, string fieldConn)
        {                     
            bool error = false;
            List<string> errList = new List<string>();
            foreach (Polyline2D p in polylines)
            {
                string thisFieldKey = p.GetAttributeValue(fieldId, "").ToString();
                try
                {
                    int fieldKeyInt = Int32.Parse(thisFieldKey);
                }
                catch
                {
                    string err = "Only integer reach and conn values are supported:" + fieldId + " " + thisFieldKey;
                        ;
                    errList.Add(err);
                    return errList.ToArray();
                }

                string[] thisConn = parseConn(p.GetAttributeValue(fieldConn,"").ToString());
                Console.WriteLine(p.GetAttributeValue(fieldNConn, 0));
                int thisNConn = Convert.ToInt32(p.GetAttributeValue(fieldNConn, 0));
                //make sure nconn == conn.length
                if (thisNConn != thisConn.Length)
                {                    
                    string err = "nconn != conn.length for reach " + thisFieldKey;
                    errList.Add(err);
                    error = true;
                }
                //make sure each connection is in the polylines
                foreach (string c in thisConn)
                {
                    bool found = checkConnectionExists(c, fieldId, polylines);
                    if (found == false)
                    {                 
                        string err = "connection " + c + " for polyline " + thisFieldKey + " not found.";
                        errList.Add(err);
                        error = true;
                    }
                }
                //make sure each connection is symmetric
                for (int c = 0; c < thisConn.Length; c++)
                {
                    if (thisConn[c] == thisFieldKey)
                    {                     
                        string err = "reach " + thisFieldKey + " is self-connected";
                        errList.Add(err);
                        error = true;
                    }
                                        
                    bool found = findBackConnection(thisFieldKey, thisConn[c], fieldId, fieldConn, polylines);
                    if (found == false)
                    {                       
                        string err = "asymmetric connection from polyline " + thisFieldKey
                                    + " to polyline " + thisConn[c];
                        errList.Add(err);
                        error = true;
                    }
                }
            }            
            return errList.ToArray();
        }

        private static bool checkConnectionExists(string thisConn, string fieldId, Polyline2D[] polylines)
        {
            foreach (Polyline2D p in polylines)
            {
                string thisFieldKey = p.GetAttributeValue(fieldId, "").ToString();
                if (thisFieldKey == thisConn)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool findBackConnection(string thisFieldKey, string thisConn, string fieldId, string fieldConn, Polyline2D[] polylines)
        {
            bool found = false;
            foreach (Polyline2D p in polylines)
            {
                string thisPolylineFieldKey = p.GetAttributeValue(fieldId, "").ToString();
                if (thisConn == thisPolylineFieldKey)
                {
                    string[] thisConnList = parseConn(p.GetAttributeValue(fieldConn,"").ToString());
                    foreach (string c in thisConnList)
                    {
                        if (c == thisFieldKey) found = true;
                    }
                    return found;
                }
            }
            return found;
        }

        private static void writeParameters(string discretizationPath, string shapefilePath,
                                            Point2D anchorPoint, double rotation, bool southernHemisphere,
                                            double minimumElementLength,
                                            string fieldId, string fieldNConn, string fieldConn,
                                            SegmentationType segmentationType,
                                            double reachGroupLength, bool warnFlag,
                                            string preferredDirection, bool xmlFlag,
                                            string xmlFile, int startReachNumber)
        {
            double rotationDegrees = rotation * 360.0 /( Math.PI * 2.0);
            XmlDocument xd = new XmlDocument();
            
            XmlElement el = (XmlElement)xd.AppendChild(xd.CreateElement("parameters"));
            
            el.SetAttribute("discretizationPath", discretizationPath);
            el.SetAttribute("shapefilePath", shapefilePath);
            el.SetAttribute("anchorPointX", anchorPoint.GetX().ToString());
            el.SetAttribute("anchorPointY", anchorPoint.GetY().ToString());
            el.SetAttribute("rotation", rotationDegrees.ToString());
            el.SetAttribute("southernHemisphere", southernHemisphere.ToString());
            //el.SetAttribute("minimumElementLength", minimumElementLength.ToString());
            el.SetAttribute("fieldId", fieldId);
            el.SetAttribute("fieldNConn", fieldNConn);
            el.SetAttribute("fieldConn", fieldConn);
            el.SetAttribute("segmentationType", segmentationType.ToString());
            el.SetAttribute("reachGroupLength", reachGroupLength.ToString());
            el.SetAttribute("warnFlag", warnFlag.ToString());
            el.SetAttribute("preferredDirection", preferredDirection);
            el.SetAttribute("xmlFlag", xmlFlag.ToString());
            el.SetAttribute("startReachNumber", startReachNumber.ToString());
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter writer = XmlWriter.Create(xmlFile, settings);

            xd.Save(writer);
            writer.Close();
            return;

            
        }
        
        private static string[] checkPolylinesForDuplicates(Polyline2D[] polylines, string fieldId)
        {
            Polyline2D thisPolyline = polylines[0];
            List<string> dupList = new List<string>();
            List<string> errList = new List<string>();
            for (int i = 0; i < polylines.Length; i++)
            {
                string thisReach = polylines[i].GetAttributeValue(fieldId, "").ToString();
                for (int j = 0; j < polylines.Length; j++)
                {
                    if ((polylines[j].GetAttributeValue(fieldId, "").ToString() == thisReach)
                        && (j != i))
                    {
                        dupList.Add(thisReach);
                    }
                }
            }
            if (dupList.Count > 0)
            {
                string wrn = "Duplicate " + KEY_IREACH + " values found:";
                foreach (string d in dupList)
                {
                    wrn = wrn + " " + d;
                    errList.Add(wrn);
                }             
            }
            return errList.ToArray();      



            
        }
    }
}
