using System;
using System.Linq;
using betthelper.Model;

namespace betthelper
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new ReportGenerator();
            helper.CreateFullReport();
            helper.WriteReportToFile("report.md");
        }
    }
}
