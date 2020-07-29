using PostSharp.Patterns.Model;
using PostSharp.Tutorials.Threading.Model;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    [NotifyPropertyChanged]
    internal class BoardViewModel : IViewModel<Board>
    {
        public Board Model { get; } = new Board();

        public CreatureViewModelCollection Creatures { get; }

        public CreatureViewModel SelectedCreature { get; set; }

        public BoardViewModel()
        {
            this.Creatures = new CreatureViewModelCollection(this.Model.Creatures, this);
        }
    }
}

