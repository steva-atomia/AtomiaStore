using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public class SearchTerm
    {
        public SearchTerm(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public SearchTerm()
        {

        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
