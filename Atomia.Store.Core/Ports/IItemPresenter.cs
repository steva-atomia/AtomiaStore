using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Item that can be presented by <see cref="ItemPresenter"/>
    /// </summary>
    public interface IPresentableItem
    {
        /// <summary>
        /// The item's article number
        /// </summary>
        string ArticleNumber { get; }

        /// <summary>
        /// The item's custom attributes
        /// </summary>
        List<CustomAttribute> CustomAttributes { get; }
    }

    /// <summary>
    /// Get name, description and category for an <see cref="IPresentableItem"/>.
    /// </summary>
    public interface IItemPresenter
    {
        /// <summary>
        /// Get human-readable name
        /// </summary>
        string GetName(IPresentableItem item);

        /// <summary>
        /// Get human-readable description
        /// </summary>
        string GetDescription(IPresentableItem item);

        /// <summary>
        /// Get categories
        /// </summary>
        IEnumerable<Category> GetCategories(IPresentableItem item);
    }
}
