using System;
using System.Windows.Input;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Xaml;
using PostSharp.Tutorials.MVVM.Model;

namespace PostSharp.Tutorials.MVVM.ViewModel
{
    [NotifyPropertyChanged]
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


        [Command] public ICommand SetPrincipalAddressCommand { get; private set; }

        public bool CanExecuteSetPrincipalAddress =>
            this.DisplayedAddress != null &&
            this.Customer != null &&
            !this.IsDisplayAddressPrincipal;

        private void ExecuteSetPrincipalAddress() => this.AssignDisplayedAddressToPrincipal();
    }
}