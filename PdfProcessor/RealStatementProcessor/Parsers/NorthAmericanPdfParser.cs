using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class NorthAmericanPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "NorthAmerican.ConverterX" );
                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "NorthAmerican_F2.ConverterX" );
                    if (extractResult.CSVRowCount == 0) {
                        return ParsePdf( pdfFilePath, "NorthAmerican_F3.ConverterX" );
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing North American PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );
            }
        }
    }
}
