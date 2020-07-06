using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PostSharp.Patterns.Xaml;
using PostSharp.Tutorials.MVVM.Model;
using PostSharp.Tutorials.MVVM.ViewModel;

namespace PostSharp.Tutorials.MVVM
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomerViewModel _customerViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // Replace the design-time data context with real data.

            this._customerViewModel = new CustomerViewModel
            {
                Customer =
                    new CustomerModel
                    {
                        FirstName = "Jan",
                        LastName = "Novák",
                        Addresses = new ObservableCollection<AddressModel>
                        {
                            new AddressModel
                            {
                                Line1 = "Šaldova 1G",
                                Town = "Praha"
                            },
                            new AddressModel
                            {
                                Line1 = "Tyršova 25",
                                Town = "Brno"
                            },
                            new AddressModel
                            {
                                Line1 = "Pivovarká 154",
                                Town = "Plzeň"
                            }
                        }
                    }
            };

            _customerViewModel.Customer.PrincipalAddress = _customerViewModel.Customer.Addresses[0];

            DataContext = _customerViewModel;

        }

        private void ExecuteSetPrincipalAddress() => this._customerViewModel.Customer.PrincipalAddress = this._customerViewModel.CurrentAddress;


        [Command] 
        public ICommand SetPrincipalAddressCommand { get; private set; }

        public bool CanExecuteSetPrincipalAddress =>
            this._customerViewModel != null &&
            this._customerViewModel.CurrentAddress != null &&
            this._customerViewModel.Customer != null &&
            !this._customerViewModel.IsCurrentAddressPrincipal;

        


    }
}