using System;
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
        static string folder;
        static List<string> notifList;

        static void Main(string[] args)
        {
            /**************************
             * Initialize
             **************************/
            notifList = new List<string>();

            folder = "RSC_" + DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;

            string path = "E:\\RSC\\" + folder;

            /**************************
             * Create Directory
             **************************/

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            /**************************
             * Get RSC Update emails (Manual)
             **************************/

            Console.WriteLine("\nPlease save off the RSC Notifications to the current folder: " + path + "\\");
            Console.Write("Press any key to continue...");
            Console.ReadLine();

            /**************************
             * Enumerate the directory
             **************************/

            var dirList = new DirectoryInfo(path).GetFiles().OrderBy(f => f.LastWriteTime).ToList();

            foreach (var f in dirList)
            {
                string fName = f.Name;

                fName = "call gRscUpd2.bat ## \"" + fName + "\"";

                notifList.Add(fName);
            }

            /**************************
             * Write List.txt
             **************************/

            FileStream fs = null;
            try
            {
                fs = new FileStream(path + "\\list.txt", FileMode.Create, FileAccess.Write);
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

            /**************************
             * Create Zip file
             **************************/

            string root = @"E:\RSC\";
            string temp = root + "tmp\\";
            string zip = path + "\\" + folder + ".zip";

            FileInfo[] FileShare = new DirectoryInfo(path).GetFiles();

            Directory.CreateDirectory(temp);

            foreach (FileInfo file in FileShare)
            {
                string temppath = Path.Combine(temp, file.Name);
                file.CopyTo(temppath, false);
            }           

            ZipFile.CreateFromDirectory(temp, zip, CompressionLevel.Optimal, false);

            Directory.Delete(temp, true);

            /**************************
             * Copy to USB Key (Manual)
             **************************/
            // http://social.msdn.microsoft.com/Forums/vstudio/en-US/9f9eb8f5-297f-4acd-a9af-aafbe384fd71/usb-drives-list-but-only-those?forum=csharpgeneral

            Console.Write("Please insert a USB key...");
            Console.ReadLine();
        }
    }
}