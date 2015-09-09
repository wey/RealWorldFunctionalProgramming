using System.Diagnostics;
using System.IO;

namespace RealWorldImperativeProgrammingCSharp
{
    public class OpenFileAction : IFileAction
    {
        public string Name
        {
            get
            {
                return "Datei öffnen";
            }
        }

        public void Execute(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    Process.Start(path);
                }
                catch
                {  
                }
            }
        }
    }
}
