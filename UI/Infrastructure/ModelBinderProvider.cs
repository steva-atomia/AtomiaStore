using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.UI.Infrastructure
{
    public sealed class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType.IsAbstract || modelType.IsInterface)
            {
                return new ModelBinder();
            }

            return null;
        }
    }
}