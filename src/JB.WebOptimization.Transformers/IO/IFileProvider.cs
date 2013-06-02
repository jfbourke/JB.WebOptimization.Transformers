namespace JB.WebOptimization.Transformers.IO
{
    /// <summary>
    /// A warpper interface for a reduced set of file system access methods
    /// </summary>
    public interface IFileProvider
    {
        bool Exists(string path);
        byte[] ReadAllBytes(string path);
        string ReadAllText(string path);
        long Length(string path);
    }
}