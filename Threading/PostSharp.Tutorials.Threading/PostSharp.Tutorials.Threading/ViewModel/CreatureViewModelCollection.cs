using System;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Tutorials.Threading.Model;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    internal class CreatureViewModelCollection : ViewModelKeyedCollection<Guid, Creature, CreatureViewModel>
    {
        private readonly BoardViewModel board;

        public CreatureViewModelCollection([Required] AdvisableCollection<Creature> model, [Required] BoardViewModel board) : base(model)
        {
            this.board = board;

            this.AddFromModel();
        }

        protected override CreatureViewModel CreateViewModel(Creature modelItem)
         => new CreatureViewModel(modelItem, this.board);

        protected override Guid GetKeyForItem(CreatureViewModel item) => item.Id;

        protected override Guid GetKeyForModelItem(Creature modelItem) => modelItem.Id;
        
    }

}

