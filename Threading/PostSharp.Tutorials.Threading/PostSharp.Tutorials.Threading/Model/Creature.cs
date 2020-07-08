using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;

namespace PostSharp.Tutorials.Threading
{
    [ReaderWriterSynchronized]
    [NotifyPropertyChanged( PreventFalsePositives = true ) ]
    public class Creature
    {
        // The setter is needed for serialization.
        public Guid Id { get; set; } = Guid.NewGuid();

        [Range(-10, 10)]
        public double X { get; set; }

        [Range(-10, 10)]
        public double Y { get; set; }

        public double Orientation { get; set; }

        
        public string Color { get; set; }


        [Writer]
        public bool TryMove(double step)
        {
            var radians = 2 * Math.PI * this.Orientation / 360.0;

            return this.TryMoveTo( this.X + Math.Cos(radians) * step, this.Y + Math.Sin(radians) * step);
        }


        [Writer]
        public void Rotate(double degrees)
        {
            this.Orientation += degrees;
        }

        [Writer]
        public void MoveTo([Range(-10, 10)] double x, [Range(-10, 10)] double y)
        {
            this.X = x;
            this.Y = y;
        }

        [Writer]
        public bool TryMoveTo(double x, double y)
        {
            if ( x < -10 || x > 10 || y < -10 || y > 10 )
            {
                return false;
            }
            else
            {
                this.MoveTo(x, y);
                return true;
            }
        }

    }
}

