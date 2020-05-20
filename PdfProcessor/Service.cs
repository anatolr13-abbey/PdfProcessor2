using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

using Abbey.PdfProcessor.Utility;

using Timer = System.Timers.Timer;

namespace Abbey.PdfProcessor
{
    public partial class PdfProcessorService : ServiceBase
    {
        private Timer _timer;
        private static int _timerInterval;

        public PdfProcessorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            FileHelper.DeleteFile(@"c:\temp\PDFExtract.log");
            FileHelper.DeleteFile(@"c:\temp\PDFExtract.csv");
            FileHelper.DeleteFile(@"c:\temp\PDFExtractEmptyFiles.csv");

            _timer = new Timer(5000)
            {
                AutoReset = false
            };
            _timer.Elapsed += ParsePdfs;
            _timer.Start();
            _timerInterval = Settings.TimerInterval * 1000;
        }

        protected override void OnStop()
        {
            _timer.Close();
        }

        // ENTRY POINT
        private void ParsePdfs(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _timer.Stop();

            try
            {
                var pdfDirectories = Directory.GetDirectories(Settings.PDFInputFolderPath);
                ProcessDirectories(pdfDirectories);
            }
            catch (Exception exception)
            {
                const string errorMessage = "Processing Error";
                LogFileWriter.WriteErrorLine("------------------------------------------------------------------");
                LogFileWriter.WriteErrorLine(errorMessage);
                LogFileWriter.WriteErrorLine(exception.Message);
                LogFileWriter.WriteErrorLine("------------------------------------------------------------------");
            }
            finally
            {
                // Restart timer.
                _timer.Interval = _timerInterval;
                _timer.Start();
            }
        }

        private static void ProcessDirectories(IEnumerable<string> directories)
        {
            foreach (var directoryPath in directories)
            {
                ProcessDirectoryPdfFiles(directoryPath);
            }
        }

        private static void ProcessDirectoryPdfFiles(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var pdfFileInfoList = directoryInfo.GetFiles("*.pdf");

            //if (Settings.PdfFileNameContains != "")
            //{
            //    pdfFileInfoList = directoryInfo.GetFiles("*" + Settings.PdfFileNameContains + "*.pdf");
            //}

            foreach (var fileInfo in pdfFileInfoList)
            {
                if (fileInfo.LastWriteTime > Settings.LastModifiedDate)
                {
                    var threadData = new ThreadData
                    {
                        FileName = fileInfo.Name,
                        FullyQualifiedName = fileInfo.FullName
                    };
                    ThreadPool.QueueUserWorkItem(AsyncProcessPdf, threadData);
                }
            }
        }

        private static void AsyncProcessPdf(object stateInfo)
        {
            var threadParameters = (ThreadData) stateInfo;

            try
            {
                var durationStopWatch = new Stopwatch();
                durationStopWatch.Start();
                var startTime = DateTime.Now;

                var statementProcessor = Factory.CreateInstance<IStatementProcessor>(Settings.ClassSpec);
                var parseOutput = statementProcessor.ProcessPdf(threadParameters.FullyQualifiedName);

                durationStopWatch.Stop();

                parseOutput.StartTime = startTime;
                parseOutput.EndTime = DateTime.Now;
                parseOutput.Duration = durationStopWatch.ElapsedMilliseconds / 1000.0M;

                if (parseOutput.DocumentWasStored)
                {
                    LogFileWriter.WriteParseOutput(parseOutput);
                }
            }
            catch (Exception exception)
            {
                var errorMessage = "Error processing " + threadParameters.FileName;
                LogFileWriter.WriteErrorLine("------------------------------------------------------------------");
                LogFileWriter.WriteErrorLine(errorMessage);
                LogFileWriter.WriteErrorLine(exception.Message);
                LogFileWriter.WriteErrorLine("------------------------------------------------------------------");
            }
        }
    }
}