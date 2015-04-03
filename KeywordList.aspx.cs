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
    public partial class KeywordList : System.Web.UI.Page
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
        /// Gets the keywords.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Keyword> GetKeywords()
        {
            using (var data = new USATodayBookListEntities())
            {
                var keywords = data.Keywords;
                return keywords.ToList<USAToday.Booklist.Business.Keyword>();

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
                this.BindData();

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;
            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <remarks></remarks>
        private void BindData()
        {
            // Get the links
            List<USAToday.Booklist.Business.Keyword> keywords = GetKeywords();

            // Bind
            KeywordGridView.DataSource = keywords;
            KeywordGridView.DataBind();

        }

        /// <summary>
        /// Handles the Click event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void Filter_Click(object sender, EventArgs e)
        {
            if (this.KeywordTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var keywords = from a in data.Keywords
                                     where a.KeywordDescription.Contains(this.KeywordTextBox.Text.Trim())
                                     orderby a.KeywordDescription
                                     select a;
                    if (keywords.Count() == 0)
                        KeywordGridView.AllowSorting = false;

                    KeywordGridView.DataSource = keywords;
                    KeywordGridView.DataBind();
                }
            }
            else
            {
                KeywordGridView.DataSource = GetKeywords();
                KeywordGridView.DataBind();
            }
        }

        /// <summary>
        /// Handles the RowEditing event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            KeywordGridView.EditIndex = e.NewEditIndex;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowCancelingEdit event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            KeywordGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowUpdating event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                int Id = int.Parse(e.Keys[0].ToString());
                var Keyword = data.Keywords.Single(l => l.Id == Id);
                Keyword.KeywordDescription = e.NewValues[0].ToString();
                data.SaveChanges();
            }

            KeywordGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowDeleting event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                int Id = int.Parse(e.Keys[0].ToString());
                var keyword = data.Keywords.Single(l => l.Id == Id);
                data.Keywords.DeleteObject(keyword);
                data.SaveChanges();
            }
            KeywordGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the Sorting event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.Keyword> keywords = default(List<USAToday.Booklist.Business.Keyword>);
            keywords = GetKeywords();

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "KeywordDescription":
                    keywords = Sort(keywords, keyword => keyword.KeywordDescription);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            KeywordGridView.DataSource = keywords;
            KeywordGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Keyword> Sort(List<USAToday.Booklist.Business.Keyword> list,
                Func<USAToday.Booklist.Business.Keyword, string> sortKey)
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
        private List<USAToday.Booklist.Business.Keyword> Sort(List<USAToday.Booklist.Business.Keyword> list,
                Func<USAToday.Booklist.Business.Keyword, decimal> sortKey)
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
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddTextBox.Text.Trim() == "")
                ErrorDiv.Visible = true;
            else
            {
                ErrorDiv.Visible = false;
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.Keyword keyword = new USAToday.Booklist.Business.Keyword();

                    keyword.KeywordDescription = AddTextBox.Text;

                    data.Keywords.AddObject(keyword);
                    data.SaveChanges();

                    //clear text box
                    this.AddTextBox.Text = "";

                    //rebind data to show changes
                    this.BindData();
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
            this.KeywordTextBox.Text = "";
            this.BindData();
        }     
    }
}