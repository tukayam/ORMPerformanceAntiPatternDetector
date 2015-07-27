using System.Threading.Tasks;

namespace Detector.Main
{
    public interface IExtractionManager
    {
        Task ExtractAllAsync(string solutionUnderTest, string folderPath);
    }
}