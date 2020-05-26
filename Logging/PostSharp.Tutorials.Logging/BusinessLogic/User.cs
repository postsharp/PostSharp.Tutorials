namespace PostSharp.Tutorials.Logging.BusinessLogic
{
    public class User
    {
        public string Name { get; }
        public int VacationDaysLeft { get; set; }

        public User(string name, int vacationDaysLeft )
        {
            Name = name;
            VacationDaysLeft = vacationDaysLeft;
        }
        
     
        public override string ToString()
        {
            return this.Name;
        }
    }
}