using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class StandardPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                return ParsePdf( pdfFilePath, "Standard_iTrust.ConverterX" );
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing Standard_iTrust PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
