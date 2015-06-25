using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonApp;
using System.Configuration;
using System.Data;

namespace USATodayBookList.AppCode
{
    public class DABooklist : DACBase
    {
        public DABooklist()
        {
            base.ConnectionString = ConfigurationManager.ConnectionStrings["USATodayBookList"].ConnectionString;
        }

        public DataTable GetISBNWorkingList(int RecordCount, string ISBN, string Title, string Author, string Mode, string SortExpression)
        {
            return base.ExecuteStoredProcedure("GetISBNWorkingList", RecordCount, ISBN, Title, Author, Mode, SortExpression);
        }

        public void Archive()
        {
            base.ExecuteStoredProcedure("Archive");
        }
    }
}