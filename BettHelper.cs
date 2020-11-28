using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using betthelper.Model;

namespace betthelper
{
    public class BettHelper
    {
        private List<string> file = new List<string>();

        private void Write(string s) 
        {
            file.Add(s);
        }

        private void WriteAll() 
        {
            File.WriteAllLines("results.md", file);
        }

        public void Calculate() 
        {
            Write("# BettHelper-Report vom " + DateTime.Now.ToShortDateString());
            Write("Bewertung der nächsten acht Spiele.");
            Write("");

            var mr = new MatchRepository();
            mr.LoadAll();
            
            foreach (var m in mr.UpcomingMatches().Take(8)) 
            {
                var match_count = 5;
                var agh = mr.GetAverageGoalsAsHomeTeam(m.Team1.ShortName, match_count);
                var aga = mr.GetAverageGoalsAsHomeTeam(m.Team2.ShortName, match_count);

                Write("## " + 
                    m.Team1.ShortName + " gegen " + 
                    m.Team2.ShortName + " am " + 
                    m.MatchDateTime.ToShortDateString() + " um " + m.MatchDateTime.ToShortTimeString());

                Write("- Heimtore von " + m.Team1.ShortName + " in den letzten " + match_count + " Heimspielen: " + Math.Round(agh,2));
                Write("- Auswärtstore von " + m.Team2.ShortName + " in den letzten " + match_count + " Auswärtsspielen: " + Math.Round(aga,2));
                Write("");
                
                var pd = new PoissonDistribution(m.Team1.ShortName, agh, m.Team2.ShortName, aga);
                pd.CalculateProbablities();
                var probabilities = (from p in pd.Probabilities orderby p.ProbablityInPercent descending select p).Take(3).ToList();

                Write("|Wahrscheinlichkeit|Spielausgang|");
                Write("|------------------|------------|");

                foreach (var p in probabilities)
                    Write(String.Format("| {0:0.00}% | {1} |", p.ProbablityInPercent, p.Result));

                Write("");
            }

            WriteAll();
        }
    }
}