using System;
using System.Collections.Generic;

namespace betthelper.Model
{
    public class Match
    {
        public int League { get; set; }
        public int Season { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public List<MatchResult> MatchResults { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int MatchID { get; set; }
        public bool MatchIsFinished { get; set; }
        public Group Group { get; set; }

        public string WinnerTeamName
        {
            get
            {
                if (MatchResults[0].PointsTeam1 > MatchResults[0].PointsTeam2) 
                    return Team1.ShortName;

                if (MatchResults[0].PointsTeam2 > MatchResults[0].PointsTeam1) 
                    return Team2.ShortName;
                
                return "";
            }
        }

        public string Result
        {
            get
            {
                return MatchResults[0].PointsTeam1 + ":" + MatchResults[0].PointsTeam2;
            }
        }
    }
}