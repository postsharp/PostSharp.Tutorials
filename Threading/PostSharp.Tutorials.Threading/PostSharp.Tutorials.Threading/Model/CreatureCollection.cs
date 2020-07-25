using System;
using PostSharp.Patterns.Collections;

namespace PostSharp.Tutorials.Threading.Model
{
    public class CreatureCollection  : AdvisableKeyedCollection<Guid,Creature>
    {
        protected override Guid GetKeyForItem(Creature item) => item.Id;
    }


}

