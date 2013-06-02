using System;
using JB.WebOptimization.Transformers.IO;

namespace JB.WebOptimization.Transformers.Exclusions
{
    /// <summary>
    /// Allows for excluding a file based on its size from being converted to a data URI.
    /// </summary>
    public class FileSizeExclusion : IExclude
    {
        private readonly IFileProvider _fileProvider;
        private readonly int _maxFileSize;

        /// <summary>
        /// Constructs an instance of the <see cref="FileSizeExclusion"/> class
        /// </summary>
        /// <param name="fileProvider">A wrapper for access to the file system</param>
        /// <param name="maxFileSize">The maximumfile size in bytes</param>
        public FileSizeExclusion(IFileProvider fileProvider, int maxFileSize)
        {
            if (null == fileProvider)
            {
                throw new ArgumentNullException("fileProvider");
            }
            _fileProvider = fileProvider;
            _maxFileSize = maxFileSize;
        }
        
        /// <summary>
        /// The implementation of the <see cref="IExclude"/> interface. Returns true if the size of the file is greater than or equal to maxFileSize
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exclude(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }
            return _fileProvider.Length(path) >= _maxFileSize;
        }
    }
}