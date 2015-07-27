using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class RoslynSolutionGenerator
    {
        public async Task<Solution> GetSolutionAsync(string solutionPath)
        {
            var msWorkspace = MSBuildWorkspace.Create();           

            //You must install the MSBuild Tools or this line will throw an exception:
            return await msWorkspace.OpenSolutionAsync(solutionPath);
        }
    }
}
