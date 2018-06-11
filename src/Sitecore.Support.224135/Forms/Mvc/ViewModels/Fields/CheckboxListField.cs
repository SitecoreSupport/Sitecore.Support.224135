namespace Sitecore.Support.Forms.Mvc.ViewModels.Fields
{
    using Sitecore.Forms.Core.Data;
    using Sitecore.Forms.Mvc.Attributes;
    using Sitecore.Forms.Mvc.Interfaces;
    using Sitecore.Forms.Mvc.TypeConverters;
    using Sitecore.Forms.Mvc.ViewModels;
    using Sitecore.WFFM.Abstractions.Actions;
    using Sitecore.WFFM.Abstractions.Dependencies;
    using Sitecore.WFFM.Abstractions.Shared;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;
    public class CheckboxListField : ValuedFieldViewModel<List<string>>, ISelectList
    {
        private readonly ListFieldValueFormatter listFieldValueFormatter;

        [TypeConverter(typeof(ListSelectItemsConverter))]
        public List<SelectListItem> Items
        {
            get;
            set;
        }

        [ParameterName("selectedvalue"), TypeConverter(typeof(ListItemsConverter))]
        public override List<string> Value
        {
            get;
            set;
        }

        public CheckboxListField() : this(new ListFieldValueFormatter(DependenciesManager.Resolve<ISettings>()))
        {
        }

        public CheckboxListField(ListFieldValueFormatter listFieldValueFormatter)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull(listFieldValueFormatter, "listFieldValueFormatter");
            this.listFieldValueFormatter = listFieldValueFormatter;
            this.Items = new List<SelectListItem>();
        }

        public override ControlResult GetResult()
        {
            return this.listFieldValueFormatter.GetFormattedResult(base.FieldItemId, this.Name, this.Value, this.Value);
        }

        public override void Initialize()
        {
            List<string> selectedValues = this.Value;
            if (this.Items == null)
            {
                this.Items = new List<SelectListItem>();
            }
            if (selectedValues != null)
            {
                this.Items.ForEach(delegate (SelectListItem x)
                {
                    x.Selected = selectedValues.Contains(x.Value);
                });
            }
        }

        public override void SetValueFromQuery(string valueFromQuery)
        {
        }
    }
}
