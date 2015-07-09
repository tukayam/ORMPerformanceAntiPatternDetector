using MVVMbasics.Models;
using MVVMbasics.Attributes;

namespace Core.Models
{
    public class SampleModel : BaseModel
    {
        //TODO: Add properties and, if necessary, declare them as bindable, as shown in the following example:
        [MvvmBindable]
        public int SampleProperty { get; set; }
    }
}
