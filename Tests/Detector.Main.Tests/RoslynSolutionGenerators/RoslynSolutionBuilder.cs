using Detector.Extractors.Base;
using Detector.Main.Tests.Stubs;
using Detector.Models.Base;
using Detector.Models.ORM;
using Detector.Models.ORM.DatabaseAccessingMethodCalls;
using Detector.Models.ORM.DatabaseEntities;
using Detector.Models.ORM.DatabaseQueries;
using Detector.Models.Others;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Generic;

namespace Detector.Main.Tests.RoslynSolutionGenerators
{
    public class RoslynSolutionBuilder
    {
        Solution _roslynSolution;
        public ExtractorFactory<FakeORMToolType> ExtractorsFactory { get; private set; }

        ProjectId _projectId;

        public RoslynSolutionBuilder()
        {
            _projectId = ProjectId.CreateNewId();
            DocumentId DataContextClassDocumentId = DocumentId.CreateNewId(_projectId);
            DocumentId OrderClassDocumentId = DocumentId.CreateNewId(_projectId);
            DocumentId EmployeeClassDocumentId = DocumentId.CreateNewId(_projectId);

            _roslynSolution = new AdhocWorkspace().CurrentSolution
                             .AddProject(_projectId, "MyProject", "MyProject", LanguageNames.CSharp)
                             .AddDocument(DataContextClassDocumentId, "DataContext.cs", string.Empty)
                             .AddDocument(OrderClassDocumentId, "Order.cs", string.Empty)
                             .AddDocument(EmployeeClassDocumentId, "Employee.cs", string.Empty);
        }

        public Solution GetRoslynSolution(string textToPlaceInMethod)
        {
            return this._roslynSolution;
        }

        public RoslynSolutionBuilder WithTwoDocumentsContainingOneCodeExecutionPath()
        {
            DocumentId mainClassDocumentId = DocumentId.CreateNewId(_projectId);
            DocumentId repositoryClassDocumentId = DocumentId.CreateNewId(_projectId);

            _roslynSolution.AddDocument(mainClassDocumentId, "Main.cs", string.Empty)
                            .AddDocument(repositoryClassDocumentId, "Repository.cs", string.Empty);
            DatabaseAccessingMethodCallExtractor<FakeORMToolType> fakeDbAccessingMethodCallExt = GetDDAccessingMethodCallExtractorReturningOneMethodCall();

            var mockExtractorsFactory = new Mock<ExtractorFactory<FakeORMToolType>>();
            mockExtractorsFactory.Setup(m => m.GetDatabaseAccessingMethodCallsExtractor()).Returns(fakeDbAccessingMethodCallExt);

            return this;
        }

        private static DatabaseAccessingMethodCallExtractor<FakeORMToolType> GetDDAccessingMethodCallExtractorReturningOneMethodCall()
        {
            //Create fake database query for db accessing method call
            var dbEntityDeclaration = new DatabaseEntityDeclaration<FakeORMToolType>("Employee", null);
            var dbEntityDeclarationsReturnedByDbQuery = new HashSet<DatabaseEntityDeclaration<FakeORMToolType>>() { dbEntityDeclaration };
            var dbQueryVariable = new DatabaseQueryVariable<FakeORMToolType>("", null);

            //Create fake CompilationInfo for db accessing method call
            var methodDeclarationCompilationInfo = new CompilationInfo(null, null);
           // var methodDeclarationContainingDbAccessingMethodCall = new MethodDeclaration("GetEmployee", methodDeclarationCompilationInfo);

            var dbAccessingMethodCallCompilationInfo = new CompilationInfo(null, null);

            //Create fake db accessing method call
            DatabaseAccessingMethodCallStatement<FakeORMToolType> dbAccessingMethodCall = new DatabaseAccessingMethodCallStatement<FakeORMToolType>("", dbEntityDeclarationsReturnedByDbQuery, dbAccessingMethodCallCompilationInfo);
            dbAccessingMethodCall.SetDatabaseQueryVariable(dbQueryVariable);

            var fakeDbAccessingMethodCallExt = new Mock<DatabaseAccessingMethodCallExtractor<FakeORMToolType>>();
            var dbAccessingMethodCallsToReturn = new HashSet<DatabaseAccessingMethodCallStatement<FakeORMToolType>>() { dbAccessingMethodCall };
            fakeDbAccessingMethodCallExt.Setup(f => f.DatabaseAccessingMethodCalls).Returns(dbAccessingMethodCallsToReturn);
            return fakeDbAccessingMethodCallExt.Object;
        }

        private string GetDataContextCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.DatabaseAttribute]
                            public partial class NorthWindDataClassesDataContext : System.Data.Linq.DataContext
                            {
                                   partial void OnCreated();
                                    private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
                                    public NorthWindDataClassesDataContext() :
                                        base("", mappingSource)
                                    {
                                        OnCreated();
                                    }

                                    public System.Data.Linq.Table<Employee> Employees
                                    {
                                        get
                                        {
                                            return this.GetTable<Employee>();
                                        }
                                    }

                                    public System.Data.Linq.Table<Order> Orders
                                    {
                                        get
                                        {
                                            return this.GetTable<Order>();
                                        }
                                    }
                            }                            
                        }";

            return text;
        }

        private string GetOrderClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.TableAttribute]
                            public partial class Order
                            {
                                [global::System.Data.Linq.Mapping.ColumnAttribute]
                                public int OrderID;
                            }          
                        }";

            return text;
        }

        private string GetEmployeeClassCSharpDocumentText()
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            [global::System.Data.Linq.Mapping.TableAttribute]
                            public partial class Employee
                            {
                                 [global::System.Data.Linq.Mapping.ColumnAttribute]
                                 public int EmployeeID;

                                 [global::System.Data.Linq.Mapping.AssociationAttribute]
                                 public EntitySet<Order> Orders;
                            }       
                        }";

            return text;
        }

        private string GetMainClassCSharpDocumentText(string textToPlaceInMethod)
        {
            string text = @" using System.Data.Linq;
                            using System.Data.Linq.Mapping;
                            using System.Linq;
						namespace L2S_Northwind
						{
                            public class ClassUnderTest
							{
								public static Employee GetEmployeeById(int empId)
								{
									" + textToPlaceInMethod + @"
								}
							}    
                        }";

            return text;
        }
    }
}
