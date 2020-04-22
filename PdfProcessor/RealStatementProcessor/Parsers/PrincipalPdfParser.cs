using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class PrincipalPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "Principal.ConverterX" );

                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "Principal_F1.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "Principal_F2.ConverterX" );

                        if (extractResult.CSVRowCount == 0) {
                            extractResult = ParsePdf( pdfFilePath, "Principal_Producer_F1.ConverterX" );

                            if (extractResult.CSVRowCount == 0) {
                                extractResult = ParsePdf( pdfFilePath, "Principal_Producer_F2.ConverterX" );

                                if (extractResult.CSVRowCount == 0) {
                                    extractResult = ParsePdf( pdfFilePath, "Principal_F3.ConverterX" );

                                    if (extractResult.CSVRowCount == 0) {
                                        extractResult = ParsePdf( pdfFilePath, "Principal_MiscDeposit.ConverterX" );
                                        extractResult.IsMiscDeposit = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing Principal PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );
            }
        }

    }
}
