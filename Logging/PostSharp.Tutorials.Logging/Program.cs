using System;
using PostSharp.Tutorials.Logging.BusinessLogic;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using Serilog;

namespace PostSharp.Tutorials.Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            // The output template must include {Indent} for nice output.
            const string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Indent:l}{Message}{NewLine}{Exception}";

            // Configure a Serilog logger.
            var logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File("serilog.log", outputTemplate: template)
              .WriteTo.ColoredConsole(outputTemplate: template)
              .CreateLogger();

            // Configure PostSharp Logging to use Serilog
            var backend = new SerilogLoggingBackend(logger);
            // backend.Options.UseSerilogFormatters = true;
            LoggingServices.DefaultBackend = backend;

            RequestProcessor requestProcessor = new RequestProcessor();
            requestProcessor.ProcessRequests("requestsQueue");
        }
    }
}
