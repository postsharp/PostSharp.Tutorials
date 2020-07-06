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

        public AddressModel CurrentAddress { get; set; }

        public bool IsCurrentAddressPrincipal => this.CurrentAddress == this.Customer?.PrincipalAddress;

        
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