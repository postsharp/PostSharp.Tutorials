using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Xaml;
using System.Windows.Controls;

namespace PostSharp.Samples.Xaml
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