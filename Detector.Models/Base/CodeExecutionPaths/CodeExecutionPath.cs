using System.Collections.Generic;

namespace Detector.Models.Base
{
    public class CodeExecutionPath
    {
        List<ModelBase> _models;
        public IEnumerable<ModelBase> Models
        {
            get
            {
                return _models;
            }
        }

        public CodeExecutionPath()
        {
            _models = new List<ModelBase>();
        }

        public void AddModel(ModelBase model)
        {
            _models.Add(model);
        }
    }
}
