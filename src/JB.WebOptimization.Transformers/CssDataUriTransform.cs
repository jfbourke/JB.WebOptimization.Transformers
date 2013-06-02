using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Optimization;
using JB.WebOptimization.Transformers.Exclusions;
using JB.WebOptimization.Transformers.IO;

namespace JB.WebOptimization.Transformers
{
    /// <summary>
    /// Converts relative image references into Data URIs, <see cref="http://en.wikipedia.org/wiki/Data_URI_scheme"/>
    /// </summary>
    public class CssDataUriTransform : IBundleTransform
    {
        private readonly IList<IExclude> _exclusions;
        internal static string CssContentType = "text/css";
        private readonly IFileProvider _fileProvider;

        /// <summary>
        /// Constructs an instance of the <see cref="CssDataUriTransform"/> class
        /// </summary>
        /// <param name="fileProvider">A wrapper for access to the file system</param>
        public CssDataUriTransform(IFileProvider fileProvider)
        {
            if (null == fileProvider)
            {
                throw new ArgumentNullException("fileProvider");
            }
            _fileProvider = fileProvider;
            _exclusions = new List<IExclude>
                {
                    new RegexExclusion(new Regex(@"^(ht|f)tp(s?)\:"))
                };
        }

        /// <summary>
        /// Constructs an instance of the <see cref="CssDataUriTransform"/> class with a custom set of exclusions
        /// </summary>
        /// <param name="fileProvider">A wrapper for access to the file system</param>
        /// <param name="exclusions">The exclusions to apply to matches</param>
        public CssDataUriTransform(IFileProvider fileProvider, params IExclude[] exclusions) : this(fileProvider)
        {
            foreach (var exclusion in exclusions)
            {
                _exclusions.Add(exclusion);
            }
        }

        /// <summary>
        /// The implementation of the <see cref="IBundleTransform"/> interface
        /// </summary>
        /// <param name="context"></param>
        /// <param name="response"></param>
        public void Process(BundleContext context, BundleResponse response)
        {
            if (null == context)
            {
                throw new ArgumentNullException("context");
            }
            if (null == response)
            {
                throw new ArgumentNullException("response");
            }

            var urlMatcher = new Regex(@"url\((['""]?)(.+?)\1\)");


            var content = new StringBuilder();
            foreach (var fileInfo in response.Files)
            {
                if (fileInfo.Directory == null)
                {
                    continue;
                }
                var fileContent = _fileProvider.ReadAllText(fileInfo.FullName);
                var directory = fileInfo.Directory.FullName;
                content.Append(urlMatcher.Replace(fileContent, match => ProcessMatch(match, directory)));
            }
            
            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();
            response.Content = content.ToString();
            response.ContentType = CssContentType;
        }

        private string ProcessMatch(Match match, string directory)
        {
            if (!match.Success)
            {
                return string.Empty;
            }

            var uri = match.Groups[2].Value;
            var path = Path.Combine(directory, uri);

            return ExcludeUri(uri) ? match.ToString() : GenerateDataUri(match, path);
        }

        private bool ExcludeUri(string uri)
        {
            return _exclusions.Any(exclusion => exclusion.Exclude(uri));
        }

        private string GenerateDataUri(Match match, string path)
        {
            var uri = match.Groups[2].Value;
            //var path = context.HttpContext.Server.MapPath(uri.Replace("../",""));

            if (!_fileProvider.Exists(path))
            {
                return uri;
            }

            var imageBytes = _fileProvider.ReadAllBytes(path);
            var dataUrl = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));

            return match.ToString().Replace(uri, dataUrl);
        }
    }
}
