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
    public partial class PublisherList : System.Web.UI.Page
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
        /// Gets the publishers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Publisher> GetPublishers()
        {
            using (var data = new USATodayBookListEntities())
            {
                var publishers = data.Publishers;
                return publishers.ToList<USAToday.Booklist.Business.Publisher>();
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
                // Get publishers
                List<USAToday.Booklist.Business.Publisher> publishers = GetPublishers();

                if (publishers.Count() == 0)
                    PublisherGridView.AllowSorting = false;

                // Bind
                PublisherGridView.DataSource = publishers;
                PublisherGridView.DataBind();

                // Store in session variable
                Session.Add("Publishers", publishers);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            PublisherGridView.Sorting += PublisherGridView_Sorting;
            
            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
            
        }

        /// <summary>
        /// Handles the Sorting event of the PublisherGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void PublisherGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of publishers from the session 
            List<USAToday.Booklist.Business.Publisher> publishers = default(List<USAToday.Booklist.Business.Publisher>);
            publishers = Session["Publishers"] as List<USAToday.Booklist.Business.Publisher>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "PublisherName":
                    publishers = Sort(publishers, p => p.PublisherName);
                    break;
                case "ContactName":
                    publishers = Sort(publishers, p => p.ContactName);
                    break;
                case "Phone":
                    publishers = Sort(publishers, p => p.Phone);
                    break;
                case "Email":
                    publishers = Sort(publishers, p => p.Email);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            PublisherGridView.DataSource = publishers;
            PublisherGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Publisher> Sort(List<USAToday.Booklist.Business.Publisher> list,
                Func<USAToday.Booklist.Business.Publisher, string> sortKey)
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
        /// Handles the Click event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void Filter_Click(object sender, EventArgs e)
        {
            if (this.PublisherTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var a = from p in data.Publishers
                            where p.PublisherName.Contains(this.PublisherTextBox.Text.Trim())
                            select p;
                    if (a.Count() == 0)
                        PublisherGridView.AllowSorting = false;
                    else
                        PublisherGridView.AllowSorting = true;

                    List<USAToday.Booklist.Business.Publisher> publishers = a.ToList<USAToday.Booklist.Business.Publisher>();
                    PublisherGridView.DataSource = publishers;
                    PublisherGridView.DataBind();

                    Session.Add("Publishers", publishers);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("PublisherList.aspx");
        }
        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Publisher.aspx?Id=0");
        }

    }
}