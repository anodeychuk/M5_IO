using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace M5_IO
{
    public static class FilesService
    {
        public static void SaveToFile(string path, string info)
        {
            if (File.Exists(path)) File.Delete(path);

            using (FileStream fStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(info);
                fStream.Write(array, 0, array.Length);
                Console.WriteLine("File saved");
            }
        }

        public static void FileToArchive(string path, string zipPath)
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);

            using (FileStream sourceStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(zipPath))
                {
                    using (GZipStream gzip = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(gzip);
                    }
                }
            }
        }

    }
}
