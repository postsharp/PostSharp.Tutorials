using System.Collections.ObjectModel;
using PostSharp.Patterns.Contracts;

namespace PostSharp.Tutorials.MVVM.Model
{
    public class CustomerModel : ModelBase
    {
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public ObservableCollection<AddressModel> Addresses { get; set; }

        public AddressModel PrincipalAddress { get; set; }

    }
}