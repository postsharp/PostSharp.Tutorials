using PostSharp.Patterns.Model;

namespace PostSharp.Samples.Xaml
{
    // We're adding two aspects to the base class of all Model classes and the aspects
    // will be automatically added by all children classes.

    [NotifyPropertyChanged]
    public abstract class ModelBase
    {
    }
}