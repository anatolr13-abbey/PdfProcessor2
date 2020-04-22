using Abbey.Core.Application;

namespace Abbey.PdfProcessor
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ServiceProcessHelper.Run(new PdfProcessorService(), args);
        }
    }
}