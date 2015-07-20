using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Threading.Tasks;

namespace TestBase.RoslynSolutionGenerators
{
    public class RoslynSolutionGenerator
    {
        public async Task<Solution> GetSolutionAsync(string relativePath)
        {
            var msWorkspace = MSBuildWorkspace.Create();
            string currentDir = Environment.CurrentDirectory;
            string solutionPath = currentDir + relativePath;

            //You must install the MSBuild Tools or this line will throw an exception:
            return await msWorkspace.OpenSolutionAsync(solutionPath);
        }
    }
}
