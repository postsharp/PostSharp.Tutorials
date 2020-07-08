using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    internal class CreatureViewModelCollection : ViewModelKeyedCollection<Guid, Creature, CreatureViewModel>
    {
        private readonly BoardViewModel board;

        public CreatureViewModelCollection([Required] AdvisableCollection<Creature> model, [Required] BoardViewModel board) : base(model)
        {
            this.board = board;

            this.AddFromModel();
        }

        [Reader]
        protected override CreatureViewModel CreateViewModel(Creature modelItem)
         => new CreatureViewModel(modelItem, this.board);

        [Reader]
        protected override Guid GetKeyForItem(CreatureViewModel item) => item.Id;
       
    }


}

