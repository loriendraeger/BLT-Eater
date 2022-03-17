using System;

namespace BLT_Eater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: provide 1 argument with path to BLT file");
            }
            string[] fileLines = System.IO.File.ReadAllLines(args[0]);
            Election election = new Election(fileLines);

            Console.WriteLine("Ballots:");
            foreach (Ballot b in election.Ballots)
            {
                Console.WriteLine(b);
            }
        }
    }
}
