using System;
using System.Collections.Generic;

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

            Console.WriteLine();
            Console.WriteLine("Ballots for Spreadsheet:");
            string rankList = "";
            for (int i = 1; i <= election.OptionCount; i++)
            {
                rankList += "\t" + i;
            }
            Console.WriteLine("Number\tWeight"+rankList);
            foreach (Ballot b in election.Ballots)
            {
                Console.WriteLine(b.ToTSVString());
            }

            Console.WriteLine();
            Console.WriteLine("Ranks By Option:");
            Console.WriteLine("Option" + rankList + "\tUnranked");
            List<OptionStat> stats = election.GetStatsByOption();
            foreach (OptionStat os in stats)
            {
                Console.WriteLine(os.ToTSVString(election.OptionCount));
            }
        }
    }
}
