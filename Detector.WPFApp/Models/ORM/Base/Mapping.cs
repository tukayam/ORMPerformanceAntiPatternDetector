﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detector.WPFApp.Models
{
    public abstract class Mapping
    {
        private DatabaseEntityClass[] entities;
        public DatabaseEntityClass[] Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = new DatabaseEntityClass[2];
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
