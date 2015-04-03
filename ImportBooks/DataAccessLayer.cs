using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace USATodayBookList.ImportBooks {
    public abstract class DataAccessLayer {
        protected static string _ConnectionString= ConnectionString();

        protected static string ConnectionString() {
            var key= ConfigurationManager.AppSettings["ImportConnectionString"];
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        protected static SqlCommand SqlCommand(string storedProcedureName) {
            var sqlConnection = new SqlConnection(_ConnectionString);
            var cmd = new SqlCommand(storedProcedureName, sqlConnection);
            cmd.CommandTimeout = 225;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        protected static int? Execute(SqlCommand cmd) {
            using (cmd) {
                var return_value= cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                return_value.Direction= ParameterDirection.ReturnValue;
                using (var con= cmd.Connection) {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return return_value.Value as int?;
                }
            }
        }

        protected static void WriteTable(SimpleJson json, SqlCommand cmd) {
            json.Start("[", "]");
            using (cmd) {
                using (var con= cmd.Connection) {
                    con.Open();
                    var dr= cmd.ExecuteReader();
                    while (dr.Read()) {
                        json.EmitSeparator(",");
                        json.Start("{","}");
                        for (int j= 0; j < dr.FieldCount; j++) {
                            var key= dr.GetName(j);
                            json.EmitSeparator(",");
                            json.EmitQuoted(key);
                            json.Emit(":");
                            json.EmitQuoted(dr.IsDBNull(j) ?"" :dr.GetString(j));
                        }
                        json.End();
                    }
                }
            }
            json.End();
        }
    }
}