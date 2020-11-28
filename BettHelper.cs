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
        private Markdown md = new Markdown();
        private MatchRepository repository = new MatchRepository();
        
        private int current_season;

        public BettHelper() 
        {
            current_season = DateTime.Now.Year;
            if (DateTime.Now.Month < 9)
                current_season--;

            repository.LoadAll(current_season);
        }

        private void WriteReportForSingleMatch(Model.Match m) 
        {
            var agh = repository.GetWeightedAverageGoalsAsHomeTeam(m.Team1.ShortName);
            var aga = repository.GetWeightedAverageGoalsAsAwayTeam(m.Team2.ShortName);

            md.AddText("\\pagebreak");
            md.AddSubSection( 
                m.Team1.ShortName + " gegen " + 
                m.Team2.ShortName + " am " + 
                m.MatchDateTime.ToShortDateString() + " um " + m.MatchDateTime.ToShortTimeString());
            
            md.AddSubsubSection(m.Team1.ShortName);
            md.AddBulletpoint("Gewichtete, durchschnittliche Heimtore: " + Math.Round(agh,2));
            md.AddBulletpoint("Letzte Heimspiele:");
            md.AddEmpty();
            md.AddText("|Saison|Liga|Datum|Gegner|Ergebnis|");
            md.AddText("|------|----|-----|------|--------|");
            foreach (var ma in repository.HomeMatches(m.Team1.ShortName).Take(5))
            {
                md.AddText("|" + ma.Season + "|" + ma.League + "|" + 
                    ma.MatchDateTime.ToShortDateString() + "|" + 
                    ma.Team2.ShortName + "|" + ma.Result + "|");
            }
            md.AddEmpty();

            md.AddSubsubSection(m.Team2.ShortName);
            md.AddBulletpoint("Gewichtete, durchschnittliche Auswärtstore: " +  Math.Round(aga,2));
            md.AddBulletpoint("Letzte Auswärtsspiele:");
            md.AddEmpty();
            md.AddText("|Saison|Liga|Datum|Gegner|Ergebnis|");
            md.AddText("|------|----|-----|------|--------|");
            foreach (var ma in repository.AwayMatches(m.Team2.ShortName).Take(5))
            {
                md.AddText("|" + ma.Season + "|" + ma.League + "|" + 
                    ma.MatchDateTime.ToShortDateString() + "|" + 
                    ma.Team1.ShortName + "|" + ma.Result + "|");
            }
            md.AddEmpty();
            
            var pd = new PoissonDistribution(m.Team1.ShortName, agh, m.Team2.ShortName, aga);
            pd.CalculateProbablities();
            var probabilities = (from p in pd.Probabilities orderby p.ProbablityInPercent descending select p).Take(5).ToList();

            md.AddSubsubSection("Poison-Verteilung:");
            md.AddEmpty();
            md.AddText("|Wahrscheinlichkeit|Spielausgang|");
            md.AddText("|------------------|------------|");

            foreach (var p in probabilities)
                md.AddText(String.Format("| {0:0.00}% | {1} |", p.ProbablityInPercent, p.Result));
        }

        private void WriteReportForMatches()
        {
            foreach (var m in repository.UpcomingMatches().Take(8)) 
                WriteReportForSingleMatch(m);
        }

        public void CreateFullReport()
        {
            md.AddSection("BettHelper-Report vom " + DateTime.Now.ToShortDateString());
            md.AddEmpty();

            var table = new Table(repository);
            table.CreateTable(current_season);
            table.WriteReport(md);

            WriteReportForMatches();
        }

        public void WriteReportToFile(string filename) 
        {
            md.WriteToFile(filename);
        }
    }
}