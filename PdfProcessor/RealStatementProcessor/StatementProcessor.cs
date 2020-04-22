using System;
using System.IO;
using System.Threading;

using Abbey.PdfProcessor.PremiumTransactionService;
using Abbey.PdfProcessor.RealStatementProcessor.Parsers;
using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor.RealStatementProcessor {
    internal class StatementProcessor: IStatementProcessor
    {
        public ParseOutput ProcessPdf ( string pdfFileName ) {
            try {
                var parseOutput = ParsePdf( pdfFileName );
                var storedPdfGuid = ImportPdfData( parseOutput, pdfFileName );

                if (storedPdfGuid != string.Empty) {
                    parseOutput.DocumentWasStored = true;
                }

                return parseOutput;
            }
            catch (Exception)
            {
                try {
                    Thread.Sleep( 5000 );

                    var parseOutput = ParsePdf( pdfFileName );
                    var storedPdfGuid = ImportPdfData( parseOutput, pdfFileName );

                    if(storedPdfGuid != string.Empty) {
                        parseOutput.DocumentWasStored = true;
                    }

                    return parseOutput;
                }
                catch (Exception exception) {
                    throw new ApplicationException( "Error processing " + pdfFileName, exception );
                }
            }
        }

        private static string ImportPdfData ( ParseOutput parseOutput, string pdfFileName ) {
            var storedDocumentGuid = string.Empty;
            if (parseOutput.CSVFileName != string.Empty) {
                var statementGuid = Guid.NewGuid().ToString();
                var simpleFileName = FileHelper.GetFileNameFromPath( pdfFileName ) + ".pdf";

                // Import PDF Document
                storedDocumentGuid = StorePDF( statementGuid, pdfFileName, parseOutput );

                if (storedDocumentGuid != string.Empty) {
                    // Import data in CSV file
                    CsvImporter.ImportCsvFile( storedDocumentGuid, Settings.CsvOutputFolderPath + parseOutput.CSVFileName, pdfFileName, parseOutput.IsMiscDeposit, parseOutput.IsBonusStatement );
                }

                // Archive CSV File 
                var archiveFolderPath = Settings.ImportArchvieFolderPath + simpleFileName + "\\";
                FileHelper.WipeDirectory( archiveFolderPath );
                FileHelper.MoveFile( Settings.CsvOutputFolderPath + parseOutput.CSVFileName, archiveFolderPath + parseOutput.CSVFileName );
            }

            return storedDocumentGuid;
        }

        private static ParseOutput ParsePdf ( string pdfFileName ) {
            var parseOutput = new ParseOutput();
            if (pdfFileName.Contains( "Protective" )) {
                parseOutput = ProtectivePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Protective";
            }
            else if (pdfFileName.Contains( "Principal" )) {
                parseOutput = PrincipalPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Principal";
            }
            else if (pdfFileName.Contains( "Prudential" )) {
                parseOutput = PrudentialPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Prudential";
            }
            else if (pdfFileName.Contains( "Genworth" )) {
                parseOutput = GenworthPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Genworth";
            }
            else if (pdfFileName.Contains( "Brighthouse" )) {
                parseOutput = BrightHousePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Brighthouse";
            }
            else if (pdfFileName.Contains( "Hancock" )) {
                parseOutput = JohnHancockPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "John Hancock";
            }
            else if (pdfFileName.Contains( "Lincoln" )) {
                parseOutput = LincolnPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Lincoln";
            }
            else if (pdfFileName.Contains( "Transamerica" )) {
                parseOutput = TransamericaPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Transamerica";
            }
            else if (pdfFileName.Contains( "Omaha" )) {
                parseOutput = MutualOfOmahaPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Mutual Of Omaha";
            }
            else if (pdfFileName.Contains( "American National" )) {
                parseOutput = AmericanNationalPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "American National";
            }
            else if (pdfFileName.Contains( "American General" )) {
                parseOutput = AmericanGeneralPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "American General";
            }
            else if (pdfFileName.Contains( "Penn Mutual" )) {
                parseOutput = PennMutualPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Penn Mutual";
            }
            else if (pdfFileName.Contains( "VOYA" )) {
                parseOutput = VoyaPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Voya";
            }
            else if (pdfFileName.Contains( "Symetra" )) {
                parseOutput = SymetraPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Symetra";
            }
            else if (pdfFileName.Contains( "Zurich" )) {
                parseOutput = ZurichPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Zurich";
            }
            else if (pdfFileName.Contains( "Reliance Standard" )) {
                parseOutput = RelianceStandardPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Reliance Standard";
            }
            else if (pdfFileName.Contains( "Nationwide" )) {
                parseOutput = NationwidePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Nationwide";
            }
            else if (pdfFileName.Contains( "North American" )) {
                parseOutput = NorthAmericanPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "North American";
            }
            else if (pdfFileName.Contains( "William Penn" )) {
                parseOutput = WilliamPennPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "William Penn";
            }
            else if (pdfFileName.Contains( "Assurity" )) {
                parseOutput = AssurityPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Assurity";
            }
            else if (pdfFileName.Contains( "Hartford" )) {
                parseOutput = HartfordPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Hartford";
            }
            else if (pdfFileName.Contains( "Mass Mutual" )) {
                parseOutput = MassMutualPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Mass Mutual";
            }
            else if (pdfFileName.Contains( "MetLife" )) {
                parseOutput = MetLifePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "MetLife";
            }
            else if (pdfFileName.Contains( "Standard -" )) {
                parseOutput = StandardPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Standard";
            }
            else if (pdfFileName.Contains( "AXA" )) {
                parseOutput = AxaPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "AXA";
            }
            else if (pdfFileName.Contains( "Banner" )) {
                parseOutput = BannerPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Banner";
            }
            else if (pdfFileName.Contains( "Minnesota" )) {
                parseOutput = MinnesotaLifePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Minnesota Life";
            }
            else if (pdfFileName.Contains( "Pacific" )) {
                parseOutput = PacificLifePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Pacific Life";
            }
            else if (pdfFileName.Contains( "Allstate" )) {
                parseOutput = AllstatePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Allstate";
            }
            else if (pdfFileName.Contains( "Athene" )) {
                parseOutput = AthenePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Athene";
            }
            else if (pdfFileName.Contains( "Foresters" )) {
                parseOutput = ForestersPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Foresters";
            }
            else if (pdfFileName.Contains( "Equitable" )) {
                parseOutput = EquitablePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Equitable";
            }
            else if (pdfFileName.Contains( "National Life" )) {
                parseOutput = NationalLifePdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "National Life";
            }
            else if (pdfFileName.Contains( "NBC National" )) {
                parseOutput = NBCPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "NBC Marketing";
            }
            else if (pdfFileName.Contains( "Legacy Marketing" )) {
                parseOutput = LegacyPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Ameritas";
            }
            else if (pdfFileName.Contains( "Guggenheim" )) {
                parseOutput = GuggenheimPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Guggenheim";
            }
            else if (pdfFileName.Contains( "Security Mutual" )) {
                parseOutput = SecurityMutualPdfParser.ParsePdf( pdfFileName );
                parseOutput.CarrierName = "Security Mutual";
            }
            return parseOutput;
        }

        protected static string StorePDF ( string statementGuid, string pdfFileName, ParseOutput parseOutput ) {
            // Convert PDF to Base64 string
            var bytes = File.ReadAllBytes( pdfFileName );
            var base64FileContent = Convert.ToBase64String( bytes );

            // Use Import Service to store the Base64 File content
            var importService = new PremiumTransactionServiceSoapClient();
            return importService.StorePDF( Settings.AgencyId, parseOutput.CarrierName, statementGuid, base64FileContent, pdfFileName, false );
        }
    }
}
