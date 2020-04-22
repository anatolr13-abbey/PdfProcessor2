using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Abbey.PdfProcessor.RealStatementProcessor.Parsers {
    internal class PrudentialPdfParser : PdfParser {
        /// <summary>
        /// Extract data from PDF and write out data to a Csv file
        /// </summary>
        /// <param name="pdfFilePath">Fully qualified path of the PDF file</param>
        public static ParseOutput ParsePdf ( string pdfFilePath ) {
            try {
                var extractResult = ParsePdf( pdfFilePath, "Prudential.ConverterX" );
                if (extractResult.CSVRowCount == 0) {
                    extractResult = ParsePdf( pdfFilePath, "Prudential_F2.ConverterX" );

                    if (extractResult.CSVRowCount == 0) {
                        extractResult = ParsePdf( pdfFilePath, "Prudential_F3.ConverterX" );
                    }
                }
                return extractResult;
            }
            catch (Exception e) {
                var errorMessage = new StringBuilder();
                errorMessage.AppendFormat( "Error Parsing Standard Prudential PDF: {0}", pdfFilePath );

                throw new ApplicationException( errorMessage.ToString(), e );

            }
        }

        private static void PreProcessCSVFile ( string csvFileName ) {
            // Initialize local variables
            var csvFile = new StreamReader( csvFileName );
            var rawLines = new List<string>();

            // Read CSV file into memory
            var currentLine = csvFile.ReadLine();
            while (currentLine != null) {
                rawLines.Add( currentLine );
                currentLine = csvFile.ReadLine();
            }
            csvFile.Close();

            // Merge Lines 
            var mergedLines = MergeLines( rawLines );

            // Overwrite CSV file with Merged lines
            var outputFile = new StreamWriter( csvFileName, false );
            foreach (var mergedLine in mergedLines) {
                outputFile.WriteLine( mergedLine );
            }
            outputFile.Flush();
            outputFile.Close();
        }

        private static List<string> MergeLines ( List<string> rawLines ) {
            // Initialize local variables
            var mergedLines = new List<string> {
                rawLines[0]
            };

            if (rawLines.Count == 2) {
                mergedLines.Add( CleanUpPolicyNumber( rawLines[1] ) );
                return mergedLines;
            }

            // Iterate over each line
            var loopIndex = 1;
            while (loopIndex < rawLines.Count - 1) {
                // Extract the Policy number
                var currentLine = rawLines[loopIndex];
                var currentLineParts = currentLine.Split( '|' );
                var policyNumber = currentLineParts[0];

                if (policyNumber.Contains( "(" )) {
                    // Extract the Policy number from the next Line
                    var nextLine = rawLines[loopIndex + 1];
                    var nextLineParts = nextLine.Split( '|' );
                    var nextPolicyNumber = nextLineParts[0];

                    if (!nextPolicyNumber.Contains( "(" )) {
                        // Replace the Date on the Current line with the Date from the Next line
                        var currentDueDate = currentLineParts[1];
                        var nextDueDate = nextLineParts[1];
                        currentLine = currentLine.Replace( currentDueDate, nextDueDate );

                        // Replace the Date on the Current line with the Date from the Next line
                        currentDueDate = currentLineParts[6];
                        nextDueDate = nextLineParts[6];
                        currentLine = currentLine.Replace( currentDueDate, nextDueDate );
                    }
                    else {
                        mergedLines.Add( CleanUpPolicyNumber( nextLine ) );
                    }

                    if (loopIndex == 1) {
                        // Cleanup the Policy number and Add the current line into the MergedLines List
                        mergedLines.Add( CleanUpPolicyNumber( currentLine ) );
                    }
                }
                loopIndex++;
            }
            return mergedLines;
        }

        private static string CleanUpPolicyNumber ( string currentLine ) {
            // Extract the dirtry Policy number
            var currentLineParts = currentLine.Split( '|' );
            var dirtyPolicyNumber = currentLineParts[0];

            // Remove unwanted characters and create a clean Policy number
            var startIndex = dirtyPolicyNumber.IndexOf( '(' ) + 1;
            var endIndex = dirtyPolicyNumber.IndexOf( ')' );
            var cleanPolicyNumber = dirtyPolicyNumber.Substring( startIndex, endIndex - startIndex );

            // Replace the dirty Policy number with the Clean policy number and return the Line
            return currentLine.Replace( dirtyPolicyNumber, cleanPolicyNumber );
        }

    }
}
