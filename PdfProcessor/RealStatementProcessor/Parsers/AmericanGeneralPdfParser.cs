using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class AmericanGeneralPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "AmericanGeneral.ConverterX" );
                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "AmericanGeneral_F2.ConverterX" );
                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "USL-NY.ConverterX" );
                        if (extractResult.CSVRowCount == 0) {
                            extractResult = ParsePdf( pdfFilePath, "USL-NY_F2.ConverterX" );
                            if (extractResult.CSVRowCount == 0) {
                                return ParsePdf( pdfFilePath, "AmericanGeneral_AgentSearch.ConverterX" );
                            }
                        }
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing American General PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
