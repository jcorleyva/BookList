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
    public partial class AuthorLookup : PageBase
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
        /// Gets or sets the assigned authors.
        /// </summary>
        /// <value>The assigned authors.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Author> AssignedAuthors
        {
            get { return (List<USAToday.Booklist.Business.Author>)Session["AssignedAuthors" + this.BookId.ToString()]; }
            set { Session["AssignedAuthors" + this.BookId.ToString()] = value; }
        }

        /// <summary>
        /// Gets the authors.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Author> GetAuthors()
        {

            string where = string.Empty;
            if (this.FirstNameTextBox.Text.Trim().Length > 0)
            {
                if(this.FirstNameTextBox.Text != string.Empty)
                    where = " and c.FirstName like '%" +  this.FirstNameTextBox.Text.Trim().Replace("'","''") + "%'" ;
            }

            if (this.LastNameTextBox.Text.Trim().Length > 0)
            {
                if(this.LastNameTextBox.Text != string.Empty)
                    where += " and c.LastName like '%" + this.LastNameTextBox.Text.Trim().Replace("'","''") + "%'";
            }

            using (var data = new USATodayBookListEntities())
            {
                string sql = "Select value c from Authors as c where 1=1 " + where;

                var authors = Global.BookEntities.CreateQuery<USAToday.Booklist.Business.Author>(sql);

                return authors.ToList<USAToday.Booklist.Business.Author>();
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
            this.BookId = int.Parse(Request["bookId"]);

            if (!IsPostBack)
            {
                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;
            }
            else if (base.GetClientParamName() == "RefreshSearch")
            {
                if (base.GetClientParamValue("RefreshSearch") != "undefined")
                {
                    this.FirstNameTextBox.Text = base.GetClientParamValue("RefreshSearch",0);
                    this.LastNameTextBox.Text = base.GetClientParamValue("RefreshSearch", 1);
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
            var authors = GetAuthors();
            if (authors.Count() == 0)
                AuthorGridView.AllowSorting = false;
            else
                AuthorGridView.AllowSorting = true;

            AuthorGridView.DataSource = authors;
            AuthorGridView.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the FilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void FilterButton_Click( object sender, EventArgs e)
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
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            BindData();
        }
        /// <summary>
        /// Handles the Click event of the AddAuthorButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AddAuthorButton_Click(object sender, EventArgs e)
        {
            base.CloseClientWindow("RedirectToAuthor");
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the AuthorGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AuthorGridView.PageIndex = e.NewPageIndex;
            BindData();

        }

        /// <summary>
        /// Handles the Sorting event of the AuthorGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.Author> authors = default(List<USAToday.Booklist.Business.Author>);
            authors = GetAuthors();

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "FirstName":
                    AssignedAuthors = Sort(authors, author => author.FirstName);
                    break;
                case "LastName":
                    AssignedAuthors = Sort(authors, author => author.LastName);
                    break;
                case "Initial":
                    AssignedAuthors = Sort(authors, author => author.Initial);
                    break;
                case "Suffix":
                    AssignedAuthors = Sort(authors, author => author.Suffix);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            AuthorGridView.DataSource = AssignedAuthors;
            AuthorGridView.DataBind();
        }
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Author> Sort(List<USAToday.Booklist.Business.Author> list,
                Func<USAToday.Booklist.Business.Author, string> sortKey)
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
        /// Handles the RowDataBound event of the AuthorGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the First Name of Author
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = ((LinkButton)e.Row.Cells[0].Controls[0]);
                USAToday.Booklist.Business.Author a = ((USAToday.Booklist.Business.Author)e.Row.DataItem);
                lb.Text = a.LastName;
                lb.CommandName = "SELECT";
                lb.CommandArgument = a.Id.ToString();
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the AuthorGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SELECT")
            {
                //Retrieve author and add to session object.
                int authorId = int.Parse(e.CommandArgument.ToString());
                USAToday.Booklist.Business.Author a = Global.BookEntities.Authors.Single(l => l.Id == authorId);
                
                AssignedAuthors.Add(a);
                base.CloseClientWindow();
            }
        }
    }
}