using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Collections;
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
        [Reference]
        private readonly Board board;

        [Reference]
        private IBoardService serviceProxy;

        public event EventHandler Closed;

        private BoardServiceClient(Board board)
        {
            this.board = board;

            this.Connect();

            this.board.Creatures.CollectionChanged += OnCollectionChanged;

            this.Subscribe(this.board.Creatures);
        }

        void Connect()
        {
            ServiceModelSectionGroup group = ServiceModelSectionGroup.GetSectionGroup(
                  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

            //create duplex channel factory
            var serviceFactory = new DuplexChannelFactory<IBoardService>(
                                          this, group.Client.Endpoints[0].Name);
            //create a communication channel and register for its events
            this.serviceProxy = serviceFactory.CreateChannel();

            IClientChannel channel = (IClientChannel)serviceProxy;
            channel.Open();
            channel.Closed += OnChannelClosed;
            channel.Faulted += OnChannelClosed;


            this.board.Creatures.Clear();
            foreach (var creature in serviceProxy.GetCreatures())
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
            foreach ( Creature creature in creatures )
            {
                Subscribe(creature);
            }
        }

        private void Subscribe(Creature creature)
        {
            Post.Cast<Creature, INotifyPropertyChanged>(creature).PropertyChanged += OnCreaturePropertyChanged;
        }

        private void OnCreaturePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Creature creature = (Creature)sender;

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
                        Subscribe(creature);
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
                Subscribe(creature);

            }
        }

        public void Close()
        {
            ((IClientChannel)serviceProxy).Open();
        }

       
    }
}
