namespace betthelper.Model
{
    public class ResultProbability
    {
        public string Result { get; set; }
        public double ProbablityInPercent { get; set; }

        public ResultProbability(string result, double probability)
        {
            Result = result;
            ProbablityInPercent = probability;
        }
    }
}
