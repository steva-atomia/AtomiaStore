using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{
    public class OrderFlowModel
    {
        public IEnumerable<OrderFlowStepModel> Steps { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public OrderFlowStepModel CurrentStep { get; set; }
    }
}
