namespace ViewModelLocator
{
    public interface ILocator
    {
        string LocateDataContext();
        string LocateFile(string path, string solutionPath);
    }
}