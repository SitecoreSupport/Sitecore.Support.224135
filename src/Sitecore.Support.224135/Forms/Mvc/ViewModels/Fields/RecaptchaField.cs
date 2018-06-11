namespace Sitecore.Support.Forms.Mvc.ViewModels.Fields
{
    using Sitecore.Form.Core.Data;
    using Sitecore.Form.Web.UI.Controls;
    using Sitecore.Forms.Mvc.Attributes;
    using Sitecore.Forms.Mvc.Interfaces;
    using Sitecore.Support.Forms.Mvc.Validators;
    using Sitecore.Forms.Mvc.ViewModels.Fields;
    using Sitecore.WFFM.Abstractions.Actions;
    using Sitecore.WFFM.Abstractions.Dependencies;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    public class RecaptchaField : SingleLineTextField, IConfiguration, IValidatableObject
    {
        public string Theme
        {
            get;
            set;
        }

        public string CaptchaType
        {
            get;
            set;
        }

        public string SiteKey
        {
            get;
            set;
        }

        public string SecretKey
        {
            get;
            set;
        }

        [TypeConverter(typeof(ProtectionSchemaAdapter))]
        public virtual ProtectionSchema RobotDetection
        {
            get;
            set;
        }

        public virtual bool IsRobot
        {
            get
            {
                return DependenciesManager.AnalyticsTracker.IsRobot;
            }
        }

        [RequestFormValue("g-recaptcha-response"), RecaptchaResponseValidator(ParameterName = "RecaptchaValidatorError")]
        public override string Value
        {
            get;
            set;
        }

        public RecaptchaField()
        {
            this.Theme = "light";
            this.CaptchaType = "image";
        }

        public override ControlResult GetResult()
        {
            return new ControlResult(base.FieldItemId, this.Name, this.Value, null, true);
        }

        public override void Initialize()
        {
            this.SiteKey = (this.GetAppSetting("RecaptchaPublicKey") ?? this.GetSitecoreSetting("WFM.RecaptchaSiteKey", null));
            this.SecretKey = (this.GetAppSetting("RecaptchaPrivateKey") ?? this.GetSitecoreSetting("WFM.RecaptchaSecretKey", null));
            base.Visible = (this.RobotDetection == null || !this.RobotDetection.Enabled);
        }

        public override void SetValueFromQuery(string valueFromQuery)
        {
        }

        public virtual string GetAppSetting(string key)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNullOrEmpty(key, "key");
            return ConfigurationManager.AppSettings[key];
        }

        public virtual string GetSitecoreSetting(string key, string defaultValue)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNullOrEmpty(key, "key");
            return Sitecore.Configuration.Settings.GetSetting(key, defaultValue);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (base.Visible || this.RobotDetection == null || !this.RobotDetection.Enabled)
            {
                return new ValidationResult[]
                {
                    ValidationResult.Success
                };
            }
            Sitecore.Data.ID iD = new Sitecore.Data.ID(base.FormId);
            if (this.RobotDetection != null && this.RobotDetection.Session.Enabled)
            {
                this.RobotDetection.AddSubmitToSession(iD);
            }
            if (this.RobotDetection != null && this.RobotDetection.Server.Enabled)
            {
                this.RobotDetection.AddSubmitToServer(iD);
            }
            bool isRobot = this.IsRobot;
            bool flag = false;
            bool flag2 = false;
            if (this.RobotDetection.Session.Enabled)
            {
                flag = this.RobotDetection.IsSessionThresholdExceeded(iD);
            }
            if (this.RobotDetection.Server.Enabled)
            {
                flag2 = this.RobotDetection.IsServerThresholdExceeded(iD);
            }
            if (isRobot || flag || flag2)
            {
                base.Visible = true;
                return new ValidationResult[]
                {
                    new ValidationResult("You've been treated as a robot. Please enter the captcha to proceed")
                };
            }
            return new ValidationResult[]
            {
                ValidationResult.Success
            };
        }
    }
}
