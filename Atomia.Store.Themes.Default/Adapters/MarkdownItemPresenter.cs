using Atomia.Store.Core;
using MarkdownSharp;
using System;

namespace Atomia.Store.Themes.Default.Adapters
{
    /// <summary>
    /// <see cref="Atomia.Store.Core.IItemPresenter"/> that handles Markdown descriptions.
    /// </summary>
    public sealed class MarkdownItemPresenter : IItemPresenter
    {
        private readonly IItemPresenter itemPresenter;

        public MarkdownItemPresenter(IItemPresenter itemPresenter)
        {
            if (itemPresenter == null)
            {
                throw new ArgumentNullException("itemPresenter");
            }

            this.itemPresenter = itemPresenter;
        }

        public string GetName(IPresentableItem item)
        {
            return this.itemPresenter.GetName(item);
        }

        public string GetDescription(IPresentableItem item)
        {
            var description = this.itemPresenter.GetDescription(item);
            var markdown = new Markdown();
            var transformedDescription = markdown.Transform(description);

            return transformedDescription;
        }

        public string GetCategory(IPresentableItem item)
        {
            return this.itemPresenter.GetCategory(item);
        }
    }
}
