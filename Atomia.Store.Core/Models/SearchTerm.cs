
namespace Atomia.Store.Core
{
    public sealed class SearchTerm
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
