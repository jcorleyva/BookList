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
    public partial class ISBNList : System.Web.UI.Page
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
        /// Gets the book ISB ns.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.BookISBN> GetBookISBNs()
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbns = data.BookISBNs;
                return isbns.ToList<USAToday.Booklist.Business.BookISBN>();
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
                List<USAToday.Booklist.Business.BookISBN> isbns = new List<BookISBN>();

                // Bind
                ISBNGridView.DataSource = isbns;
                ISBNGridView.DataBind();

                // Store in session variable
                Session.Add("ISBNs", isbns);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            ISBNGridView.PageIndexChanging += ISBNGridView_PageIndexChanging;
            ISBNGridView.Sorting += ISBNGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ISBNGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Get the list of isbns from the session 
            List<USAToday.Booklist.Business.BookISBN> isbns = default(List<USAToday.Booklist.Business.BookISBN>);
            isbns = Session["ISBNs"] as List<USAToday.Booklist.Business.BookISBN>;

            // Set the index 
            ISBNGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            ISBNGridView.DataSource = isbns;
            ISBNGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ISBNGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of isbns from the session 
            List<USAToday.Booklist.Business.BookISBN> isbns = default(List<USAToday.Booklist.Business.BookISBN>);
            isbns = Session["ISBNs"] as List<USAToday.Booklist.Business.BookISBN>;
            

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ISBN":
                    isbns = Sort(isbns, i => i.ISBN);
                    break;
                case "Title":
                    isbns = Sort(isbns, i => i.Title);
                    break;
                case "Author":
                    isbns = Sort(isbns, i => i.Author);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            ISBNGridView.DataSource = isbns;
            ISBNGridView.DataBind();
            // Store in a session variable 
            Session.Add("ISBNs", isbns);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookISBN> Sort(List<USAToday.Booklist.Business.BookISBN> list,
                Func<USAToday.Booklist.Business.BookISBN, string> sortKey)
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
        protected virtual void Filter_Click( object sender, EventArgs e)
        {
            if (this.ISBNTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var isbns = from i in data.BookISBNs
                                where i.ISBN.StartsWith(this.ISBNTextBox.Text.Trim())
                                select i;
                    if (isbns.Count() == 0)
                        ISBNGridView.AllowSorting = false;

                    ISBNGridView.DataSource = isbns;
                    ISBNGridView.DataBind();
                }
            }
        }
        /// <summary>
        /// BTNs the add click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAddClick(object sender, EventArgs e)
        {
            Response.Redirect("ISBN.aspx?Id=0");
        }

        /// <summary>
        /// Handles the Click event of the ClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilter_Click(object sender, EventArgs e)
        {
            this.ISBNTextBox.Text = "";
            Response.Redirect("ISBNList.aspx");
        }
    }
}