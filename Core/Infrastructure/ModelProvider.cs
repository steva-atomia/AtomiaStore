using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atomia.OrderPage.Core.Infrastructure
{
    public sealed class ModelProvider : IModelProvider
    {
        public TViewModel Create<TViewModel>()
        {
            return DependencyResolver.Current.GetService<TViewModel>();
        }
    }
}