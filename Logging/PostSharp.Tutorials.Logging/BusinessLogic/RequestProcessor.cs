using System;
using System.Threading;

namespace PostSharp.Tutorials.Logging.BusinessLogic
{
    public class RequestProcessor
    {

        public CrmClient Crm { get; } = new CrmClient("localhost", "user", "supersecret");


        public void ProcessRequests(string queuePath)
        {
            foreach (var request in Crm.GetPendingRequests(queuePath))
            {
                try
                {
                    ProcessRequest(request);
                }
                catch ( Exception )
                {
                    request.Status = RequestStatus.Failed;
                    Crm.Update(request);
                }
            }

        }

        private void ProcessRequest(Request request)
        {
            switch (request)
            {
                case VacationRequest vacationRequest:
                    ProcessVacationRequest(vacationRequest);
                    break;
                
                default:
                    throw new ArgumentNullException(nameof(request));
                    
                    
            }

        }

        private void ProcessVacationRequest(VacationRequest vacationRequest)
        {
            var user = Crm.GetUser(vacationRequest.User);

            
            if (vacationRequest.Days > user.VacationDaysLeft)
            {
                vacationRequest.Status = RequestStatus.Denied;
                Crm.Update(vacationRequest);
            }
            else
            {
                vacationRequest.Status = RequestStatus.Accepted;
                user.VacationDaysLeft -= vacationRequest.Days;
                Crm.Update(vacationRequest);
                Crm.Update(user);
            }
        }
    }
}