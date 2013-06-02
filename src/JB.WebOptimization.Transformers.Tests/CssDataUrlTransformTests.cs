using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Optimization;
using JB.WebOptimization.Transformers.IO;
using NSubstitute;
using NUnit.Framework;

namespace JB.WebOptimization.Transformers.Tests
{
    [TestFixture]
    public class CssDataUrlTransformTests
    {
        [Test]
        public void ProcessReturnsCssWithDataUrls()
        {
            const string css = "#myclass{background:url(\"../Images/orderedList0.png\") no-repeat;}#myclass2{background:url('../Images/orderedList0.png') no-repeat;}#myclass3{background:url(../Images/orderedList0.png) no-repeat;}";

            var fileProvider = Substitute.For<IFileProvider>();
            fileProvider.Exists(Arg.Any<string>()).Returns(true);
            fileProvider.ReadAllText(Arg.Any<string>()).Returns(css);
            fileProvider.ReadAllBytes(Arg.Any<string>()).Returns(System.Text.Encoding.ASCII.GetBytes(@"test"));

            var httpContext = Substitute.For<HttpContextBase>();
            var context = new BundleContext(httpContext, new BundleCollection(), string.Empty);
            var files = new List<FileInfo>
                {
                    new FileInfo(@"css\test.css")
                };
            var response = new BundleResponse(css, files);

            var transform = new CssDataUriTransform(fileProvider);
            transform.Process(context, response);

            Assert.AreEqual("text/css", response.ContentType);
            Assert.AreEqual("#myclass{background:url(\"data:image/png;base64,dGVzdA==\") no-repeat;}#myclass2{background:url('data:image/png;base64,dGVzdA==') no-repeat;}#myclass3{background:url(data:image/png;base64,dGVzdA==) no-repeat;}", response.Content);
        }

        [Test]
        public void ProcessReturnsCssWithDataUrlsExcludingSchemaReferencedImages()
        {
            const string css = "#myclass{background:url(\"../Images/orderedList0.png\") no-repeat;}#myclass2{background:url(\"http://local/Images/orderedList0.png\") no-repeat;}";
            var fileProvider = Substitute.For<IFileProvider>();
            fileProvider.Exists(Arg.Any<string>()).Returns(true);
            fileProvider.ReadAllText(Arg.Any<string>()).Returns(css);
            fileProvider.ReadAllBytes(Arg.Any<string>()).Returns(System.Text.Encoding.ASCII.GetBytes(@"test"));

            var httpContext = Substitute.For<HttpContextBase>();
            var context = new BundleContext(httpContext, new BundleCollection(), string.Empty);
            var files = new List<FileInfo>
                {
                    new FileInfo(@"css\test.css")
                };
            var response = new BundleResponse(css, files);

            var transform = new CssDataUriTransform(fileProvider);
            transform.Process(context, response);

            Assert.AreEqual("text/css", response.ContentType);
            Assert.AreEqual("#myclass{background:url(\"data:image/png;base64,dGVzdA==\") no-repeat;}#myclass2{background:url(\"http://local/Images/orderedList0.png\") no-repeat;}", response.Content);
        }
    }
}
