using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using PostSharp.Patterns.Collections;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace PostSharp.Tutorials.Threading
{
    [NotifyPropertyChanged]
    internal class Board
    {
        public CreatureCollection Creatures { get; } = new CreatureCollection();

        public Board()
        {
            // Initialize the board with a few creatures.
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
            this.Creatures.Add(RandomGenerator.Instance.CreateCreature());
        }
    }


}

