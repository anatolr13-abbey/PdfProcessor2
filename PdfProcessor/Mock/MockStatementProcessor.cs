using Abbey.PdfProcessor.Utility;

namespace Abbey.PdfProcessor.Mock
{
    internal class MockStatementProcessor : IStatementProcessor
    {
        /// <inheritdoc />
        public ParseOutput ProcessPdf(string pdfFileName)
        {
            LogFileWriter.WriteLine($"Processed file {pdfFileName}.");
            return new ParseOutput();
        }
    }
}