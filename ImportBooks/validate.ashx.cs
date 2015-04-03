using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for validate
    /// </summary>
    public class validate :IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            if (0 == BooksDataAccessLayer.Validate()) {
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