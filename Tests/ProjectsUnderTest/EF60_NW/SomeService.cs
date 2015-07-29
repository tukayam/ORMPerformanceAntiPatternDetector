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
            var customers = repository.GetAllCustomers();
        }

        public void SomeMethodCallingConcreteRepository()
        {
            var customers = new Repository().GetCustomerUsingMethodSyntax(1);
        }
    }
}
