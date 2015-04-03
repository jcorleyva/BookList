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
    public partial class AuthorList : PageBase
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


        public List<USAToday.Booklist.Business.Author> Authors
        {
            get
            {
                if (Session["Authors"] == null)
                {
                    List<USAToday.Booklist.Business.Author> authors = default(List<USAToday.Booklist.Business.Author>);
                    Session["Authors"] = authors;
                }
                return (List<USAToday.Booklist.Business.Author>)Session["Authors"];
            }
            set
            {
                Session["Authors"] = value;
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
                // Bind
                AuthorsGridView.DataSource = Authors;
                AuthorsGridView.DataBind();

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            AuthorsGridView.PageIndexChanging += AuthorsGridView_PageIndexChanging;
            AuthorsGridView.Sorting += ChannelTypeGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the AuthorsGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void AuthorsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index 
            AuthorsGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            AuthorsGridView.DataSource = Authors;
            AuthorsGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the ChannelTypeGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ChannelTypeGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.Author> authors = Authors;

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "FirstName":
                    authors = Sort(authors, author => author.FirstName);
                    break;
                case "LastName":
                    authors = Sort(authors, author => author.LastName);
                    break;
                case "DisplayName":
                    authors = Sort(authors, author => author.DisplayName);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            AuthorsGridView.DataSource = authors;
            AuthorsGridView.DataBind();

            Authors = authors;
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
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Author> Sort(List<USAToday.Booklist.Business.Author> list,
                Func<USAToday.Booklist.Business.Author, decimal> sortKey)
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
        /// Filters the click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void FilterClick(object sender, EventArgs e)
        {
            this.BindData();

        }

        /// <summary>
        /// BTNs the add click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAddClick(object sender, EventArgs e)
        {
            Response.Redirect("Author.aspx?ID=0");
        }

        protected void ClearFilter_Click(object sender, EventArgs e)
        {
            this.FirstNameTextBox.Text = "";
            this.LastNameTextBox.Text = "";
            Authors = null;
            AuthorsGridView.DataSource = Authors;
            AuthorsGridView.DataBind();
        }

        protected void AuthorsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {                
                int Id = int.Parse(e.Keys[0].ToString());
                var author = data.Authors.Single(l => l.Id == Id);
                data.Authors.DeleteObject(author);
                try
                {
                    data.SaveChanges();
                    base.ShowClientMessage("Author deleted successfully.");
                }
                catch (Exception ex)
                {
                    base.ShowClientMessage("This author cannot be deleted because it is associated with a book. Please remove the association to perform this operation.");
                }
            }

            AuthorsGridView.EditIndex = -1;
            this.BindData();
        }

        private void BindData()
        {
            string where = string.Empty;
            if (this.FirstNameTextBox.Text.Trim().Length > 0)
            {
                if (this.FirstNameTextBox.Text != string.Empty)
                    where = " and c.FirstName like '%" + this.FirstNameTextBox.Text.Trim().Replace("'","''") + "%'";
            }

            if (this.LastNameTextBox.Text.Trim().Length > 0)
            {
                if (this.LastNameTextBox.Text != string.Empty)
                    where += " and c.LastName like '%" + this.LastNameTextBox.Text.Trim().Replace("'","''") + "%'";
            }

            using (var data = new USATodayBookListEntities())
            {
                string sql = "Select value c from Authors as c where 1=1 " + where + " order by c.LastName";

                var authors = Global.BookEntities.CreateQuery<USAToday.Booklist.Business.Author>(sql);

                if (authors.Count() == 0)
                    AuthorsGridView.AllowSorting = false;

                AuthorsGridView.DataSource = authors;
                AuthorsGridView.DataBind();

                Authors = authors.ToList<USAToday.Booklist.Business.Author>();
            }
        }
    }
}