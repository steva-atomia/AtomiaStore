using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Atomia.OrderPage.Sdk.Infrastructure
{
    public sealed class DependencyResolverModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = DependencyResolver.Current.GetService(bindingContext.ModelType);

            if (model != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
                bindingContext.ModelMetadata = metaData;
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }

    public sealed class DependencyResolverModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType.IsAbstract || modelType.IsInterface)
            {
                return new DependencyResolverModelBinder();
            }

            return null;
        }
    }
}