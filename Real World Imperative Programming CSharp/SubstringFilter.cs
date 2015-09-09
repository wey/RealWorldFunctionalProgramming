using System.IO;

namespace RealWorldImperativeProgrammingCSharp
{
    public class SubstringFilter : IFileFilter
    {
        public string Name
        {
            get
            {
                return "Teilstring-Filter";
            }
        }

        public bool Apply(string filterText, string path)
        {
            var fileName = Path.GetFileName(path);
            
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }
            
            return fileName.Contains(filterText);
        }
    }
}
