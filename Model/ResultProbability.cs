using System;

namespace betthelper.Model
{
    public class ResultProbability
    {
        public int PointsTeam1 { get; set; }
        public int PointsTeam2 { get; set; }
        
        public double ProbablityInPercent { get; set; }

        public ResultProbability(int pt1, int pt2, double probability)
        {
            PointsTeam1 = pt1;
            PointsTeam2 = pt2;
            ProbablityInPercent = probability;
        }

        public string ResultString 
        { 
            get
            {
                return String.Format("{0}:{1}", PointsTeam1, PointsTeam2);
            } 
        }

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
