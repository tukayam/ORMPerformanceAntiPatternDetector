namespace EF60_NW
{
    public class SomeService
    {
        IRepository repository;
        public SomeService(IRepository repository)
        {
            this.repository = repository;
        }

        public void SomeMethodCallingIRepository()
        {
            var customers = repository.GetCustomerUsingMethodSyntaxAndAssignToVariable(1);           
        }

        public void SomeMethodCallingConcreteRepository()
        {
            var customers = new Repository().GetCustomerUsingMethodSyntax(1);
        }
    }
}
