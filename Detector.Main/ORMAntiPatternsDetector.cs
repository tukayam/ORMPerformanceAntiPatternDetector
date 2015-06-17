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
        IGodClass _godClass;
        ORMModelTreeExtractor _ORMModelTreeExtractor;

        public ORMAntiPatternsDetector(IGodClass godClass, ORMModelTreeExtractor ORMModelTreeExtractor)
        {
            _godClass = godClass;
            _ORMModelTreeExtractor = ORMModelTreeExtractor;
        }

        public async Task<List<AntiPatternBase>> Detect(Solution roslynSolution)
        {
            await _godClass.ExtractFromRoslynSolutionAsync(roslynSolution);

            List<ORMModelTree> codeExecutionPaths = new List<ORMModelTree>();
            foreach (var methodDeclaration in _godClass.MethodDeclarations)
            {
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
