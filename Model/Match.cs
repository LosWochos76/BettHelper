using System;
using System.Collections.Generic;

namespace betthelper.Model
{
    public class Match
    {
        public int League { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public List<MatchResult> MatchResults { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int MatchID { get; set; }
        public bool MatchIsFinished { get; set; }
        public Group Group { get; set; }
    }
}