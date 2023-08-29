using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ScenarioManagerMG5
{
    public class DialogHelper
    {
        public static OpenFileDialog GetHeadsFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "MODFLOW Output Heads Files (*.hds)|*.hds|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }
        public static OpenFileDialog GetNameFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "MODFLOW Name Files (*.nam)|*.nam|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static OpenFileDialog GetCbbFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "MODFLOW Cell-By-Cell Files (*.cbb)|*.cbb|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        internal static OpenFileDialog GetSmpFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "Borehole Sample Files (*.smp)|*.smp|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static SaveFileDialog GetXmlSaveFileDialog()
        {
            // Make the dialog.
            SaveFileDialog dialog = new SaveFileDialog();

            // Set the file filter.
            dialog.Filter = "Extensible Markup Language (XML) Files (*.xml)|*.xml|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static OpenFileDialog GetXmlOpenFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "Extensible Markup Language (XML) Files (*.xml)|*.xml|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static OpenFileDialog GetOpenGroupFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static OpenFileDialog GetOpenSADialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "Scenario Analyzer Files (*.sa)|*.sa|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static SaveFileDialog getSaveSADialog()
        {
            // Make the dialog.
            SaveFileDialog dialog = new SaveFileDialog();

            // Set the file filter.
            dialog.Filter = "Scenario Analyzer Files (*.sa)|*.sa|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        public static OpenFileDialog GetOpenDiscretizationFileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "MODFLOW Discretization Files (*.dis)|*.dis|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }

        internal static OpenFileDialog GetOpenShapefileDialog()
        {
            // Make the dialog.
            OpenFileDialog dialog = new OpenFileDialog();

            // Set the file filter.
            dialog.Filter = "Shapefiles (*.shp)|*.shp|All Files (*.*)|*.*";

            // Return the result.
            return dialog;
        }
    }
}
