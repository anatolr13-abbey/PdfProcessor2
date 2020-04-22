namespace Abbey.PdfProcessor
{
    internal interface IStatementProcessor
    {
        ParseOutput ProcessPdf(string pdfFileName);
    }
}