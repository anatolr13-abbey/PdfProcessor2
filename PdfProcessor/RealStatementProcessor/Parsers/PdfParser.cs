using System;
using System.Text;
using System.Threading;

using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal abstract class PdfParser {
        private static readonly object _myLock = new object();

        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        /// <param name="converterFileName"></param>
        protected static ParseOutput ParsePdf ( string pdfFilePath, string converterFileName ) {
            try {
                LogFileWriter.WriteErrorLine( "[" + converterFileName + "] Processing " + FileHelper.GetFileNameFromPath( pdfFilePath ) + ".pdf (" + DateTime.Now.ToString( "HH:mm:ss" ) + ")" );

                return TryParse( pdfFilePath, converterFileName );
            }
            catch {
                try {
                    Thread.Sleep( 10000 );
                    return TryParse( pdfFilePath, converterFileName );
                }
                catch (Exception e) {
                    var errorMessage = new StringBuilder();
                    errorMessage.AppendFormat( "********************** Error Parsing PDF: {0} [{1}]", FileHelper.GetFileNameFromPath( pdfFilePath ) + ".pdf", e.Message );
                    LogFileWriter.WriteErrorLine( pdfFilePath );
                    LogFileWriter.WriteErrorLine( errorMessage.ToString() );
                    LogFileWriter.WriteErrorLine( e.StackTrace );
                    return new ParseOutput {
                        IsMiscDeposit = false,
                        PdfFileName = pdfFilePath,
                        ConverterName = converterFileName,
                        CSVFileName = string.Empty,
                        CSVRowCount = 0
                    };
                }
            }
        }

        private static ParseOutput TryParse ( string pdfFilePath, string converterFileName ) {
            int outputLineCount;
            var csvFileName = FileHelper.GetFileNameFromPath( pdfFilePath ) + ".csv";
            var outputFilePath = Settings.CsvOutputFolderPath + csvFileName;

            lock (_myLock) {
                // Prepare ConverterX instance
                ConverterX cnv = new ConverterX();
                try {
                    cnv.OpenProject( Settings.TextConverterProjectFolderPath + converterFileName );
                }
                catch {
                    LogFileWriter.WriteErrorLine( "Open failed (" + pdfFilePath + ")" );
                    LogFileWriter.WriteErrorLine( "Reattempting to open: " + FileHelper.GetFileNameFromPath( pdfFilePath ) + ".csv (" + DateTime.Now.ToString( "HH:mm:ss" ) + ")" );
                    cnv.OpenProject( Settings.TextConverterProjectFolderPath + converterFileName );
                }
                cnv.SetInputFile( pdfFilePath );

                // Extract data from PDF File
                ITDataSource src = cnv.GetOutputDS();

                // Prepare for Csv creation
                src.SetDS( outputFilePath, string.Empty );
                src.csv_include_column_names = true;
                src.csv_col_separator = "|";
                cnv.SetBatch( 0 );

                // Create CSV extract file
                outputLineCount = cnv.Convert( "" );
                var csvMessage = new StringBuilder();
                csvMessage.AppendFormat( "Output: {0} \t\t Lines: {1}\r\n", csvFileName, outputLineCount );
                LogFileWriter.WriteLine( csvMessage.ToString() );

                // Close ConverterX instance
                cnv.Close();
            }

            return new ParseOutput {
                IsMiscDeposit = false,
                PdfFileName = pdfFilePath,
                ConverterName = converterFileName,
                CSVFileName = csvFileName,
                CSVRowCount = outputLineCount
            };
        }

    }
}
