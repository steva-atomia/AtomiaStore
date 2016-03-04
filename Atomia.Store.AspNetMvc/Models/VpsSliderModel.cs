namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Default model for VPS sliders JSON API (VpsSliderController::GetData)
    /// </summary>
    public class VpsSliderModel
    {
        /// <summary>
        /// Use sliders
        /// </summary>
        public virtual bool UseSliders { get; set; }

        /// <summary>
        /// Slider configuration
        /// </summary>
        public virtual string SliderConfig { get; set; }

        /// <summary>
        /// Decimal places for VPS prices
        /// </summary>
        public virtual int VpsPriceDecimalPlaces { get; set; }
    }
}
