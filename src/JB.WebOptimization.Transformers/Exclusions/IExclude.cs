namespace JB.WebOptimization.Transformers.Exclusions
{
    /// <summary>
    /// Allows for adding custom exclusions into the process pipeline
    /// </summary>
    public interface IExclude
    {
        /// <summary>
        /// Returns true if path fits exclusion rules
        /// </summary>
        /// <param name="path">The value within the braces for a URI reference in CSS</param>
        /// <returns></returns>
        bool Exclude(string path);
    }
}