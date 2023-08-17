using System.Web;
using System.Web.Optimization;

namespace Doctor_Loan
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Jquery").Include(
                        "~/Assets/libs/jquery/jquery-3.7.0.min.js"));

            #region Core Scripts
            bundles.Add(new ScriptBundle("~/Scripts/CoreScripts").Include(
                "~/Assets/libs/SuperFish/hoverIntent.js",
                "~/Assets/libs/SuperFish/superfish.min.js",
                "~/Assets/libs/glightbox/glightbox.min.js",
                "~/Assets/js/main.js",
                "~/Assets/js/common.js"
                ));
            #endregion

            #region Core CSS
            bundles.Add(new StyleBundle("~/Contents/CoreStyle").Include(
                "~/Assets/libs/Bootstrap/bootstrap.min.css",
                "~/Assets/libs/glightbox/glightbox.min.css"
            //"~/Assets/css/styles.min.css" 
            ));
            #endregion

            #region Owl Carousel
            bundles.Add(new ScriptBundle("~/Scripts/OwlCarousel").Include(
                "~/Assets/libs/OwlCarousel/owl.carousel.min.js"
            ));

            bundles.Add(new StyleBundle("~/Styles/OwlCarousel").Include(
                "~/Assets/libs/OwlCarousel/owl.carousel.min.css"
            ));
            #endregion

            BundleTable.EnableOptimizations = true;
        }
    }
}
