using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace M5_IO
{
    class Program
    {
        public static event FileServiceHandler SaveToFileEvent;
        public static event FileServiceHandler SaveToArcEvent;
        public delegate void FileServiceHandler(string a, string b);

        static void Main(string[] args)
        {
            SaveToFileEvent += FilesService.SaveToFile;
            SaveToArcEvent += FilesService.FileToArchive;

            #region constants
            string path = $@"C:\Users\{Environment.UserName}\source";
            string fileName = "discInfo";
            string pathFile = $@"C:\Users\{Environment.UserName}\Desktop\{fileName}.txt";
            string pathArc = $@"C:\Users\{Environment.UserName}\Desktop\{fileName}.zip";
            bool isDisplay = false;
            #endregion
            
            string info = "";

            DisplayFilesInDir(path, 0, isDisplay, ref info);

            SaveToFileEvent(pathFile, info);
            SaveToArcEvent(pathFile, pathArc);

        }

        public static void DisplayFilesInDir(string path, int level, bool isDisplay, ref string info)
        {
            string space = "";
            for (int i = 0; i < level; i++)
            {
                space += "    ";
            }

            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                string dirName = space + "**" + di.Name;
                info += dirName + "\r\n"; 
                if(isDisplay) Console.WriteLine(dirName);

                foreach (var dir in di.GetDirectories())
                {
                    if (IsCorrectDate(dir.CreationTime))
                        DisplayFilesInDir(dir.FullName, level + 1, isDisplay, ref info);
                }

                FileInfo[] fi = di.GetFiles();
                foreach (var file in fi)
                {
                    if (IsCorrectDate(file.CreationTime))
                    {
                        string fileName = space + "    --" + file.Name;
                        info += fileName + "\r\n";
                        if (isDisplay) Console.WriteLine(fileName);
                    }      
                }
            }
            catch(UnauthorizedAccessException)
            {
                string error = "Unauthorized Access Exception " + path;
                info += error;
                if (isDisplay) Console.WriteLine(error);
            }
        }

        private static bool IsCorrectDate(DateTime time)
        {
            if (time.CompareTo(DateTime.Now.AddMonths(-1)) <= 0)
                return true;
            else
                return false;
        }
    }
}
