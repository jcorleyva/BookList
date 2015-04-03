using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;

namespace USATodayBookList {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class Book :System.Web.UI.Page {
        /// <summary>
        /// Gets or sets the last sort key.
        /// </summary>
        /// <value>The last sort key.</value>
        /// <remarks></remarks>
        public string LastSortKey {
            get { return ViewState["LastSortKey"].ToString(); }
            set { ViewState["LastSortKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the last sort direction.
        /// </summary>
        /// <value>The last sort direction.</value>
        /// <remarks></remarks>
        public string LastSortDirection {
            get { return ViewState["LastSortDirection"].ToString(); }
            set { ViewState["LastSortDirection"] = value; }
        }

        /// <summary>
        /// Gets the books.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Book> GetBooks() {
            using (var data = new USATodayBookListEntities()) {
                var books = data.Books;
                return books.ToList<USAToday.Booklist.Business.Book>();

            }
        }
        /// <summary>
        /// Gets the books.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Book> GetBooks(string title) {
            using (var data = new USATodayBookListEntities()) {
                var books = from a in data.Books
                            where a.Title.Contains(title)
                            select a;
                return books.ToList<USAToday.Booklist.Business.Book>();
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) 
            {
                List<USAToday.Booklist.Business.Book> books = new List<USAToday.Booklist.Business.Book>();
               
                BooksGridView.DataSource = books;
                BooksGridView.DataBind();

                //// Store in session variable
                Session.Add("Books", books);

                //// Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            BooksGridView.PageIndexChanging += BooksGridView_PageIndexChanging;
            BooksGridView.Sorting += BooksGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false) {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the BooksGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void BooksGridView_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            // Get the list of channel Types from the session 
            List<USAToday.Booklist.Business.Book> books = default(List<USAToday.Booklist.Business.Book>);
            books = Session["Books"] as List<USAToday.Booklist.Business.Book>;

            // Set the index 
            BooksGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            BooksGridView.DataSource = books;
            BooksGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the BooksGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void BooksGridView_Sorting(object sender, GridViewSortEventArgs e) {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.Book> books = default(List<USAToday.Booklist.Business.Book>);
            books = Session["Books"] as List<USAToday.Booklist.Business.Book>;

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression) {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression) {
                case "Title":
                    books = Sort(books, book => book.Title);
                    break;
                case "AuthorDisplay":
                    books = Sort(books, book => book.AuthorDisplay);
                    break;
                case "PubDate":
                    books = Sort(books, book => book.PubDate.ToString());
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            BooksGridView.DataSource = books;
            BooksGridView.DataBind();
            // Store in a session variable 
            Session.Add("Books", books);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Book> Sort(List<USAToday.Booklist.Business.Book> list,
                Func<USAToday.Booklist.Business.Book, string> sortKey) {
            if (LastSortDirection == "ASC") {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            } else {
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
        private List<USAToday.Booklist.Business.Author> Sort(List<USAToday.Booklist.Business.Author> list,
                Func<USAToday.Booklist.Business.Author, decimal> sortKey) {
            if (LastSortDirection == "ASC") {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            } else {
                list = list.OrderBy(sortKey).ToList();
                LastSortDirection = "ASC";
            }
            return list;
        }

        /// <summary>
        /// Filters the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void FilterClick(object sender, EventArgs e) {
            if (this.TitleTextBox.Text != "") {
                List<USAToday.Booklist.Business.Book> books = GetBooks(this.TitleTextBox.Text.Trim());
                if (books.Count() == 0)
                    BooksGridView.AllowSorting = false;
                else
                    BooksGridView.AllowSorting = true;

                BooksGridView.DataSource = books;
                BooksGridView.DataBind();
                Session.Add("Books", books);
            }
        }

        /// <summary>
        /// BTNs the add click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAddClick(object sender, EventArgs e) {
            Response.Redirect("Book.aspx?Id=0");
        }

        /// <summary>
        /// Handles the Click event of the ClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilter_Click(object sender, EventArgs e) {
            this.TitleTextBox.Text = "";
            Response.Redirect("BookList.aspx");

        }
    }
}