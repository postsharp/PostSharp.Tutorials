using System;
using System.Windows.Media;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace PostSharp.Tutorials.Threading
{
    [NotifyPropertyChanged]
    internal class CreatureViewModel
    {
        private static readonly BrushConverter brushConverter = new BrushConverter();

        private readonly BoardViewModel board;

        public Creature Creature { get; }

        public CreatureViewModel([Required] Creature creature, [Required] BoardViewModel board)
        {
            this.Creature = creature;
            this.board = board;
        }

        public Guid Id => this.Creature.Id;

        public double X => this.Creature.X * 20 + 210;

        public double Y => this.Creature.Y * 20 + 210;

        public bool IsSelected => this.board.SelectedCreature == this;

        public double Orientation => this.Creature.Orientation;

        [SafeForDependencyAnalysis]
        public Brush FillColor => (SolidColorBrush)brushConverter.ConvertFromString(this.Creature.Color);
      
        public Brush StrokeColor => this.IsSelected ? Brushes.MediumAquamarine : Brushes.Transparent;

        public double Opacity => this.IsSelected ? 1 : 0.6;
        
    }
}

