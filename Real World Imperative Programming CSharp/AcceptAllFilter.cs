namespace RealWorldImperativeProgrammingCSharp
{
    public class AcceptAllFilter : IFileFilter
    {
        public string Name
        {
            get
            {
                return "Kein Filter";
            }
        }

        public bool Apply(string filterText, string path)
        {
            return true;
        }
    }
}
