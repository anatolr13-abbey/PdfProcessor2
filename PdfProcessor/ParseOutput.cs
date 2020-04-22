using System;

namespace Abbey.PdfProcessor
{
    internal class ParseOutput
    {
        public bool IsMiscDeposit { get; set; }
        public bool IsBonusStatement { get; set; }
        public bool DocumentWasStored { get; set; }
        public string CarrierName { get; set; }
        public string PdfFileName { get; set; }
        public string CSVFileName { get; set; }
        public string ConverterName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Duration { get; set; }
        public int CSVRowCount { get; set; }
    }
}