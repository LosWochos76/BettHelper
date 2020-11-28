namespace betthelper.Model
{
    public class TablePositon
    {
        public string Team { get; set; }
        public int Points { get; set; }
        public int OwnGoals { get; set; }
        public int AgainstGoals { get; set; }
        public int MatchCount { get; set; }

        public int Diff 
        {
            get
            {
                return OwnGoals - AgainstGoals;
            }
        }

        public TablePositon(string team)
        {
            Team = team;
        }
    }
}