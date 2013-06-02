using System.Text.RegularExpressions;
using JB.WebOptimization.Transformers.Exclusions;
using NUnit.Framework;

namespace JB.WebOptimization.Transformers.Tests.Exclusions
{
    [TestFixture]
    public class RegexExclusionTests
    {
        [Test]
        public void WhenTheRegexIsMatchedReturnTrue()
        {
            const string filePath = @"http://local/image.png";
            var regex = new Regex(@"^((ht|f)tp(s?)\:)");

            var exclusion = new RegexExclusion(regex);

            Assert.IsTrue(exclusion.Exclude(filePath));
        }

        [Test]
        public void WhenTheRegexIsNotMatchedReturnFalse()
        {
            const string filePath = @"path\to\file";
            var regex = new Regex(@"^((ht|f)tp(s?)\:)");

            var exclusion = new RegexExclusion(regex);

            Assert.IsFalse(exclusion.Exclude(filePath));
        }
    }
}