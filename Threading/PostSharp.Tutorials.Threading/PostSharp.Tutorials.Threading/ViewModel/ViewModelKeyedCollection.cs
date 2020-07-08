using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    public abstract class ViewModelKeyedCollection<TKey, TModel, TViewModel>  : AdvisableKeyedCollection<TKey,TViewModel>
    {
        [Reference]
        public AdvisableCollection<TModel> Model { get; }

        public ViewModelKeyedCollection( [Required] AdvisableCollection<TModel> model)
        {
            Model = model;

            model.CollectionChanged += OnModelCollectionChanged;

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
