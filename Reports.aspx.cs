using System;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
//using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Data;
using System.Net;
using System.Text;
using USATodayBookList.ReportingService;

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

		protected void btnPublish_Click(object sender, EventArgs e)
		{
			var message = "";

			try
			{
				var reportName = ((Button)sender).CommandArgument;
				var rs = new ReportExecutionService
				{
					Credentials = CredentialCache.DefaultNetworkCredentials
				};

				// Render arguments
				const string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
				string reportPath = "/USATodayBookList/" + reportName;
				string format = ConfigurationManager.AppSettings["ReportFileFormat"];
				if (reportName == "Top150")
				{
					format = ConfigurationManager.AppSettings["Top150ReportFileFormat"];
				}

				var execHeader = new ExecutionHeader();

				rs.ExecutionHeaderValue = execHeader;
				rs.LoadReport(reportPath, null);

				string extension;
				string encoding;
				string mimeType;
				Warning[] warnings;
				string[] streamIDs;
				byte[] result = rs.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);

				// JMC - Bytes mentioned in original comment below are a byte order mark for a UTF8 document.
				// Reporting services is putting three junk bytes at the beginning 239,187,191 (Don't know where it is coming from)
				// I'm utilizing them to store needed characters
				if (result[0] == 239 && result[1] == 187 && result[2] == 191)
				{
					result[0] = 126; // ~
					result[1] = 13;  // carriage return
					result[2] = 10; // newline feed
				}

				Byte[] copyResult = new Byte[result.Length];
				int bytesToRemove = 0;
				int j = 0;

				// Reporting Services encapsulates strings with double quotes with a double quote as 
				// a qualifier. We remove these extra double quotes here.
				for (int i = 0; i < result.Length; i++)
				{
					if (result[i] != 34 || (result[i] == 34 && result[i + 1] == 34))
					{
						copyResult[j] = result[i];
						j++;
					}
					else
						bytesToRemove++;
				}
				Array.Resize(ref copyResult, copyResult.Length - bytesToRemove);

				int publishCount = 1;
				if (reportName == "Top50")
					publishCount = 2;

				for (int i = 1; i <= publishCount; i++)
				{
					reportName = (i == 2 && reportName == "Top50") ? "GNStop50" : reportName;
					string publishDir = ConfigurationManager.AppSettings["ReportPublishingPath"];
					string publishPath = publishDir + reportName + "_" + DateTime.Now.ToString("MMddyy") + "." + format.ToLower();
					var fileHeader = Encoding.ASCII.GetBytes("~" + Environment.NewLine + "DU:" + reportName + Environment.NewLine);

					if (!Directory.Exists(publishDir))
					{
						Directory.CreateDirectory(publishDir);
					}

					FileStream stream = File.Create(publishPath, copyResult.Length);
					stream.Write(fileHeader, 0, fileHeader.Length);
					stream.Write(copyResult, 0, copyResult.Length);
					stream.Close();

					message = message + "Report " + reportName + " published successfully.<br/>";
				}
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch //(Exception ex)
			{
				//Logger.WriteExceptionEntry(ex);
				//message = "There was an error in publishing this report." + ex.Message;
			}
		}

        protected void ReportsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((DataRowView)e.Row.DataItem).Row["ButtonText"].ToString() != "")
                    (e.Row.Cells[1].Controls[1]).Visible = true;
                else
                    e.Row.Cells[1].Controls.Clear();
            }
        }
    }
}