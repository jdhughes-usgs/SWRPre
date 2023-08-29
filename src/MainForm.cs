using System.Windows.Forms;
using System;
using ScenarioManagerMG5;
using System.Text;
using System.Xml;

namespace SWRPre
{
    public partial class MainForm : Form
    {
        

        // These store the last-selected item in the combo boxes.
        private string lastIdentifierField;
        private string lastStartConnectionField;
        private string lastEndConnectionField;

        public MainForm(string[] args)
        {            
            InitializeComponent();
            comboBoxPreferredDirection.SelectedIndex = 0;
            
            
        }

        private void createFileButton_Click(object sender, System.EventArgs e)
        {
            // Get the information from the form.
            string discretizationPath = discretizationPathTextBox.Text;
            string shapefilePath = shapefilePathTextBox.Text;
            
            double anchorX = 0.0;
            double.TryParse(xCoordinateTextBox.Text, out anchorX);
            double anchorY = 0.0;
            double.TryParse(yCoordinateTextBox.Text, out anchorY);
            Point2D anchorPoint = new Point2D(anchorX, anchorY);

            double rotationDegrees = 0.0;
            double.TryParse(rotationAngleTextBox.Text, out rotationDegrees);
            double rotationRadians = rotationDegrees / 360.0 * Math.PI * 2.0;

            bool southernHemisphere = false;
            if (hemisphereComboBox.Text == "Southern")
            { 
                southernHemisphere = true; 
            }

            bool warnFlag = false;
            bool xmlFlag = false;
            
            if (xmlBox.Checked)
            {
                xmlFlag = true;
            }



            SwrProcessor.SegmentationType segmentationType;
            if (reachGroupButtonCombine.Checked)
            {
                segmentationType = SwrProcessor.SegmentationType.NoClip;
            }
            else if (reachGroupButtonExact.Checked)
            {
                segmentationType = SwrProcessor.SegmentationType.Exact;
            }
            else  // reachGroupButtonEqual
            {
                segmentationType = SwrProcessor.SegmentationType.Equal;
            }

            
            

            //string orderDirection = "";
            
            double reachGroupLength = 0.0;
            double.TryParse(reachGroupLengthTextBox.Text, out reachGroupLength);

            int startReachNumber = 1;
            int.TryParse(startReachTextBox.Text, out startReachNumber);
            
            string status = "";
            // Process the file.
            try
            {
                
                string[] paths = SwrProcessor.CreateFiles(discretizationPath, shapefilePath, 
                    anchorPoint, (float)rotationRadians, southernHemisphere, 0.0f, 
                    comboBoxIdentifier.SelectedItem + "", comboBoxNConn.SelectedItem + "", 
                    comboBoxConn.SelectedItem + "", segmentationType, reachGroupLength,
                    warnFlag, comboBoxPreferredDirection.SelectedItem+"", xmlFlag,
                    ref status, startReachNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"There was an error processing the files", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void discretizationPathBrowseButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = DialogHelper.GetOpenDiscretizationFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                discretizationPathTextBox.Text = dialog.FileName;
            }
        }

        private void shapefilePathBrowseButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = DialogHelper.GetOpenShapefileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                shapefilePathTextBox.Text = dialog.FileName;
            }
        }
        private void shapefilePathTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Get the shapefile field names.
                string[] fieldNames = Shape2D.GetShapefileFieldNames(shapefilePathTextBox.Text);
                //string[] fieldNames = Shape2D.GetShapefileFieldNamesPuma(shapefilePathTextBox.Text);

                // Populate the selection boxes with the shapefile field names.
                comboBoxIdentifier.Items.Clear();
                comboBoxNConn.Items.Clear();
                comboBoxConn.Items.Clear();
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    comboBoxIdentifier.Items.Add(fieldNames[i]);
                    comboBoxNConn.Items.Add(fieldNames[i]);
                    comboBoxConn.Items.Add(fieldNames[i]);
                }

                

                // If the selected indices are invalid, make them valid.
                if (fieldNames.Length > 0)
                {
                    if (comboBoxIdentifier.SelectedIndex < 0)
                    {
                        comboBoxIdentifier.SelectedIndex = 0;
                    }
                    if (comboBoxNConn.SelectedIndex < 0)
                    {
                        comboBoxNConn.SelectedIndex = 0;
                    }
                    if (comboBoxConn.SelectedIndex < 0)
                    {
                        comboBoxConn.SelectedIndex = 0;
                    }
                }

            }
            catch { }
        }
        private void comboBoxIdentifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Store the selected value.
            if (comboBoxIdentifier.SelectedItem != null)
            {
                lastIdentifierField = comboBoxIdentifier.SelectedItem + "";
            }
        }
        private void comboBoxStartConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxNConn.SelectedItem != null)
            {
                lastStartConnectionField = comboBoxNConn.SelectedItem + "";
            }
        }
        private void comboBoxEndConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxConn.SelectedItem != null)
            {
                lastEndConnectionField = comboBoxConn.SelectedItem + "";
            }
        }

        private void reachGroupLengthTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void topLeft_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxPreferredDirection_SelectedIndexChanged(object sender, EventArgs e)
        {            

        }

        private void reachGroupButtonExact_CheckedChanged(object sender, EventArgs e)
        {
            targetLengthLabel.ForeColor = System.Drawing.Color.Black;
            reachGroupLengthTextBox.Enabled = true;
            //reachGroupLengthTextBox.Text = "0.0";
        }

        private void xmlBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void reachGroupButtonCombine_CheckedChanged(object sender, EventArgs e)
        {
            targetLengthLabel.ForeColor = System.Drawing.Color.Gray;
            reachGroupLengthTextBox.Enabled = false;
            reachGroupLengthTextBox.Text = "0.0";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void reachGroupButtonEqual_CheckedChanged(object sender, EventArgs e)
        {
            targetLengthLabel.ForeColor = System.Drawing.Color.Black;
            reachGroupLengthTextBox.Enabled = true;
            //reachGroupLengthTextBox.Text = "0.0";

        }

        private void discretizationPathTextBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mnu_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnu_openXML_Click(object sender, EventArgs e)
        {
            string parmFile = "";

            
            openFD.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFD.Filter = "XML|*.xml";            
            openFD.ShowDialog();
            parmFile = openFD.FileName;
            
            XmlTextReader xr = new XmlTextReader(parmFile);
            try
            {
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
                                    discretizationPathTextBox.Text = xr.Value.ToString();
                                    break;
                                case ("shapefilePath"):
                                    shapefilePathTextBox.Text = xr.Value.ToString();
                                    break;
                                case ("anchorPointX"):
                                    xCoordinateTextBox.Text = xr.Value;
                                    break;
                                case ("anchorPointY"):
                                    yCoordinateTextBox.Text = xr.Value;
                                    break;
                                case ("rotation"):
                                    rotationAngleTextBox.Text = xr.Value;
                                    break;
                                case ("southernHemisphere"):
                                    if (xr.Value == "True")
                                    {
                                        hemisphereComboBox.Text = "Southern";
                                        hemisphereComboBox.SelectedItem = "Southern";
                                        hemisphereComboBox.SelectedIndex = 1;
                                    }
                                    else
                                    { 
                                        hemisphereComboBox.Text = "Northern";
                                        hemisphereComboBox.SelectedItem = "Northern";
                                        hemisphereComboBox.SelectedIndex = 0;
                                    }
                                    hemisphereComboBox.Update();
                                    break;
                                case ("fieldId"):
                                    comboBoxIdentifier.SelectedItem = xr.Value.ToString();
                                    break;
                                case ("fieldNConn"):
                                    comboBoxNConn.SelectedItem = xr.Value.ToString();
                                    break;
                                case ("fieldConn"):
                                    comboBoxConn.SelectedItem = xr.Value.ToString();
                                    break;
                                case ("segmentationType"):
                                    string segType = null;
                                    segType = xr.Value.ToString();
                                    if (segType == "NoClip")
                                    {
                                        reachGroupButtonCombine.Checked = true;
                                    }
                                    else if (segType == "Exact")
                                    {
                                        reachGroupButtonExact.Checked = true;
                                    }
                                    else
                                    {
                                        reachGroupButtonEqual.Checked = true;
                                    }
                                    break;
                                case ("reachGroupLength"):
                                    reachGroupLengthTextBox.Text = xr.Value;
                                    break;
                                case ("preferredDirection"):
                                    comboBoxPreferredDirection.SelectedItem = xr.Value.ToString();
                                    break;
                                case ("xmlFlag"):
                                    Boolean xmlFlag = Boolean.Parse(xr.Value);
                                    if (xmlFlag)
                                    {
                                        xmlBox.Checked = true;
                                    }
                                    break;
                                case ("startReachNumber"):
                                    startReachTextBox.Text = xr.Value;
                                    break;
                                default:
                                    break;
                                //throw new System.Xml.XmlException("Unrecongnized Element in xml parameter file");
                                //startGui(args);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string wrn = "Error loading SWRPre parameters from XML file \n";
                MessageBox.Show(wrn,ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
                // Open the shapefile.
                //IntPtr shapefileReference = ShapeLib.SHPOpen(shapefilePathTextBox.Text, "rb");

                // Determine the DBF path.
                //string dbfPath = shapefilePathTextBox.Text.Substring(0, shapefilePathTextBox.Text.Length - 3) + "dbf";

                // Open the DBF file.
                //IntPtr dbfReference = ShapeLib.DBFOpen(dbfPath, "rb");

                //ShapeLib.SHPClose(shapefileReference);
                //ShapeLib.DBFClose(dbfReference);

            



            xr.Close();                        
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {
            
        }

        private void hemisphereComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
