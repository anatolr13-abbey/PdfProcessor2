using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class NationalLifePdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "NationalLife.ConverterX" );
                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "NationalLife_F2.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "NationalLife_F3.ConverterX" );

                        if (extractResult.CSVRowCount == 0) {
                            extractResult = ParsePdf( pdfFilePath, "NationalLife_F4.ConverterX" );

                            if (extractResult.CSVRowCount == 0) {
                                extractResult = ParsePdf( pdfFilePath, "NationalLife_F5.ConverterX" );

                                if (extractResult.CSVRowCount == 0) {
                                    extractResult = ParsePdf( pdfFilePath, "NationalLife_F6.ConverterX" );
                                }
                            }
                        }
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing NationalLife PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
