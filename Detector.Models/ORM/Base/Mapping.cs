using System;
using System.Linq;

namespace Detector.Models.ORM.Base
{
    public abstract class Mapping
    {
        private DatabaseEntityDeclaration[] entities;
        public DatabaseEntityDeclaration[] Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = new DatabaseEntityDeclaration[2];
                }
                return entities;
            }
            set
            {
                if (value.ToArray().Length == 2)
                {
                    entities = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Mapping must have 2 entities.");
                }
            }
        }

    }
}
