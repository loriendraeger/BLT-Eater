using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BLT_Eater
{
    public class Ballot
    {
        public Ballot(int number, int weight, ICollection<Option> selections)
        {
            Number = number;
            Weight = weight;
            Selections = ImmutableList<Option>.Empty;
            Selections = Selections.AddRange(selections);
        }

        public int Number { get; private set; }
        public int Weight { get; private set; }
        public ImmutableList<Option> Selections { get; private set; }

        public override string ToString()
        {
            return string.Format("Number: {0} Weight: {1} Selections: {2}", Number, Weight, string.Join(", ", Selections.Select(s => s.Name)));
        }

        public string ToTSVString()
        {
            return Number + "\t" + Weight + "\t" + string.Join("\t", Selections.Select(s => s.Name));
        }

        public int? GetRankForOption(Option option)
        {
            for(int i = 0; i < Selections.Count; i++)
            {
                if (option == Selections[i])
                {
                    return i + 1; //0th in the array is first choice
                }
            }
            //unranked
            return null;
        }
    }   
}
