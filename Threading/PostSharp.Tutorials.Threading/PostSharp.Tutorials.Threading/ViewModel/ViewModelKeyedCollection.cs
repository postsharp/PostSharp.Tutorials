using System;
using System.Collections.Specialized;
using System.Windows.Threading;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Contracts;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    public abstract class
        ViewModelKeyedCollection<TKey, TModel, TViewModel> : AdvisableKeyedCollection<TKey, TViewModel>,
            IViewModel<AdvisableCollection<TModel>>
    {
        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        
        public AdvisableCollection<TModel> Model
        {
            get;
        }

        protected ViewModelKeyedCollection([Required] AdvisableCollection<TModel> model)
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

        protected abstract TKey GetKeyForModelItem(TModel modelItem);

        protected abstract TViewModel CreateViewModel(TModel modelItem);

        private void OnModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            void RaiseEvents()
            {
                switch (e.Action)
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

                    case NotifyCollectionChangedAction.Remove:
                        foreach (TModel modelItem in e.OldItems)
                        {
                            if (this.TryGetValue(this.GetKeyForModelItem(modelItem), out var item))
                            {
                                this.Remove(item);
                            }
                        }

                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            if (this.dispatcher.CheckAccess())
            {
                RaiseEvents();
            }
            else
            {
                this.dispatcher.BeginInvoke(new Action(RaiseEvents));
            }
        }
    }
}