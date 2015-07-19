using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Threading.Tasks;

namespace Detector.Extractors.EF60.Tests.Helpers.RoslynSolutionGenerators
{
    public class RoslynProjectGenerator
    {
        public async Task<Project> GetEF60_NWProjectAsync()
        {
            var msWorkspace = MSBuildWorkspace.Create();
            string currentDir = Environment.CurrentDirectory;
            string projectPath = currentDir + @"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.csproj";

            //You must install the MSBuild Tools or this line will throw an exception:
            return await msWorkspace.OpenProjectAsync(projectPath);
        }

        internal async Task<Solution> GetEF60_NWSolutionAsync()
        {
            var msWorkspace = MSBuildWorkspace.Create();
            string currentDir = Environment.CurrentDirectory;
            string solutionPath = currentDir + @"..\..\..\..\ProjectsUnderTest\EF60_NW\EF60_NW.sln";

            //You must install the MSBuild Tools or this line will throw an exception:
            return await msWorkspace.OpenSolutionAsync(solutionPath);
        }
    }
}
