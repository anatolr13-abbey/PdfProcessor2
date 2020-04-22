using System;

using Abbey.Core.Application;
using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor
{
    internal static class Settings
    {
        public static int TimerInterval { get; }
        public static int ThreadCount { get; }
        public static int BatchSize { get; }

        public static int AgencyId { get; }
        public static bool WipeDb { get; }
        public static DateTime LastModifiedDate { get; }

        public static string WipeCarrierName { get; }
        public static string PdfFileNameContains { get; }

        public static string OCRInputFolderPath { get; }
        public static string OCROutputFolderPath { get; }
        public static string OCRArchiveFolderPath { get; }

        public static string CsvOutputFolderPath { get; }
        public static string PDFInputFolderPath { get; }
        public static string ImportArchvieFolderPath { get; }
        public static string TextConverterProjectFolderPath { get; }

        public static ClassSpec ClassSpec { get; set; }

        static Settings()
        {
            TimerInterval = Configuration.GetRequiredValue<int>("TimerInterval");
            ThreadCount = Configuration.GetRequiredValue<int>("ThreadCount");
            BatchSize = Configuration.GetRequiredValue<int>("BatchSize");
            AgencyId = Configuration.GetRequiredValue<int>("AgencyId");
            WipeDb = Configuration.GetRequiredValue<bool>("WipeDb");
            LastModifiedDate = Configuration.GetRequiredValue<DateTime>("LastModifiedDate");

            WipeCarrierName = Configuration.GetRequiredValue("WipeCarrierName");
            PdfFileNameContains = Configuration.GetRequiredValue("PdfFileNameContains");

            OCRInputFolderPath = Configuration.GetRequiredValue("OCRInputFolderPath");
            OCROutputFolderPath = Configuration.GetRequiredValue("OCROutputFolderPath");
            OCRArchiveFolderPath = Configuration.GetRequiredValue("OCRArchiveFolderPath");

            CsvOutputFolderPath = Configuration.GetRequiredValue("CsvOutputFolderPath");
            PDFInputFolderPath = Configuration.GetRequiredValue("PDFInputFolderPath");
            ImportArchvieFolderPath = Configuration.GetRequiredValue("ImportArchvieFolderPath");
            TextConverterProjectFolderPath = Configuration.GetRequiredValue("TextConverterProjectFolderPath");

            ClassSpec = Configuration.GetRequiredValue("ClassSpec", ParseClassSpec);
        }

        private static ClassSpec ParseClassSpec(string classSpecText)
        {
            var split = classSpecText.Split(':');
            return new ClassSpec
            {
                AssemblyName = split[0].Trim(),
                ClassName = split[1].Trim()
            };
        }
    }
}