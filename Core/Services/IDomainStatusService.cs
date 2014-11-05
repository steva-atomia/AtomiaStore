using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.OrderPage.Core.Models;

namespace Atomia.OrderPage.Core.Services
{
    public interface IDomainStatusService
    {
        DomainStatusResult CheckStatus(DomainStatusQuery query);
    }
}
