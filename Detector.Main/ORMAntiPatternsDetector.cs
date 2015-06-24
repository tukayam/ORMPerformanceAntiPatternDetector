using Detector.Extractors.LINQToSQL40;
using Detector.Main.DetectionRules;
using Detector.Models.AntiPatterns;
using Detector.Models.ORM;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Detector.Main
{
    public class ORMAntiPatternsDetector<T> where T : ORMToolType
    {
        public async Task<List<AntiPatternBase>> DetectAsync(Solution roslynSolution)
        {
            var godClass = new GodClass();
            await godClass.ExtractFromRoslynSolutionAsync(roslynSolution);

            List<DetectionRule> detectionRules = new DetectionRuleFactory<T>().GetDetectionRules();
            var detectedAntiPatterns = new List<AntiPatternBase>();
            foreach (var modelTree in godClass.ORMModelTrees)
            {
                foreach (var detectionRule in detectionRules)
                {                    
                    if (detectionRule.AppliesToModelTree(modelTree))
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
