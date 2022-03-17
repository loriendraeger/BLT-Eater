using System;
using System.Collections.Generic;

namespace BLT_Eater
{
    public class Election
    {
        public Election(string[] fileLines)
        {
            if (fileLines == null || fileLines.Length < 1)
            {
                throw new Exception("File empty");
            }

            int index = 0;
            bool done = false;

            List<String> ballotLines = new List<string>();

            //first line has number of options and winners
            string[] firstLine = fileLines[index].Split(" ");
            OptionCount = int.Parse(firstLine[0]);
            WinnerCount = int.Parse(firstLine[1]);

            index++;

            //then we move into withdrawals (not handled)
            while (!done && index < fileLines.Length)
            {
                if (!fileLines[index].StartsWith("-"))
                {
                    done = true;
                    break;
                }
                index++;
            }

            //on to the ballots themselves - just save these off for now
            done = false;
            while (!done && index < fileLines.Length)
            {
                if (fileLines[index].StartsWith("0"))
                {
                    done = true;
                    break;
                }

                ballotLines.Add(fileLines[index]);

                index++;
            }

            //skip past the "0" line that separates ballots and options
            index++;
            //index should now be the very first option

            //see if we have any trailing empty lines here
            int lastLine = fileLines.Length - 1;

            for (int i = lastLine; i >= index; i--)
            {
                if (!string.IsNullOrWhiteSpace(fileLines[0]))
                {
                    lastLine = i;
                    break;
                }
            }

            //last line is the name of the vote
            ElectionName = fileLines[lastLine].Replace("\"", "");

            Options = new Dictionary<string, Option>();

            //everything and the lines before that are option names
            for (int i = index; i < lastLine; i++)
            {
                string id = (i - index + 1).ToString();
                string name = fileLines[i].Replace("\"", "");
                Option o = new Option(id, name);
                Options.Add(id, o);
            }

            Ballots = new List<Ballot>();

            for (int i = 0; i < ballotLines.Count; i++)
            {
                string ballot = ballotLines[i];
                Ballots.Add(ParseBallot(i + 1, ballot));
            }
        }

        public int OptionCount { get; private set; }
        public int WinnerCount { get; private set; }
        public string ElectionName { get; private set; }
        public Dictionary<string, Option> Options { get; private set; }
        public List<Ballot> Ballots { get; private set; }

        public List<OptionStat> GetStatsByOption()
        {
            List <OptionStat> result  = new List<OptionStat>();
            foreach (Option o in Options.Values)
            {
                result.Add(new OptionStat()
                {
                    Option = o,
                    Ranks = new Dictionary<int, int>(),
                    Unranked = 0
                });
            }
            foreach (Ballot b in Ballots)
            {
                foreach (OptionStat os in result)
                {
                    int? rank = b.GetRankForOption(os.Option);
                    if (rank == null)
                    {
                        os.Unranked++;
                    }
                    else
                    {
                        int r = (int)rank;
                        if(!os.Ranks.ContainsKey(r))
                        {
                            os.Ranks.Add(r, 0);
                        }
                        os.Ranks[r]++;
                    }
                }
            }
            return result;
        }

        private Ballot ParseBallot(int number, string ballot)
        {            
            if (string.IsNullOrWhiteSpace(ballot))
            {
                return null;
            }
            string[] ballotEntries = ballot.Split(" ");

            if (ballotEntries[ballotEntries.Length - 1] != "0")
            {
                throw new Exception("Missing 0 at end of ballot");
            }
            int weight = int.Parse(ballotEntries[0]);

            List<Option> ballotSelections = new List<Option>();

            //the entries excluding first and last are actual rankings
            for (int i = 1; i < ballotEntries.Length - 1; i++)
            {
                string selection = ballotEntries[i];
                ballotSelections.Add(Options[selection]);
            }

            return new Ballot(number, weight, ballotSelections);
        }
    }

    public class OptionStat
    {
        internal OptionStat()
        {

        }

        public Option Option { get; internal set; }
        public Dictionary<int, int> Ranks { get; internal set; }
        public int Unranked { get; internal set; }

        public string ToTSVString(int optionCount)
        {
            string rankings = "";
            for (int i = 1; i <= optionCount; i++)
            {
                int ballots = 0;
                int gotBallots;
                if (Ranks.TryGetValue(i, out gotBallots))
                {
                    ballots = gotBallots;
                }
                rankings += "\t" + ballots;
            }
            return Option.Name + rankings + "\t" + Unranked;
        }
    }
}
