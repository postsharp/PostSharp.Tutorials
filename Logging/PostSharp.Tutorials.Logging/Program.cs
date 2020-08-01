using System;
using PostSharp.Tutorials.Logging.BusinessLogic;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;

namespace PostSharp.Tutorials.Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure PostSharp Logging to write to the console.
            var backend = new ConsoleLoggingBackend();
            backend.Options.Theme = ConsoleThemes.Dark;
            backend.Options.IncludeExceptionDetails = false;
            LoggingServices.DefaultBackend = backend;

            RequestProcessor requestProcessor = new RequestProcessor();
            requestProcessor.ProcessRequests("requestsQueue");
        }
    }
}
