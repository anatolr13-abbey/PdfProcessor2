using System;

using Abbey.Core.Application;
using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor
{
    internal static class Settings
    {
        public static int TimerInterval { get; }

        public static int AgencyId { get; }
        public static string WipeCarrierName { get; }
        public static DateTime LastModifiedDate { get; }

        public static string CsvOutputFolderPath { get; }
        public static string PDFInputFolderPath { get; }
        public static string ImportArchiveFolderPath { get; }
        public static string TextConverterProjectFolderPath { get; }

        public static ClassSpec ClassSpec { get; set; }

        static Settings()
        {
            TimerInterval = Configuration.GetRequiredValue<int>("TimerInterval");

            AgencyId = Configuration.GetRequiredValue<int>("AgencyId");
            WipeCarrierName = Configuration.GetRequiredValue("WipeCarrierName");
            LastModifiedDate = Configuration.GetRequiredValue<DateTime>("LastModifiedDate");

            CsvOutputFolderPath = Configuration.GetRequiredValue("CsvOutputFolderPath");
            PDFInputFolderPath = Configuration.GetRequiredValue("PDFInputFolderPath");
            ImportArchiveFolderPath = Configuration.GetRequiredValue("ImportArchiveFolderPath");
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