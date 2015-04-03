using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList
{
    public partial class ConfirmImport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ProviderData> data = new List<ProviderData>();

            data.Add(new ProviderData("Barnes & Noble Online", "xls", "qry_F_USATODAY_INTERNET_Top_500.xls"));
            data.Add(new ProviderData("Amazon ebooks", "xls", "USA TODAY_we.xls"));
            data.Add(new ProviderData("Amazon", "xls", "Top500_ShippedBooks.xls"));
            data.Add(new ProviderData("Walmart", "txt", "WMUSATODAY.txt"));

            ConfirmGridView.DataSource = data;
            ConfirmGridView.DataBind();
        }
    }
}