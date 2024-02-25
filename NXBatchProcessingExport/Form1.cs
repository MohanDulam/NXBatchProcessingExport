using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace NXBatchProcessingExport
{
    public partial class NXBatchProcessingExport : Form
    {
        public NXBatchProcessingExport()
        {
            InitializeComponent();
        }

        private void btnBrowseInputFile_Click(object sender, EventArgs e)
        {
            txtInputFolder.Clear();
            lblInfo.Text = "Status Info...";
            lblInfo.ForeColor = Color.White;

            using(FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult dialogResult = fbd.ShowDialog();

                if(dialogResult == DialogResult.OK)
                    
                    txtInputFolder.Text = fbd.SelectedPath.Trim();
                               
            }
            
        }

        private void btnBrowseExportFolder_Click(object sender, EventArgs e)
        {
            txtExportFolder.Clear();            

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult dialogResult = fbd.ShowDialog();

                if (dialogResult == DialogResult.OK)  
                    
                    txtExportFolder.Text = fbd.SelectedPath.Trim();                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear the text box content  
            txtInputFolder.Clear();
            txtExportFolder.Clear();
            
            // Uncheck the all the Check boxes
            chkbxStep.Checked = false;
            chkbxIGES.Checked = false;
            chkbxParasolid.Checked = false;
            chkbxDWG.Checked = false;
            chkbxDXF.Checked = false;
            chkbxPDF.Checked = false;

            // Change to Status Info...
            lblInfo.Text = "Status Info...";
            lblInfo.ForeColor = Color.White;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {       
                // Check Directory for both Part Files and Export Files
                if (string.IsNullOrWhiteSpace(txtInputFolder.Text) &&
                    string.IsNullOrWhiteSpace(txtExportFolder.Text))
                {
                    MessageBox.Show("Please Browse the Input Part File Directory and Export Files Directory");
                    return;                    
                }
                
                // Check for Tick option for different Export Options 
                if(chkbxStep.Checked == false && chkbxIGES.Checked == false && chkbxParasolid.Checked==false
                    && chkbxDWG.Checked == false && chkbxDXF.Checked == false && chkbxPDF.Checked == false)
                {
                    MessageBox.Show("Please any one of the Export Option");
                    return;
                }

                // Check for STEP File Export Option
                if (chkbxStep.Checked)
                {
                    lblInfo.Text = "Step Exporting is Processing";
                    lblInfo.ForeColor = Color.Green;
                    Program.CreateStepFile(txtInputFolder.Text, txtExportFolder.Text);

                    lblInfo.Text = "Step Export Completed";

                    //MessageBox.Show("Export Part Files to Step Files");
                }
                // Check for IGES File Export Option
                if (chkbxIGES.Checked)
                {
                    lblInfo.Text = "IGES Exporting is Processing";
                    lblInfo.ForeColor = Color.Green;
                    Program.CreateIGESFile(txtInputFolder.Text, txtExportFolder.Text);

                    lblInfo.Text = "IGES Export Completed";
                    //MessageBox.Show("ExportPart Files to IGES Fiels");
                }
                // Check for Parasolid File Export Option
                if (chkbxParasolid.Checked)
                {
                    lblInfo.Text = "Parasolid Exporting is Processing";
                    lblInfo.ForeColor = Color.Green;
                    Program.CreateParasolidFile(txtInputFolder.Text, txtExportFolder.Text);

                    lblInfo.Text = "Parasolid Export Completed";
                    //MessageBox.Show("ExportPart Files to IGES Fiels");
                }
                // Check for DWG File Export Option
                if (chkbxDWG.Checked)
                {
                    MessageBox.Show("DWG Export Coding is not Developed. under process ");
                }
                // Check for DXF File Export Option
                if (chkbxDXF.Checked)
                {
                    MessageBox.Show("DXF Export Coding is not Developed. under process ");
                }
                // Check for PDF File Export Option
                if (chkbxPDF.Checked)
                {
                    MessageBox.Show("PDF Export Coding is not Developed. under process ");
                }
                                
                //Program.TestMethod();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Export files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }
    }
}
