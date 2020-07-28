using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;
using PostSharp.Tutorials.Threading.Model;

namespace PostSharp.Tutorials.Threading.Communication
{
  

    [Immutable]
    internal class BoardService : ServiceHost, IConnection
    {
        public Board Board { get; }
        
        // Using a ConcurrentDictionary within an Immutable class is the best choice here. An alternative design would
        // be to have a [ReaderWriterSynchronized] model on BoardService, but this would result in higher complexity
        // and higher thread contention.
        [Reference]
        private readonly ConcurrentDictionary<Guid, BoardServiceSession> sessions = new ConcurrentDictionary<Guid, BoardServiceSession>();


        private BoardService(Board board, string baseAddress) : base(typeof(BoardServiceSession), new Uri(baseAddress))
        {
            this.Board = board;

            board.Creatures.CollectionChanged += this.OnCreaturesCollectionChanged;
            this.Subscribe(board.Creatures);
        }

        internal void AddSession(BoardServiceSession session)
        {
            this.sessions.TryAdd(session.Id, session);
        }

        internal void RemoveSession(Guid sessionId)
        {
            this.sessions.TryRemove(sessionId, out _ );
        }

        public static BoardService StartService(Board board)
        {
            var group = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            var service = group.Services.Services[0];
            var baseAddress = service.Endpoints[0].Address.AbsoluteUri.Replace(service.Endpoints[0].Address.AbsolutePath, String.Empty);


            var serviceHost = new BoardService(board, baseAddress);

            serviceHost.AddServiceEndpoint(typeof(IBoardService),
                                    new NetNamedPipeBinding(),
                                    service.Endpoints[0].Address.AbsolutePath);
            serviceHost.Open();

            return serviceHost;

        }


        private void Subscribe(IEnumerable<Creature> creatures)
        {
            foreach (var creature in creatures)
            {
                this.Subscribe(creature);
            }
        }

        private void Subscribe(Creature creature)
        {
            Post.Cast<Creature, INotifyPropertyChanged>(creature).PropertyChanged += this.OnCreaturePropertyChanged;
        }

        private void Unsubscribe(IEnumerable<Creature> creatures)
        {
            foreach (var creature in creatures)
            {
                this.Unsubscribe(creature);
            }
        }

        private void Unsubscribe(Creature creature)
        {
            Post.Cast<Creature, INotifyPropertyChanged>(creature).PropertyChanged += this.OnCreaturePropertyChanged;
        }

        [Background]
        private void OnCreaturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.Subscribe(e.NewItems.Cast<Creature>());
                    foreach (Creature creature in e.NewItems)
                    {
                        this.InvokeCallback(callback => callback.OnCreatureCreated(creature));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    this.Unsubscribe(e.OldItems.Cast<Creature>());
                    foreach (Creature creature in e.OldItems)
                    {
                        this.InvokeCallback(callback => callback.OnCreatureDeleted(creature.Id));
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        [Background]
        private void OnCreaturePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var creature = (Creature)sender;

            switch (e.PropertyName)
            {
                case "Position":
                    this.InvokeCallback(callback =>
                        {
                            var position = creature.Position;

                            callback.OnCreatureMoved(creature.Id, position.X, position.Y);
                        });
                    break;

                case "Orientation":
                    this.InvokeCallback(callback => callback.OnCreatureRotated(creature.Id, creature.Orientation));
                    break;

            }

        }


        private void InvokeCallback(Action<IBoardCallback> action)
        {
            foreach (var entry in this.sessions)
            {
                // We can invoke each client in parallel.
                Task.Run(() =>
                {
                    try
                    {
                        action(entry.Value.Callback);
                    }
                    catch (Exception)
                    {
                        this.RemoveSession(entry.Key);
                    }
                });


            }
        }

        public new void Close()
        {
            base.Close();
            this.Unsubscribe(this.Board.Creatures);
            this.Board.Creatures.CollectionChanged -= this.OnCreaturesCollectionChanged;
        }

    }

}
