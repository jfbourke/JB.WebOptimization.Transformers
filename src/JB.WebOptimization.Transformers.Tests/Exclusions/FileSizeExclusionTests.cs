using JB.WebOptimization.Transformers.Exclusions;
using JB.WebOptimization.Transformers.IO;
using NSubstitute;
using NUnit.Framework;

namespace JB.WebOptimization.Transformers.Tests.Exclusions
{
    [TestFixture]
    public class FileSizeExclusionTests
    {
        [Test]
        public void WhenTheFileSizeIsEqualToThanMaxReturnTrue()
        {
            const string filePath = @"path\to\file";
            var fileProvider = Substitute.For<IFileProvider>();
            fileProvider.Length(Arg.Is(filePath)).Returns(5000);

            var exclusion = new FileSizeExclusion(fileProvider, 5000);

            Assert.IsTrue(exclusion.Exclude(filePath));
        }

        [Test]
        public void WhenTheFileSizeIsGreaterThanMaxReturnTrue()
        {
            const string filePath = @"path\to\file";
            var fileProvider = Substitute.For<IFileProvider>();
            fileProvider.Length(Arg.Is(filePath)).Returns(5001);

            var exclusion = new FileSizeExclusion(fileProvider, 5000);

            Assert.IsTrue(exclusion.Exclude(filePath));
        }

        [Test]
        public void WhenTheFileSizeIsLessThanMaxReturnFalse()
        {
            const string filePath = @"path\to\file";
            var fileProvider = Substitute.For<IFileProvider>();
            fileProvider.Length(Arg.Is(filePath)).Returns(4500);

            var exclusion = new FileSizeExclusion(fileProvider, 5000);

            Assert.IsFalse(exclusion.Exclude(filePath));
        }
    }
}
