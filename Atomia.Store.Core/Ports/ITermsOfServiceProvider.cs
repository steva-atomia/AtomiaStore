using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface ITermsOfServiceProvider
    {
        IEnumerable<TermsOfService> GetTermsOfService();

        TermsOfService GetTermsOfService(string id);
    }
}
