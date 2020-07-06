using System.Collections.ObjectModel;

namespace PostSharp.Tutorials.MVVM.Model
{
    public class CustomerModel : ModelBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public ObservableCollection<AddressModel> Addresses { get; set; }

        public AddressModel PrincipalAddress { get; set; }

    }
}