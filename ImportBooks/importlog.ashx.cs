using System.Web;
using System;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for importlog
    /// </summary>
    public class ImportLog :IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/javascript";
            try {
                context.Response.Write(GetImportLog(context.Request["callback"]));
            } catch (Exception e) {
                context.Response.Write(SimpleJson.ExceptionJson(e).Finish());
            }
        }

        public static string GetImportLog(string callback) {
            var json= new SimpleJson();
            json.Callback= callback;
            json.Start("{", "}");
            json.EmitQuoted("importLog");
            json.Emit(":");
            BooksDataAccessLayer.WriteImportLog(json);
            return json.Finish();
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}