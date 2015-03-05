using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Atomia.Store.Themes.Default.HtmlHelpers
{
    public static class FormRowForHelper
    {
        public static MvcHtmlString FormRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, Boolean required)
        {
            var asterisk = required ? @"<span class=""required"">*</span>" : "";
            var label = required ? html.LabelFor(expression, labelText, new { @class = "required" }) : html.LabelFor(expression, labelText);
            var textBox = html.TextBoxFor(expression);
            var validationMessage = html.ValidationMessageFor(expression);

            return new MvcHtmlString(String.Format(@"<div class=""formrow"">
                <h5>{0}{1}</h5>
                <div class=""col2row"">{2}{3}</div>
                <br class=""clear"">
            </div>", asterisk, label, textBox, validationMessage));
        }

        public static MvcHtmlString FormRowFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, Boolean required, string dataBindAttribute)
        {
            var asterisk = required ? @"<span class=""required"">*</span>" : "";
            var label = required ? html.LabelFor(expression, labelText, new { @class = "required" }) : html.LabelFor(expression, labelText);
            var textBox = html.TextBoxFor(expression);
            var validationMessage = html.ValidationMessageFor(expression);

            return new MvcHtmlString(String.Format(@"<div class=""formrow"">
                <h5>{0}{1}</h5>
                <div class=""col2row"" data-bind=""{2}"">{3}{4}</div>
                <br class=""clear"">
            </div>", asterisk, label, dataBindAttribute, textBox, validationMessage));
        }
    }
}
