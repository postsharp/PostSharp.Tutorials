using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using PostSharp.Patterns.Xaml;
using PostSharp.Patterns.Model;
using PostSharp.Tutorials.Threading.Communication;
using PostSharp.Patterns.Threading;
using PostSharp.Tutorials.Threading.Model;
using PostSharp.Tutorials.Threading.ViewModel;

namespace PostSharp.Tutorials.Threading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [NotifyPropertyChanged]
    [ThreadAffine]
    public partial class MainWindow : Window
    {
        [SafeForDependencyAnalysis]
        private BoardViewModel Board => (BoardViewModel)this.DataContext;

        private IConnection connection;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Creature_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            var creature = (CreatureViewModel)rectangle.DataContext;

            this.Board.SelectedCreature = creature;
        }

        private const double step = 0.1;

        [Command]
        public ICommand MoveForwardCommand { get; private set; }

        private void MoveForward() => this.Board.SelectedCreature?.Model.TryMove(step);

        [Command]
        public ICommand MoveBackwardCommand { get; private set; }

        private void MoveBackward() => this.Board.SelectedCreature?.Model.TryMove(-step);

        [Command]
        public ICommand RotatePositiveCommand { get; private set; }

        private void RotatePositive() => this.Board.SelectedCreature?.Model.Rotate(15);

        [Command]
        public ICommand RotateNegativeCommand { get; private set; }

        private void RotateNegative() => this.Board.SelectedCreature?.Model.Rotate(-15);

        [Command]
        public ICommand DeleteCommand
        {
            get; private set;
        }

        private void Delete() 
        {
            if (this.Board.SelectedCreature != null)
            {
                this.Board.Model.Creatures.Remove(this.Board.SelectedCreature.Model);
            }
        }

        [Command]
        public ICommand ConnectAsServerCommand { get; private set; }


        private void ConnectAsServer()
        {

            this.SetConnection(BoardService.StartService(this.Board.Model));

            this.Title += " (server)";
        }


        public bool CanConnectAsServer => this.connection == null;



        [Command]
        public ICommand ConnectAsClientCommand { get; private set; }

        private void ConnectAsClient()
        {
            this.Board.Model.IsMaster = false;
            this.SetConnection(BoardServiceClient.Connect(this.Board.Model));
            

            this.Title += " (client)";

        }
        
        [Command]
        public ICommand DisconnectCommand { get; private set; }

        public bool CanExecuteDisconnect => this.connection != null;

        private void Disconnect()
        {
            this.connection?.Close();
            this.Board.Model.IsMaster = true;
        }

        private void SetConnection( IConnection connection )
        {
            this.connection = connection;
            this.connection.Closed += this.OnConnectionClosed;

        }

        [Dispatched]
        private void OnConnectionClosed(object sender, EventArgs e)
        {
            this.connection = null;
        }

        public bool CanConnectAsClient => this.connection == null;

        [Command]
        public ICommand AddCreatureCommand { get; private set; }

        private void AddCreature()
        {
            var creature = RandomGenerator.Instance.CreateCreature();
            this.Board.Model.Creatures.Add(creature);
            this.Board.SelectedCreature = this.Board.Creatures[creature.Id];
        }


    }
}
