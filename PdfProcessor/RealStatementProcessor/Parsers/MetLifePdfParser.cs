using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class MetLifePdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "MetLife.ConverterX" );

                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "MetLife_F2.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "MetLife_F3.ConverterX" );


                        if (extractResult.CSVRowCount == 0) {
                            extractResult = ParsePdf( pdfFilePath, "MetLife_F4.ConverterX" );
                        }
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing MetLife PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
