using System.Collections.Generic;
using System.IO;

namespace betthelper 
{
    public class Markdown
    {
        private List<string> content = new List<string>();
        
        public void AddText(string s) 
        {
            content.Add(s);
        }

        public void AddSection(string s) 
        {
            content.Add("# " + s);
        }

        public void AddSubSection(string s) 
        {
            content.Add("## " + s);
        }

        public void AddSubsubSection(string s)
        {
            content.Add("### " + s);
        }

        public void AddEmpty()
        {
            content.Add("");
        }

        public void AddBulletpoint(string s)
        {
            content.Add("- " + s);
        }

        public void WriteToFile(string filename) 
        {
            File.WriteAllLines(filename, content);
        }
    }
}