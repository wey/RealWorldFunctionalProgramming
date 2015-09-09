namespace RealWorldImperativeProgrammingCSharp
{
    public class FileEntry
    {
        private readonly string path;

        public FileEntry(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get
            {
                return path;
            }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(path);
            }
        }
    }
}
