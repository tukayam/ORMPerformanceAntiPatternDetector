using Detector.Extractors;
using Detector.Extractors.Base;
using Detector.Main.DetectionRules;
using Detector.Models;
using Detector.Models.AntiPatterns;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class ORMAntiPatternsDetector<T> where T : ORMToolType
    {
        public async Task<List<AntiPatternBase>> Detect(Solution roslynSolution)
        {
            var godClass = new GodClass();
            await godClass.ExtractFromRoslynSolutionAsync(roslynSolution);

           var codeExecutionPaths = new List<ORMModelTree>();
            foreach (var methodDeclaration in godClass.MethodDeclarations)
            {
                ORMModelTreeExtractor _ORMModelTreeExtractor = new RoslynORMModelTreeExtractor(godClass.DatabaseQueries);
                ORMModelTree ORMModelTree = _ORMModelTreeExtractor.Extract(methodDeclaration.Value as MethodDeclarationSyntax);

                codeExecutionPaths.Add(ORMModelTree);
            }

            List<DetectionRule> detectionRules = new DetectionRuleFactory<T>().GetDetectionRules();
            var detectedAntiPatterns = new List<AntiPatternBase>();
            foreach (var codeExecutionPath in codeExecutionPaths)
            {
                foreach (var detectionRule in detectionRules)
                {                    
                    if (detectionRule.AppliesToModelTree(codeExecutionPath))
                    {
                        detectedAntiPatterns.AddRange(detectionRule.DetectedAntiPatterns);
                    }
                }               
            }

            return detectedAntiPatterns;
        }

        public List<AntiPatternBase> Detect(string pathToFolderContainingExtractedInformation)
        {
            return new List<AntiPatternBase>();
        }
    }
}
