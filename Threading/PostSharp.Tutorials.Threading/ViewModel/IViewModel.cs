using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    internal interface IViewModel<out TModel>
    {
        TModel Model { get; }
    }
}

