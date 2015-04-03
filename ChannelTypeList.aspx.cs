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
    public partial class ChannelTypeList : System.Web.UI.Page
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
        /// Gets the links.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.ChannelType> GetLinks()
        {
            using (var data = new USATodayBookListEntities())
            {
                var links = data.ChannelTypes;
                return links.ToList<USAToday.Booklist.Business.ChannelType>();

            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the links
                List<USAToday.Booklist.Business.ChannelType> channelTypes = GetLinks();

                // Bind
                ChannelTypeGridView.DataSource = channelTypes;
                ChannelTypeGridView.DataBind();

                // Store in session variable
                Session.Add("ChannelTypes", channelTypes);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            ChannelTypeGridView.PageIndexChanging += ChannelTypeGridView_PageIndexChanging;
            ChannelTypeGridView.Sorting += ChannelTypeGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the ChannelTypeGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ChannelTypeGridView_PageIndexChanging(object sender,  GridViewPageEventArgs e)
        {
            // Get the list of channel Types from the session 
            List<USAToday.Booklist.Business.ChannelType> channelTypes = default(List<USAToday.Booklist.Business.ChannelType>);
            channelTypes = Session["ChannelTypes"] as List<USAToday.Booklist.Business.ChannelType>;
            // Set the index 
            ChannelTypeGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            ChannelTypeGridView.DataSource = channelTypes;
            ChannelTypeGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the ChannelTypeGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ChannelTypeGridView_Sorting(object sender,  GridViewSortEventArgs e)
        {
            // Get the list of channel types from the session 
            List<USAToday.Booklist.Business.ChannelType> channelTypes = default(List<USAToday.Booklist.Business.ChannelType>);
            channelTypes = Session["ChannelTypes"] as List<USAToday.Booklist.Business.ChannelType>;
            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }
            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ChannelTypeDescription":
                    channelTypes = Sort(channelTypes,  channelType => channelType.ChannelTypeDescription);
                    break;
                case "WeightedSales":
                    channelTypes = Sort(channelTypes, link => link.WeightedSales.ToString());
                    break;
            }
            LastSortKey = e.SortExpression;
            // Rebind 
            ChannelTypeGridView.DataSource = channelTypes;
            ChannelTypeGridView.DataBind();
            // Store in a session variable 
            Session.Add("ChannelTypes", channelTypes);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ChannelType> Sort(List<USAToday.Booklist.Business.ChannelType> list,
                Func<USAToday.Booklist.Business.ChannelType, string> sortKey)
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
        private List<USAToday.Booklist.Business.ChannelType> Sort(List<USAToday.Booklist.Business.ChannelType> list,
                Func<USAToday.Booklist.Business.ChannelType, decimal> sortKey)
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
        /// BTNs the add click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAddClick(object sender, EventArgs e)
        {
            Response.Redirect("ChannelType.aspx?ID=0");
        }
    }

    
}