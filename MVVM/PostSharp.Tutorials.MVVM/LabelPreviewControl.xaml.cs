using System.Windows.Controls;

namespace PostSharp.Tutorials.MVVM
{

    public partial class LabelPreviewControl : UserControl
    {
        public LabelPreviewControl()
        {
            InitializeComponent();
        }


        public string Text { get; set; }

    }
}