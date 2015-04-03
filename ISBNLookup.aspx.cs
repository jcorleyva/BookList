using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;
using USATodayBookList.AppCode;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class ISBNLookup : PageBase
    {

        private int BookId { get; set; }

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
        /// Gets or sets the assigned ISB ns.
        /// </summary>
        /// <value>The assigned ISB ns.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookISBN> AssignedISBNs
        {
            get { return (List<USAToday.Booklist.Business.BookISBN>)Session["AssignedISBNs" + this.BookId.ToString()]; }
            set { Session["AssignedISBNs" + this.BookId.ToString()] = value; }
        }
        private List<USAToday.Booklist.Business.BookISBNView> AssignedISBNViews
        {
            get { return (List<USAToday.Booklist.Business.BookISBNView>)Session["AssignedISBNs" + this.BookId.ToString()]; }
            set { Session["AssignedISBNs" + this.BookId.ToString()] = value; }
        }

        /// <summary>
        /// Gets the ISB ns.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookISBN> GetISBNs()
        {
            using (var data = new USATodayBookListEntities())
            {
                string where = string.Empty;
                if (this.ISBNTextBox.Text.Trim().Length > 0)
                {
                    where += " and c.ISBN like '" + this.ISBNTextBox.Text.Trim() + "%'";
                }

                if (this.TitleTextBox.Text.Trim().Length > 0)
                {
                    where += " and c.Title like '" + this.TitleTextBox.Text.Trim().Replace("'","''") + "%'";
                }


                if (this.AuthorTextBox.Text.Trim().Length > 0)
                {
                    where += " and c.Author like '" + this.AuthorTextBox.Text.Trim().Replace("'","''") + "%'";
                }


                string sql = "Select value c from BookISBNs as c where 1=1 " + where;

                var isbns = Global.BookEntities.CreateQuery<USAToday.Booklist.Business.BookISBN>(sql);
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
            this.BookId = int.Parse(Request["BookId"]);

            if (!IsPostBack)
            {
                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <remarks></remarks>
        private void BindData()
        {
            var isbns = GetISBNs();
            if (isbns.Count() == 0)
                ISBNGridView.AllowSorting = false;
            else
                ISBNGridView.AllowSorting = true;

            ISBNGridView.DataSource = isbns;
            ISBNGridView.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the FilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void FilterButton_Click(object sender, EventArgs e)
        {
            BindData();
        }
        /// <summary>
        /// Handles the Click event of the ClearFilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilterButton_Click(object sender, EventArgs e)
        {
            ISBNTextBox.Text = "";
            TitleTextBox.Text = "";
            AuthorTextBox.Text = "";

            ISBNGridView.DataSource = new List<BookISBN>();
            ISBNGridView.DataBind();

        }

        /// <summary>
        /// Handles the PageIndexChanging event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ISBNGridView.PageIndex = e.NewPageIndex;
            BindData();

        }

        /// <summary>
        /// Handles the Sorting event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.BookISBN> isbns = default(List<USAToday.Booklist.Business.BookISBN>);
            isbns = GetISBNs();

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ISBN":
                    AssignedISBNs = Sort(isbns, isbn => isbn.ISBN);
                    break;
                case "Title":
                    AssignedISBNs = Sort(isbns, isbn => isbn.Title);
                    break;
                case "Author":
                    AssignedISBNs = Sort(isbns, isbn => isbn.Author);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            ISBNGridView.DataSource = AssignedISBNs;
            ISBNGridView.DataBind();
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
        /// Handles the RowDataBound event of the ISBNGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the First Name of Author
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = ((LinkButton)e.Row.Cells[0].Controls[0]);
                USAToday.Booklist.Business.BookISBN b = ((USAToday.Booklist.Business.BookISBN)e.Row.DataItem);
                lb.Text = b.ISBN;
                lb.CommandName = "SELECT";
                lb.CommandArgument = b.Id.ToString();

                //show Associated ISBNs in green color
                if (b.BookID != null)
                    lb.ForeColor = System.Drawing.Color.Green;
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
            {
                //Retrieve author and add to session object.
                USATodayBookListEntities data = new USATodayBookListEntities();
                int isbnId = int.Parse(e.CommandArgument.ToString());
                USAToday.Booklist.Business.BookISBNView b = data.BookISBNViews.Single(l => l.Id == isbnId);

                AssignedISBNViews.Add(b);
                base.CloseClientWindow();
            }
        }
    }
   
}