using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace PostSharp.Samples.Xaml
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