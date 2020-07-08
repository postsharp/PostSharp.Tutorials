using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace PostSharp.Tutorials.Threading.Communication
{

    internal static class BoardService 
    {
        private static Board board;

        private static readonly ConcurrentDictionary<Guid, WeakReference<Session>> sessions = new ConcurrentDictionary<Guid, WeakReference<Session>>();

        public static IConnection StartService(Board board)
        {

            BoardService.board = board;
            board.Creatures.CollectionChanged += OnCreaturesCollectionChanged;
            Subscribe(board.Creatures);

            var group = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
            var service = group.Services.Services[0];
            var baseAddress = service.Endpoints[0].Address.AbsoluteUri.Replace(service.Endpoints[0].Address.AbsolutePath, String.Empty);


            var host = new ServiceHost(typeof(Session), new[] { new Uri(baseAddress) });

            host.AddServiceEndpoint(typeof(IBoardService),
                                    new NetNamedPipeBinding(),
                                    service.Endpoints[0].Address.AbsolutePath);
            host.Open();


            return new Connection(host);
        }


        private static void Subscribe(IEnumerable<Creature> creatures)
        {
            foreach (var creature in creatures)
            {
                Subscribe(creature);
            }
        }

        private static void Subscribe(Creature creature)
        {
            Post.Cast<Creature, INotifyPropertyChanged>(creature).PropertyChanged += OnCreaturePropertyChanged;
        }

        private static void OnCreaturesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Subscribe(e.NewItems.Cast<Creature>());
                    foreach (Creature creature in e.NewItems)
                    {
                        ForEachSession(session => session.Callback.OnCreatureCreated(creature));
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }


        private static void OnCreaturePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var creature = (Creature)sender;

            try
            {

                switch (e.PropertyName)
                {
                    case "X":
                    case "Y":
                        ForEachSession(session => session.Callback.OnCreatureMoved(creature.Id, creature.X, creature.Y));
                        break;

                    case "Orientation":
                        ForEachSession(session => session.Callback.OnCreatureRotated(creature.Id, creature.Orientation));
                        break;

                }

            }
            catch
            {

            }
        }




        private static void ForEachSession(Action<Session> action)
        {
            foreach (var entry in sessions)
            {
                var remove = false;
                if (entry.Value.TryGetTarget(out var session))
                {
                    try
                    {
                        action(session);
                    }
                    catch (Exception)
                    {
                        remove = true;
                    }
                }
                else
                {
                    remove = true;
                }

                if (remove)
                {
                    sessions.TryRemove(entry.Key, out _);
                }
            }
        }



        [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
        class Session : IBoardService
        {

            public IBoardCallback Callback { get; }

            private readonly Guid guid = Guid.NewGuid();

            public Session()
            {
                this.Callback = OperationContext.Current.GetCallbackChannel<IBoardCallback>();
                

                sessions.TryAdd(this.guid, new WeakReference<Session>(this));
            }



            public List<Creature> GetCreatures()
            {
                return board.Creatures.ToList();
            }

            public void MoveCreatureTo(Guid id, double x, double y)
            {
                var creature = board.Creatures[id];
                creature.MoveTo(x, y);
            }

            public void RotateCreatureTo(Guid id, double angle)
            {
                var creature = board.Creatures[id];
                creature.Orientation = angle;
            }

            public void CreateCreature(Creature creature)
            {
                if (!board.Creatures.Contains(creature.Id))
                {
                    board.Creatures.Add(creature);
                    Subscribe(creature);
                }
            }

          
        }

        class Connection : IConnection
        {
            private readonly ServiceHost host;

            public Connection(ServiceHost host)
            {
                this.host = host;
            }


            public void Close()
            {
                this.host.Close();
            }

#pragma warning disable CS0067
            public event EventHandler Closed;

        }

    }
}
