using Sitecore.Form.Core.Data;
using Sitecore.Forms.Mvc.Html;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.ViewModels;
using Sitecore.Support.Forms.Mvc.ViewModels.Fields;
using Sitecore.Mvc;
namespace Sitecore.Support.Forms.Mvc.Html
{
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Recaptcha(this HtmlHelper helper, IViewModel model = null)
        {
            RecaptchaField recaptchaField = (model ?? helper.ViewData.Model) as RecaptchaField;
            Sitecore.Diagnostics.Assert.IsNotNull(recaptchaField, "view");
            ProtectionSchema robotDetection = recaptchaField.RobotDetection;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(helper.OpenFormField(recaptchaField, robotDetection == null || !robotDetection.Enabled));
            stringBuilder.Append(helper.Hidden("Value"));
            if (recaptchaField.Visible)
            {
                TagBuilder tagBuilder = new TagBuilder("div");
                tagBuilder.AddCssClass("g-recaptcha");
                tagBuilder.MergeAttribute("data-sitekey", recaptchaField.SiteKey);
                tagBuilder.MergeAttribute("data-theme", recaptchaField.Theme);
                tagBuilder.MergeAttribute("data-type", recaptchaField.CaptchaType);
                TagBuilder tagBuilder2 = new TagBuilder("script");
                tagBuilder2.MergeAttribute("src", "https://www.google.com/recaptcha/api.js");
                stringBuilder.Append(tagBuilder);
                stringBuilder.Append(tagBuilder2);
            }
            stringBuilder.Append(helper.CloseFormField(recaptchaField, true));
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
    }
}
