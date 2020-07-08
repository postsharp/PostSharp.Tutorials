using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PostSharp.Patterns.Xaml;
using PostSharp.Patterns.Model;
using PostSharp.Tutorials.Threading.Communication;
using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [NotifyPropertyChanged]
    [ThreadAffine]
    public partial class MainWindow : Window
    {
        [Reference]
        Random random = new Random();

        [SafeForDependencyAnalysis]
        private BoardViewModel Board => (BoardViewModel)this.DataContext;

        [Reference]
        private IConnection connection;

        public MainWindow()
        {
            InitializeComponent();
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

        private void MoveForward() => this.Board.SelectedCreature?.Creature.TryMove(step);

        [Command]
        public ICommand MoveBackwardCommand { get; private set; }

        private void MoveBackward() => this.Board.SelectedCreature?.Creature.TryMove(-step);

        [Command]
        public ICommand RotatePositiveCommand { get; private set; }

        private void RotatePositive() => this.Board.SelectedCreature?.Creature.Rotate(15);

        [Command]
        public ICommand RotateNegativeCommand { get; private set; }

        private void RotateNegative() => this.Board.SelectedCreature?.Creature.Rotate(-15);



        [Command]
        public ICommand ConnectAsServerCommand { get; private set; }


        private void ConnectAsServer()
        {

            this.SetConnection(BoardService.StartService(this.Board.Board));

            this.Title += " (server)";
        }


        public bool CanConnectAsServer => this.connection == null;



        [Command]
        public ICommand ConnectAsClientCommand { get; private set; }

        private void ConnectAsClient()
        {
            this.SetConnection(BoardServiceClient.Connect(this.Board.Board));
            

            this.Title += " (client)";

        }
        
        [Command]
        public ICommand DisconnectCommand { get; private set; }

        public bool CanExecuteDisconnect => this.connection != null;

        private void Disconnect() => this.connection?.Close();

        private void SetConnection( IConnection connection )
        {
            this.connection = connection;
            this.connection.Closed += OnConnectionClosed;

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
            this.Board.Board.Creatures.Add(creature);
            this.Board.SelectedCreature = this.Board.Creatures[creature.Id];
        }


    }
}
