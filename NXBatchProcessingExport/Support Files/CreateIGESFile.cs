using System;
using System.IO;
using System.Windows.Forms; 

using NXOpen;
using NXOpen.UF;

namespace StepFileCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Calling CreateIGESFile function from BatchExport class
            BatchExport.CreateIGESFile(args);
        }

        public static int GetUnloadOption(string dummy)
        {
            return (int)NXOpen.Session.LibraryUnloadOption.Immediately;
        }

    }

    public class BatchExport
    {
        // class Memebers
        // Get the NX Session
        private static Session theSession = Session.GetSession();

        /// <summary>
        /// Batch Export of the IGES File
        /// </summary>
        /// <param name="args">Arguments 
        /// 1. Part File Directory
        /// 2. Directory of Step to Save</param>
        /// <returns></returns>
        public static void CreateIGESFile(string[] args)
        {
            try
            {
                // check for Arguments 
                if (args.Length < 1)
                {
                    //Console.WriteLine("No part(s) specified. Exit.");
                    MessageBox.Show("Input Arguments are not Found");
                    return;
                }

                // check for Arguments length
                if (args.Length == 2)
                {
                    // Input Arguments 
                    string firstArgPartFile = args[0];
                    string secondArgExportDir = args[1];

                    //Console.WriteLine("1 Arg: " + firstArgPartFile);
                    //Console.WriteLine("2 Arg: " + secondArgExportDir);

                    // Check the Work Part load Status
                    PartLoadStatus loadStatus;
                    Part workPart = theSession.Parts.OpenDisplay(firstArgPartFile, out loadStatus);
                    if (workPart == null)
                    {
                        //Console.WriteLine("Part file is null Return");
                        MessageBox.Show("Part file is not Found");
                        return;
                    }

                    // Calling IGESFileCreator function
                    IGESFileCreator(workPart, secondArgExportDir);
                    // Close the open Work Part
                    workPart.Close(BasePart.CloseWholeTree.True, BasePart.CloseModified.CloseModified, null);
                }               
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error : " + ex.ToString());
            }

        }

        /// <summary>
        /// Create Part File as IGES File
        /// </summary>
        /// <param name="part">Part File need to Export to IGES File</param>
        /// <param name = "igesFileDir" > Directory where IGES File to Save</param>
        private static void IGESFileCreator(Part part, string igesFileDir)
        {
            // Name of Step file to save
            string displayPartName = part.Name;

            // location of the file
            string inputPartFileDir = part.FullPath;
            //string partFileDir = inputPartFileDir.Substring(0, inputPartFileDir.LastIndexOf("\\")) + "\\";
            //Console.WriteLine("Input File: "+inputPartFileDir);

            // File Path and Name of the Step file where to save it
            string outputIgesFileDir = igesFileDir + $"\\" + displayPartName;
            //Console.WriteLine("Output File: "+outputStepFileDir);

            // Chack for any old step file is exist in given directory 
            string delOldIgesFile = outputIgesFileDir+".igs";
            if (File.Exists(delOldIgesFile))
            {
                File.Delete(delOldIgesFile);
            }

            // Declaration of IGES file Definition in NX
            string igesSettingFile; 
            // Location of the IGES file Definition in NX
            string IGES_DIR = theSession.GetEnvironmentVariableValue("IGES_DIR");
            igesSettingFile = Path.Combine(IGES_DIR, "igesexport.def");

            #region IGES Creator Builder
            //  IGES Creator Builder
            NXOpen.IgesCreator igesCreator3; 

            igesCreator3 = theSession.DexManager.CreateIgesCreator();
            // IGES file Definition File
            igesCreator3.SettingsFile = igesSettingFile;
            //igesCreator3.SettingsFile = "C:\\Program Files\\Siemens\\NX 12.0\\iges\\igesexport.def";

            // File Path and Name of the Step file where to save it
            igesCreator3.OutputFile = outputIgesFileDir+ ".igs"; //partFilePath + displayName + ".igs";
            // Input File Path
            igesCreator3.InputFile = inputPartFileDir; 

            igesCreator3.ExportModelData = true;
            igesCreator3.ExportDrawings = true;
            igesCreator3.MapTabCylToBSurf = true;
            igesCreator3.BcurveTol = 0.050799999999999998;
            igesCreator3.IdenticalPointResolution = 0.001;
            igesCreator3.MaxThreeDMdlSpace = 10000.0;
            igesCreator3.ObjectTypes.Curves = true;
            igesCreator3.ObjectTypes.Surfaces = true;
            igesCreator3.ObjectTypes.Annotations = true;
            igesCreator3.ObjectTypes.Structures = true;
            igesCreator3.ObjectTypes.Solids = true;
            igesCreator3.ExportDrawings = false;
            igesCreator3.FlattenAssembly = true;
            igesCreator3.MapRevolvedFacesTo = NXOpen.IgesCreator.MapRevolvedFacesOption.BSurfaces;
            igesCreator3.MapCrossHatchTo = NXOpen.IgesCreator.CrossHatchMapEnum.SectionArea;
            igesCreator3.BcurveTol = 0.050799999999999998;
            igesCreator3.FlattenAssembly = false;
            igesCreator3.FileSaveFlag = false;
            igesCreator3.LayerMask = "1-256";
            igesCreator3.ViewList = "Top,Front,Right,Back,Bottom,Left,Isometric,Trimetric,User Defined";
            igesCreator3.ProcessHoldFlag = true;

            // Commit the Builder
            NXOpen.NXObject nXObject3;
            nXObject3 = igesCreator3.Commit();

            // Destroy the IGES File Creator Builder
            igesCreator3.Destroy();
            #endregion

            // Delete Log file
            string delLogFile = outputIgesFileDir + ".log";
            if (File.Exists(delLogFile))
            {
                File.Delete(delLogFile);
            }

        }

    }
       
}
