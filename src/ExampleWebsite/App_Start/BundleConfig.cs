using System.Web.Optimization;
using JB.WebOptimization.Transformers;
using JB.WebOptimization.Transformers.Exclusions;
using JB.WebOptimization.Transformers.IO;

namespace ExampleWebsite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            // limits converted files to a max of 20kb
            var cssDataUriTransformer = new CssDataUriTransform(new DefaultFileProvider(),
                                                                new FileSizeExclusion(new DefaultFileProvider(), 20000));

            var contentStyleBundle = new StyleBundle("~/Content/css").Include("~/Content/site.css");
            contentStyleBundle.Transforms.Add(cssDataUriTransformer);
            contentStyleBundle.Transforms.Add(new CssMinify());
            bundles.Add(contentStyleBundle);

            var baseStyleBundle = new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css");
            baseStyleBundle.Transforms.Add(cssDataUriTransformer);
            baseStyleBundle.Transforms.Add(new CssMinify());
            bundles.Add(baseStyleBundle);

            // Enable the following to force transformations - should only do this during development
            //BundleTable.EnableOptimizations = true;
        }
    }
}