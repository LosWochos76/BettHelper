using System;
using System.Linq;
using betthelper.Model;

namespace betthelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new BettHelper();
            helper.CreateFullReport();
            helper.WriteReportToFile("results.md");
        }
    }
}
