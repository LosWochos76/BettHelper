using System;
using System.Linq;
using betthelper.Model;

namespace betthelper
{
    public class SeasonSimulator
    {
        private int saeson;
        private MatchRepository repository;

        public SeasonSimulator(int saeson)
        {
            this.saeson = saeson;
            this.repository = new MatchRepository();
            repository.LoadAll(saeson);
        }

        public void Simulate()
        {
            for (int i = 7; i <= 14; i++)
            {
                int sum_points = Simulate(repository, i, 1.5, 0.3);
                Console.WriteLine(i + " -> " + sum_points);
            }
        }

        public int Simulate(MatchRepository repository, int games_to_analyze, double newest_weight, double leage2_malus)
        {
            var sum_points = 0;

            foreach (var m in repository.Matches)
            {
                if (m.Season == saeson && m.League == 1)
                {
                    var hm = repository.HomeMatches(m.Team1.ShortName).Where(match => match.MatchDateTime < m.MatchDateTime).Take(games_to_analyze).ToList();
                    var agh = repository.GetWeightedAverageGoalsAsHomeTeam(hm, newest_weight, leage2_malus);
                    var am = repository.AwayMatches(m.Team2.ShortName).Where(match => match.MatchDateTime < m.MatchDateTime).Take(games_to_analyze).ToList();
                    var aga = repository.GetWeightedAverageGoalsAsAwayTeam(am, newest_weight, leage2_malus);

                    var pd = new PoissonDistribution(m.Team1.ShortName, agh, m.Team2.ShortName, aga);
                    pd.CalculateProbablities();
                    var probability = (from p in pd.Probabilities orderby p.ProbablityInPercent descending select p).First();
                    sum_points += GetKicktippPoints(m, probability);
                }
            }

            return sum_points;
        }

        public int GetKicktippPoints(Match m, ResultProbability p)
        {
            if (m.Result.IsHomeWin && p.IsHomeWin)
            {
                if (m.Result.PointsTeam1 == p.PointsTeam1 && m.Result.PointsTeam2 == p.PointsTeam2)
                    return 4;

                var diff1 = m.Result.PointsTeam1 - m.Result.PointsTeam2;
                var diff2 = p.PointsTeam1 - p.PointsTeam2;
                if (diff1 == diff2)
                    return 3;

                return 2;
            }

            if (m.Result.IsAwayWin && p.IsAwayWin)
            {
                if (m.Result.PointsTeam1 == p.PointsTeam1 && m.Result.PointsTeam2 == p.PointsTeam2)
                    return 5;

                var diff1 = m.Result.PointsTeam1 - m.Result.PointsTeam2;
                var diff2 = p.PointsTeam1 - p.PointsTeam2;
                if (diff1 == diff2)
                    return 4;

                return 3;
            }

            if (m.Result.IsDraw && p.IsDraw)
            {
                if (m.Result.PointsTeam1 == p.PointsTeam1 && m.Result.PointsTeam2 == p.PointsTeam2)
                    return 5;

                return 4;
            }

            return 0;
        }
    }
}