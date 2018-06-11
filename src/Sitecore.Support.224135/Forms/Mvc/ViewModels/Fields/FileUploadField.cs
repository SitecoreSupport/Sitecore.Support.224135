namespace Sitecore.Support.Forms.Mvc.ViewModels.Fields
{
    using Sitecore.Forms.Mvc.ViewModels;
    using Sitecore.WFFM.Abstractions.Actions;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Web;
    public class FileUploadField : ValuedFieldViewModel<HttpPostedFileBase>
    {
        [DefaultValue("/sitecore/media library")]
        public string UploadTo
        {
            get;
            set;
        }

        [DataType(DataType.Upload)]
        public override HttpPostedFileBase Value
        {
            get;
            set;
        }

        public override string ResultParameters
        {
            get
            {
                return "medialink";
            }
        }

        public FileUploadField()
        {
            if (string.IsNullOrEmpty(this.UploadTo))
            {
                this.UploadTo = "/sitecore/media library";
            }
        }

        public override ControlResult GetResult()
        {
            HttpPostedFileBase value = this.Value;
            if (value != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                value.InputStream.CopyTo(memoryStream);
                return new ControlResult(base.FieldItemId, this.Name, new PostedFile(memoryStream.ToArray(), value.FileName, this.UploadTo), this.ResultParameters, false);
            }
            return new ControlResult(base.FieldItemId, this.Name, null, this.ResultParameters, false);
        }

        public override void SetValueFromQuery(string valueFromQuery)
        {
        }
    }
}
