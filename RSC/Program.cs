﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace RSC
{
    class Program
    {
        const int MIN_NUMBER_TO_PROCESS = 3;
        const int CUTOFF_TIME = 22; // 22 = 10 pm
        const string ROOT = "E:\\RSC\\";

        static bool ManualOverride;

        static void Main(string[] args)
        {
            /**************************
             * Initialize
             **************************/
            ManualOverride = false;

            /**************************
             * Parse args
             **************************/
            if (args.Contains("-m") || args.Contains("-M")) ManualOverride = true;

            /**************************
            * Start
            **************************/
            ProcessUpdates();

            /**************************
             * Copy to USB Key 
             * (Not Implemented yet)
             **************************/
            // http://social.msdn.microsoft.com/Forums/vstudio/en-US/9f9eb8f5-297f-4acd-a9af-aafbe384fd71/usb-drives-list-but-only-those?forum=csharpgeneral
        }

        /// <summary>
        /// Write List.txt
        /// </summary>
        /// <param name="dir">Directory to append or create list.txt</param>
        /// <param name="notifList">The list of files to be appended to list.txt</param>
        private static void WriteListFile(string dir, List<string> notifList)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(dir + "\\list.txt", FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    fs = null;
                    foreach (string f in notifList)
                    {
                        if (!f.Contains("list.txt"))
                            sw.WriteLine(f);
                    }
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }
        }

        /// <summary>
        /// Create Directory for processing
        /// </summary>
        /// <param name="path">The name and path of the directory to create</param>
        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Check the RSC folder for updates and process them as needed.
        /// </summary>
        private static void ProcessUpdates()
        {
            FileInfo[] oldList = null;
            string delPath = null;

            // Check previous RSC directories to see if the zip file is still present (if so that means they weren't processed)
            // Remark: Should I put in redundancy? Logically there should never be more than one folder that isn't processed.
            var dirList = new DirectoryInfo(ROOT).GetDirectories().OrderBy(f => f.CreationTime).ToList();

            foreach (var dir in dirList)
            {
                if (dir.EnumerateFiles(dir.Name + ".zip").Count() != 0)
                {
                    ManualOverride = true; // Automatically true because there are unprocessed notifications

                    delPath = Path.Combine(dir.Root.ToString(), dir.Parent.ToString());
                    delPath = Path.Combine(delPath, dir.Name);

                    File.Delete(Path.Combine(delPath, dir.Name + ".zip"));

                    oldList = new DirectoryInfo(delPath).GetFiles();

                    break;
                }
            }

            var RSCList = new DirectoryInfo(ROOT).GetFiles("AMIRSC*.txt").OrderBy(f => f.CreationTime).ToList();

            if ((RSCList.Count() >= MIN_NUMBER_TO_PROCESS) || (ManualOverride))
            {
                string folder = "RSC_" + DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
                string path = Path.Combine(ROOT, folder);
                CreateDir(path);

                foreach (var f in RSCList)
                {
                    if ((f.LastWriteTime.Date < DateTime.Now.Date && f.CreationTime.Hour <= CUTOFF_TIME) ||
                        ManualOverride)
                    {
                        f.MoveTo(Path.Combine(path, f.Name));
                    }
                }
                List<string> notifList = null;

                if (Directory.EnumerateFiles(path).Count() > 0)
                {
                    notifList = GetNoteList(path);
                }

                // Copy the unprocessed files (if any)
                if (oldList != null && delPath != null)
                {
                    foreach (var file in oldList)
                    {
                        file.MoveTo(Path.Combine(path, file.Name));
                    }

                    Directory.Delete(delPath);
                }

                if (Directory.EnumerateFiles(path).Count() > 0)
                {
                    if (notifList != null)
                    {
                        // Write or append list.txt
                        WriteListFile(path, notifList);
                    }

                    // Create the zip file
                    CreateZip(path, Path.Combine(path, folder + ".zip"));
                }
                else
                {
                    Directory.Delete(path);
                }
            }
        }

        /// <summary>
        /// Create a zip archive of the folder
        /// </summary>
        /// <param name="path">The folder to create the archive of</param>
        /// <param name="zip">The name of the zip file</param>
        private static void CreateZip(string path, string zip)
        {
            string temp = Path.Combine(ROOT, "tmp");
            FileInfo[] FileShare = new DirectoryInfo(path).GetFiles();

            Directory.CreateDirectory(temp);

            foreach (FileInfo file in FileShare)
            {
                string temppath = Path.Combine(temp, file.Name);
                file.CopyTo(temppath, false);
            }

            ZipFile.CreateFromDirectory(temp, zip, CompressionLevel.Optimal, false);

            Directory.Delete(temp, true);
        }

        /// <summary>
        /// Enumerate the directory and get the notification list
        /// </summary>
        /// <param name="path">the directory to enumerate</param>
        /// <returns>List of notifications ordered by date</returns>
        private static List<string> GetNoteList(string path)
        {
            List<string> retVal = new List<string>();
            var dirList = new DirectoryInfo(path).GetFiles().OrderBy(f => f.LastWriteTime).ToList();

            foreach (var f in dirList)
            {
                string fName = f.Name;

                fName = "call gRscUpd2.bat ## \"" + fName + "\"";

                retVal.Add(fName);
            }

            return retVal;
        }
    }
}