using System.IO;

namespace RealWorldImperativeProgrammingCSharp
{
    public class DeleteFileAction : IFileAction
    {
        public string Name
        {
            get
            {
                return "Datei löschen";
            }
        }

        public void Execute(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
            }
                
        }
    }
}
