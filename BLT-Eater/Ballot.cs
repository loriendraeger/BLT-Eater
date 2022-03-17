using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BLT_Eater
{
    public class Ballot
    {
        public Ballot(int weight, ICollection<Option> selections)
        {
            Weight = weight;
            Selections = ImmutableList<Option>.Empty;
            Selections = Selections.AddRange(selections);
        }

        public int Weight { get; private set; }
        public ImmutableList<Option> Selections { get; private set; }

        public override string ToString()
        {
            return string.Format("Weight: {0} Selections: {1}", Weight, string.Join(", ", Selections.Select(s => s.Name)));
        }

        public string ToTSVString()
        {
            return Weight + "\t" + string.Join("\t", Selections.Select(s => s.Name));
        }
    }
   
}
