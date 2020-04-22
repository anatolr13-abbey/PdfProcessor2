namespace Abbey.PdfProcessor.RealStatementProcessor
{
    internal class ConverterX
    {
        public void OpenProject(string fileName)
        {
        }

        public void SetInputFile(string fileName)
        {
        }

        public void SetBatch(int num)
        {
        }

        public int Convert(string s)
        {
            return 0;
        }

        public ITDataSource GetOutputDS()
        {
            return null;
        }

        internal void Close()
        {
        }
    }

    internal interface ITDataSource
    {
        void SetDS(string outputFilePath, string s );
        bool csv_include_column_names { get; set; }
        string csv_col_separator { get; set; }
    }
}
