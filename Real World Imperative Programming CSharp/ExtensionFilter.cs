using System.IO;

namespace RealWorldImperativeProgrammingCSharp
{
    public class ExtensionFilter : IFileFilter
    {
        public string Name
        {
            get
            {
                return "Dateierweiterungsfilter";
            }
        }

        public bool Apply(string filterText, string path)
        {
            var extension = Path.GetExtension(path);
            
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }
            
            return extension.Substring(1) == filterText;  
        }
    }
}
