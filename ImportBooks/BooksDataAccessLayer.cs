using System.Web;

namespace USATodayBookList.ImportBooks {
    public class BooksDataAccessLayer :DataAccessLayer{

        public static int? DeleteWeek(string monday, string sure) {
            var cmd=SqlCommand("DeleteWeek");
            cmd.Parameters.AddWithValue("@monday", monday);
            cmd.Parameters.AddWithValue("@annoyingareyousureparameter", sure);
            return Execute(cmd);
        }

        public static int? Import() {
            return Execute(SqlCommand("RunImportScript"));
        }

        public static int? Validate() {
            return Execute(SqlCommand("ValidateImportFiles"));
        }

        public static int? SetCurrentWeek(string monday) {
            var cmd= SqlCommand("SetCurrentWeek");
            cmd.Parameters.AddWithValue("@monday", monday);
            return Execute(cmd);
        }

        public static void WriteImportLog(SimpleJson json) {
            WriteTable(json, SqlCommand("GetImportLog"));
        }
        /*
        public static void WriteImportErrorLog(SimpleJson json) {
            WriteTable(json, SqlCommand("GetImportErrorLog"));
        }
        */
        public static void WriteMondays(SimpleJson json) {
            WriteTable(json, SqlCommand("GetMondays"));
        }

        public static void WriteProviders(SimpleJson json) {
            WriteTable(json, SqlCommand("GetProviders"));
        }

    }
}