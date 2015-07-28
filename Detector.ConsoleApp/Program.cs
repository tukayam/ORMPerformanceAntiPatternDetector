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
            //Console.Write("Please enter folder path:");
            //string folderPath = Console.ReadLine();

            //Console.Write("Please enter solution file name:");
            //string solutionUnderTest = Console.ReadLine();

            //string folderPath = @"D:\School\Thesis Bug prediction with antipatterns\Projects\nopcommerce-f930277908c2cb606620cefe46ab19519e2c2bf7\src";
            //string solutionUnderTest = "NopCommerce";

            string folderPath = @"D:\School\Thesis Bug prediction with antipatterns\Projects\vc-community\PLATFORM";
            string solutionUnderTest = "VirtoCommerce.WebPlatform";

            Console.WriteLine("Extracting for solution: " + folderPath + @"\" + solutionUnderTest);
            var extractionManager = new DependencyResolver().GetExtractionManager();

            await extractionManager.ExtractAllAsync(solutionUnderTest, folderPath);
        }
    }
}
