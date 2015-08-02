using System.Collections.Generic;

namespace Detector.Models.Base
{
    public class CodeExecutionPath
    {
        List<Model> _models;
        public IEnumerable<Model> Models
        {
            get
            {
                return _models;
            }
        }

        public CodeExecutionPath()
        {
            _models = new List<Model>();
        }

        public void AddModel(Model model)
        {
            _models.Add(model);
        }
    }
}
