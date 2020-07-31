using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    [ThreadAffine]
    internal interface IViewModel<out TModel>
    {
        TModel Model { get; }
    }
}

