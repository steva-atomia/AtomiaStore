using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class ModelBinder : DefaultModelBinder
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
}
