using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using PostSharp.Tutorials.Threading.Model;

namespace PostSharp.Tutorials.Threading.Communication
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class BoardServiceSession : IBoardService, IDisposable
    {
        private readonly BoardService service;

        public IBoardCallback Callback { get; }

        public Guid Id { get; } = Guid.NewGuid();

        public BoardServiceSession()
        {
            this.service = (BoardService) OperationContext.Current.Host;

            this.Callback = OperationContext.Current.GetCallbackChannel<IBoardCallback>();

            this.service.AddSession(this);
        }



        public List<Creature> GetCreatures()
        {
            return this.service.Board.Creatures.ToList();
        }

        public void MoveCreatureTo(Guid id, double x, double y)
        {
            if (this.service.Board.Creatures.TryGetValue(id, out var creature))
            {
                creature.MoveTo(x, y);
            }
        }

        public void RotateCreatureTo(Guid id, double angle)
        {
            if (this.service.Board.Creatures.TryGetValue(id, out var creature))
            {
                creature.Orientation = angle;
            }
        }

        public void CreateCreature(Creature creature)
        {
            if (!this.service.Board.Creatures.Contains(creature.Id))
            {
                this.service.Board.Creatures.Add(creature);
            }
        }

        public void DeleteCreature(Guid id)
        {
            if (this.service.Board.Creatures.TryGetValue(id, out var creature))
            {
                this.service.Board.Creatures.Remove(creature);
            }
        }

        public void Dispose()
        {
            this.service.RemoveSession(this.Id);
        }

      
    }

}
