
namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class PaymentMethodForm
    {
        public abstract string Id { get; }

        public virtual string PartialViewName { get { return "_" + this.Id; } }
    }
}
