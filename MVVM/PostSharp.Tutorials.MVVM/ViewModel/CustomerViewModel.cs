using System;
using System.Windows.Input;
using PostSharp.Tutorials.MVVM.Model;

namespace PostSharp.Tutorials.MVVM.ViewModel
{
    public class CustomerViewModel
    {
        public CustomerModel Customer { get; set; }

        public AddressModel DisplayedAddress { get; set; }

        public bool IsDisplayAddressPrincipal => this.DisplayedAddress == this.Customer?.PrincipalAddress;

        public void AssignDisplayedAddressToPrincipal() => this.Customer.PrincipalAddress = this.DisplayedAddress;

        public string LabelContent
        {
            get
            {
                if (Customer == null)
                {
                    return "(No Data)";
                }

                return $"{Customer.FirstName} {Customer.LastName}\n\f{Customer.PrincipalAddress?.FullAddress?.Replace("; ", Environment.NewLine)}";
            }
        }
     
    }
}