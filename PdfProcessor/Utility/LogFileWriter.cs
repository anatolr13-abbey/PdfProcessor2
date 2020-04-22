using System;
using System.IO;
using System.Text;

namespace Abbey.PdfProcessor.Utility
{
    internal class LogFileWriter
    {
        private static readonly object _myLock = new object();

        public static void WriteLine(string textToWrite)
        {
            //lock (myLock) {
            //    Console.WriteLine( textToWrite );
            //    File.AppendAllText( @"c:\temp\PDFExtract.log", textToWrite + "\r\n" );
            //}
        }

        public static void WriteErrorLine(string textToWrite)
        {
            lock (_myLock)
            {
                Console.WriteLine(textToWrite);
                File.AppendAllText(@"c:\temp\PDFExtract.log", textToWrite + "\r\n");
            }
        }

        public static void WriteParseOutput(ParseOutput parseOutput)
        {
            var csvOutPut = new StringBuilder();
            csvOutPut.Append(
                $"{Settings.AgencyId}, {parseOutput.CarrierName}, {parseOutput.PdfFileName}, {parseOutput.ConverterName}, {parseOutput.StartTime}, {parseOutput.EndTime}, {parseOutput.Duration}, {parseOutput.CSVRowCount}\r\n");
            lock (_myLock)
            {
                if (parseOutput.CSVRowCount == 0)
                {
                    File.AppendAllText(@"c:\temp\PDFExtractEmptyFiles.csv", csvOutPut.ToString());
                }
                else
                {
                    File.AppendAllText(@"c:\temp\PDFExtract.csv", csvOutPut.ToString());
                }
            }
        }

        private static void CreateDirectory(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

    }
}