using PostSharp.Patterns.Threading;
using System;
using System.Linq;
using System.Windows.Media;

namespace PostSharp.Tutorials.Threading
{
    [Synchronized]
    internal class RandomGenerator
    {

        public static readonly RandomGenerator Instance = new RandomGenerator();

        private readonly Random random = new Random();

        private readonly string[] colors = typeof(Brushes).GetProperties().Select(p => p.Name).ToArray();

        public string GetRandomColor()
        {
            return this.colors[this.random.Next(this.colors.Length)];
        }

        public Creature CreateCreature()
        {
            return new Creature
            {
                X = this.random.NextDouble() * 20 - 10,
                Y = this.random.NextDouble() * 20 - 10,
                Orientation = this.random.Next(359),
                Color = this.GetRandomColor()
            };
                
        }
    }


}

