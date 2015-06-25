using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
//using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;

namespace USATodayBookList
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ReportDisplayName");
                dt.Columns.Add("ReportUrl");
                dt.Columns.Add("ReportName");
                dt.Columns.Add("ButtonText");
                DataRow dr = dt.NewRow();
                dr["ReportDisplayName"] = "Snapshot Top 5";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fSnapShot";
                dr["ReportName"] = "SnapShot";
                dr["ButtonText"] = "Send to NewsGate Wire";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ReportDisplayName"] = "Top 50";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fTop50";
                dr["ReportName"] = "Top50";
                dr["ButtonText"] = "Send to Gannett NS/NewsGate Wire";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ReportDisplayName"] = "BizBooks";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fBizBooks";
                dr["ReportName"] = "BizBooks";
                dr["ButtonText"] = "Send to Finger Post";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ReportDisplayName"] = "Top 150";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fTop150";
                dr["ReportName"] = "Top150";
                dr["ButtonText"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ReportDisplayName"] = "Top 150 Buzz";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fTop150Buzz";
                dr["ReportName"] = "Top150Buzz";
                dr["ButtonText"] = "";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ReportDisplayName"] = "New This Week";
                dr["ReportUrl"] = "http://usat-vocprddb08/Reports/Pages/Report.aspx?ItemPath=%2fUSATodayBookList%2fNewThisWeek";
                dr["ReportName"] = "NewThisWeek";
                dr["ButtonText"] = "";
                dt.Rows.Add(dr);


                ReportsGridView.DataSource = dt;
                ReportsGridView.DataBind();
            }
        }

        private static string GetDataViaHttp(string Url)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    return client.DownloadString(Url);
                }
            }
            catch
            {
                return null;
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            string reportName = ((Button)sender).CommandArgument;
            string handlerUrl = "http://localhost/BookListReportPublisher/handler.ashx?ReportName=" + reportName;
            string message = GetDataViaHttp(handlerUrl);
            //divMessage.InnerHtml = (message == null || message == "") ? "Report could not be generated.<br/>Please contact your administrator." : message;            
        }

        protected void ReportsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((System.Data.DataRowView)e.Row.DataItem).Row["ButtonText"].ToString() != "")
                    ((Button)e.Row.Cells[1].Controls[1]).Visible = true;
                else
                    e.Row.Cells[1].Controls.Clear();
            }
        }
    }
}