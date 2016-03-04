using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// VPS slider controller
    /// </summary>
    public sealed class VpsSliderController : Controller
    {
        /// <summary>
        /// VPS sliders configuration
        /// </summary>
        /// <remarks>The view is expected to call <see cref="GetData"/> to get VPS sliders configuration.</remarks>
        [HttpGet]
        public JsonResult GetData()
        {
            var model = DependencyResolver.Current.GetService<VpsSliderModel>();

            model.UseSliders = false;
            if (ConfigurationManager.AppSettings["UseSliders"] != null)
            {
                bool useSliders;
                if (Boolean.TryParse(ConfigurationManager.AppSettings["UseSliders"].ToString().ToLower(), out useSliders))
                {
                    model.UseSliders = useSliders;
                }
            }
            if (ConfigurationManager.AppSettings["SliderConfig"] != null)
            {
                model.SliderConfig = ConfigurationManager.AppSettings["SliderConfig"].ToString().Replace("\"","\\\"");
            }
            model.VpsPriceDecimalPlaces = 4;
            if (ConfigurationManager.AppSettings["VpsPriceDecimalPlaces"] != null)
            {
                int vpsPriceDecimalPrices = 4;
                if (Int32.TryParse(ConfigurationManager.AppSettings["VpsPriceDecimalPlaces"].ToString(), out vpsPriceDecimalPrices))
                {
                    model.VpsPriceDecimalPlaces = vpsPriceDecimalPrices;
                }
            }

            return JsonEnvelope.Success(model);
        }
    }
}
