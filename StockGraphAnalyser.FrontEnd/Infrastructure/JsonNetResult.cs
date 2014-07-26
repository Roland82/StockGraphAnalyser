
namespace StockGraphAnalyser.FrontEnd.Infrastructure
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    public class JsonNetResult : ActionResult
    {
        private Encoding ContentEncoding { get; set; }
        private string ContentType { get; set; }      
        private JsonSerializerSettings SerializerSettings { get; set; }
        private Formatting Formatting { get; set; }

        public object Data { get; set; }

        private JsonNetResult()
        {
        }

        public static JsonNetResult Create(object data)
        {
            return new JsonNetResult{
                                                     Data = data,
                                                     SerializerSettings = new JsonSerializerSettings()
                                    };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType: "application/json";

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;
            if (this.Data == null) return;

            var writer = new JsonTextWriter(response.Output) { Formatting = this.Formatting };

            var serializer = JsonSerializer.Create(this.SerializerSettings);
            serializer.Serialize(writer, this.Data);

            writer.Flush();
        }
    }
}