using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for Mondays
    /// </summary>
    public class Mondays :IHttpHandler {


        public void ProcessRequest(HttpContext context) {
            context.Response.ContentType = "text/javascript";
            try {
                context.Response.Write(GetMondays(context.Request["callback"]));
            } catch (Exception e) {
                context.Response.Write(SimpleJson.ExceptionJson(e).Finish());
            }
        }

        public static string GetMondays(string callback) {
            var json= new SimpleJson();
            json.Callback= callback;
            json.Start("{", "}");
            json.EmitQuoted("mondays");
            json.Emit(":");
            BooksDataAccessLayer.WriteMondays(json);
            return json.Finish();
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}