using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using CommonApp;

namespace USATodayBookList.AppCode
{
    public class DAL : DACBase
    {
        public void UpdateBookCategory(int bookId, string assignedCategoryIds, string unAssignedCategoryIds)
        {
            base.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["USATodayBookList"].ConnectionString;
            base.ExecuteStoredProcedure("AddCategoriesToBook", bookId, assignedCategoryIds, unAssignedCategoryIds);
        }
    }
}