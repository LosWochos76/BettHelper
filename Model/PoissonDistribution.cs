using System;
using System.Collections.Generic;

namespace betthelper.Model
{
    public class PoissonDistribution
    {
        public string HomeTeam { get; private set; }
        public string AwayTeam { get; private set; }
        public double HomeTeamAverageGoals { get; private set; }
        public double AwayTeamAverageGoals { get; private set; }
        public List<ResultProbability> Probabilities { get; private set; }

        public PoissonDistribution(string homeTeam, double average_home_team_goals, string awayTeam, double average_away_team_goals)
        {
            Probabilities = new List<ResultProbability>();
            HomeTeam = homeTeam;
            HomeTeamAverageGoals = average_home_team_goals;
            AwayTeam = awayTeam;
            AwayTeamAverageGoals = average_away_team_goals;
        }

        public void CalculateProbablities()
        {
            double[] poisson_home = new double[6];
            for (int i = 0; i< 6; i++)
                poisson_home[i] = poisson(HomeTeamAverageGoals, i);

            double[] poisson_away = new double[6];
                for (int i = 0; i< 6; i++)
                    poisson_away[i] = poisson(AwayTeamAverageGoals, i);

            double[,] pd = new double[6, 6];
            for (int i = 0; i<6; i++)
            {
                for (int j = 0; j<6; j++)
                {
                    var prob = Math.Truncate(poisson_home[i] * poisson_away[j] * 10000) / 100;
                    var probability = new ResultProbability(i, j, prob);
                    Probabilities.Add(probability);
                }
            }
        }

        private static double poisson(double m, int k)
        {
            return Math.Exp(m * -1) * Math.Pow(m, k) / fak(k);
        }

        private static int fak(int k)
        {
            if (k == 0)
                return 1;
            else
                return k * fak(k - 1);
        }
    }
}
