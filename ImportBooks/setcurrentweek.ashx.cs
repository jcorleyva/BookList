using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for setweek
    /// </summary>
    public class SetCurrentWeek :IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            var dt= context.Request["monday"];
            DateTime date;
            if (DateTime.TryParse(dt, out date) && 0 == BooksDataAccessLayer.SetCurrentWeek(dt)) {
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
