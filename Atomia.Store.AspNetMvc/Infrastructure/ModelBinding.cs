using System;
using System.Linq;
using System.Web.Mvc;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class AbstractModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = DependencyResolver.Current.GetService(bindingContext.ModelType);

            if (model != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
                bindingContext.ModelMetadata = metaData;
            }

            var boundModel = base.BindModel(controllerContext, bindingContext);

            return boundModel;
        }
    }

    public sealed class PaymentMethodFormBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = bindingContext.Model;

            if (model != null)
            {
                var metaData = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
                bindingContext.ModelMetadata = metaData;
            }

            var boundModel = base.BindModel(controllerContext, bindingContext);

            return boundModel;
        }
    }

    public sealed class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(PaymentMethodForm))
            {
                return new PaymentMethodFormBinder();
            }
            else if (modelType.IsAbstract || modelType.IsInterface)
            {
                return new AbstractModelBinder();
            }

            return null;
        }
    }
}
