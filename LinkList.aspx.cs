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
    public partial class LinkList : System.Web.UI.Page
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
        public static List<USAToday.Booklist.Business.Link> GetLinks()
        {
            using (var data = new USATodayBookListEntities())
            {
                var links = data.Links;
                return links.ToList<USAToday.Booklist.Business.Link>();

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
            
            if (!IsPostBack )
            {
                // Get the links
                List<USAToday.Booklist.Business.Link> links =  GetLinks();

                // Bind
                LinksGridView.DataSource = links;
                LinksGridView.DataBind();

                // Store in session variable
                Session.Add("Links", links);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty; 

            }

            // Set up the events
            LinksGridView.PageIndexChanging += LinkGridView_PageIndexChanging;
            LinksGridView.Sorting += LinkGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the LinkGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void LinkGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Get the list of Links from the session 
            List<USAToday.Booklist.Business.Link> links = default(List<USAToday.Booklist.Business.Link>);
            links = Session["Links"] as List<USAToday.Booklist.Business.Link>;
            // Set the index 
            LinksGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            LinksGridView.DataSource = links;
            LinksGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the LinkGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void LinkGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of Links from the session 
            List<USAToday.Booklist.Business.Link> links = default(List<USAToday.Booklist.Business.Link>);
            links = Session["Links"] as List<USAToday.Booklist.Business.Link>;
            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }
            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "URL":
                    links = Sort(links,
                                        link => link.URL);
                    break;
                case "LinkDesc":
                    links = Sort(links,
                                        link => link.LinkDesc);
                    break;
            }
            LastSortKey = e.SortExpression;
            // Rebind 
            LinksGridView.DataSource = links;
            LinksGridView.DataBind();
            // Store in a session variable 
            Session.Add("Links", links);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Link> Sort(List<USAToday.Booklist.Business.Link> list,
                Func<USAToday.Booklist.Business.Link, string> sortKey)
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
            Response.Redirect("Link.aspx?ID=0");
        }
    }
    
}