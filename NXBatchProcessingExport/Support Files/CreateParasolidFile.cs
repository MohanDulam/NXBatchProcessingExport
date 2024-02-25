using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms; 

using NXOpen;
using NXOpen.UF;
using NXOpen.Assemblies;

namespace ParasolidFileCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BatchExport.CreateParasolidFile(args);
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
        private static UFSession theUFSession = UFSession.GetUFSession();
        private static string parasolidFileDir;

        /// <summary>
        /// Batch Export of the STEP File
        /// </summary>
        /// <param name="args">Arguments 
        /// 1. Part File Directory
        /// 2. Directory of Step to Save</param>
        /// <returns></returns>
        public static void CreateParasolidFile(string[] args)
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
                    parasolidFileDir = secondArgExportDir;
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

                    // Calling ParaSolidFileExport function
                    ParaSolidFileExport(workPart);
                    
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
        /// Create Parasolid File from Part File
        /// </summary>
        /// <param name="workPart">workPart</param>
        private static void ParaSolidFileExport(Part workPart)
        {
            try
            {
                // rootComponent for the Assembly Part file 
                Component rootComponent = workPart.ComponentAssembly.RootComponent;

                // Check for Part File or Assembly File
                if (rootComponent == null)
                    ParasolidExport(workPart);
                else
                    ParasolidExport(rootComponent);

            }
            catch (Exception ex)
            {
                //Message.Echo("Error While Exporting Parasolid File " + ex.ToString());
            }
        }

        /// <summary>
        /// Create Parasolid File
        /// </summary>
        /// <param name="BodyTags">Body Tag</param>
        /// <param name="fileName">File Name string</param>
        private static void ParasolidFile(Part workPart, Tag[] BodyTags, string fileName)
        {
            try
            {
                // Name of the Parasolid file to save
                string displayBodyName = fileName;
                // location of the file
                string filePaths = workPart.FullPath;
                //filePaths = filePaths.Substring(0, filePaths.LastIndexOf("\\")) + "\\";

                // File Path and Name of the Parasolid file where to save it
                string parasolidFileName = parasolidFileDir + "\\" + displayBodyName + ".x_t";

                // Check for any old parasolid file is exist in given directory 
                string delOldParasolidFile = parasolidFileName;
                if (File.Exists(delOldParasolidFile))
                {
                    File.Delete(delOldParasolidFile);
                }

                int numUnexported;
                UFPs.Unexported[] unexported_tags = null;
                // UF functio to Export Parasolid File
                theUFSession.Ps.ExportLinkedData(BodyTags, BodyTags.Length, parasolidFileName, 160,
                    null, out numUnexported, out unexported_tags);
            }
            catch (Exception ex)
            {
               // Message.Echo("Error While Parasolid File Funcion" + ex.ToString());
            }
        }

        /// <summary>
        /// Create Parasolid File from an Assembly File
        /// </summary>
        /// <param name="rootComponent">Component</param>
        private static void ParasolidExport(Component rootComponent)
        {
            try
            {
                // Part from the rootComponent
                Part thePart = (Part)rootComponent.OwningPart;
                // Name of the Parasolid file to save
                string displayPartName = rootComponent.DisplayName;

                // Tag Array to collect bodies of Child Components
                List<Tag> componentsBodies = new List<Tag>();

                //Loop all Child Components of the Assembly
                foreach (Component comp in rootComponent.GetChildren())
                {
                    // Converting Component to Part                
                    Part thisPart = comp.Prototype as Part;
                    // Loop all the Bodies in Part
                    foreach (Body body in thisPart.Bodies)
                    {
                        // Adding Bodies of Child Component to List
                        componentsBodies.Add(body.Tag);
                    }
                }
                // Calling Function to Create Parasolid File
                ParasolidFile(thePart, componentsBodies.ToArray(), displayPartName);

                componentsBodies.Clear(); // // Clear the List
            }
            catch (Exception ex)
            {
                //Message.Echo("Error in Assembly Parasolid Export" + ex.ToString());
            }
        }

        /// <summary>
        /// Create Parasolid File from Part File
        /// </summary>
        /// <param name="part">Part</param>
        private static void ParasolidExport(Part part)
        {
            try
            {
                // Name of the Parasolid file to save
                string displayPartName = part.Name;
                // Tag Array to collect bodies of Part
                List<Tag> partBodies = new List<Tag>();

                // Loop all the Bodies in Part
                foreach (Body body in part.Bodies)
                {
                    partBodies.Add(body.Tag);  // Adding Bodies of Part to List
                }
                // Calling Function to Create Parasolid File
                ParasolidFile(part, partBodies.ToArray(), displayPartName);

                partBodies.Clear();  // Clear the List
            }
            catch (Exception ex)
            {
               // Message.Echo("Error in Part Parasolid Export" + ex.ToString());
            }
        }
    }

}
