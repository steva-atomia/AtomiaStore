using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atomia.OrderPage.Sdk.Infrastructure
{
    public sealed class DependencyResolverModelProvider : IModelProvider
    {
        public TViewModel Create<TViewModel>()
        {
            return DependencyResolver.Current.GetService<TViewModel>();
        }
    }
}