using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class AthenePdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "Athene.ConverterX" );
                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "Athene_F2.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "Athene_F3.ConverterX" );
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing Athene PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
