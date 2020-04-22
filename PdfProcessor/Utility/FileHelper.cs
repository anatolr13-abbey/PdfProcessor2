using System.IO;

namespace Abbey.PdfProcessor.Utility
{
    internal class FileHelper
    {
        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// Moves file from the Source File path to the Destination File path
        /// </summary>
        /// <param name="sourceFilePath">Source location of file</param>
        /// <param name="destFilePath">Destination for the file</param>
        public static void MoveFile(string sourceFilePath, string destFilePath)
        {
            File.Move(sourceFilePath, destFilePath);
        }

        /// <summary>
        /// Return list of PDF files in the PDF Input folder
        /// </summary>
        /// <returns>List of fully qualified PDF file names</returns>
        public static string[] GetPdfFiles()
        {
            return FindFiles(Settings.PDFInputFolderPath, "*.pdf");
        }

        /// <summary>
        /// Return list of Csv files in the Csv Output folder
        /// </summary>
        /// <returns>List of fully qualified Csv file names</returns>
        public static string[] GetCsvFiles()
        {
            return FindFiles(Settings.CsvOutputFolderPath, "*.csv");
        }

        /// <summary>
        /// Extracts the Filename without an extension from a given fully qualified file name
        /// </summary>
        /// <param name="filePath">Fully qualified file name</param>
        /// <returns>Filename without an extension</returns>
        public static string GetFileNameFromPath(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        public static void WipeDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }

            Directory.CreateDirectory(directoryPath);
        }

        private static string[] FindFiles(string searchFolder, string searchString)
        {
            return Directory.GetFiles(searchFolder, searchString, SearchOption.TopDirectoryOnly);
        }
    }
}