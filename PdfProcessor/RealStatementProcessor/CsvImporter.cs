using System;
using System.IO;

using Abbey.PdfProcessor.PremiumTransactionService;
using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor.RealStatementProcessor
{
    internal class CsvImporter
    {
        public static void ImportCsvFile(string statementGuid, string csvFilename, string pdfFileName, bool isMiscDeposit, bool isBonusTransaction)
        {
            LogFileWriter.WriteLine("Staging data from: " + csvFilename);

            using (var csvFile = new StreamReader(csvFilename))
            {
                var currentLine = csvFile.ReadLine();
                try
                {
                    var skipLine = true;
                    while (currentLine != null)
                    {
                        if (skipLine)
                        {
                            // skip the header row
                            skipLine = false;
                        }
                        else
                        {
                            if (isMiscDeposit)
                            {
                                // Import current Csv Deposit
                                ImportCsvDeposit(statementGuid, currentLine);
                            }
                            else if (isBonusTransaction)
                            {
                                // Import current Csv Transaction
                                ImportCsvBonusTransaction(statementGuid, pdfFileName, currentLine);
                            }
                            else
                            {
                                // Import current Csv Transaction
                                ImportCsvTransaction(statementGuid, pdfFileName, currentLine);
                            }
                        }

                        currentLine = csvFile.ReadLine();
                    }
                }
                catch (Exception exception)
                {
                    var message = $"Error reading CSV file {csvFilename} --> line. ({currentLine})";
                    throw new ApplicationException(message, exception);
                }
            }
        }

        public static void WipeStaged()
        {
            var importService = new PremiumTransactionServiceSoapClient();
            importService.WipeStagedTransactions(Settings.AgencyId, Settings.WipeCarrierName);
        }

        private static void ImportCsvDeposit(string statementGuid, string currentCsvLine)
        {
            var importService = new PremiumTransactionServiceSoapClient();
            var newPdfPremiumTransaction = ExtractDepositFromCsvLine(currentCsvLine);
            newPdfPremiumTransaction.StatementGuid = statementGuid;

            importService.StorePdfDeposit(Settings.AgencyId, newPdfPremiumTransaction);
        }

        private static void ImportCsvTransaction(string statementGuid, string carrierName, string currentCsvLine)
        {
            var importService = new PremiumTransactionServiceSoapClient();
            var newPdfPremiumTransaction = ExtractDataFromCsvLine(currentCsvLine);
            newPdfPremiumTransaction.StatementGuid = statementGuid;

            importService.StorePdfTransaction(Settings.AgencyId, carrierName, newPdfPremiumTransaction);
        }

        private static void ImportCsvBonusTransaction(string statementGuid, string carrierName, string currentCsvLine)
        {
            var importService = new PremiumTransactionServiceSoapClient();
            var newPdfBonusTransaction = ExtractBonusFromCsvLine(currentCsvLine);
            newPdfBonusTransaction.StatementGuid = statementGuid;

            importService.StorePdfBonusTransaction(Settings.AgencyId, carrierName, newPdfBonusTransaction);
        }

        private static PdfPremiumTransaction ExtractDepositFromCsvLine(string csvLine)
        {
            var csvValues = csvLine.Split('|');
            var newPremiumTransaction = new PdfPremiumTransaction
            {
                StatementDate = Convert.ToDateTime(csvValues[0]).ToString("yyyy-MM-dd"),
                StatementProducer = csvValues[1],
                DisbursementAmount = Convert.ToDecimal(csvValues[2]),
                TransactionDate = DateTime.Today.ToString("yyyy-MM-dd"),
                EffectiveDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd"),
                PaidToDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd"),
                StagedDate = DateTime.Today.ToString("yyyy-MM-dd"),
                ImportDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd")
            };

            return newPremiumTransaction;
        }

        private static PdfPremiumTransaction ExtractDataFromCsvLine(string csvLine)
        {
            var csvValues = csvLine.Split('|');
            var newPremiumTransaction = new PdfPremiumTransaction
            {
                StatementContractNumber = csvValues[0],
                StatementDate = Convert.ToDateTime(csvValues[1]).ToString("yyyy-MM-dd"),
                StatementProducer = csvValues[2],
                DisbursementAmount = Convert.ToDecimal(csvValues[3]),
                PolicyProducer = csvValues[4],
                ProducerContractNumber = csvValues[5],
                PolicyNumber = csvValues[6],
                TransactionDate = Convert.ToDateTime(csvValues[7]).ToString("yyyy-MM-dd"),
                Amount = Convert.ToDecimal(csvValues[8]),
                ExcessAmount = Convert.ToDecimal(csvValues[9]),
                PaidToProducer = Convert.ToDecimal(csvValues[10]),
                PaidToBGA = Convert.ToDecimal(csvValues[11]),
                StagedDate = DateTime.Today.ToString("yyyy-MM-dd"),
                ImportDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd")
            };

            if (csvValues.Length > 12)
            {
                newPremiumTransaction.EffectiveDate = Convert.ToDateTime(csvValues[12]).ToString("yyyy-MM-dd");
                newPremiumTransaction.Insured = csvValues[13];
                newPremiumTransaction.Product = csvValues[14];
                newPremiumTransaction.IssueState = csvValues[15];
                newPremiumTransaction.Mode = csvValues[16];
            }
            else
            {
                newPremiumTransaction.EffectiveDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd");
                newPremiumTransaction.Insured = string.Empty;
                newPremiumTransaction.Product = string.Empty;
                newPremiumTransaction.IssueState = string.Empty;
                newPremiumTransaction.Mode = string.Empty;
            }

            if (csvValues.Length > 17)
            {
                newPremiumTransaction.PaidToDate = Convert.ToDateTime(csvValues[17]).ToString("yyyy-MM-dd");
            }
            else
            {
                newPremiumTransaction.PaidToDate = Convert.ToDateTime("01/01/1900").ToString("yyyy-MM-dd");
            }

            return newPremiumTransaction;
        }

        private static PdfBonusTransaction ExtractBonusFromCsvLine(string csvLine)
        {
            var csvValues = csvLine.Split('|');
            var newBonusTransaction = new PdfBonusTransaction
            {
                StatementContractNumber = csvValues[0],
                StatementDate = ExtractDateValue(csvValues[1]).ToString("yyyy-MM-dd"),
                DisbursementAmount = Convert.ToDecimal(csvValues[2]),
                PolicyProducer = csvValues[3],
                ProducerContractNumber = csvValues[4],
                PolicyNumber = csvValues[5],
                TransactionDate = ExtractDateValue(csvValues[6]).ToString("yyyy-MM-dd"),
                Amount = Convert.ToDecimal(csvValues[7]),
                ExcessAmount = Convert.ToDecimal(csvValues[8]),
                PaidToBGA = Convert.ToDecimal(csvValues[9]),
                StagedDate = DateTime.Today.ToString("yyyy-MM-dd"),
                ImportDate = ExtractDateValue("01/01/1900").ToString("yyyy-MM-dd")
            };

            return newBonusTransaction;
        }

        private static DateTime ExtractDateValue(string dateTextValue)
        {
            try
            {
                return Convert.ToDateTime(dateTextValue);
            }
            catch (Exception e)
            {
                var message = $"Error converting to date ({dateTextValue})";
                throw new ApplicationException(message, e);
            }
        }
    }
}