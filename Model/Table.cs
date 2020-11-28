using System.Collections.Generic;
using System.Linq;

namespace betthelper.Model
{
    public class Table
    {
        private MatchRepository repository;
        private List<TablePositon> table = new List<TablePositon>();
        
        public Table(MatchRepository repository)
        {
            this.repository = repository;
        }

        public void CreateTable(int current_season)
        {
            var teams = repository.TeamNames(1, current_season);
            foreach (var team in teams) 
            {
                var pos = new TablePositon(team);

                var matches = new List<Model.Match>();
                matches.AddRange(repository.HomeMatches(team, 1, current_season));
                matches.AddRange(repository.AwayMatches(team, 1, current_season));

                foreach (var m in matches)
                {
                    var winner = m.WinnerTeamName;
                    if (winner == team) 
                        pos.Points += 3;
                    
                    if (winner == "")
                        pos.Points++;
                    
                    pos.OwnGoals += m.Team1.ShortName == team ? m.MatchResults[0].PointsTeam1 : m.MatchResults[0].PointsTeam2;
                    pos.AgainstGoals += m.Team1.ShortName == team ? m.MatchResults[0].PointsTeam2 : m.MatchResults[0].PointsTeam1;
                    pos.MatchCount++;
                }

                table.Add(pos);
            }
        }

        public void WriteReport(Markdown md) 
        {
            md.AddSubSection("Aktuelle Tabelle");
            md.AddEmpty();
            md.AddText("|#|Team|Punkte|Anzahl Spiele|Torverhältnis|Differenz|");
            md.AddText("|-|----|------|-------------|-------------|---------|");

            var positions = (from p in table orderby p.Points descending, p.Diff select p).ToList();
            for (int i=0; i<positions.Count; i++) 
            {
                var p = positions[i];
                md.AddText(
                    (i+1).ToString() + "|" + 
                    p.Team + "|" +
                    p.Points + "|" +
                    p.MatchCount + "|" +
                    p.OwnGoals + ":" + p.AgainstGoals + "|" +
                    p.Diff
                );
            }

            md.AddEmpty();
        }
    }
}