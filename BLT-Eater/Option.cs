using System;
namespace BLT_Eater
{
    public class Option
    {
        public Option(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is Option other && Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public string Name { get; private set; }
        public string Id { get; private set; }
    }
}
