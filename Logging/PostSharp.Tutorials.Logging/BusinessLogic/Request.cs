using System;

namespace PostSharp.Tutorials.Logging.BusinessLogic
{
    public class Request
    {
        public int Id { get; }
        public string User { get; }

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public Request(int id, string user)
        {
            Id = id;
            User = user;
        }

        public override string ToString()
        {
            return $"Request Id={Id}";
        }
    }

    public enum RequestStatus
    {
        Pending,
        Accepted,
        Denied,
        Failed
    }

    public class VacationRequest : Request
    {
        public DateTime From { get; }
        public int Days { get; }

        public VacationRequest(int id, string user, DateTime @from, int days) : base(id, user)
        {
            From = @from;
            Days = days;
        }
    }
}