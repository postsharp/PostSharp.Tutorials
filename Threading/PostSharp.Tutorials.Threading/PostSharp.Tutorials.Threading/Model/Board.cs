using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    [NotifyPropertyChanged]
    internal class Board
    {
        [Child(ItemsRelationship = RelationshipKind.Child)]
        public CreatureCollection Creatures { get; } = new CreatureCollection();

        public Board()
        {
            // Initialize the board with a few creatures.
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
        }
    }


}

