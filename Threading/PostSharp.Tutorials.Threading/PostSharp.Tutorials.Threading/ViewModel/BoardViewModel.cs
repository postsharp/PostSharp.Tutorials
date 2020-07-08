using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    [NotifyPropertyChanged]
    internal class BoardViewModel
    {
        [Child]
        public Board Board { get; } = new Board();

        [Child( ItemsRelationship = RelationshipKind.Child )]
        public CreatureViewModelCollection Creatures { get; }

        public CreatureViewModel SelectedCreature { get; set; }

        public BoardViewModel()
        {
            this.Creatures = new CreatureViewModelCollection(this.Board.Creatures, this);
        }

    }


}

