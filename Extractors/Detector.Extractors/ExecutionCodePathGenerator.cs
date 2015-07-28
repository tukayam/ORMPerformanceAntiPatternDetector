using System.Collections.Generic;
using Detector.Models;
using System;
using System.Linq;
using Detector.Models.Base;
using Detector.Models.ORM.ORMTools;

namespace Detector.Extractors
{
    public sealed class ExecutionCodePathGenerator<T> : ExecutionCodePathGeneratorBase<T> where T : ORMToolType
    {
        public ORMModelTree ORMModelTree { get; private set; }
        private ORMModelNode _LastVisitedNode;

        #region -- Visit methods --

        public void Visit(ModelBase model)
        {
            if (model is MethodDeclarationBase)
            {
                this.Visit(model as MethodDeclarationBase);
            }
            else
            {
                var node = new ORMModelNode(model);

                if (_LastVisitedNode is LoopDeclarationBase)
                {
                    _LastVisitedNode.ChildNodes.Add(node);
                }
                else
                {
                    ORMModelTree.RootNode.ChildNodes.Add(node);
                    _LastVisitedNode = node;
                }
            }
        }

        private void Visit(MethodDeclarationBase model)
        {
            if (this.ORMModelTree.RootNode != null)
            {
                throw new ArgumentException("There can only be one MethodDeclaration in an ORMModelTree.");
            }
            var rootNode = new ORMModelNode(model);
            this.ORMModelTree = new ORMModelTree(rootNode);

            SetLastVisitedNode(rootNode);
        }

        private void SetLastVisitedNode(ORMModelNode node)
        {
            _LastVisitedNode = node;
        }

        #endregion      

        public ExecutionCodePathGenerator()
        {
            ORMModelTree = new ORMModelTree();
        }

        public ORMModelTree GenerateFromModelList(List<ModelBase> models)
        {
            models.Sort(new ModelBaseComparer());

            if (models.First() is MethodDeclarationBase == false)
            {
                throw new ArgumentException("ORMModelTree rootnode must be a MethodDeclaration. First model in the list should be the rootnode and it is not deriven from MethodDeclarationBase.");
            }

            foreach (var model in models)
            {
                this.Visit(model);
            }

            return this.ORMModelTree;
        }

        internal class ModelBaseComparer : IComparer<ModelBase>
        {
            public int Compare(ModelBase x, ModelBase y)
            {
                int lineNumberX = x.CompilationInfo.SyntaxNode.SpanStart;
                int lineNumberY = y.CompilationInfo.SyntaxNode.SpanStart;
                return lineNumberX > lineNumberY ? 1 : lineNumberX > lineNumberY ? -1 : 0;
            }
        }
    }
}
