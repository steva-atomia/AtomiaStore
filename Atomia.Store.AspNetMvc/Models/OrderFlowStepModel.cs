using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{
    public class OrderFlowStepModel
    {
        public int StepNumber { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Previous { get; set; }

        public string Next { get; set; }
    }
}
