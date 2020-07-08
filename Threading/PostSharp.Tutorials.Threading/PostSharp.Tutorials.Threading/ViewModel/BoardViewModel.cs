using PostSharp.Patterns.Model;

namespace PostSharp.Tutorials.Threading
{
    [NotifyPropertyChanged]
    internal class BoardViewModel
    {
        public Board Board { get; } = new Board();

        public CreatureViewModelCollection Creatures { get; }

        public CreatureViewModel SelectedCreature { get; set; }

        public BoardViewModel()
        {
            this.Creatures = new CreatureViewModelCollection(this.Board.Creatures, this);
        }
    }
}

