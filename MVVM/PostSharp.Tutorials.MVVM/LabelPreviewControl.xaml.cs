using System.Windows.Controls;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Xaml;

namespace PostSharp.Tutorials.MVVM
{
  
  public partial class LabelPreviewControl : UserControl
  {
    public LabelPreviewControl()
    {
      InitializeComponent();
    }


    [DependencyProperty]
    [Required]
    public string Text { get; set; }

  }
}