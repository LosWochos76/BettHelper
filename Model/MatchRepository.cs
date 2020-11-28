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
                m.League = league;
            
            return matches;
        }

        public void LoadAll() 
        {
            int current_year = DateTime.Now.Year;
            if (DateTime.Now.Month < 9)
                current_year--;

            Matches.Clear();
            Matches.AddRange(LoadSingleYear(1, current_year).GetAwaiter().GetResult());
            Matches.AddRange(LoadSingleYear(1, current_year-1).GetAwaiter().GetResult());
            Matches.AddRange(LoadSingleYear(2, current_year-1).GetAwaiter().GetResult());
        }

        public double GetAverageGoalsAsHomeTeam(string team_name, int match_count)
        {
            int sum = 0;
            int count = 0;

            var matches = (from m in Matches 
                where m.MatchIsFinished && m.Team1.ShortName == team_name 
                orderby m.MatchDateTime descending select m);

            foreach (var m in matches) 
            {
                if (m.League == 2)
                    sum+= m.MatchResults[0].PointsTeam1 / 2;
                else
                    sum+=m.MatchResults[0].PointsTeam1;
                
                count++;
                
                if (count + 1 == match_count)
                    break;
            }

            return (double)sum / count;
        }

        public double GetAverageGoalsAsAwayTeam(string team_name, int match_count)
        {
            int sum = 0;
            int count = 0;

            var matches = (from m in Matches 
                where m.MatchIsFinished && m.Team2.ShortName == team_name 
                orderby m.MatchDateTime descending select m);
                
            foreach (var m in matches) 
            {
                if (m.League == 2)
                    sum+= m.MatchResults[0].PointsTeam2 / 2;
                else
                    sum+=m.MatchResults[0].PointsTeam2;
                
                count++;
                
                if (count + 1 == match_count)
                    break;
            }

            return (double)sum / count;
        }

        public List<string> TeamNames(int league) 
        {
            return (from m in Matches where m.League == league select m.Team1.ShortName).Distinct().ToList();
        }

        public List<Match> UpcomingMatches() 
        {
            return (from m in Matches 
                where !m.MatchIsFinished && m.League == 1
                orderby m.MatchDateTime
                select m).ToList();
        }
    }
}