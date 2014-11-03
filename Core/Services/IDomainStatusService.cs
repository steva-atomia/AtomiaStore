using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atomia.OrderPage.Core.Services
{

    public class DomainStatusQuery
    {

    }

    public class DomainStatusResult
    {

    }

    public interface IDomainStatusService
    {
        DomainStatusResult CheckStatus(DomainStatusQuery query);
    }
}
