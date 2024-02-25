using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using NXOpen;
using NXOpen.UF;

namespace NXBatchProcessingExport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NXBatchProcessingExport());
        }

        /// <summary>
        /// To Create Step files from given input folder and Export folder
        /// </summary>
        /// <param name="inputPartFiles">Part Files Input Folder</param>
        /// <param name="exportFilesDir">Step Files Export Folder</param>
        public static void CreateStepFile(string inputPartFiles, string exportFilesDir)
        {
            // Part Files and Export Files Directory
            string filesDirectoryPath = inputPartFiles;
            string[] files = Directory.GetFiles(filesDirectoryPath, "*.prt");
            string stepFolder = exportFilesDir + @"\Step-Files";
            string[] directories = Directory.GetDirectories(filesDirectoryPath);

            // Folder for Step Files
            Directory.CreateDirectory(stepFolder);

            // Declerartion Exe File Path
            string exeFileLoc;

            // Exe file loation
            // Get the file and path of the CreateStepFile.exe
            exeFileLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exeFileLoc = exeFileLoc.Substring(0, exeFileLoc.LastIndexOf("\\"));
            exeFileLoc = exeFileLoc + @"\CreateStepFile.exe";

            // Get the file and path of the run_managed.exe
            string argFirstSection = Environment.GetEnvironmentVariable("UGII_BASE_DIR") + @"\NXBIN\run_managed.exe";

            // output and error messages from ug_edit_part_names.exe
            string output, errorout;

            foreach (string file in files)
            {
                Console.WriteLine(file);
                string argSecondSection = $" \"{exeFileLoc}\"" + " " + $"\"{file}\"" + " " + $"\"{stepFolder}\"";

                NewProcessor(argFirstSection, argSecondSection, out output, out errorout);

                argSecondSection = null;
                output = null; errorout = null;
            }
        
        }

        /// <summary>
        /// To Create IGES files from given input folder and Export folder
        /// </summary>
        /// <param name="inputPartFiles">Part Files Input Folder</param>
        /// <param name="exportFilesDir">IGES Files Export Folder</param>
        public static void CreateIGESFile(string inputPartFiles, string exportFilesDir)
        {
            // Part Files and Export Files Directory
            string filesDirectoryPath = inputPartFiles;
            string[] files = Directory.GetFiles(filesDirectoryPath, "*.prt");
            string stepFolder = exportFilesDir + @"\IGES-Files";
            string[] directories = Directory.GetDirectories(filesDirectoryPath);

            // Folder for IGES Files to Save
            Directory.CreateDirectory(stepFolder);

            // Declerartion Exe File Path
            string exeFileLoc;

            // Exe file loation
            // Get the file and path of the CreateStepFile.exe
            exeFileLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exeFileLoc = exeFileLoc.Substring(0, exeFileLoc.LastIndexOf("\\"));
            exeFileLoc = exeFileLoc + @"\CreateIGESFile.exe";

            // Get the file and path of the run_managed.exe
            string argFirstSection = Environment.GetEnvironmentVariable("UGII_BASE_DIR") + @"\NXBIN\run_managed.exe";

            // output and error messages from ug_edit_part_names.exe
            string output, errorout;

            foreach (string file in files)
            {
                Console.WriteLine(file);
                string argSecondSection = $" \"{exeFileLoc}\"" + " " + $"\"{file}\"" + " " + $"\"{stepFolder}\"";

                NewProcessor(argFirstSection, argSecondSection, out output, out errorout);

                argSecondSection = null;
                output = null; errorout = null;
            }

        }

        /// <summary>
        /// To Create Parasolid files from given input folder and Export folder
        /// </summary>
        /// <param name="inputPartFiles">Part Files Input Folder</param>
        /// <param name="exportFilesDir">Parasolid Files Export Folder</param>
        public static void CreateParasolidFile(string inputPartFiles, string exportFilesDir)
        {
            // Part Files and Export Files Directory
            string filesDirectoryPath = inputPartFiles;
            string[] files = Directory.GetFiles(filesDirectoryPath, "*.prt");
            string stepFolder = exportFilesDir + @"\Parasolid-Files";
            string[] directories = Directory.GetDirectories(filesDirectoryPath);

            // Folder for Parasolid Files
            Directory.CreateDirectory(stepFolder);

            // Declerartion Exe File Path
            string exeFileLoc;

            // Exe file loation
            // Get the file and path of the CreateStepFile.exe
            exeFileLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exeFileLoc = exeFileLoc.Substring(0, exeFileLoc.LastIndexOf("\\"));
            exeFileLoc = exeFileLoc + @"\CreateParasolidFile.exe";

            // Get the file and path of the run_managed.exe
            string argFirstSection = Environment.GetEnvironmentVariable("UGII_BASE_DIR") + @"\NXBIN\run_managed.exe";

            // output and error messages from ug_edit_part_names.exe
            string output, errorout;

            foreach (string file in files)
            {
                Console.WriteLine(file);
                string argSecondSection = $" \"{exeFileLoc}\"" + " " + $"\"{file}\"" + " " + $"\"{stepFolder}\"";

                NewProcessor(argFirstSection, argSecondSection, out output, out errorout);

                argSecondSection = null;
                output = null; errorout = null;
            }
        }

        public static void TestMethod()
        {
            // Declerartion DLL File Path
            string exeFileLoc;

            // Dll file loation
            exeFileLoc = System.Reflection.Assembly.GetExecutingAssembly().Location;
            exeFileLoc = exeFileLoc.Substring(0, exeFileLoc.LastIndexOf("bin"));
            exeFileLoc = exeFileLoc + @"Support Files\StepFileCreator.cs";
            // get the file and path of the ug_edit_part_names.exe
            string argFirstSection = Environment.GetEnvironmentVariable("UGII_BASE_DIR") + @"\NXBIN\run_journal.exe";
            
            // Construct the second argument section
            string argSecondSection = " " + exeFileLoc +" -args "+ @"C:\Users\MOHAN DULAM\Documents\NX Models\Re\bodies.prt";

            // output and error messages from ug_edit_part_names.exe
            string output, error;

            // Calling the NewProcessor for runing the ug_edit_part_names.exe
            NewProcessor(argFirstSection, argSecondSection, out output, out error);

        }

        /// <summary>
        /// To Start New Processor 
        /// </summary>
        /// <param name="argFirstSectionOfAppllication">Appliction with Path</param>
        /// <param name="argSecondSection">Commond for the Application</param>
        /// <param name="outputOfApplication">Output of the Application</param>
        /// <param name="errorOfApplication">Errror of application</param>
        private static void NewProcessor(string argFirstSectionOfAppllication, string argSecondSection, out string outputOfApplication, out string errorOfApplication)
        {
            outputOfApplication = null;
            errorOfApplication = null;
            try
            {
                // Arguments for the Processor 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = argFirstSectionOfAppllication;
                startInfo.Arguments = argSecondSection;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;

                // Intillazation of the New Processor
                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start(); // Start Processor

                // Output and error from the Application
                outputOfApplication = process.StandardOutput.ReadToEnd();
                errorOfApplication = process.StandardError.ReadToEnd();

                process.WaitForExit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in handling the Processor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //throw;
            }
        }

    }
}
