using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;
using USATodayBookList.AppCode;
using System.Data;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class BookListLookup : PageBase
    {

        private int IsbnId 
        {
            get { return int.Parse(Request["isbnId"]); }
        }

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
        /// Gets or sets the associated book.
        /// </summary>
        /// <value>The associated book.</value>
        /// <remarks></remarks>
        private USAToday.Booklist.Business.Book AssociatedBook
        {
            get { return (USAToday.Booklist.Business.Book)Session["AssociatedBook" + this.IsbnId.ToString()]; }
            set { Session["AssociatedBook" + this.IsbnId.ToString()] = value; }
        }

        /// <summary>
        /// Gets the books.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookAuthorView> GetBooks()
        {
            string where = string.Empty;
            if (this.AuthorNameTextBox.Text.Trim().Length > 0)
            {
                where = " and c.AuthorName like '%" + this.AuthorNameTextBox.Text.Trim().Replace("'", "''") + "%'";
            }

            if (this.TitleTextBox.Text.Trim().Length > 0)
            {
                where += " and c.Title like '%" + this.TitleTextBox.Text.Trim().Replace("'", "''") + "%'";
            }

            using (var data = new USATodayBookListEntities())
            {
                string sql = "Select value c from BookAuthorViews as c where 1=1 " + where;

                var books = Global.BookEntities.CreateQuery<USAToday.Booklist.Business.BookAuthorView>(sql).Distinct();

                return books.ToList<USAToday.Booklist.Business.BookAuthorView>();
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
                AddBookButton.OnClientClick = "addBook('" + Request["isbn"] + "');";

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;
            }
            else if (base.GetClientParamName() == "RefreshSearch")
            {
                string RefreshSearch = base.GetClientParamValue("RefreshSearch");
                if(RefreshSearch != "undefined")
                {
                    this.TitleTextBox.Text = RefreshSearch;
                    BindData();
                }
            }
        }


        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <remarks></remarks>
        private void BindData()
        {
            List<USAToday.Booklist.Business.BookAuthorView> books = GetBooks();

            BookGridView.DataSource = books;
            BookGridView.DataBind();
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
            this.TitleTextBox.Text = "";
            this.AuthorNameTextBox.Text = "";
            BindData();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the BookGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void BookGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BookGridView.PageIndex = e.NewPageIndex;
            BindData();

        }

        /// <summary>
        /// Handles the Sorting event of the BookGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void BookGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            //// Get the list of books
            List<USAToday.Booklist.Business.BookAuthorView> books = default(List<USAToday.Booklist.Business.BookAuthorView>);
            books = GetBooks();

            //// Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "Title":
                    books = Sort(books, book => book.Title);
                    break;
                case "AuthorName":
                    books = Sort(books, book => book.AuthorName);
                    break;
                case "PubDate":
                    books = Sort(books, book => book.PubDate.Value);
                    break;
            }

            LastSortKey = e.SortExpression;

            //// Rebind 
            BookGridView.DataSource = books;
            BookGridView.DataBind();
        }
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookAuthorView> Sort(List<USAToday.Booklist.Business.BookAuthorView> list,
                Func<USAToday.Booklist.Business.BookAuthorView, string> sortKey)
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
        private List<USAToday.Booklist.Business.BookAuthorView> Sort(List<USAToday.Booklist.Business.BookAuthorView> list,
                Func<USAToday.Booklist.Business.BookAuthorView, DateTime> sortKey)
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
        /// Handles the RowDataBound event of the BookGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void BookGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the book title
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = ((LinkButton)e.Row.Cells[0].Controls[0]);
                USAToday.Booklist.Business.BookAuthorView b = ((USAToday.Booklist.Business.BookAuthorView)e.Row.DataItem);
                lb.Text = b.Title;
                lb.CommandName = "SELECT";
                lb.CommandArgument = b.Id.ToString();

                string pubDate = e.Row.Cells[2].Text;
                DateTime pubDateTime;
                if (DateTime.TryParse(pubDate, out pubDateTime))
                    if (pubDateTime == DateTime.MinValue)
                        e.Row.Cells[2].Text= "";
                    else
                        e.Row.Cells[2].Text = pubDateTime.ToShortDateString();
                else
                    e.Row.Cells[2].Text= "";

            }
        }

        /// <summary>
        /// Handles the RowCommand event of the BookGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void BookGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
            {
                //Retrieve book and add to session object.
                USATodayBookListEntities data = new USATodayBookListEntities();
                int bookId = int.Parse(e.CommandArgument.ToString());
                USAToday.Booklist.Business.Book bk = data.Books.Single(l => l.Id == bookId);

                AssociatedBook = bk;
                base.CloseClientWindow();
            }
        }
    }
}