using System;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
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