namespace RealWorldImperativeProgrammingCSharp
{
    public interface IFileFilter
    {
        string Name { get; }

        bool Apply(string filterText, string path);
    }
}
