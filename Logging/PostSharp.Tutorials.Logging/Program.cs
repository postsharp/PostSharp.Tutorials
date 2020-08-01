using PostSharp.Tutorials.Logging.BusinessLogic;
using System;

namespace PostSharp.Tutorials.Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestProcessor requestProcessor = new RequestProcessor();
            requestProcessor.ProcessRequests("requestsQueue");
        }
    }
}
