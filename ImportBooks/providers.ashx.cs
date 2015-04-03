using System;
using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for providers
    /// </summary>
    public class providers :IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/javascript";
            var json= new SimpleJson();
            json.Callback= context.Request["callback"];
            json.Start("{", "}");
            json.EmitQuoted("providers");
            json.Emit(":");
            try {
                BooksDataAccessLayer.WriteProviders(json);
            } catch (Exception e) {
                json= SimpleJson.ExceptionJson(e);
            }
            context.Response.Write(json.Finish());
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}