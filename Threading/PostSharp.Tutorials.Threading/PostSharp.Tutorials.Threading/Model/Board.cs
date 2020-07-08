using PostSharp.Patterns.Model;

namespace PostSharp.Tutorials.Threading
{
    [NotifyPropertyChanged]
    internal class Board
    {
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

