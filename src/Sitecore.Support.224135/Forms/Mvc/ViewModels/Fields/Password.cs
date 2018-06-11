namespace Sitecore.Support.Forms.Mvc.ViewModels.Fields
{
    using Sitecore.Forms.Mvc.ViewModels.Fields;
    using Sitecore.WFFM.Abstractions.Actions;
    using System.ComponentModel.DataAnnotations;
    public class PasswordField : SingleLineTextField
    {
        [DataType(DataType.Password)]
        public override string Value
        {
            get;
            set;
        }

        public override ControlResult GetResult()
        {
            return new ControlResult(base.FieldItemId, this.Name, this.Value, null, true);
        }

        public override void SetValueFromQuery(string valueFromQuery)
        {
        }
    }
}
