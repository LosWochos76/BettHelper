using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace betthelper.Model
{
    public class MatchRepository
    {
        public List<Match> Matches { get; private set; }

        public MatchRepository() 
        {
            Matches = new List<Match>();
        }

        private async Task<List<Match>> LoadSingleYear(int league, int year) 
        {
            Console.WriteLine("Loading matches for " + year + " in league " + league + "...");

            var url = String.Format("https://www.openligadb.de/api/getmatchdata/bl{0}/{1}", league, year);
            var client = new HttpClient();
            var result = await client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            var matches = JArray.Parse(content).ToObject<List<Match>>();

            foreach (var m in matches)
            {
                m.League = league;
                m.Season = year;
            }
            
            return matches;
        }

        public void LoadAll(int current_season) 
        {
            Matches.Clear();
            Matches.AddRange(LoadSingleYear(1, current_season).GetAwaiter().GetResult());
            Matches.AddRange(LoadSingleYear(1, current_season-1).GetAwaiter().GetResult());
            Matches.AddRange(LoadSingleYear(2, current_season-1).GetAwaiter().GetResult());
        }

        public double GetWeightedAverageGoalsAsHomeTeam(string team_name)
        {
            var matches = HomeMatches(team_name).Take(11).ToList();
            return GetWeightedAverageGoalsAsHomeTeam(matches);
        }

        public double GetWeightedAverageGoalsAsHomeTeam(List<Match> matches, double newest_weight=1.3, double leage2_weight_factor=0.3)
        {
            double sum = 0;
            double weight = 1;
            double sum_weights = 0;

            for (int i=0; i<matches.Count; i++)
            {
                weight = i == 0 ? newest_weight : 1;
                if (matches[i].League == 2)
                    weight = weight * leage2_weight_factor;

                sum_weights += weight;
                sum += weight * matches[i].MatchResults[0].PointsTeam1;;
            }

            return sum / sum_weights;
        }

        public double GetWeightedAverageGoalsAsAwayTeam(string team_name)
        {
            var matches = AwayMatches(team_name).Take(11).ToList();
            return GetWeightedAverageGoalsAsAwayTeam(matches);
        }

        public double GetWeightedAverageGoalsAsAwayTeam(List<Match> matches, double newest_weight=1.3, double leage2_weight_factor=0.3)
        {
            double sum = 0;
            double weight = 1;
            double sum_weights = 0;

            for (int i=0; i<matches.Count; i++)
            {
                weight = i == 0 ? newest_weight : 1;
                if (matches[i].League == 2)
                    weight = weight * leage2_weight_factor;
                
                sum_weights += weight;
                sum += weight * matches[i].MatchResults[0].PointsTeam2;
            }

            return sum / sum_weights;
        }

        public double GetSuccessRate(string team_name) 
        {
            int sum_points = 0;
            foreach (var m in AllMatches(team_name).Take(5)) 
            {
                if (m.WinnerTeamName == team_name)
                    sum_points += 3;
                
                if (m.WinnerTeamName == "")
                    sum_points++;
            }

            return (double)sum_points/15*100;
        }

        public List<string> TeamNames(int league, int season) 
        {
            return (from m in Matches 
                where m.League == league && m.Season == season
                select m.Team1.ShortName).Distinct().ToList();
        }

        public List<Match> UpcomingMatches() 
        {
            return (from m in Matches 
                where !m.MatchIsFinished && m.League == 1
                orderby m.MatchDateTime
                select m).ToList();
        }

        public List<Match> AllMatches(string team_name)
        {
            return (from m in Matches
                where m.MatchIsFinished && (m.Team1.ShortName == team_name || m.Team2.ShortName == team_name)
                orderby m.MatchDateTime descending
                select m).ToList();
        }

        public List<Match> HomeMatches(string team_name) 
        {
            return (from m in Matches
                where m.MatchIsFinished && m.Team1.ShortName == team_name
                orderby m.MatchDateTime descending
                select m).ToList();
        }

        public List<Match> HomeMatches(string team_name, int league, int season) 
        {
            return (from m in HomeMatches(team_name)
                where  m.League == league && m.Season == season 
                select m).ToList();
        }

        public List<Match> AwayMatches(string team_name) 
        {
            return (from m in Matches
                where m.MatchIsFinished && m.Team2.ShortName == team_name
                orderby m.MatchDateTime descending
                select m).ToList();
        }

        public List<Match> AwayMatches(string team_name, int league, int season) 
        {
            return (from m in AwayMatches(team_name)
                where m.League == league && m.Season == season 
                orderby m.MatchDateTime descending
                select m).ToList();
        }
    }
}