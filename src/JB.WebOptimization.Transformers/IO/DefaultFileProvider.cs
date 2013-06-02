namespace JB.WebOptimization.Transformers.IO
{
    /// <summary>
    /// Default implementation of <see cref="IFileProvider"/>
    /// </summary>
    public class DefaultFileProvider : IFileProvider
    {
        public bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public byte[] ReadAllBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public long Length(string path)
        {
            return !Exists(path) ? 0 : new System.IO.FileInfo(path).Length;
        }
    }
}
