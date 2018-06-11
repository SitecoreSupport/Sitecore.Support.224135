namespace Sitecore.Support.Forms.Mvc.Validators
{
    using Newtonsoft.Json;
    using Sitecore.Forms.Mvc.Interfaces;
    using Sitecore.Forms.Mvc.Validators;
    using Sitecore.Support.Forms.Mvc.ViewModels.Fields;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false), DisplayName("TITLE_ERROR_MESSAGE_RECAPTCHA")]
    public class RecaptchaResponseValidatorAttribute : Sitecore.Forms.Mvc.Validators.RecaptchaResponseValidatorAttribute
    {
        protected override ValidationResult ValidateFieldValue(IViewModel model, object value, ValidationContext validationContext)
        {
            Sitecore.Diagnostics.Assert.IsNotNull(model, "model");
            if (value == null)
            {
                return new ValidationResult(this.FormatError(model, new object[]
                {
                    "Invalid captcha text"
                }));
            }
            WebClient webClient = new WebClient();
            string value2 = webClient.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", ((RecaptchaField)model).SecretKey, value));
            ReCaptchaResponse reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(value2);
            if (reCaptchaResponse.Success)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(this.FormatError(model, reCaptchaResponse.ErrorCodes.ToArray()));
        }
    }
}
