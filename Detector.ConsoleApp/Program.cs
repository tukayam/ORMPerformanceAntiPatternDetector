using System;
using System.Threading.Tasks;

namespace Detector.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
            Console.WriteLine("Press enter to exit...");
            Console.Read();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Tool started at " + DateTime.Now.ToLongTimeString());
            string folderPath = @"D:\School\Thesis Bug prediction with antipatterns\Projects\vc-community\PLATFORM";
            string solutionUnderTest = "VirtoCommerce.WebPlatform";

            Console.WriteLine("Extracting for solution: " + folderPath + @"\" + solutionUnderTest);
            var extractionManager = new DependencyResolver().GetExtractionManager();

            await extractionManager.ExtractAllAsync(solutionUnderTest, folderPath);
            Console.WriteLine("Tool stopped at " + DateTime.Now.ToLongTimeString());
        }
    }
}
