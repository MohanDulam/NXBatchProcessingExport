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
            BatchExport.CreateStepFile(args);
        }

        public static int GetUnloadOption(string dummy)
        {
            return (int)NXOpen.Session.LibraryUnloadOption.Immediately;
        }

    }

    public class BatchExport {

        // class Memebers
        // Get the NX Session
        private static Session theSession = Session.GetSession();

        /// <summary>
        /// Batch Export of the STEP File
        /// </summary>
        /// <param name="args">Arguments 
        /// 1. Part File Directory
        /// 2. Directory of Step to Save</param>
        /// <returns></returns>
        public static void CreateStepFile(string[] args)
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

                    // Calling StepFileExport function
                    StepFileExport(workPart, secondArgExportDir);

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
        /// Export Step File form Part File
        /// </summary>
        /// <param name="part">Part File need to Export to STEP File</param>
        /// <param name="stepFileDir">Directory where STEP File to Save</param>
        private static void StepFileExport(Part part, string stepFileDir)
        {
            try
            {
                // Name of Step file to save
                string displayPartName = part.Name;

                // location of the file
                string inputPartFileDir = part.FullPath;
                //string partFileDir = inputPartFileDir.Substring(0, inputPartFileDir.LastIndexOf("\\")) + "\\";
                //Console.WriteLine("Input File: "+inputPartFileDir);

                // File Path and Name of the Step file where to save it
                string outputStepFileDir = stepFileDir + $"\\" + displayPartName;
                //Console.WriteLine("Output File: "+outputStepFileDir);

                // Chack for any old step file is exist in given directory 
                string delOldStepFile = outputStepFileDir + ".stp";
                if (File.Exists(delOldStepFile))
                {
                    File.Delete(delOldStepFile);
                }

                // Declaration of Step file Definition in NX
                string stepSettingFile;
                // Location of the Step file Definition in NX 
                string STEP214UG_Dir = theSession.GetEnvironmentVariableValue("STEP214UG_DIR");
                // Step file Definition File
                stepSettingFile = Path.Combine(STEP214UG_Dir, "ugstep214.def");
                //stepSettingFile = @"C:\Program Files\Siemens\NX 12.0\STEP214UG\ugstep214.def";


                // Step File Creator Builder 
                NXOpen.StepCreator step214Creator1;
                step214Creator1 = theSession.DexManager.CreateStepCreator();
                step214Creator1.ExportAs = NXOpen.StepCreator.ExportAsOption.Ap214;
                step214Creator1.ObjectTypes.Solids = true;
                step214Creator1.ObjectTypes.Surfaces = true;
                step214Creator1.ObjectTypes.Curves = true;
                // File Path and Name of the Step file where to save it
                step214Creator1.OutputFile = outputStepFileDir; 
                // Input File Path
                step214Creator1.InputFile = inputPartFileDir; 
                                
                // Path of the NX STEP file Definitation
                step214Creator1.SettingsFile = stepSettingFile;
                step214Creator1.ExportExtRef = false;
                step214Creator1.FileSaveFlag = false;
                step214Creator1.LayerMask = "1-256";
                step214Creator1.ProcessHoldFlag = true;

                // Commit the Builder
                NXObject nXObject;
                nXObject = step214Creator1.Commit();

                // Destroy the STEP File Creator Builder
                step214Creator1.Destroy();

                // Delete Log file
                string delLogFile = outputStepFileDir + ".log";
                if (File.Exists(delLogFile))
                {
                    File.Delete(delLogFile);
                }

            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                // Message.Echo("Error While Exporting Part File" + ex.ToString());
            }
        }

    }
       
}
