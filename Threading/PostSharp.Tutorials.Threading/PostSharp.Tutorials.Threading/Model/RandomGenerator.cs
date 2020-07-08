using System;
using System.Linq;
using System.Windows.Media;

namespace PostSharp.Tutorials.Threading
{
    internal static class RandomGenerator
    {
        static readonly Random random = new Random();
        static readonly string[] colors = typeof(Brushes).GetProperties().Select(p => p.Name).ToArray();

        public static string GetRandomColor()
        {
            return colors[random.Next(colors.Length)];
        }

        public static Creature CreateCreature()
        {
            return new Creature
            {
                X = random.NextDouble() * 20 - 10,
                Y = random.NextDouble() * 20 - 10,
                Orientation = random.Next(359),
                Color = GetRandomColor()
            };
                
        }
    }


}

