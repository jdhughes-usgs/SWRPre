using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Reflection;
namespace SWRPre
{
    static class Program
    {
        //for testing
        private const string DISCRETIZATION_FILE = @".\test_prob.dis";
        private const string SHAPEFILE = @".\reaches.shp";
        private const float X_CORNER = 543750.9f;
        private const float Y_CORNER = 2824250.0f;
        private const float ROTATION = (float)(0.0);
        private const float MINIMUM_ELEMENT_LENGTH = 0.0f;
        private const float RCHGRP_LENGTH = 1000.0f;
        private const bool WARN_FLAG = true;
        private const string REACH_DIRECTION = "SE";
        private const bool XML_FLAG = true;
       
        [STAThread]
        static void Main(string[] args)
        {                                  
            // if a command line argument is passed, try to parse it as an XML parameters file
            if (args.Length > 0)
            {
                SwrProcessor.SegmentationType segmentationType = SwrProcessor.SegmentationType.NoClip;
                string discretizationPath = "";
                string shapefilePath = "";
                double anchorPointX = -1.0;
                double anchorPointY = -1.0;
                double  rotation = -1.0;
                bool southernHemisphere = false;
                double minimumElementLength = 0.0;
                string fieldId = "";
                string fieldNConn = "";
                string fieldConn = "";
                string segType = "";
                double reachGroupLength = -1.0;
                bool warnFlag = true;
                string preferredDirection = "";
                bool xmlFlag = true;
                bool[] parmCheck = new bool[15];
                int startReachNumber = 0;
                string[] parmNames = new string[]
                        {"discretizationPath", "shapefilePath",
                            "anchorPointX","anchorPointY","rotation",
                            "minimumElementLength","fieldId",
                            "fieldNConn","fieldConn","segType",
                            "reachGroupLength","warnFlag","preferredDirection","xmlFlag",
                            "startReachNumber"};
                
                //set up parameter check array
                for (int i = 0; i < parmCheck.Length; i++)
                {
                    parmCheck[i] = false;
                }
                parmCheck[5] = true;
                try
                {
                    XmlTextReader xr = new XmlTextReader(args[0]);
                    while (xr.Read())
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            while (xr.MoveToNextAttribute())
                            {
                                switch (xr.Name)
                                {
                                    case ("parameters"):
                                    case ("discretizationPath"):
                                        discretizationPath = xr.Value.ToString();
                                        parmCheck[0] = true;
                                        break;
                                    case ("shapefilePath"):
                                        shapefilePath = xr.Value.ToString();
                                        parmCheck[1] = true;
                                        break;
                                    case ("anchorPointX"):
                                        anchorPointX = Double.Parse(xr.Value);
                                        parmCheck[2] = true;
                                        break;
                                    case ("anchorPointY"):
                                        anchorPointY = Double.Parse(xr.Value);
                                        parmCheck[3] = true;
                                        break;
                                    case ("rotation"):
                                        rotation = Double.Parse(xr.Value);
                                        parmCheck[4] = true;
                                        break;
                                    case ("southernHemisphere"):
                                        southernHemisphere = bool.Parse(xr.Value);
                                        break;
                                    case ("minimumElementLength"):
                                        minimumElementLength = Double.Parse(xr.Value);
                                        parmCheck[5] = true;
                                        break;
                                    case ("fieldId"):
                                        fieldId = xr.Value.ToString();
                                        parmCheck[6] = true;
                                        break;
                                    case ("fieldNConn"):
                                        fieldNConn = xr.Value.ToString();
                                        parmCheck[7] = true;
                                        break;
                                    case ("fieldConn"):
                                        fieldConn = xr.Value.ToString();
                                        parmCheck[8] = true;
                                        break;
                                    case ("segmentationType"):
                                        segType = xr.Value.ToString();
                                        parmCheck[9] = true;
                                        break;
                                    case ("reachGroupLength"):
                                        reachGroupLength = Double.Parse(xr.Value);
                                        parmCheck[10] = true;
                                        break;
                                    case ("warnFlag"):
                                        warnFlag = Boolean.Parse(xr.Value);
                                        parmCheck[11] = true;
                                        break;
                                    case ("preferredDirection"):
                                        preferredDirection = xr.Value.ToString();
                                        parmCheck[12] = true;
                                        break;
                                    case ("xmlFlag"):
                                        xmlFlag = Boolean.Parse(xr.Value);
                                        parmCheck[13] = true;
                                        break;
                                    case ("startReachNumber"):
                                        startReachNumber = int.Parse(xr.Value);
                                        break;                                    
                                    default:                                        
                                        Console.WriteLine("Unrecongnized Element in xml parameter file - starting SWRPre GUI");
                                        startGui(args);
                                        return;
                                }
                            }
                        }
                    }
                    xr.Close();
                }
                catch (Exception e)
                {
                    string wrn = "XML parameter file not found"+e;
                    MessageBox.Show(wrn, "Starting SWRPre GUI..", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    startGui(args);
                }

                //check to make sure all parameters were read
                for (int i = 0; i < parmCheck.Length; i++)
                {
                    if (parmCheck[i] == false)
                    {
                        string wrn = "parameter " + parmNames[i] + " not read from xml input file";
                        MessageBox.Show("There was an error processing the XML parameter file.\nPlease check your input files.\nERROR: " + wrn,
                        "Starting SWRPre GUI...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        startGui(args);
                    }
                }

                //convert rotation
                double rotationRadians = rotation / 360.0 * Math.PI * 2.0;
                
                if (segType == "NoClip")
                {
                    segmentationType = SwrProcessor.SegmentationType.NoClip;
                }
                else if (segType == "Equal")
                {
                    segmentationType = SwrProcessor.SegmentationType.Equal;
                }
                else if (segType == "Exact")
                {
                    segmentationType = SwrProcessor.SegmentationType.Exact;
                }
                else
                {
                    string wrn = "Could not cast segmentationType,should be NoClip, Equal, or Exact";                    
                    startGui(args,wrn);
                }
                string status = "";
                try
                {                    
                    SwrProcessor.CreateFiles(discretizationPath, shapefilePath,
                        new Point2D(anchorPointX, anchorPointY), rotationRadians, southernHemisphere, 
                        minimumElementLength, fieldId, fieldNConn, fieldConn, 
                        segmentationType, reachGroupLength, warnFlag, preferredDirection, 
                        xmlFlag, ref status, startReachNumber);                   
                }
                catch (Exception e)
                {
                    string wrn = "Error running SWRPre from XML parameter file \n"+e;                                        
                    startGui(args,wrn);
                }
            }
            else
            {
                startGui(args);   
            }
        }

        private static void startGui(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
            Environment.Exit(1);            
        }

        private static void startGui(string[] args,string wrn)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            MessageBox.Show(wrn, "Starting SWRPre GUI...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Run(new MainForm(args));
            Environment.Exit(1);

        } 
    }
}
