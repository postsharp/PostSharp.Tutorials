using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Specialized;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    public abstract class ViewModelKeyedCollection<TKey, TModel, TViewModel>  : AdvisableKeyedCollection<TKey,TViewModel>
    {
        public AdvisableCollection<TModel> Model { get; }

        public ViewModelKeyedCollection( [Required] AdvisableCollection<TModel> model)
        {
            this.Model = model;

            model.CollectionChanged += this.OnModelCollectionChanged;

        }

        protected void AddFromModel()
        {
            foreach (var modelItem in this.Model)
            {
                this.Add(this.CreateViewModel(modelItem));
            }
        }

        protected abstract TViewModel CreateViewModel( TModel modelItem );

        private void OnModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch ( e.Action )
            {
                case NotifyCollectionChangedAction.Reset:
                    this.Clear();
                    break;

                case NotifyCollectionChangedAction.Add:
                    foreach (TModel modelItem in e.NewItems)
                    {
                        this.Add(this.CreateViewModel(modelItem));
                    }
                    break;

                default:
                    throw new NotImplementedException();

            }
        }
    }
}
