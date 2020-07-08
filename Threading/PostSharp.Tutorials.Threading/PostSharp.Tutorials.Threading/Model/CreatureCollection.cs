using PostSharp.Patterns.Collections;
using System;

namespace PostSharp.Tutorials.Threading
{
    public class CreatureCollection  : AdvisableKeyedCollection<Guid,Creature>
    {
        protected override Guid GetKeyForItem(Creature item) => item.Id;
    }


}

