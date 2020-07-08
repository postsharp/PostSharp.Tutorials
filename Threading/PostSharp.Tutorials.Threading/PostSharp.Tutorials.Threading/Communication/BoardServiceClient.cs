using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace PostSharp.Tutorials.Threading.Communication
{
    internal interface IConnection
    {
        event EventHandler Closed;
        void Close();
    }

    [Immutable]
    internal class BoardServiceClient : IBoardCallback, IConnection
    {
        private readonly Board board;

        private IBoardService serviceProxy;

        public event EventHandler Closed;

        private BoardServiceClient(Board board)
        {
            this.board = board;

            this.Connect();

            this.board.Creatures.CollectionChanged += this.OnCollectionChanged;

            this.Subscribe(this.board.Creatures);
        }

        void Connect()
        {
            var group = ServiceModelSectionGroup.GetSectionGroup(
                  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

            //create duplex channel factory
            var serviceFactory = new DuplexChannelFactory<IBoardService>(
                                          this, group.Client.Endpoints[0].Name);
            //create a communication channel and register for its events
            this.serviceProxy = serviceFactory.CreateChannel();

            var channel = (IClientChannel) this.serviceProxy;
            channel.Open();
            channel.Closed += this.OnChannelClosed;
            channel.Faulted += this.OnChannelClosed;


            this.board.Creatures.Clear();
            foreach (var creature in this.serviceProxy.GetCreatures())
            {
                this.board.Creatures.Add(creature);
            }
        }

        private void OnChannelClosed(object sender, EventArgs e)
        {
            this.Closed?.Invoke(sender, e);
        }

        public static BoardServiceClient Connect(Board board)
        {
            return new BoardServiceClient(board);
        }

        private void Subscribe(IEnumerable<Creature> creatures)
        {
            foreach ( var creature in creatures )
            {
                this.Subscribe(creature);
            }
        }

        private void Subscribe(Creature creature)
        {
            Post.Cast<Creature, INotifyPropertyChanged>(creature).PropertyChanged += this.OnCreaturePropertyChanged;
        }

        private void OnCreaturePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var creature = (Creature)sender;

            try
            {

                switch (e.PropertyName)
                {
                    case "X":
                    case "Y":
                        this.serviceProxy.MoveCreatureTo(creature.Id, creature.X, creature.Y);
                        break;

                    case "Orientation":
                        this.serviceProxy.RotateCreatureTo(creature.Id, creature.Orientation);
                        break;

                }

            }
            catch
            {

            }
      
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch ( e.Action )
            {
                case NotifyCollectionChangedAction.Add:
                    foreach ( Creature creature in e.NewItems )
                    {
                        try
                        {
                            this.serviceProxy.CreateCreature(creature);
                        }
                        catch
                        {

                        }
                        this.Subscribe(creature);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void IBoardCallback.OnCreatureMoved(Guid id, double x, double y)
        {
            if ( this.board.Creatures.TryGetValue( id, out var creature  ) )
            {
               creature.MoveTo(x, y);
            }
        }

        public void OnCreatureRotated(Guid id, double orientation)
        {
            if (this.board.Creatures.TryGetValue(id, out var creature))
            {
                creature.Orientation = orientation;
            }
        }

        void IBoardCallback.OnCreatureCreated(Creature creature)
        {
            if (!this.board.Creatures.Contains(creature.Id))
            {
                this.board.Creatures.Add(creature);
                this.Subscribe(creature);

            }
        }

        public void Close()
        {
            ((IClientChannel) this.serviceProxy).Open();
        }

       
    }
}
