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
    public partial class BookWorkingList : PageBase
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
        /// Gets the book working list.
        /// </summary>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="maxRecords">The max records.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.BookWorkingList> GetBookWorkingList(string  Mode, int maxRecords,string Title, string ISBN, string AuthorFirstName, string AuthorLastName, string SortExpression)
        {
            using (var data = new USATodayBookListEntities())
            {
                var bwl = data.GetBookWorkingList(maxRecords, ISBN, Title, AuthorFirstName, AuthorLastName, Mode, SortExpression); 
                return bwl.ToList<USAToday.Booklist.Business.BookWorkingList>();

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
                FilterTypeRadioButtonList.SelectedValue= ""+Session["BookWorkingListFilterTypeRadioButton"];
                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

                BindGrid();
            }
            else if (base.GetClientParamName() == "RefreshGrid")
            {
                BindGrid();
            } 
            else 
            {
                Session["BookWorkingListFilterTypeRadioButton"]= FilterTypeRadioButtonList.SelectedValue;
            }
            // Set up the events
            BookWorkingListGridView.PageIndexChanging += BookWorkingListGridView_PageIndexChanging;
            BookWorkingListGridView.Sorting += BookWorkingListGridView_Sorting;
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
            //string where = "1=1 ";
            //if(this.TitleTextBox.Text != "")
            //    where = "&& b.Title='" + this.TitleTextBox.Text + "'";
            
            var bookWorkingList = GetBookWorkingList(FilterTypeRadioButtonList.SelectedValue,max, this.TitleTextBox.Text, "", this.FirstNameTextBox.Text, this.LastNameTextBox.Text, sortExpression);

            if (bookWorkingList.Count() == 0)
                BookWorkingListGridView.AllowSorting = false;
            else
                BookWorkingListGridView.AllowSorting = true;

            BookWorkingListGridView.DataSource = bookWorkingList;
            BookWorkingListGridView.DataBind();

            // Store in session variable
            Session.Add("BookWorkingList", bookWorkingList);

        }

        /// <summary>
        /// Handles the PageIndexChanging event of the BookWorkingListGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void BookWorkingListGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Get book working list from the session 
            List<USAToday.Booklist.Business.BookWorkingList> bwl = default(List<USAToday.Booklist.Business.BookWorkingList>);
            bwl = Session["BookWorkingList"] as List<USAToday.Booklist.Business.BookWorkingList>;

            // Set the index 
            BookWorkingListGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            BookWorkingListGridView.DataSource = bwl;
            BookWorkingListGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the BookWorkingListGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void BookWorkingListGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get book working list from the session 
            List<USAToday.Booklist.Business.BookWorkingList> bwl = default(List<USAToday.Booklist.Business.BookWorkingList>);
            bwl = Session["BookWorkingList"] as List<USAToday.Booklist.Business.BookWorkingList>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "Rank":
                    bwl = Sort(bwl, b => (b.Rank == null ? 0 : b.Rank.Value));
                    break;
                case "LastWeekRank":
                    bwl = Sort(bwl, b => (b.LastWeekRank == null ? 0 : b.LastWeekRank.Value));
                    break;
                case "Sales":
                    bwl = Sort(bwl, b => (b.Sales == null ? 0 : b.Sales.Value));
                    break;
                case "Title":
                    bwl = Sort(bwl, b => b.Title);
                    break;
                case "AuthorDisplay":
                    bwl = Sort(bwl, b => b.AuthorDisplay);
                    break;
                case "EditStatus":
                    bwl = Sort(bwl, b => b.EditStatus);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            BookWorkingListGridView.DataSource = bwl;
            BookWorkingListGridView.DataBind();
            // Store in a session variable 
            Session.Add("BookWorkingList", bwl);
        }
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookWorkingList> Sort(List<USAToday.Booklist.Business.BookWorkingList> list,
                Func<USAToday.Booklist.Business.BookWorkingList, string> sortKey)
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
        private List<USAToday.Booklist.Business.BookWorkingList> Sort(List<USAToday.Booklist.Business.BookWorkingList> list,
                Func<USAToday.Booklist.Business.BookWorkingList, long> sortKey)
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
        /// Handles the Click event of the ClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilter_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookWorkingList.aspx");
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
        protected void BookWorkingListGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the book title
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                USAToday.Booklist.Business.BookWorkingList b = ((USAToday.Booklist.Business.BookWorkingList)e.Row.DataItem);

                if (b.EditStatus != null && b.EditStatus.ToUpper() == "DONE EDITING")
                    e.Row.Cells[5].Style.Add("color", "green");
            }
        }

    }
}