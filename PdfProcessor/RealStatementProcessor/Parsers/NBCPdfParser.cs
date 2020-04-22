using System;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class NBCPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "NBCBonus.ConverterX" );

                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "NBCBonus_F1a.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "NBCBonus_F1.ConverterX" );

                        if (extractResult.CSVRowCount == 0) {
                            extractResult = ParsePdf( pdfFilePath, "NBCBonus_F1c.ConverterX" );

                            if (extractResult.CSVRowCount == 0) {
                                extractResult = ParsePdf( pdfFilePath, "NBCBonus_F2.ConverterX" );

                                if (extractResult.CSVRowCount == 0) {
                                    extractResult = ParsePdf( pdfFilePath, "NBCBonus_F5.ConverterX" );

                                    if (extractResult.CSVRowCount == 0) {
                                        extractResult = ParsePdf( pdfFilePath, "NBCBonus_F4.ConverterX" );

                                        if (extractResult.CSVRowCount == 0) {
                                            extractResult = ParsePdf( pdfFilePath, "NBCBonus_F7.ConverterX" );

                                            if (extractResult.CSVRowCount == 0) {
                                                extractResult = ParsePdf( pdfFilePath, "NBCBonus_F3.ConverterX" );

                                                if (extractResult.CSVRowCount == 0) {
                                                    extractResult = ParsePdf( pdfFilePath, "NBCBonus_F6.ConverterX" );

                                                    if (extractResult.CSVRowCount == 0) {
                                                        extractResult = ParsePdf( pdfFilePath, "NBCBonus_F8.ConverterX" );

                                                        if (extractResult.CSVRowCount == 0) {
                                                            extractResult = ParsePdf( pdfFilePath, "NBCBonus_F9.ConverterX" );
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                extractResult.IsBonusStatement = true;
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing NBC Marketing PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }
    }
}
