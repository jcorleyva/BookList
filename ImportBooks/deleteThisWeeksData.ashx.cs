using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for deleteThisWeeksData
    /// </summary>
    public class deleteThisWeeksData :IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            if (0 == BooksDataAccessLayer.DeleteWeek(context.Request.Params["week"], context.Request.Params["areyousure"])) {
                context.Response.ContentType = "image/gif";
                context.Response.BinaryWrite(ClearGif.Bytes);
            } else {
                context.Response.Write("Did not get success result from validate stored procedure");
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}