namespace PostSharp.Tutorials.Threading.Model
{
    public readonly struct Position
    {
        public readonly double X;
        public readonly double Y;

        public Position(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

