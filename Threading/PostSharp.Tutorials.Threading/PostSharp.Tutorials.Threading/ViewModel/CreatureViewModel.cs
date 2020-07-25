using System;
using System.Windows.Media;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Tutorials.Threading.Model;

namespace PostSharp.Tutorials.Threading.ViewModel
{
    [NotifyPropertyChanged]
    internal class CreatureViewModel : IViewModel<Creature>
    {
        private static readonly BrushConverter brushConverter = new BrushConverter();

        private readonly BoardViewModel board;

        public Creature Model { get; }

        public CreatureViewModel([Required] Creature creature, [Required] BoardViewModel board)
        {
            this.Model = creature;
            this.board = board;
        }

        public Guid Id => this.Model.Id;

        public double X => this.Model.X * 20 + 210;

        public double Y => this.Model.Y * 20 + 210;

        public bool IsSelected => this.board.SelectedCreature == this;

        public double Orientation => this.Model.Orientation;

        [SafeForDependencyAnalysis]
        public Brush FillColor => (SolidColorBrush)brushConverter.ConvertFromString(this.Model.Color);
      
        public Brush StrokeColor => this.IsSelected ? Brushes.MediumAquamarine : Brushes.Transparent;

        public double Opacity => this.IsSelected ? 1 : 0.6;
        
    }
}

