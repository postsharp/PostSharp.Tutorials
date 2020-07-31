using System;
using System.Collections.Generic;
using System.Threading;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading.Model
{
    [ReaderWriterSynchronized]
    [NotifyPropertyChanged]
    internal class Board : IDisposable
    {
        private Timer timer;
        private readonly Random random = new Random();
        

        [Child(ItemsRelationship = RelationshipKind.Child)]
        public CreatureCollection Creatures { get; } = new CreatureCollection();

        public Board()
        {
            // Initialize the board with a few creatures.
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());

            this.IsMaster = true;
            
        }

        [Writer]
        private void OnTimer(object state)
        {
           
            var dyingCreatures = new List<Creature>();

            foreach (var creature in this.Creatures)
            {
                // Creatures have some spontaneous movements.
                creature.Rotate((this.random.NextDouble() - 0.5) * 5);
                creature.TryMove(this.random.NextDouble() * 0.1);

                // Creatures may die.
                if (this.random.NextDouble() < 0.005)
                {
                    // Schedule for deletion. We cannot do it in an enumerator.
                    dyingCreatures.Add(creature);
                }
            }

            foreach (var dyingCreature in dyingCreatures)
            {
                this.Creatures.Remove(dyingCreature);
            }

            // Creatures get born.
            if (this.random.NextDouble() < 0.005 * ( 10 - this.Creatures.Count ) )
            {
                this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            }

        }

        [Writer]
        public void Dispose()
        {
            this.timer?.Dispose();
            this.timer = null;
        }

        public bool IsMaster
        {

            get => this.timer != null;

            set
            {
                
                if (this.IsMaster != value)
                {
                    if (value)
                    {
                        // Start a timer.
                        this.timer = new Timer(this.OnTimer, null, 100, 100);
                    }
                    else
                    {
                        this.timer.Dispose();
                        this.timer = null;
                    }
                
                }
            }
        }
    }


}

