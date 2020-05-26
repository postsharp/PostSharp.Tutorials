using System;
using System.Collections.Generic;
using System.Threading;

namespace PostSharp.Tutorials.Logging.BusinessLogic
{
    public class CrmClient
    {
        private readonly string password;
        public string Server { get; }
        public string Login { get; }

        public CrmClient(string server, string login, string credentials)
        {
            Server = server;
            Login = login;
            this.password = credentials;
        }

        public IEnumerable<Request> GetPendingRequests(string queue)
        {
            yield return new VacationRequest(67, "anticgold", new DateTime(2020, 5, 20 ), 5 );
            yield return new VacationRequest(145, "slopyplain", new DateTime(2020, 6, 15 ), 10 );
            yield return new VacationRequest(56, null, new DateTime(2020, 7, 1 ), 3 );
            yield return new VacationRequest(86, "guest", new DateTime(2020, 4, 3 ), 5 );
        }


        public User GetUser(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            
            switch (name)
            {
                case "anticgold":
                    return new User(name, 10);
                
                case "slopyplain":
                    Thread.Sleep(100);
                    return new User(name, 20);
                
                default:
                    throw new ArgumentOutOfRangeException();
                    
            }
        }

        public void Update(object entity)
        {
            Thread.Sleep(60);
        }

        public override string ToString()
        {
            return $"CrmClient {this.Login}@{this.Server}";
        }
    }
}