namespace RealWorldImperativeProgrammingCSharp
{
    public interface IFileAction
    {
        string Name { get; }

        void Execute(string path);
    }
}
