using System;
using System.Text.RegularExpressions;

namespace JB.WebOptimization.Transformers.Exclusions
{
    /// <summary>
    /// Allows for excluding a file based on the result of a Regular Expression test
    /// </summary>
    public class RegexExclusion : IExclude
    {
        private readonly Regex _regex;

        /// <summary>
        /// Constructs an instance of the <see cref="RegexExclusion"/> class
        /// </summary>
        /// <param name="regex">The Regular Expression to test with</param>
        public RegexExclusion(Regex regex)
        {
            if (null == regex)
            {
                throw new ArgumentNullException("regex");
            }
            _regex = regex;
        }

        /// <summary>
        /// The implementation of the <see cref="IExclude"/> interface. Returns true if there is a match for the path within the defined regex
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Exclude(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }
            return _regex.Match(path) != Match.Empty;
        }
    }
}