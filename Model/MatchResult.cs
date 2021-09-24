namespace betthelper.Model
{
    public class MatchResult
    {
        public int ResultTypeID { get; set; }
        public int PointsTeam1 { get; set; }
        public int PointsTeam2 { get; set; }

        public bool IsHomeWin
        {
            get 
            {
                return PointsTeam1 > PointsTeam2;
            }
        }

        public bool IsAwayWin
        {
            get
            {
                return PointsTeam2 > PointsTeam1;
            }
        } 

        public bool IsDraw
        {
            get
            {
                return PointsTeam1 == PointsTeam2;
            }
        } 
    }
}