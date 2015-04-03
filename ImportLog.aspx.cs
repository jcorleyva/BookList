using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class ImportLog : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets the last sort key.
        /// </summary>
        /// <value>The last sort key.</value>
        /// <remarks></remarks>
        public string LastSortKey
        {
            get { return ViewState["LastSortKey"].ToString(); }
            set { ViewState["LastSortKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the last sort direction.
        /// </summary>
        /// <value>The last sort direction.</value>
        /// <remarks></remarks>
        public string LastSortDirection
        {
            get { return ViewState["LastSortDirection"].ToString(); }
            set { ViewState["LastSortDirection"] = value; }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                this.ProviderDropDownList.DataSource = GetProviders();
                this.ProviderDropDownList.DataBind();

                var logs = GetImportLog();
                LogsGridView.DataSource = logs;
                LogsGridView.DataBind();

                Session.Add("ImportLogs", logs);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty; 
            }

            // Set up the events
            LogsGridView.PageIndexChanging += LogsGridView_PageIndexChanging;
            LogsGridView.Sorting += LogsGridView_Sorting;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Provider> GetProviders()
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                var providers = data.Providers.OrderBy(p => p.ProviderName);
                return providers.ToList<USAToday.Booklist.Business.Provider>();
            }
        }

        /// <summary>
        /// Gets the import log.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ImportLogView> GetImportLog()
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                var importLog = data.ImportLogViews.OrderByDescending(p => p.DateCompleted);
                return importLog.ToList<USAToday.Booklist.Business.ImportLogView>();
            }
        }

        /// <summary>
        /// Gets the import log.
        /// </summary>
        /// <param name="ProviderID">The provider ID.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ImportLogView> GetImportLog(int ProviderID)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                var importLog = data.ImportLogViews.Where(p => p.ProviderID == ProviderID).OrderByDescending(p => p.DateCompleted);
                return importLog.ToList<USAToday.Booklist.Business.ImportLogView>();
            }
        }

        /// <summary>
        /// Filters the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void FilterClick(object sender, EventArgs e)
        {
            int providerID = int.Parse(this.ProviderDropDownList.SelectedValue);

            var logs = GetImportLog(providerID);
            LogsGridView.DataSource = logs;
            LogsGridView.DataBind();

            Session.Add("ImportLogs", logs);
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the LogsGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void LogsGridView_PageIndexChanging(object sender,  GridViewPageEventArgs e)
        {
            // Get the list of Logs from the session 
            List<USAToday.Booklist.Business.ImportLogView> logs = default(List<USAToday.Booklist.Business.ImportLogView>);
            logs = Session["ImportLogs"] as List<USAToday.Booklist.Business.ImportLogView>;
            
            // Set the index 
            LogsGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            LogsGridView.DataSource = logs;
            LogsGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the LogsGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void LogsGridView_Sorting(object sender,  GridViewSortEventArgs e)
        {
            // Get the list of Links from the session 
            List<USAToday.Booklist.Business.ImportLogView> logs = default(List<USAToday.Booklist.Business.ImportLogView>);
            logs = Session["ImportLogs"] as List<USAToday.Booklist.Business.ImportLogView>;
            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }
            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ProviderName":
                    logs = Sort(logs, log => log.ProviderName);
                    break;
                case "DateCompleted":
                    logs = Sort(logs, log => log.DateCompleted.ToShortDateString());
                    break;
                case "RowsImported":
                    logs = Sort(logs, log => log.RowsImported);
                    break;
                case "Messages":
                    logs = Sort(logs, log => log.Messages);
                    break;
               
            }
            LastSortKey = e.SortExpression;
            
            // Rebind 
            LogsGridView.DataSource = logs;
            LogsGridView.DataBind();
            
            // Store in a session variable 
            Session.Add("ImportLogs", logs);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ImportLogView> Sort(List<USAToday.Booklist.Business.ImportLogView> list,
                Func<USAToday.Booklist.Business.ImportLogView, string> sortKey)
        {
            if (LastSortDirection == "ASC")
            {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            }
            else
            {
                list = list.OrderBy(sortKey).ToList();
                LastSortDirection = "ASC";
            }
            return list;
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ImportLogView> Sort(List<USAToday.Booklist.Business.ImportLogView> list,
               Func<USAToday.Booklist.Business.ImportLogView, int> sortKey)
        {
            if (LastSortDirection == "ASC")
            {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            }
            else
            {
                list = list.OrderBy(sortKey).ToList();
                LastSortDirection = "ASC";
            }
            return list;
        } 

    }
}