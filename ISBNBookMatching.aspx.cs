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
    public partial class ISBNBookMatching : PageBase
    {
        enum ISBNStatus
        {
            New=1,
            NeedsEditing=2,
            DoneEditing=3
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
            get { return (USAToday.Booklist.Business.Book)Session["AssociatedBook"]; }
            set { Session["AssociatedBook"] = value; }
        }

        /// <summary>
        /// Gets the ISBN book matching.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="MaxRecords">The max records.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ISBNWorkingList> GetISBNBookMatching(string Mode, int MaxRecords, string ISBN, string Title, string Author, string SortExpression)
        {            
            using (var data = new USATodayBookListEntities())
            {
                var s = data.GetISBNWorkingList(MaxRecords, ISBN, Title, Author, Mode, SortExpression);
                return s.ToList<USAToday.Booklist.Business.ISBNWorkingList>();
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
            if (!IsPostBack) {
                FilterTypeRadioButtonList.SelectedValue= ""+Session["ISBNBookMatchingFilterTypeRadioButton"];
                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

                AssociatedBook = null;

                BindGrid();
            } 
            else 
            {
                Session["ISBNBookMatchingFilterTypeRadioButton"]= FilterTypeRadioButtonList.SelectedValue;
                if (base.GetClientParamName() == "RefreshBook") 
                {
                    if (base.GetClientParamValue() == "RedirectToBook") 
                    {
                        Response.Redirect("Book.aspx?Id=0");
                    }
                }
                else if (base.GetClientParamName() == "RefreshISBN")
                {
                    BindGrid();
                }
            }

            // Set up the events
            ISBNMatchingGridView.PageIndexChanging += ISBNMatchingGridView_PageIndexChanging;
            ISBNMatchingGridView.Sorting += ISBNMatchingGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
                this.ISBNMatchingGridView.Columns[7].Visible = false;
            }

        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        /// <remarks></remarks>
        private void BindGrid()
        {
            int max = 50;
            int.TryParse(this.MaxRecordTextBox.Text, out max);
            string sortExpression = LastSortKey + " " + LastSortDirection;

            var isbnBookMatching = GetISBNBookMatching(FilterTypeRadioButtonList.SelectedValue, max, this.ISBNTextBox.Text, this.TitleTextBox.Text, this.AuthorTextBox.Text,sortExpression.Trim());
            //DABooklist da = new DABooklist();
            //DataTable isbnBookMatching = da.GetISBNWorkingList(FilterTypeRadioButtonList.SelectedValue, max, this.ISBNTextBox.Text, this.TitleTextBox.Text, this.AuthorTextBox.Text, sortExpression.Trim());
           
            if (isbnBookMatching == null || isbnBookMatching.Count() == 0)
                ISBNMatchingGridView.AllowSorting = false;
            else
                ISBNMatchingGridView.AllowSorting = true;

            ISBNMatchingGridView.DataSource = isbnBookMatching;
            ISBNMatchingGridView.DataBind();

            // Store in session variable
            Session.Add("ISBNBookMatching", isbnBookMatching);

        }
        /// <summary>
        /// Handles the PageIndexChanging event of the ISBNMatchingGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ISBNMatchingGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Get book working list from the session 
            List<USAToday.Booklist.Business.ISBNWorkingList> ibm = default(List<USAToday.Booklist.Business.ISBNWorkingList>);
            ibm = Session["ISBNBookMatching"] as List<USAToday.Booklist.Business.ISBNWorkingList>;

            // Set the index 
            ISBNMatchingGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            ISBNMatchingGridView.DataSource = ibm;
            ISBNMatchingGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the ISBNMatchingGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ISBNMatchingGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get book working list from the session 
            List<USAToday.Booklist.Business.ISBNWorkingList> ibm = default(List<USAToday.Booklist.Business.ISBNWorkingList>);
            ibm = Session["ISBNBookMatching"] as List<USAToday.Booklist.Business.ISBNWorkingList>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "Rank":
                    ibm = Sort(ibm, b => (b.Rank == null ? 0 : b.Rank.Value));
                    break;
/*                case "LastWeekRank":
                    ibm = Sort(ibm, b => (b.LastWeekRank == null ? 0 : b.LastWeekRank.Value));
                    break; */
                case "Sales":
                    ibm = Sort(ibm, b => (b.Sales == null ? 0 : b.Sales.Value));
                    break;
                case "BookTitle":
                    ibm = Sort(ibm, b => b.BookTitle);
                    break;
                case "ISBN":
                    ibm = Sort(ibm, b => b.ISBN);
                    break;
                case "Title":
                    ibm = Sort(ibm, b => b.Title);
                    break;
                case "Author":
                    ibm = Sort(ibm, b => b.Author);
                    break;
                case "Providers":
                    ibm= Sort(ibm, b => (b.Providers == null ? 0 : b.Providers.Value));
                    break;
                case "eProviders":
                    ibm= Sort(ibm, b => (b.eProviders == null ? 0 : b.eProviders.Value));
                    break;
                case "BookStatus":
                    ibm = Sort(ibm, b => (b.BookStatus == null ? "" : b.BookStatus));
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            ISBNMatchingGridView.DataSource = ibm;
            ISBNMatchingGridView.DataBind();
            // Store in a session variable 
            Session.Add("ISBNBookMatching", ibm);
        }
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ISBNWorkingList> Sort(List<USAToday.Booklist.Business.ISBNWorkingList> list,
                Func<USAToday.Booklist.Business.ISBNWorkingList, string> sortKey)
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
        private List<USAToday.Booklist.Business.ISBNWorkingList> Sort(List<USAToday.Booklist.Business.ISBNWorkingList> list,
                Func<USAToday.Booklist.Business.ISBNWorkingList, long> sortKey)
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
        protected void Filter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        /// <summary>
        /// Handles the Click event of the AddBook control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void AddBook_Click(object sender, EventArgs e)
        {
            Response.Redirect("Book.aspx?Id=0");
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
            this.TitleTextBox.Text = "";
            this.AuthorTextBox.Text = "";
            BindGrid();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the FilterType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void FilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
                BindGrid();
        }

        /// <summary>
        /// Handles the RowCommand event of the ISBNMatchingGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNMatchingGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ASSIGN")
            {
                if (Session["AssociatedBook" + e.CommandArgument.ToString()] != null)
                {
                    USATodayBookListEntities data = new USATodayBookListEntities();
                    int bookISBNId = int.Parse(e.CommandArgument.ToString());
                    USAToday.Booklist.Business.BookISBN isbn = data.BookISBNs.Single(l => l.Id == bookISBNId);
                    int? bookid= isbn.BookID = ((USAToday.Booklist.Business.Book)Session["AssociatedBook" + e.CommandArgument.ToString()]).Id;
                    data.SaveChanges();
                    data.MaybeAdjustBookStatus(bookid);

                    BindGrid();
                }
            }

        }

        /// <summary>
        /// Handles the RowDataBound event of the ISBNMatchingGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNMatchingGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the book title
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                USAToday.Booklist.Business.ISBNWorkingList b = ((USAToday.Booklist.Business.ISBNWorkingList)e.Row.DataItem);

                LinkButton lb = ((LinkButton)e.Row.Cells[5].Controls[0]);
                if (b.BookId == null)
                    lb.Text = "Assign Book";
                else
                {
                    lb.Text = "Assigned";
                    lb.Style.Add("color", "green");
                }


                lb.OnClientClick = "addBook(" + b.BookISBNId + "," + b.ISBN + ");";
                lb.CommandArgument = b.BookISBNId.ToString();

                if (b.ISBNStatus != null && b.ISBNStatus.ToUpper() == "DONE EDITING")
                    e.Row.Cells[1].CssClass = "greenLinks";

                if (b.BookStatus != null && b.BookStatus.ToUpper() == "DONE EDITING")
                    e.Row.Cells[9].Style.Add("color", "green");
            }
        }

        protected string GetBookId(object o)
        {
            if (o == null || ((Nullable<int>)0).Value.ToString() == "")
                return "0";
            else
                return ((Nullable<int>)o).Value.ToString();
        }
    }

}