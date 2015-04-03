using System.Web;

namespace USATodayBookList.ImportBooks {
    /// <summary>
    /// Summary description for import
    /// </summary>
    public class import :IHttpHandler {
        public void ProcessRequest(HttpContext context) {
            if (0 == BooksDataAccessLayer.Import()) {
                context.Response.ContentType = "image/gif";
                context.Response.BinaryWrite(ClearGif.Bytes);
            } else {
                context.Response.Write("Did not get success result from import stored procedure");
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}